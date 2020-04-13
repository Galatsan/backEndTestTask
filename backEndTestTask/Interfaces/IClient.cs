using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backEndTestTask.Interfaces
{
    public interface IClient
    {
        Task<TRes> PostDataAsJsonAsync<TRes>(string url, object data);

        Task<TRes> GetDataJsonAsync<TRes>(string url, string token);
    }
}
