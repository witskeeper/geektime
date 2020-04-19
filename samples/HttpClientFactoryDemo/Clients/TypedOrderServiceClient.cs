using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
namespace HttpClientFactoryDemo.Clients
{
    public class TypedOrderServiceClient
    {
        HttpClient _client;
        public TypedOrderServiceClient(HttpClient client)
        {
            _client = client;
        }


        public async Task<string> Get()
        {
           return  await _client.GetStringAsync("/OrderService"); //这里使用相对路径来访问
        }
    }
}
