using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Runtime.Serialization;

namespace Sats.Utils
{
    [DataContract]
    public class DataContainer
    {
        [IgnoreDataMember]
        public DataTable DataTable
        {
            get
            {
                if (DataTableContainer != null)
                {
                    var dt = new DataTable();

                    using (var sr = new StringReader(DataTableContainer))
                    {
                        dt.ReadXmlSchema(sr);
                    }

                    using (var sr = new StringReader(DataTableContainer))
                    {
                        dt.ReadXml(sr);
                        return dt;
                    }
                }
                return null;
            }
            set
            {
                if (value != null)
                {
                    var sw = new StringWriter();
                    value.TableName = "Result";
                    value.WriteXml(sw, XmlWriteMode.WriteSchema);
                    DataTableContainer = sw.ToString();
                }
            }
        }

        [IgnoreDataMember]
        public DataSet DataSet
        {
            get
            {
                if (DataSetContainer != null)
                {
                    var ds = new DataSet();

                    using (var sr = new StringReader(DataSetContainer))
                    {
                        ds.ReadXmlSchema(sr);
                    }

                    using (var sr = new StringReader(DataSetContainer))
                    {
                        ds.ReadXml(sr);
                        return ds;
                    }
                }
                return null;
            }
            set
            {
                if (value != null)
                {
                    var sw = new StringWriter();
                    value.WriteXml(sw, XmlWriteMode.WriteSchema);
                    DataSetContainer = sw.ToString();
                }
            }
        }

        [DataMember]
        public string DataTableContainer { get; set; }

        [DataMember]
        public string DataSetContainer { get; set; }         

        public void FillTable(DataTable resultTable)
        {
            if (DataTableContainer != null)
            {
                using (var sr = new StringReader(DataTableContainer))
                {
                    var tempResult = new DataTable();
                    tempResult.ReadXml(sr);
                    foreach(DataRow row in tempResult.Rows)
                    {
                        resultTable.ImportRow(row);
                    }
                    resultTable.AcceptChanges();
                }
            }
        }        
    }
}
