
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LogServicePoc.Services;

public interface IDbService
{
    Task<T> GetAsync<T>(string command, object parms); 
    Task<int> BulkInsert(string command, object parms); 
    Task<List<dynamic>> GetAll<dynamic>(string command, object parms );
    Task<dynamic> GetCount(string command, object parms);
    Task<dynamic> EditData(string command, object parms);
    Task<dynamic> InsertData(string command, object parms);
    Task<dynamic> BulkUpdate(string command, dynamic parms);
    
}
