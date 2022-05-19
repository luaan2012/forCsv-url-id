using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PDF.Database
{
    public class DynamicTableResult
    {
        public IList<Dictionary<string, string>> Data { get; set; }
        public bool HasData { get => Data?.Any() ?? false; }
        public int RowCount { get; set; }
        public DynamicTableResult()
        {
            Data = new List<Dictionary<string, string>>();
        }

    }

    public class DbConvert
    {
        private readonly string _connectionString;
        private readonly bool _multipleSqlConnection;
        private static SqlConnection _sqlConnection;
        public DbConvert(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("default");
            _multipleSqlConnection = configuration["MultipleSqlConnection"].EndsWith(bool.TrueString);
        }
        private SqlConnection BuildConnection()
        {
            return new SqlConnection(_connectionString);
        }
        protected SqlConnection GetConnection()
        {
            var connection = new SqlConnection();

            if (_multipleSqlConnection)
                connection = BuildConnection();
            else
                connection = _sqlConnection ??= BuildConnection();

            connection.Open();

            return connection;
        }
        protected virtual void LogException(Exception exception)
        {
            Console.WriteLine(exception);
        }
        public async Task<IEnumerable<T>> ProcMultipleAsync<T>(string sql, object @params = null)
        {
            var connection = GetConnection();
            try
            {
                var result = await connection.QueryAsync<T>(sql, @params, commandType: System.Data.CommandType.StoredProcedure);
                return result;
            }
            catch (Exception ex)
            {
                LogException(ex);
            }
            finally
            {
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }

            return default;
        }
        public async Task<T> ProcFirstAsync<T>(string sql, object @params = null)
        {
            var connection = GetConnection();
            try
            {
                var result = await connection.QueryFirstAsync<T>(sql, @params, commandType: System.Data.CommandType.StoredProcedure);
                return result;
            }
            catch (Exception ex)
            {
                LogException(ex);
            }
            finally
            {
                await connection.CloseAsync();
                await connection.DisposeAsync();
            }

            return default;
        }
   
    }
}

