using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Sats.CommonLibrary;
using Sats.DataAccessLayer;

namespace ReadHoseDataTool
{
    public static class DataUtils
    {
        public static void ReadASTDLFileToDB(string filePath, string tableName, string parentTxnum, string txDate, string busDate)
        {
            try
            {
                var v_stream = new System.IO.StreamReader(filePath);
                string v_strLine;
                string v_strConfirmNo, v_strMatch_Date, v_strMatch_Time, v_strSec_Code, v_strSET_TYPE;
                string v_strQty = ""; 
                string v_strPrice, v_strB_ACC_NO, v_strS_ACC_NO, v_strBlock_Tran;
                string v_strB_CODE_TRADE, v_strS_CODE_TRADE, v_strB_ORDER_NO, v_strS_ORDER_NO;
                string v_strB_PC_PLAG = ""; 
                string v_strS_PC_PLAG = "";
                string v_strStatus;

                v_strBlock_Tran = "0";
                v_strSET_TYPE = "3";
                var v_obj = new DataAccess();
                v_obj.NewDBInstance(modCommond.gc_MODULE_HOST);
                v_obj.ExecuteNonQuery(CommandType.Text, "UPDATE sysvar SET varvalue = '1' WHERE grname = 'SYSTEM' AND varname = 'SETT_UPD_ASTDL_STATUS' AND brid = '0002'");

                v_obj.ExecuteNonQuery(CommandType.Text, "TRUNCATE TABLE " + tableName);
                DataSet v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, "SELECT * FROM " + tableName + " WHERE 0=1");
                int v_int = 0;
                int v_intOffset = 0;
                string v_strBatchSize = "";
                int v_intBatchSize = 0;
                v_obj.GetSysVar("SYSTEM", "SETT_UPD_BATCH_SIZE", "0002", ref v_strBatchSize);
                if (v_strBatchSize == null)
                {
                    v_intBatchSize = 5000;
                }
                else
                {
                    v_intBatchSize = Convert.ToInt32(v_strBatchSize);
                }
                string v_strSettUpdType = "3.0";
                v_obj.GetSysVar("SYSTEM", "SETT_UPD_TYPE", "0002", ref v_strSettUpdType);

                while(!v_stream.EndOfStream)
                {
            
                    DataRow v_row = v_ds.Tables[0].NewRow();
                    v_row["COL_VALUE01"] = parentTxnum;
                    v_row["COL_TYPE01"] = "C";
                    v_row["COL_DESC01"] = parentTxnum;
                    v_row["COL_VALUE02"] = txDate;
                    v_row["COL_TYPE02"] = "D";
                    v_row["COL_DESC02"] = txDate;
                    v_row["COL_VALUE03"] = busDate;
                    v_row["COL_TYPE03"] = "D";
                    v_row["COL_DESC03"] = busDate;

                    v_strLine = v_stream.ReadLine();
                    if (v_strLine != "")
                    {
                        if (v_strSettUpdType == "3.0")
                        {
                            v_strStatus = v_strLine.Substring(89, 2).Trim();
                            if ((v_strStatus != "XC") && (v_strStatus != "XS") && (v_strStatus != ""))
                            { }
                            else
                            {
                                v_strConfirmNo = String.Format("{0:000000}", Convert.ToInt32(v_strLine.Substring(0, 6)));
                                v_row["COL_VALUE04"] = v_strConfirmNo.Trim();
                                v_row["COL_TYPE04"] = "C";
                                v_row["COL_DESC04"] = v_strConfirmNo.Trim();

                                v_strMatch_Time = v_strLine.Substring(54, 8);
                                v_row["COL_VALUE05"] = v_strMatch_Time.Trim();
                                v_row["COL_TYPE05"] = "C";
                                v_row["COL_DESC05"] = v_strMatch_Time.Trim();

                                v_strMatch_Date = v_strLine.Substring(62, 10);
                                v_row["COL_VALUE06"] = v_strMatch_Date;
                                v_row["COL_TYPE06"] = "C";
                                v_row["COL_DESC06"] = v_strMatch_Date;

                                v_strSec_Code = v_strLine.Substring(91, 8);
                                v_row["COL_VALUE07"] = v_strSec_Code.Trim();
                                v_row["COL_TYPE07"] = "C";
                                v_row["COL_DESC07"] = v_strSec_Code.Trim();

                                v_strPrice = (Convert.ToDouble(v_strLine.Substring(107, 9)) * 1000).ToString();
                                v_row["COL_VALUE08"] = v_strPrice.Trim();
                                v_row["COL_TYPE08"] = "N";
                                v_row["COL_DESC08"] = v_strPrice.Trim();

                                v_strQty = v_strLine.Substring(99, 8);
                                v_row["COL_VALUE09"] = v_strQty.Trim();
                                v_row["COL_TYPE09"] = "N";
                                v_row["COL_DESC09"] = v_strQty.Trim();

                                v_strB_ACC_NO = v_strLine.Substring(116, 10);
                                v_row["COL_VALUE10"] = v_strB_ACC_NO.Trim();
                                v_row["COL_TYPE10"] = "C";
                                v_row["COL_DESC10"] = v_strB_ACC_NO.Trim();

                                v_strS_ACC_NO = v_strLine.Substring(126, 10);
                                v_row["COL_VALUE11"] = v_strS_ACC_NO.Trim();
                                v_row["COL_TYPE11"] = "C";
                                v_row["COL_DESC11"] = v_strS_ACC_NO.Trim();

                                v_strB_CODE_TRADE = v_strLine.Substring(82, 3);
                                v_row["COL_VALUE14"] = v_strB_CODE_TRADE.Trim();
                                v_row["COL_TYPE14"] = "C";
                                v_row["COL_DESC14"] = v_strB_CODE_TRADE.Trim();

                                v_strS_CODE_TRADE = v_strLine.Substring(85, 3);
                                v_row["COL_VALUE15"] = v_strS_CODE_TRADE.Trim();
                                v_row["COL_TYPE15"] = "C";
                                v_row["COL_DESC15"] = v_strS_CODE_TRADE.Trim();

                                v_strB_ORDER_NO = v_strLine.Substring(6, 8);
                                v_row["COL_VALUE16"] = v_strB_ORDER_NO.Trim();
                                v_row["COL_TYPE16"] = "C";
                                v_row["COL_DESC16"] = v_strB_ORDER_NO.Trim();

                                v_strS_ORDER_NO = v_strLine.Substring(24, 8);
                                v_row["COL_VALUE17"] = v_strS_ORDER_NO.Trim();
                                v_row["COL_TYPE17"] = "C";
                                v_row["COL_DESC17"] = v_strS_ORDER_NO.Trim();

                                v_strB_PC_PLAG = v_strLine.Substring(80, 1);
                                v_row["COL_VALUE18"] = v_strB_PC_PLAG.Trim();
                                v_row["COL_TYPE18"] = "C";
                                v_row["COL_DESC18"] = v_strB_PC_PLAG.Trim();

                                v_strS_PC_PLAG = v_strLine.Substring(81, 1);
                                v_row["COL_VALUE19"] = v_strS_PC_PLAG.Trim();
                                v_row["COL_TYPE19"] = "C";
                                v_row["COL_DESC19"] = v_strS_PC_PLAG.Trim();

                                v_row["COL_VALUE12"] = v_strSET_TYPE.Trim();
                                v_row["COL_TYPE12"] = "N";
                                v_row["COL_DESC12"] = v_strSET_TYPE.Trim();

                                v_row["COL_VALUE13"] = v_strBlock_Tran.Trim();
                                v_row["COL_TYPE13"] = "N";
                                v_row["COL_DESC13"] = v_strBlock_Tran.Trim();

                                v_row["TLTXCD"] = "4084";
                                v_row["REAL_ROW"] = v_int;

                                v_ds.Tables[0].Rows.Add(v_row);
                                v_int = v_int + 1;
                                v_intOffset = v_intOffset + 1;
                                if (v_intOffset == 5000)
                                {
                                    if (v_obj.SaveUsingOracleBulkCopy(tableName, v_ds.Tables[0]))
                                    {
                                        v_intOffset = 0;
                                        v_ds.Tables[0].Rows.Clear();
                                    }
                                    else
                                    {
                                        throw new Exception("Error in SaveUsingOracleBulkCopy...");
                                    }
                                }
                            }
                        }
                        else if (v_strSettUpdType == "4.3")
                        {
                            //v_strStatus = v_strLine.Substring(89, 2).Trim();
                            v_strStatus = v_strLine.Substring(95, 2).Trim();
                            if ((v_strStatus != "XC") && (v_strStatus != "XS") && (v_strStatus != ""))
                            { }
                            else
                            {
                                //v_strConfirmNo = String.Format("{0:000000}", Convert.ToInt32(v_strLine.Substring(0, 6)));
                                v_strConfirmNo = String.Format("{0:000000000000}", Convert.ToInt64(v_strLine.Substring(0, 12)));
                                v_row["COL_VALUE04"] = v_strConfirmNo.Trim();
                                v_row["COL_TYPE04"] = "C";
                                v_row["COL_DESC04"] = v_strConfirmNo.Trim();

                                //v_strMatch_Time = v_strLine.Substring(54, 8);
                                v_strMatch_Time = v_strLine.Substring(60, 8);
                                v_row["COL_VALUE05"] = v_strMatch_Time.Trim();
                                v_row["COL_TYPE05"] = "C";
                                v_row["COL_DESC05"] = v_strMatch_Time.Trim();

                                //v_strMatch_Date = v_strLine.Substring(62, 10);
                                v_strMatch_Date = v_strLine.Substring(68, 10);
                                v_row["COL_VALUE06"] = v_strMatch_Date;
                                v_row["COL_TYPE06"] = "C";
                                v_row["COL_DESC06"] = v_strMatch_Date;

                                //v_strSec_Code = v_strLine.Substring(91, 8);
                                v_strSec_Code = v_strLine.Substring(97, 8);
                                v_row["COL_VALUE07"] = v_strSec_Code.Trim();
                                v_row["COL_TYPE07"] = "C";
                                v_row["COL_DESC07"] = v_strSec_Code.Trim();

                                //v_strPrice = (Convert.ToDouble(v_strLine.Substring(107, 9)) * 1000).ToString();
                                v_strPrice = (Convert.ToDouble(v_strLine.Substring(113, 9)) * 1000).ToString();
                                v_row["COL_VALUE08"] = v_strPrice.Trim();
                                v_row["COL_TYPE08"] = "N";
                                v_row["COL_DESC08"] = v_strPrice.Trim();

                                //v_strQty = v_strLine.Substring(99, 8);
                                v_strQty = v_strLine.Substring(105, 8);
                                v_row["COL_VALUE09"] = v_strQty.Trim();
                                v_row["COL_TYPE09"] = "N";
                                v_row["COL_DESC09"] = v_strQty.Trim();

                                //v_strB_ACC_NO = v_strLine.Substring(116, 10);
                                v_strB_ACC_NO = v_strLine.Substring(122, 10);
                                v_row["COL_VALUE10"] = v_strB_ACC_NO.Trim();
                                v_row["COL_TYPE10"] = "C";
                                v_row["COL_DESC10"] = v_strB_ACC_NO.Trim();

                                //v_strS_ACC_NO = v_strLine.Substring(126, 10);
                                v_strS_ACC_NO = v_strLine.Substring(132, 10);
                                v_row["COL_VALUE11"] = v_strS_ACC_NO.Trim();
                                v_row["COL_TYPE11"] = "C";
                                v_row["COL_DESC11"] = v_strS_ACC_NO.Trim();

                                //v_strB_CODE_TRADE = v_strLine.Substring(82, 3);
                                v_strB_CODE_TRADE = v_strLine.Substring(88, 3);
                                v_row["COL_VALUE14"] = v_strB_CODE_TRADE.Trim();
                                v_row["COL_TYPE14"] = "C";
                                v_row["COL_DESC14"] = v_strB_CODE_TRADE.Trim();

                                //v_strS_CODE_TRADE = v_strLine.Substring(85, 3);
                                v_strS_CODE_TRADE = v_strLine.Substring(91, 3);
                                v_row["COL_VALUE15"] = v_strS_CODE_TRADE.Trim();
                                v_row["COL_TYPE15"] = "C";
                                v_row["COL_DESC15"] = v_strS_CODE_TRADE.Trim();

                                //v_strB_ORDER_NO = v_strLine.Substring(6, 8);
                                v_strB_ORDER_NO = v_strLine.Substring(12, 8);
                                v_row["COL_VALUE16"] = v_strB_ORDER_NO.Trim();
                                v_row["COL_TYPE16"] = "C";
                                v_row["COL_DESC16"] = v_strB_ORDER_NO.Trim();

                                //v_strS_ORDER_NO = v_strLine.Substring(24, 8);
                                v_strS_ORDER_NO = v_strLine.Substring(30, 8);
                                v_row["COL_VALUE17"] = v_strS_ORDER_NO.Trim();
                                v_row["COL_TYPE17"] = "C";
                                v_row["COL_DESC17"] = v_strS_ORDER_NO.Trim();

                                //v_strB_PC_PLAG = v_strLine.Substring(80, 1);
                                v_strB_PC_PLAG = v_strLine.Substring(86, 1);
                                v_row["COL_VALUE18"] = v_strB_PC_PLAG.Trim();
                                v_row["COL_TYPE18"] = "C";
                                v_row["COL_DESC18"] = v_strB_PC_PLAG.Trim();

                                //v_strS_PC_PLAG = v_strLine.Substring(81, 1);
                                v_strS_PC_PLAG = v_strLine.Substring(87, 1);
                                v_row["COL_VALUE19"] = v_strS_PC_PLAG.Trim();
                                v_row["COL_TYPE19"] = "C";
                                v_row["COL_DESC19"] = v_strS_PC_PLAG.Trim();

                                v_row["COL_VALUE12"] = v_strSET_TYPE.Trim();
                                v_row["COL_TYPE12"] = "N";
                                v_row["COL_DESC12"] = v_strSET_TYPE.Trim();

                                v_row["COL_VALUE13"] = v_strBlock_Tran.Trim();
                                v_row["COL_TYPE13"] = "N";
                                v_row["COL_DESC13"] = v_strBlock_Tran.Trim();

                                v_row["TLTXCD"] = "4084";
                                v_row["REAL_ROW"] = v_int;

                                v_ds.Tables[0].Rows.Add(v_row);
                                v_int = v_int + 1;
                                v_intOffset = v_intOffset + 1;
                                if (v_intOffset == 5000)
                                {
                                    if (v_obj.SaveUsingOracleBulkCopy(tableName, v_ds.Tables[0]))
                                    {
                                        v_intOffset = 0;
                                        v_ds.Tables[0].Rows.Clear();
                                    }
                                    else
                                    {
                                        throw new Exception("Error in SaveUsingOracleBulkCopy...");
                                    }
                                }
                            }
                        }
                    }
                }
                if (v_ds.Tables[0].Rows.Count > 0) 
                {
                    var result = v_obj.SaveUsingOracleBulkCopy(tableName, v_ds.Tables[0]);
                    if (!result)
                    {
                        throw new Exception("Error in SaveUsingOracleBulkCopy...");
                    }
                }
                v_obj.ExecuteNonQuery(CommandType.Text, "UPDATE sysvar SET varvalue = '3' WHERE grname = 'SYSTEM' AND varname = 'SETT_UPD_ASTDL_STATUS' AND brid = '0002'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void ReadASTPTFileToDB(string filePath, string tableName, string parentTxnum, string txDate, string busDate)
        {
            try
            {
                var v_stream = new System.IO.StreamReader(filePath);
                string v_strLine;
                string strRows = "";
                string v_strConfirmNo, v_strMatch_Date, v_strMatch_Time, v_strSec_Code, v_strSET_TYPE;
                string v_strQty = "";
                string v_strPrice, v_strB_ACC_NO, v_strS_ACC_NO, v_strBlock_Tran;
                string v_strB_CODE_TRADE, v_strS_CODE_TRADE, v_strB_ORDER_NO, v_strS_ORDER_NO;
                string v_strB_PC_PLAG = "";
                string v_strS_PC_PLAG = "";
                string v_strStatus;

                v_strBlock_Tran = "1";
                v_strSET_TYPE = "3";
                var v_obj = new DataAccess();
                v_obj.NewDBInstance(modCommond.gc_MODULE_HOST);
                v_obj.ExecuteNonQuery(CommandType.Text, "UPDATE sysvar SET varvalue = '1' WHERE grname = 'SYSTEM' AND varname = 'SETT_UPD_ASTPT_STATUS' AND brid = '0002'");
                v_obj.ExecuteNonQuery(CommandType.Text, "TRUNCATE TABLE txfields_astpt");
                DataSet v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, "SELECT * FROM txfields_astpt WHERE 0=1");

                int v_int = 0;
                int v_intOffset = 0;
                string v_strBatchSize = "";
                int v_intBatchSize = 0;
                v_obj.GetSysVar("SYSTEM", "SETT_UPD_BATCH_SIZE", "0002", ref v_strBatchSize);
                if (v_strBatchSize == null)
                {
                    v_intBatchSize = 5000;
                }
                else
                {
                    v_intBatchSize = Convert.ToInt32(v_strBatchSize);
                }
                string v_strSettUpdType = "3.0";
                v_obj.GetSysVar("SYSTEM", "SETT_UPD_TYPE", "0002", ref v_strSettUpdType);

                while (!v_stream.EndOfStream)
                {
                    v_strLine = v_stream.ReadLine();
                    //err 1
                    if (v_strLine != "")
                    {
                        if (v_strSettUpdType == "3.0")
                        {
                            v_strStatus = v_strLine.Substring(39, 2).Trim();
                            if (v_strStatus != "XC" && v_strStatus != "XS" && v_strStatus != "")
                            { }
                            else
                            {
                                DataRow v_row = v_ds.Tables[0].NewRow();
                                v_row["COL_VALUE01"] = parentTxnum;
                                v_row["COL_TYPE01"] = "C";
                                v_row["COL_DESC01"] = parentTxnum;
                                v_row["COL_VALUE02"] = txDate;
                                v_row["COL_TYPE02"] = "D";
                                v_row["COL_DESC02"] = txDate;
                                v_row["COL_VALUE03"] = busDate;
                                v_row["COL_TYPE03"] = "D";
                                v_row["COL_DESC03"] = busDate;
                                //bangpv: check loi
                                //v_strErrMsg = "Loi tai: 'err 1: If v_strLine <> "" Then (dong:" & v_int & ")";
                                //end bangpv
                                v_strConfirmNo = String.Format("{0:900000}", Convert.ToInt32(v_strLine.Substring(0, 6)));
                                v_row["COL_VALUE04"] = v_strConfirmNo.Trim();
                                v_row["COL_TYPE04"] = "C";
                                v_row["COL_DESC04"] = v_strConfirmNo.Trim();

                                v_strMatch_Time = v_strLine.Substring(6, 8);
                                v_row["COL_VALUE05"] = v_strMatch_Time.Trim();
                                v_row["COL_TYPE05"] = "C";
                                v_row["COL_DESC05"] = v_strMatch_Time.Trim();

                                v_strMatch_Date = v_strLine.Substring(14, 10);
                                v_row["COL_VALUE06"] = v_strMatch_Date.Trim();
                                v_row["COL_TYPE06"] = "D";
                                v_row["COL_DESC06"] = v_strMatch_Date.Trim();

                                v_strSec_Code = v_strLine.Substring(41, 8);
                                v_row["COL_VALUE07"] = v_strSec_Code.Trim();
                                v_row["COL_TYPE07"] = "C";
                                v_row["COL_DESC07"] = v_strSec_Code.Trim();

                                v_strPrice = (Convert.ToDouble(v_strLine.Substring(49, 13)) * 1000).ToString();
                                v_row["COL_VALUE08"] = v_strPrice.Trim();
                                v_row["COL_TYPE08"] = "N";
                                v_row["COL_DESC08"] = v_strPrice.Trim();

                                if (Convert.ToDouble("0" + v_strLine.Substring(95, 8).Trim()) > 0)
                                {
                                    v_strQty = v_strLine.Substring(95, 8);
                                    v_strB_PC_PLAG = "P";
                                }
                                else if (Convert.ToDouble("0" + v_strLine.Substring(103, 8).Trim()) > 0)
                                {
                                    v_strQty = v_strLine.Substring(103, 8);
                                    v_strB_PC_PLAG = "C";
                                }
                                else if (Convert.ToDouble("0" + v_strLine.Substring(111, 8).Trim()) > 0)
                                {
                                    v_strQty = v_strLine.Substring(111, 8);
                                    v_strB_PC_PLAG = "M";
                                }
                                else if (Convert.ToDouble("0" + v_strLine.Substring(119, 8).Trim()) > 0)
                                {
                                    v_strQty = v_strLine.Substring(119, 8);
                                    v_strB_PC_PLAG = "F";
                                }

                                v_row["COL_VALUE09"] = v_strQty.Trim();
                                v_row["COL_TYPE09"] = "N";
                                v_row["COL_DESC09"] = v_strQty.Trim();

                                v_row["COL_VALUE18"] = v_strB_PC_PLAG.Trim();
                                v_row["COL_TYPE18"] = "C";
                                v_row["COL_DESC18"] = v_strB_PC_PLAG.Trim();

                                if (Convert.ToDouble("0" + v_strLine.Substring(159, 8).Trim()) > 0)
                                {
                                    v_strS_PC_PLAG = "P";
                                }
                                else if (Convert.ToDouble("0" + v_strLine.Substring(167, 8).Trim()) > 0)
                                {
                                    v_strS_PC_PLAG = "C";
                                }
                                else if (Convert.ToDouble("0" + v_strLine.Substring(175, 8).Trim()) > 0)
                                {
                                    v_strS_PC_PLAG = "M";
                                }
                                else if (Convert.ToDouble("0" + v_strLine.Substring(183, 8).Trim()) > 0)
                                {
                                    v_strS_PC_PLAG = "F";
                                }
                                v_row["COL_VALUE19"] = v_strS_PC_PLAG.Trim();
                                v_row["COL_TYPE19"] = "C";
                                v_row["COL_DESC19"] = v_strS_PC_PLAG.Trim();

                                v_strB_ACC_NO = v_strLine.Substring(62, 10);
                                v_row["COL_VALUE10"] = v_strB_ACC_NO.Trim();
                                v_row["COL_TYPE10"] = "C";
                                v_row["COL_DESC10"] = v_strB_ACC_NO.Trim();

                                v_strS_ACC_NO = v_strLine.Substring(72, 10);
                                v_row["COL_VALUE11"] = v_strS_ACC_NO.Trim();
                                v_row["COL_TYPE11"] = "C";
                                v_row["COL_DESC11"] = v_strS_ACC_NO.Trim();

                                v_strB_CODE_TRADE = v_strLine.Substring(32, 3);
                                v_row["COL_VALUE14"] = v_strB_CODE_TRADE.Trim();
                                v_row["COL_TYPE14"] = "C";
                                v_row["COL_DESC14"] = v_strB_CODE_TRADE.Trim();

                                v_strS_CODE_TRADE = v_strLine.Substring(35, 3);
                                v_row["COL_VALUE15"] = v_strS_CODE_TRADE.Trim();
                                v_row["COL_TYPE15"] = "C";
                                v_row["COL_DESC15"] = v_strS_CODE_TRADE.Trim();


                                v_strB_ORDER_NO = v_strConfirmNo;
                                v_row["COL_VALUE16"] = v_strB_ORDER_NO.Trim();
                                v_row["COL_TYPE16"] = "C";
                                v_row["COL_DESC16"] = v_strB_ORDER_NO.Trim();

                                v_strS_ORDER_NO = v_strConfirmNo;
                                v_row["COL_VALUE17"] = v_strS_ORDER_NO.Trim();
                                v_row["COL_TYPE17"] = "C";
                                v_row["COL_DESC17"] = v_strS_ORDER_NO.Trim();

                                v_row["COL_VALUE12"] = v_strSET_TYPE.Trim();
                                v_row["COL_TYPE12"] = "N";
                                v_row["COL_DESC12"] = v_strSET_TYPE.Trim();

                                v_row["COL_VALUE13"] = v_strBlock_Tran.Trim();
                                v_row["COL_TYPE13"] = "N";
                                v_row["COL_DESC13"] = v_strBlock_Tran.Trim();

                                v_row["TLTXCD"] = "4084";
                                v_row["REAL_ROW"] = v_int;

                                v_ds.Tables[0].Rows.Add(v_row);
                                v_int = v_int + 1;
                                v_intOffset = v_intOffset + 1;
                                if (v_intOffset == 5000)
                                {
                                    if (v_obj.SaveUsingOracleBulkCopy(tableName, v_ds.Tables[0]))
                                    {
                                        v_intOffset = 0;
                                        v_ds.Tables[0].Rows.Clear();
                                    }
                                    else
                                    {
                                        throw new Exception("Error in SaveUsingOracleBulkCopy...");
                                    }
                                }
                            }
                        }
                        else if (v_strSettUpdType == "4.3")
                        {
                            //v_strStatus = v_strLine.Substring(39, 2).Trim();
                            v_strStatus = v_strLine.Substring(45, 2).Trim();
                            if (v_strStatus != "XC" && v_strStatus != "XS" && v_strStatus != "")
                            { }
                            else
                            {
                                DataRow v_row = v_ds.Tables[0].NewRow();
                                v_row["COL_VALUE01"] = parentTxnum;
                                v_row["COL_TYPE01"] = "C";
                                v_row["COL_DESC01"] = parentTxnum;
                                v_row["COL_VALUE02"] = txDate;
                                v_row["COL_TYPE02"] = "D";
                                v_row["COL_DESC02"] = txDate;
                                v_row["COL_VALUE03"] = busDate;
                                v_row["COL_TYPE03"] = "D";
                                v_row["COL_DESC03"] = busDate;
                                //bangpv: check loi
                                //v_strErrMsg = "Loi tai: 'err 1: If v_strLine <> "" Then (dong:" & v_int & ")";
                                //end bangpv
                                //v_strConfirmNo = String.Format("{0:900000}", Convert.ToInt32(v_strLine.Substring(0, 6)));
                                v_strConfirmNo = String.Format("{0:900000000000}", Convert.ToInt64(v_strLine.Substring(0, 12)));
                                v_row["COL_VALUE04"] = v_strConfirmNo.Trim();
                                v_row["COL_TYPE04"] = "C";
                                v_row["COL_DESC04"] = v_strConfirmNo.Trim();

                                //v_strMatch_Time = v_strLine.Substring(6, 8);
                                v_strMatch_Time = v_strLine.Substring(12, 8);
                                v_row["COL_VALUE05"] = v_strMatch_Time.Trim();
                                v_row["COL_TYPE05"] = "C";
                                v_row["COL_DESC05"] = v_strMatch_Time.Trim();

                                //v_strMatch_Date = v_strLine.Substring(14, 10);
                                v_strMatch_Date = v_strLine.Substring(20, 10);
                                v_row["COL_VALUE06"] = v_strMatch_Date.Trim();
                                v_row["COL_TYPE06"] = "D";
                                v_row["COL_DESC06"] = v_strMatch_Date.Trim();

                                //v_strSec_Code = v_strLine.Substring(41, 8);
                                v_strSec_Code = v_strLine.Substring(47, 8);
                                v_row["COL_VALUE07"] = v_strSec_Code.Trim();
                                v_row["COL_TYPE07"] = "C";
                                v_row["COL_DESC07"] = v_strSec_Code.Trim();

                                //v_strPrice = (Convert.ToDouble(v_strLine.Substring(49, 13)) * 1000).ToString();
                                v_strPrice = (Convert.ToDouble(v_strLine.Substring(55, 13)) * 1000).ToString();
                                v_row["COL_VALUE08"] = v_strPrice.Trim();
                                v_row["COL_TYPE08"] = "N";
                                v_row["COL_DESC08"] = v_strPrice.Trim();

                                //if (Convert.ToDouble("0" + v_strLine.Substring(95, 8).Trim()) > 0)
                                //{
                                //    v_strQty = v_strLine.Substring(95, 8);
                                //    v_strB_PC_PLAG = "P";
                                //}
                                //else if (Convert.ToDouble("0" + v_strLine.Substring(103, 8).Trim()) > 0)
                                //{
                                //    v_strQty = v_strLine.Substring(103, 8);
                                //    v_strB_PC_PLAG = "C";
                                //}
                                //else if (Convert.ToDouble("0" + v_strLine.Substring(111, 8).Trim()) > 0)
                                //{
                                //    v_strQty = v_strLine.Substring(111, 8);
                                //    v_strB_PC_PLAG = "M";
                                //}
                                //else if (Convert.ToDouble("0" + v_strLine.Substring(119, 8).Trim()) > 0)
                                //{
                                //    v_strQty = v_strLine.Substring(119, 8);
                                //    v_strB_PC_PLAG = "F";
                                //}
                                if (Convert.ToDouble("0" + v_strLine.Substring(101, 8).Trim()) > 0)
                                {
                                    v_strQty = v_strLine.Substring(101, 8);
                                    v_strB_PC_PLAG = "P";
                                }
                                else if (Convert.ToDouble("0" + v_strLine.Substring(109, 8).Trim()) > 0)
                                {
                                    v_strQty = v_strLine.Substring(109, 8);
                                    v_strB_PC_PLAG = "C";
                                }
                                else if (Convert.ToDouble("0" + v_strLine.Substring(117, 8).Trim()) > 0)
                                {
                                    v_strQty = v_strLine.Substring(117, 8);
                                    v_strB_PC_PLAG = "M";
                                }
                                else if (Convert.ToDouble("0" + v_strLine.Substring(125, 8).Trim()) > 0)
                                {
                                    v_strQty = v_strLine.Substring(125, 8);
                                    v_strB_PC_PLAG = "F";
                                }

                                v_row["COL_VALUE09"] = v_strQty.Trim();
                                v_row["COL_TYPE09"] = "N";
                                v_row["COL_DESC09"] = v_strQty.Trim();

                                v_row["COL_VALUE18"] = v_strB_PC_PLAG.Trim();
                                v_row["COL_TYPE18"] = "C";
                                v_row["COL_DESC18"] = v_strB_PC_PLAG.Trim();

                                //if (Convert.ToDouble("0" + v_strLine.Substring(159, 8).Trim()) > 0)
                                //{
                                //    v_strS_PC_PLAG = "P";
                                //}
                                //else if (Convert.ToDouble("0" + v_strLine.Substring(167, 8).Trim()) > 0)
                                //{
                                //    v_strS_PC_PLAG = "C";
                                //}
                                //else if (Convert.ToDouble("0" + v_strLine.Substring(175, 8).Trim()) > 0)
                                //{
                                //    v_strS_PC_PLAG = "M";
                                //}
                                //else if (Convert.ToDouble("0" + v_strLine.Substring(183, 8).Trim()) > 0)
                                //{
                                //    v_strS_PC_PLAG = "F";
                                //}
                                if (Convert.ToDouble("0" + v_strLine.Substring(165, 8).Trim()) > 0)
                                {
                                    v_strS_PC_PLAG = "P";
                                }
                                else if (Convert.ToDouble("0" + v_strLine.Substring(173, 8).Trim()) > 0)
                                {
                                    v_strS_PC_PLAG = "C";
                                }
                                else if (Convert.ToDouble("0" + v_strLine.Substring(181, 8).Trim()) > 0)
                                {
                                    v_strS_PC_PLAG = "M";
                                }
                                else if (Convert.ToDouble("0" + v_strLine.Substring(189, 8).Trim()) > 0)
                                {
                                    v_strS_PC_PLAG = "F";
                                }
                                v_row["COL_VALUE19"] = v_strS_PC_PLAG.Trim();
                                v_row["COL_TYPE19"] = "C";
                                v_row["COL_DESC19"] = v_strS_PC_PLAG.Trim();

                                //v_strB_ACC_NO = v_strLine.Substring(62, 10);
                                v_strB_ACC_NO = v_strLine.Substring(68, 10);
                                v_row["COL_VALUE10"] = v_strB_ACC_NO.Trim();
                                v_row["COL_TYPE10"] = "C";
                                v_row["COL_DESC10"] = v_strB_ACC_NO.Trim();

                                //v_strS_ACC_NO = v_strLine.Substring(72, 10);
                                v_strS_ACC_NO = v_strLine.Substring(78, 10);
                                v_row["COL_VALUE11"] = v_strS_ACC_NO.Trim();
                                v_row["COL_TYPE11"] = "C";
                                v_row["COL_DESC11"] = v_strS_ACC_NO.Trim();

                                //v_strB_CODE_TRADE = v_strLine.Substring(32, 3);
                                v_strB_CODE_TRADE = v_strLine.Substring(38, 3);
                                v_row["COL_VALUE14"] = v_strB_CODE_TRADE.Trim();
                                v_row["COL_TYPE14"] = "C";
                                v_row["COL_DESC14"] = v_strB_CODE_TRADE.Trim();

                                //v_strS_CODE_TRADE = v_strLine.Substring(35, 3);
                                v_strS_CODE_TRADE = v_strLine.Substring(41, 3);
                                v_row["COL_VALUE15"] = v_strS_CODE_TRADE.Trim();
                                v_row["COL_TYPE15"] = "C";
                                v_row["COL_DESC15"] = v_strS_CODE_TRADE.Trim();


                                v_strB_ORDER_NO = v_strConfirmNo;
                                v_row["COL_VALUE16"] = v_strB_ORDER_NO.Trim();
                                v_row["COL_TYPE16"] = "C";
                                v_row["COL_DESC16"] = v_strB_ORDER_NO.Trim();

                                v_strS_ORDER_NO = v_strConfirmNo;
                                v_row["COL_VALUE17"] = v_strS_ORDER_NO.Trim();
                                v_row["COL_TYPE17"] = "C";
                                v_row["COL_DESC17"] = v_strS_ORDER_NO.Trim();

                                v_row["COL_VALUE12"] = v_strSET_TYPE.Trim();
                                v_row["COL_TYPE12"] = "N";
                                v_row["COL_DESC12"] = v_strSET_TYPE.Trim();

                                v_row["COL_VALUE13"] = v_strBlock_Tran.Trim();
                                v_row["COL_TYPE13"] = "N";
                                v_row["COL_DESC13"] = v_strBlock_Tran.Trim();

                                v_row["TLTXCD"] = "4084";
                                v_row["REAL_ROW"] = v_int;

                                v_ds.Tables[0].Rows.Add(v_row);
                                v_int = v_int + 1;
                                v_intOffset = v_intOffset + 1;
                                if (v_intOffset == v_intBatchSize)
                                {
                                    if (v_obj.SaveUsingOracleBulkCopy(tableName, v_ds.Tables[0]))
                                    {
                                        v_intOffset = 0;
                                        v_ds.Tables[0].Rows.Clear();
                                    }
                                    else
                                    {
                                        throw new Exception("Error in SaveUsingOracleBulkCopy...");
                                    }
                                }
                            }
                        }
                        
                    }
                }
                if (v_ds.Tables[0].Rows.Count > 0) 
                {
                    var result = v_obj.SaveUsingOracleBulkCopy(tableName, v_ds.Tables[0]);
                    if (!result)
                    {
                        throw new Exception("Error in SaveUsingOracleBulkCopy...");
                    }
                }
                v_obj.ExecuteNonQuery(CommandType.Text, "UPDATE sysvar SET varvalue = '3' WHERE grname = 'SYSTEM' AND varname = 'SETT_UPD_ASTPT_STATUS' AND brid = '0002'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
