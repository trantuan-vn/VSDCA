// Test34.h

#pragma once
#include < stdio.h >
#include < stdlib.h >
#include < vcclr.h >

#pragma unmanaged

#include <ttclasses/TTInclude.h>

extern TTConnectionPool pool;

#pragma managed
using namespace System;
using namespace System::Data;
using namespace System::Collections::Generic;
using namespace System::Runtime::InteropServices;

namespace TimesTenHelper {

	public ref class TTHelper
			{
				TTConnection * m_connection;
				TTStatus * m_lastError;
				IntPtr m_connString;
				DateTime m_expireDate;
			public:
				array<System::Object^>^ mv_Colunms;
			public:
				TTHelper(System::String^ connStr)
				{
					m_connString = Marshal::StringToHGlobalAnsi(connStr);
					m_expireDate = DateTime::Now.AddMinutes(1); 
				}

				~TTHelper()
				{
					Destroy();
				}

				bool isExpired()
				{
					return m_expireDate < DateTime::Now;
				}

				void Destroy()
				{
					if (m_connString!=IntPtr::Zero)
					{
						Marshal::FreeHGlobal(m_connString);
						m_connString = IntPtr::Zero;
					}

					if (m_connection)
					{
						TTStatus status;
						m_connection->Disconnect(status);
						pool.freeConnection(m_connection);
						//delete m_connection;
						m_connection = NULL;
					}
					
				}

				void ExecuteNonQuery(System::String^ command)
				{
					ExecuteNonQuery(command, 1, false);
				}

				void ExecuteNonQuery(System::String^ command, int count)
				{
					ExecuteNonQuery(command, count, false);
				}

				void ExecuteNonQuery(System::String^ command, int count, bool nocommit)
				{
					InitConnection();

					IntPtr commandString = Marshal::StringToHGlobalAnsi(command);

					TTCmd * query = PrepareQuery((char*)(void*)commandString, count);

					Marshal::FreeHGlobal(commandString);

					CloseQuery(query, nocommit);
				}

				System::Object^ ExecuteScalar(System::String^ command)
				{
					return ExecuteScalar(command, 1, false);
				}

				System::Object^ ExecuteScalar(System::String^ command, int count)
				{
					return ExecuteScalar(command, count, false);
				}

				System::Object^ ExecuteScalar(System::String^ command, int count, bool nocommit)
				{
					InitConnection();

					IntPtr commandString = Marshal::StringToHGlobalAnsi(command);

					TTCmd * query = PrepareQuery((char*)(void*)commandString, count);

					Marshal::FreeHGlobal(commandString);

					System::Object^ result = GetSingleResult(query);

					CloseQuery(query, nocommit);

					return result;
				}

				array<array<System::Object^>^>^ ExecuteObjects(System::String^ command)
				{
					InitConnection();

					IntPtr commandString = Marshal::StringToHGlobalAnsi(command);

					TTCmd * query = PrepareQuery((char*)(void*)commandString, 1);

					Marshal::FreeHGlobal(commandString);

					array<array<System::Object^>^>^ result =  GetMultipleResults(query);

					CloseQuery(query, false);

					return result;
				}

				DataSet^ ExecuteReturnDataSet(System::String^ command)
				{
					InitConnection();

					IntPtr commandString = Marshal::StringToHGlobalAnsi(command);

					TTCmd * query = PrepareQuery((char*)(void*)commandString, 1);

					Marshal::FreeHGlobal(commandString);

					DataSet^ result =  GetDataSetResults(query);

					CloseQuery(query, true);

					return result;
				}

				System::String^ LastError()
				{
					if (m_lastError)
						return Marshal::PtrToStringAnsi((System::IntPtr)m_lastError->err_msg) + Marshal::PtrToStringAnsi((System::IntPtr)m_lastError->odbc_error);
					else
						return System::String::Empty;
				}

				void Commit()
				{
					if (m_connection)
					{
						try
						{
							TTStatus status;
							m_connection->Commit(status);
						}
						catch(TTWarning warning)
						{
							OnWarning(warning);
						}
						catch(TTError error)
						{
							OnError(error);
						}
					}
				}

				void Rollback()
				{
					if (m_connection)
					{
						try
						{
							TTStatus status;
							m_connection->Rollback(status);
						}
						catch(TTWarning warning)
						{
							OnWarning(warning);
						}
						catch(TTError error)
						{
							OnError(error);
						}
					}
				}

			private:
				void InitConnection()
				{
					if (!m_connection)
					{
						TTStatus status;
						
						try
						{
							m_connection = pool.getConnection(10);
							if (!m_connection)
								m_connection = new TTConnection();
							
							if (!m_connection->isConnected())
								m_connection->Connect((char*)(void*)m_connString, status);
							
 
						}
						catch(TTWarning warning)
						{
							OnWarning(warning);
						}
						catch(TTError error)
						{
							OnError(error);
						}
					}
				}

				TTCmd * PrepareQuery(const char* command, int count)
				{
					TTStatus status;
					TTCmd * query = new TTCmd();
					try
					{
						query->PrepareBatch(m_connection, command, TTCmd::TTCMD_USER_BIND_NONE , count, status);
						if (count>1)
							query->ExecuteBatch(count, status);
						else
							query->Execute(status);
					}
					catch(TTWarning warning)
					{
						query->Drop(status);
						delete query;
						OnWarning(warning);
					}
					catch(TTError error)
					{
						query->Drop(status);
						delete query;
						OnError(error);
					}
					return query;
				}

				void CloseQuery(TTCmd * query, bool nocommit)
				{
					TTStatus status;
					try
					{
						query->Close(status);
						if (!nocommit)
							m_connection->Commit(status);
						query->Drop(status);
					}
					catch(TTWarning warning)
					{
						query->Close(status);
						query->Drop(status);
						delete query;
						OnWarning(warning);
					}
					catch(TTError error)
					{
						query->Close(status);
						query->Drop(status);
						delete query;
						OnError(error);
					}
					delete query;

					m_lastError = 0;
				}

				System::Object^ GetSingleResult(TTCmd * query)
				{
					TTStatus status;
					try
					{
						if ( !query->FetchNext(status) && !query->isColumnNull(1) )
							switch(query->getColumnType(1))
						{
							case SQL_TINYINT:	{ char result; query->getColumn(1, &result); return result; }
							case SQL_SMALLINT:	{ short int result; query->getColumn(1, &result); return result; }
							case SQL_INTEGER:	{ int result; query->getColumn(1, &result); return result; }
							case SQL_BIGINT:	{ __int64 result; query->getColumn(1, &result); return result; }
							case SQL_REAL:
							case SQL_FLOAT:     { float result; query->getColumn(1, &result); return result; }
							case SQL_NUMERIC:
							case SQL_DECIMAL:
							case SQL_DOUBLE:    { double result; query->getColumn(1, &result); return result; }
							case SQL_CHAR:
							case SQL_VARCHAR:   { char * result = new char[query->getColumnLength(1)+1]; query->getColumn(1, result); System::String^ str = Marshal::PtrToStringAnsi((System::IntPtr)result); delete result; return str; }
							case SQL_TIMESTAMP: { TIMESTAMP_STRUCT result;query->getColumn(1, &result); return gcnew System::DateTime(result.year, result.month, result.day, result.hour, result.minute, result.second); }
							case SQL_DATE:      { DATE_STRUCT result;query->getColumn(1, &result); return gcnew System::DateTime(result.year, result.month, result.day); }
							case SQL_TIME:      { TIME_STRUCT result;query->getColumn(1, &result); return gcnew System::DateTime(1,1,1,result.hour, result.minute, result.second); }
							case SQL_WVARCHAR:  
								{// char * a = (char*)"Tu\u00e2nTA"; 
								//int result_len;// = query->getColumnLength(1)+1;
								//result_len = query ->getColumnPrecision(1)+1;
								//query->DescribeColumn(i, &colName[0], &SQLType, &precision, &scale, &nullable, stat);
								//SQLWCHAR *result;// = new SQLWCHAR*[query->getColumnLength(1)+1];
								//query->getColumn(1, result, &result_len);
								wchar_t * result = query->columnP->wbufP;
								//SQLWCHAR * result = new SQLWCHAR[query->getColumnLength(1)+1];
								//*result = (SQLWCHAR) 0x0;
								//SQLWCHAR * result = (SQLWCHAR*) ((unsigned char*)a); 
								//result = *(*(*query).columnP).wbufP
								//TTStatus status;
								//query->BindWCharColumnNullable(1,result,2*query->getColumnPrecision(1)+2,&result_len,status);
								//query->getColumnNullable(1, (char *)result);
								
								//TTColumn * col;
								//col = query->get;
								//SQLTCHAR * result = col->wbufP ;

								//System::String^ str = Marshal::PtrToStringAnsi((System::IntPtr)result);
								//size_t convertedChars = 0;
								//size_t  sizeInBytes = ((str->Length + 1) * 2);
								//errno_t err = 0;
								//char    *ch = (char *)malloc(sizeInBytes);
								////char    *ch;
								//err = wcstombs_s(&convertedChars, 
								//					ch, sizeInBytes,
								//					result, sizeInBytes);

								System::String^ str1 = Marshal::PtrToStringUni((System::IntPtr)result);
								return str1;
								}
						}
					}
					catch(TTWarning warning)
					{
						OnWarning(warning);
					}
					catch(TTError error)
					{
						OnError(error);
					}

					return nullptr;
				}

				array<array<System::Object^>^>^ GetMultipleResults(TTCmd * query)
				{
					TTStatus status;
					try
					{
						int columns = query->getNColumns();
						int rows = query->getRowCount();

						List<array<System::Object^>^> resultList = gcnew List<array<System::Object^>^>();

						while(true)
						{

							if ( query->FetchNext(status) )
								break;

							array<System::Object^>^ row = gcnew array<System::Object^>(columns);

							for (int i = 1; i<= columns; i++ )
							{
								if (query->isColumnNull(i))
									row[i-1] = nullptr;
								else
									switch(query->getColumnType(i))
								{
									case SQL_TINYINT:	{ char result; query->getColumn(i, &result); row[i-1] = result; break; }
									case SQL_SMALLINT:	{ short int result; query->getColumn(i, &result); row[i-1] = result; break;  }
									case SQL_INTEGER:	{ int result; query->getColumn(i, &result); row[i-1] = result; break;  }
									case SQL_BIGINT:	{ __int64 result; query->getColumn(i, &result); row[i-1] = result; break;  }
									case SQL_REAL:
									case SQL_FLOAT:     { float result; query->getColumn(i, &result); row[i-1] = result; break;  }
									case SQL_NUMERIC:
									case SQL_DECIMAL:
									case SQL_DOUBLE:    { double result; query->getColumn(i, &result); row[i-1] = result; break;  }
									case SQL_CHAR:
									case SQL_VARCHAR:   { char * result = new char[query->getColumnLength(i)+1]; query->getColumn(i, result); row[i-1] = Marshal::PtrToStringAnsi((System::IntPtr)result); delete result; break; }
//									case SQL_VARCHAR:   { char * result; query->getColumn(i, &result); row[i-1] = Marshal::PtrToStringAnsi((System::IntPtr)result); break;  }
									case SQL_TIMESTAMP: { TIMESTAMP_STRUCT result;query->getColumn(i, &result); row[i-1] = gcnew System::DateTime(result.year, result.month, result.day, result.hour, result.minute, result.second); break;  }
									case SQL_DATE:      { DATE_STRUCT result;query->getColumn(i, &result); row[i-1] = gcnew System::DateTime(result.year, result.month, result.day); break; }
									case SQL_TIME:      { TIME_STRUCT result;query->getColumn(i, &result); row[i-1] = gcnew System::DateTime(1,1,1,result.hour, result.minute, result.second); break; }
									case SQL_WVARCHAR:  
										{
											SQLWCHAR *result;
											result = query->columnP->wbufP;
											row[i-1] = Marshal::PtrToStringUni((System::IntPtr)result); 
										}
								}
							}
							resultList.Add(row);
						}
						return resultList.ToArray();
					}
					catch(TTWarning warning)
					{
						OnWarning(warning);
					}
					catch(TTError error)
					{
						OnError(error);
					}

					return nullptr;
				}

				DataSet^ GetDataSetResults(TTCmd * query)
				{
					TTStatus status;
					try
					{
						int columns = query->getNColumns();
						int rows = query->getRowCount();
						DataSet^ mv_ds;
						DataColumn^ v_dtCol;						
						DataRow^ v_dtRow;
						mv_ds=gcnew DataSet("TimesTenDB");
						mv_ds->Tables->Add("TTTable");

						
						mv_Colunms = gcnew array<System::Object^>(columns);
						for (int i = 1; i<= columns; i++ )
						{
							char * strColName ;
							
							strColName = (char*)query->getColumnName(i);
							mv_Colunms[i-1]= Marshal::PtrToStringAnsi((System::IntPtr)strColName);
							v_dtCol=gcnew DataColumn(mv_Colunms[i-1]->ToString());
							v_dtCol->ColumnName=mv_Colunms[i-1]->ToString();

							switch(query->getColumnType(i))
							{
							case SQL_CHAR : 
							case SQL_VARCHAR : 
							case SQL_WCHAR:
							case SQL_WVARCHAR: {v_dtCol->DataType =Type::GetType("System.String"); break;}
							case SQL_SMALLINT: {v_dtCol->DataType =Type::GetType("System.Int16"); break;}
						    case SQL_INTEGER: {v_dtCol->DataType=System::Type::GetType("System.Int32"); break;}
							case SQL_BIGINT: {v_dtCol->DataType=System::Type::GetType("System.Int64"); break;}
							case SQL_REAL:
							case SQL_DOUBLE:
							case SQL_FLOAT: 
							case SQL_NUMERIC:
							case SQL_DECIMAL: {v_dtCol->DataType=System::Type::GetType("System.Double"); break;}
							case SQL_TIMESTAMP: {v_dtCol->DataType=System::Type::GetType("System.DateTime"); break;}
							case SQL_DATE: {v_dtCol->DataType=System::Type::GetType("System.DateTime"); break;}
							case SQL_TIME: {v_dtCol->DataType=System::Type::GetType("System.TimeSpan"); break;}
							}
							mv_ds->Tables[0]->Columns->Add(v_dtCol);
						}

						while(true)
						{

							if ( query->FetchNext(status) )
								break;
							
							v_dtRow=mv_ds->Tables[0]->NewRow();							
							for (int i = 1; i<= columns; i++ )
							{
								if (query->isColumnNull(i))
									switch(query->getColumnType(i))
									{
									case SQL_CHAR : 
									case SQL_VARCHAR : 
									case SQL_WCHAR:
									case SQL_WVARCHAR: {v_dtRow[i-1]=""; break;}
									case SQL_SMALLINT: 
									case SQL_INTEGER: 
									case SQL_BIGINT: 
									case SQL_REAL:
									case SQL_DOUBLE:
									case SQL_FLOAT: 
									case SQL_NUMERIC:
									case SQL_DECIMAL: {v_dtRow[i-1]=0; break;}
									case SQL_TIME:
									case SQL_DATE: 
									case SQL_TIMESTAMP: {v_dtRow[i-1]=System::DateTime::Now ; break;}
									}
								else
								switch(query->getColumnType(i))
								{
									case SQL_TINYINT:	{ char result; query->getColumn(i, &result); v_dtRow[i-1]=result; break; }
									case SQL_SMALLINT:	{ short int result; query->getColumn(i, &result); v_dtRow[i-1]=result; break;  }
									case SQL_INTEGER:	{ int result; query->getColumn(i, &result); v_dtRow[i-1]=result;break;  }
									case SQL_BIGINT:	{ __int64 result; query->getColumn(i, &result); v_dtRow[i-1]=result; break;  }
									case SQL_REAL:
									case SQL_FLOAT:     { float result; query->getColumn(i, &result); v_dtRow[i-1]=result; break;  }
									case SQL_NUMERIC:
									case SQL_DECIMAL:
									case SQL_DOUBLE:    { double result; query->getColumn(i, &result); v_dtRow[i-1]=result; break;  }
									case SQL_CHAR:
									case SQL_VARCHAR:   { char * result = new char[query->getColumnLength(i)+1]; query->getColumn(i, result); v_dtRow[i-1]=Marshal::PtrToStringAnsi((System::IntPtr)result); delete result; break; }
//									case SQL_VARCHAR:   { char * result; query->getColumn(i, &result); row[i-1] = Marshal::PtrToStringAnsi((System::IntPtr)result); break;  }
									case SQL_TIMESTAMP: { TIMESTAMP_STRUCT result;query->getColumn(i, &result); v_dtRow[i-1] = gcnew System::DateTime(result.year, result.month, result.day, result.hour, result.minute, result.second); break;  }
									case SQL_DATE:      { DATE_STRUCT result;query->getColumn(i, &result); v_dtRow[i-1]=gcnew System::DateTime(result.year, result.month, result.day); break; }
									case SQL_TIME:      { TIME_STRUCT result;query->getColumn(i, &result); v_dtRow[i-1]=gcnew System::DateTime(1,1,1,result.hour, result.minute, result.second);  break; }
									case SQL_WCHAR:
									case SQL_WVARCHAR:  
										{
											SQLWCHAR *result;
											result = query->columnP[i-1].wbufP;
											//result=query->getColumn(i, &result)
											v_dtRow[i-1]=Marshal::PtrToStringUni((System::IntPtr)result);
											//delete result;
											break;
										}
								}
							}
							mv_ds->Tables[0]->Rows->Add(v_dtRow);						
						}
						return mv_ds;
					}
					catch(TTWarning warning)
					{
						OnWarning(warning);
					}
					catch(TTError error)
					{
						OnError(error);
					}

					return nullptr;
				}


				void OnError(const TTStatus error)
				{
					try
					{
						TTStatus new_error;
						m_connection->Rollback(new_error);
					}
					catch(TTStatus&)
					{
					}

					try
					{
						TTStatus new_error;
						m_connection->Disconnect(new_error);					
					}
					catch(TTStatus&)
					{
					}

					if (m_connection)
						pool.freeConnection(m_connection);

					m_connection = NULL;
					m_lastError = new TTStatus(error);
					Console::WriteLine(LastError());
					throw gcnew System::Exception(LastError());
				}

				void OnWarning(TTStatus warning)
				{
					Console::WriteLine(Marshal::PtrToStringAnsi((System::IntPtr)warning.err_msg));
				}
			};
}
