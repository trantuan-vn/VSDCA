using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Sats.CommonLibrary;
using Sats.DataAccessLayer;
using System.Threading;

namespace ReadHoseDataTool
{
    class Program
    {
        static void Main(string[] args)
        {
            log4net.Config.BasicConfigurator.Configure();
            log4net.ILog logger = log4net.LogManager.GetLogger(typeof (Program));
            try
            {
                Assembly asb; 
                AssemblyName assamblyName = AssemblyName.GetAssemblyName(AppDomain.CurrentDomain.BaseDirectory + "Sats.DBConfig.dll");
                AppDomain myDomain = AppDomain.CreateDomain("HOST");
                asb = myDomain.Load(assamblyName);
                IDBConfig dbConfig = (asb.CreateInstance("Sats.DBConfig.DBConfig") as IDBConfig);
                GlobalDataConfig.HOST_DBCONFIG = dbConfig.GetHostConfig();
                GlobalDataConfig.INQUERY_DBCONFIG = dbConfig.GetInQueryConfig();
                AppDomain.Unload(myDomain);

                if (args.Length >= 5)
                {
                    string filePath = args[0].ToString();
                    string tableName = args[1].ToString();
                    string parentTxnum = args[2].ToString();
                    string txDate = args[3].ToString();
                    string busDate = args[4].ToString();
                    Thread.Sleep(10000);
                    logger.Info("Start read filePath=" + filePath + ", tableName=" + tableName + ", parentYxnum=" + parentTxnum + ", txDate=" + txDate + ", busDate=" + busDate);
                    if (tableName.ToUpper() == "TXFIELDS_ASTDL")
                    {
                        DataUtils.ReadASTDLFileToDB(filePath, tableName, parentTxnum, txDate, busDate);
                    }
                    else if (tableName.ToUpper() == "TXFIELDS_ASTPT")
                    {
                        DataUtils.ReadASTPTFileToDB(filePath, tableName, parentTxnum, txDate, busDate);
                    }
                    logger.Info("Finish read filePath=" + filePath + ", parentYxnum=" + parentTxnum + ", txDate=" + txDate + ", busDate=" + busDate);
                    Console.WriteLine("OK");
                }
                else
                {
                    Console.WriteLine("ReadHoseDataTool:Wrong parameters count...");
                    logger.Error("Wrong parameters count...");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ReadHoseDataTool" + ex.Message);
                logger.Error(ex);
            }
        }
    }
}
