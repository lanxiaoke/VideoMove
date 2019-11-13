using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Video.Lib
{
    public enum Enums
    {
        Core = 0,
        HisAlarm = 1,
        RT = 2,
        EMS = 3,
        HisData = 4
    }


    public class DbConnectionFactory
    {
        private static readonly string coreDbConnectionString;
        private static readonly string coreDbDatabaseType;

        private static readonly string hisAlarmDbConnectionString;
        private static readonly string hisAlarmDbDatabaseType;

        private static readonly string rtDbConnectionString;
        private static readonly string rtDbDatabaseType;

        private static readonly string emsDbConnectionString;
        private static readonly string emsDbDatabaseType;

        private static readonly string hisDataDbConnectionString;
        private static readonly string hisDataDbDatabaseType;

        static DbConnectionFactory()
        {
            var coreCollection = ConfigurationManager.ConnectionStrings["CoreConnectionString"];
            if (coreCollection != null)
            {
                coreDbConnectionString = coreCollection.ConnectionString;
                coreDbDatabaseType = coreCollection.ProviderName.ToLower();
            }

            var hisAlarmCollection = ConfigurationManager.ConnectionStrings["HisAlarmConnectionString"];
            if (hisAlarmCollection != null)
            {
                hisAlarmDbConnectionString = hisAlarmCollection.ConnectionString;
                hisAlarmDbDatabaseType = hisAlarmCollection.ProviderName.ToLower();
            }

            var rtCollection = ConfigurationManager.ConnectionStrings["RTConnectionString"];
            if (rtCollection != null)
            {
                rtDbConnectionString = rtCollection.ConnectionString;
                rtDbDatabaseType = rtCollection.ProviderName.ToLower();
            }

            var emsCollection = ConfigurationManager.ConnectionStrings["EMSConnectionString"];
            if (emsCollection != null)
            {
                emsDbConnectionString = emsCollection.ConnectionString;
                emsDbDatabaseType = emsCollection.ProviderName.ToLower();
            }

            var hisDataCollection = ConfigurationManager.ConnectionStrings["HisDataConnectionString"];
            if (hisDataCollection != null)
            {
                hisDataDbConnectionString = hisDataCollection.ConnectionString;
                hisDataDbDatabaseType = hisDataCollection.ProviderName.ToLower();
            }
        }

        public static IDbConnection CreateConnection(Enums dbName)
        {
            switch (dbName)
            {
                case Enums.Core:
                    return CreateCoreDbConnection();
                case Enums.HisAlarm:
                    return CreateHisAlarmDbConnection();
                case Enums.RT:
                    return CreateRTDbConnection();
                case Enums.EMS:
                    return CreateEMSDbConnection();
                case Enums.HisData:
                    return CreateHisDataDbConnection();
                default:
                    return CreateCoreDbConnection();
            }
        }

        /// <summary>
        /// 历史告警数据库
        /// </summary>
        /// <returns></returns>
        private static IDbConnection CreateHisAlarmDbConnection()
        {
            return CreateConnection(hisAlarmDbConnectionString, hisAlarmDbDatabaseType);
        }

        /// <summary>
        /// 历史数据库
        /// </summary>
        /// <returns></returns>
        private static IDbConnection CreateHisDataDbConnection()
        {
            return CreateConnection(hisDataDbConnectionString, hisDataDbDatabaseType);
        }

        /// <summary>
        /// 常用数据库
        /// </summary>
        /// <returns></returns>
        private static IDbConnection CreateCoreDbConnection()
        {
            return CreateConnection(coreDbConnectionString, coreDbDatabaseType);
        }

        /// <summary>
        /// 常用数据库
        /// </summary>
        /// <returns></returns>
        private static IDbConnection CreateRTDbConnection()
        {
            return CreateConnection(rtDbConnectionString, rtDbDatabaseType);
        }

        private static IDbConnection CreateEMSDbConnection()
        {
            return CreateConnection(emsDbConnectionString, emsDbDatabaseType);
        }

        private static IDbConnection CreateConnection(string connectionString, string databaseType)
        {
            IDbConnection connection = new SqlConnection(connectionString);

            switch (databaseType)
            {
                case "system.data.sqlclient":
                    connection = new SqlConnection(connectionString);
                    break;
                case "mysql":
                    //connection = new MySql.Data.MySqlClient.MySqlConnection(connectionString);
                    break;
                case "oracle":
                    //connection = new Oracle.DataAccess.Client.OracleConnection(connectionString);
                    //connection = new System.Data.OracleClient.OracleConnection(connectionString);
                    break;
                case "db2":
                    //connection = new System.Data.OleDb.OleDbConnection(connectionString);
                    break;
            }
            return connection;
        }
    }
}
