using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace logservicepoc.Services;
#nullable disable
public class DbService : IDbService
{
    private readonly IDbConnection _db;

    public DbService(IConfiguration configuration)
    {
        _db = new NpgsqlConnection(Environment.GetEnvironmentVariable("DBSTRING"));
    }

    public void CloseConnection(IDbConnection dbConnection)
    {
        if (dbConnection == null)
            return;

        if (dbConnection.State != ConnectionState.Closed)
            dbConnection.Close();
    }

    public async Task<T> GetAsync<T>(string command, object parms)
    {
        try
        {
            T result;

            result = (await _db.QueryAsync<T>(command, parms).ConfigureAwait(false)).FirstOrDefault();

            return result;
        }
        finally
        {
            CloseConnection(_db);
        }
    }

    public async Task<int> BulkInsert(string command, object parms)
    {
        try
        {
            var result = _db.Execute(command, parms);
            return result;
        }
        finally
        {
            CloseConnection(_db);
        }
    }

    public async Task<List<dynamic>> GetAll<dynamic>(string command, object parms)
    {
        try
        {
            List<dynamic> result = new List<dynamic>();

            result = (await _db.QueryAsync<dynamic>(command, parms)).ToList();

            return result;
        }
        finally
        {
            CloseConnection(_db);
        }
    }

    public async Task<dynamic> GetCount(string command, object parms)
    {
        try
        {
            dynamic result;

            result = await _db.QueryAsync<dynamic>(command, parms);

            return result;
        }
        finally
        {
            CloseConnection(_db);
        }
    }

    public async Task<dynamic> EditData(string command, object parms)
    {
        try
        {
            dynamic result;

            result = await _db.ExecuteAsync(command, parms);
    
            return result;
        }
        finally
        {
            CloseConnection(_db);
        }
    }
    
    public async Task<dynamic> InsertData(string command, object parms)
    {
        try 
        {
            dynamic result;

            result = await _db.ExecuteScalarAsync<dynamic>(command, parms);

            return result;
        }
        finally
        {
            CloseConnection(_db);
        }
    }

    public async Task<dynamic> BulkUpdate(string command, dynamic parms)
    {
        try
        {
            foreach(var obj in parms)
            {
                var result = _db.Execute(command, new[] {
                    obj
                });

            }
            return true;
        }
        finally
        {
            CloseConnection(_db);
        }
    }


}
