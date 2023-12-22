using DevExpress.Xpo.DB.Exceptions;
using DevExpress.Xpo.DB.Helpers;
using DevExpress.Xpo.DB;
using DevExpress.XtraRichEdit.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using NpgsqlTypes;

namespace MyTest.Module
{
    public class PostgreSqlConnectionProviderEx : PostgreSqlConnectionProvider
    {
        public new const string XpoProviderTypeString = "PostgreSql";
        public PostgreSqlConnectionProviderEx(IDbConnection connection, AutoCreateOption autoCreateOption) : base(connection, AutoCreateOption.DatabaseAndSchema) { }
        public override string ComposeSafeColumnName(string columnName)
        {
            return base.ComposeSafeColumnName(columnName).ToLower(Thread.CurrentThread.CurrentCulture);
        }
        public override string ComposeSafeTableName(string tableName)
        {
            return base.ComposeSafeTableName(tableName).ToLower(Thread.CurrentThread.CurrentCulture);
        }
        public override string ComposeSafeConstraintName(string constraintName)
        {
            return base.ComposeSafeConstraintName(constraintName).ToLower(Thread.CurrentThread.CurrentCulture);
        }

        public override string ComposeSafeSchemaName(string tableName)
        {
            return base.ComposeSafeSchemaName(tableName).ToLower(Thread.CurrentThread.CurrentCulture);
        }

        protected override string GetPrimaryKeyName(DBPrimaryKey cons, DBTable table)
        {
            return base.GetPrimaryKeyName(cons, table);
        }

        public new static IDataStore CreateProviderFromString(string connectionString, AutoCreateOption autoCreateOption, out IDisposable[] objectsToDisposeOnDisconnect)
        {
            IDbConnection connection = CreateConnection(connectionString);
            objectsToDisposeOnDisconnect = new IDisposable[] { connection };
            return CreateProviderFromConnection(connection, autoCreateOption);
        }
        public new static IDataStore CreateProviderFromConnection(IDbConnection connection, AutoCreateOption autoCreateOption)
        {
            return new PostgreSqlConnectionProviderEx(connection, autoCreateOption);
        }

        protected override IDbCommand CreateCommand(Query query)
        {
            IDbCommand command = CreateCommand();
            command.CommandText = query.Sql;
            PrepareParameters(command, query);
#if !CF
            Trace.WriteLineIf(xpoSwitch.TraceInfo, new DbCommandTracer(command));
#endif
            return command;
        }

        protected override IDataParameter CreateParameter(IDbCommand command, object value, string name, DBColumnType dbType, string dbTypeName, int size)
        {
            NpgsqlParameter param = (NpgsqlParameter)base.CreateParameter(command, value, name, dbType, dbTypeName, size);
            if (param.DbType == DbType.String && dbTypeName == null)
                param.NpgsqlDbType = NpgsqlDbType.Citext;
            return param;
        }

        protected override string GetSqlCreateColumnTypeForString(DBTable table, DBColumn column)
        {
            return "citext";
        }

        //public override string FormatFunction(FunctionOperatorType operatorType, params string[] operands)
        //{
        //    if (operatorType == FunctionOperatorType.Contains)
        //    {
        //        return string.Format(CultureInfo.InvariantCulture, "(Strpos(Lower({0}), Lower({1})) > 0)", operands[0], operands[1]);
        //    }
        //    return base.FormatFunction(operatorType, operands);
        //}
        public static void Register(int logSwitch) //xpo sql queries logging mode
        {
            Register();

            FieldInfo xpoSwitchF = typeof(ConnectionProviderSql).GetField("xpoSwitch", BindingFlags.Static | BindingFlags.NonPublic);
            TraceSwitch xpoSwitch = (TraceSwitch)xpoSwitchF.GetValue(null);
            try
            {
                xpoSwitch.Level = (TraceLevel)logSwitch;
            }
            catch (Exception) { }
        }

        public new static void Register()
        {
            RegisterDataStoreProvider(XpoProviderTypeString, new DataStoreCreationFromStringDelegate(CreateProviderFromString));
            RegisterDataStoreProvider("NpgsqlConnection", new DataStoreCreationFromConnectionDelegate(CreateProviderFromConnection));
            RegisterFactory(new PostgreSqlProviderFactoryEx());
        }

        private const string NpgsqlExceptionTypeName = "Npgsql.NpgsqlException";
        private const string PostgresExceptionTypeName = "Npgsql.PostgresException";
        ReflectConnectionHelper helper;
        Version npgSqlVersion;
        bool SupportNpgsqlVersion(int major, int minor, int build)
        {
            if (npgSqlVersion == null)
            {
                npgSqlVersion = Connection.GetType().Assembly.GetName().Version;
            }
            if (npgSqlVersion.Major == major)
            {
                if (npgSqlVersion.Minor == minor)
                {
                    return npgSqlVersion.Build >= build;
                }
                return npgSqlVersion.Minor >= minor;
            }
            return npgSqlVersion.Major > major;
        }

        ReflectConnectionHelper ConnectionHelper
        {
            get
            {
                if (helper == null)
                {
                    if (SupportNpgsqlVersion(3, 2, 0))
                    {
                        helper = new ReflectConnectionHelper(Connection, NpgsqlExceptionTypeName, PostgresExceptionTypeName);
                    }
                    else
                    {
                        helper = new ReflectConnectionHelper(Connection, NpgsqlExceptionTypeName);
                    }
                }
                return helper;
            }
        }
        protected override void CreateDataBase()
        {
            const string CannotOpenDatabaseError = "3D000";
            try
            {
                Connection.Open();
            }
            catch (Exception e)
            {
                object[] propertiesValues;
                if (ConnectionHelper.TryGetExceptionProperties(e, new string[] { "Errors", "Code" }, out propertiesValues)
                    && (
                    (propertiesValues != null && propertiesValues.Length > 1 && ((string)propertiesValues[1]) == CannotOpenDatabaseError)
                    ||
                    (((IList)propertiesValues[0]) != null && ((IList)propertiesValues[0]).Count > 0 && ((string)propertiesValues[1]) == CannotOpenDatabaseError)
                    )
                    && CanCreateDatabase)
                {
                    ConnectionStringParser helper = new ConnectionStringParser(ConnectionString);
                    string dbName = helper.GetPartByName("Database");
                    helper.RemovePartByName("Database");
                    using (IDbConnection conn = ConnectionHelper.GetConnection(helper.GetConnectionString() + ";Database=template1"))
                    {
                        conn.Open();
                        using (IDbCommand c = conn.CreateCommand())
                        {
                            if (!dbName.StartsWith("\""))
                                dbName = '"' + dbName + '"';
                            c.CommandText = "Create Database " + dbName + " WITH ENCODING='UNICODE'";
                            c.ExecuteNonQuery();
                        }
                        using (IDbCommand c = conn.CreateCommand())
                        {
                            c.CommandText = "CREATE EXTENSION IF NOT EXISTS citext";
                            c.ExecuteNonQuery();
                        }
                    }
                }
                else
                {
                    throw new UnableToOpenDatabaseException(ConnectionStringRemovePassword(ConnectionString), e);
                }
            }
            if (Connection is NpgsqlConnection nconn)
            {
                if (nconn.State == ConnectionState.Open)
                    nconn.ReloadTypes();
            }
        }
        internal static string ConnectionStringRemovePassword(string connectionString)
        {
            return ConnectionStringRemovePassword(new ConnectionStringParser(connectionString));
        }

        internal static string ConnectionStringRemovePassword(ConnectionStringParser helper)
        {
            const string removedString = "***REMOVED***";
            helper.UpdatePartByName("password", removedString);
            helper.UpdatePartByName("pwd", removedString);
            return helper.GetConnectionString();
        }
    }

    public class PostgreSqlProviderFactoryEx : PostgreSqlProviderFactory
    {
        public override IDataStore CreateProviderFromConnection(IDbConnection connection, AutoCreateOption autoCreateOption)
        {
            return PostgreSqlConnectionProviderEx.CreateProviderFromConnection(connection, autoCreateOption);
        }
        public override IDataStore CreateProviderFromString(string connectionString, AutoCreateOption autoCreateOption, out IDisposable[] objectsToDisposeOnDisconnect)
        {
            return PostgreSqlConnectionProviderEx.CreateProviderFromString(connectionString, autoCreateOption, out objectsToDisposeOnDisconnect);
        }
        public override string GetConnectionString(Dictionary<string, string> parameters)
        {
            if (!parameters.ContainsKey(ServerParamID) || !parameters.ContainsKey(DatabaseParamID) ||
                !parameters.ContainsKey(UserIDParamID) || !parameters.ContainsKey(PasswordParamID))
            {
                return null;
            }
            string port;
            if (parameters.TryGetValue(PortParamID, out port))
            {
                return PostgreSqlConnectionProviderEx.GetConnectionString(parameters[ServerParamID], Convert.ToInt32(port, CultureInfo.InvariantCulture),
                    parameters[UserIDParamID], parameters[PasswordParamID], parameters[DatabaseParamID]);
            }
            return PostgreSqlConnectionProviderEx.GetConnectionString(parameters[ServerParamID], parameters[UserIDParamID],
                    parameters[PasswordParamID], parameters[DatabaseParamID]);
        }
        public override IDataStore CreateProvider(Dictionary<string, string> parameters, AutoCreateOption autoCreateOption, out IDisposable[] objectsToDisposeOnDisconnect)
        {
            string connectionString = GetConnectionString(parameters);
            if (connectionString == null)
            {
                objectsToDisposeOnDisconnect = new IDisposable[0];
                return null;
            }
            ConnectionStringParser helper = new ConnectionStringParser(connectionString);
            helper.RemovePartByName(DataStoreBase.XpoProviderTypeParameterName);
            return CreateProviderFromString(helper.GetConnectionString(), autoCreateOption, out objectsToDisposeOnDisconnect);
        }
        public override bool HasPort { get { return true; } }
        public override bool HasUserName { get { return true; } }
        public override bool HasPassword { get { return true; } }
        public override bool HasIntegratedSecurity { get { return false; } }
        public override bool HasMultipleDatabases { get { return true; } }
        public override bool IsServerbased { get { return true; } }
        public override bool IsFilebased { get { return false; } }
        public override string ProviderKey { get { return PostgreSqlConnectionProviderEx.XpoProviderTypeString; } }
        public override string[] GetDatabases(string server, int port, string userId, string password)
        {
            string connectionString;
            if (port != 0)
            {
                connectionString = PostgreSqlConnectionProviderEx.GetConnectionString(server, port, userId, password, "postgres");
            }
            else
            {
                connectionString = PostgreSqlConnectionProviderEx.GetConnectionString(server, userId, password, "postgres");
            }
            ConnectionStringParser helper = new ConnectionStringParser(connectionString);
            helper.RemovePartByName(DataStoreBase.XpoProviderTypeParameterName);
            connectionString = helper.GetConnectionString();
            using (IDbConnection conn = PostgreSqlConnectionProviderEx.CreateConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (IDbCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "select datname from pg_database order by datname";
                        using (IDataReader reader = cmd.ExecuteReader())
                        {
                            var result = new List<string>();
                            while (reader.Read())
                            {
                                result.Add((string)reader.GetValue(0));
                            }
                            return result.ToArray();
                        }
                    }
                }
                catch
                {
                    return new string[0];
                }
            }
        }
        public override string[] GetDatabases(string server, string userId, string password)
        {
            return GetDatabases(server, 0, userId, password);
        }
        public override string FileFilter { get { return null; } }
        public override bool MeanSchemaGeneration { get { return true; } }
        public override bool SupportStoredProcedures { get { return false; } }
    }
}
