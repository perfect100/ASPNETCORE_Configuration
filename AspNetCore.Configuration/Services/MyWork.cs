using System.Threading.Tasks;
using AspNetCore.Configuration.Interfaces;

namespace AspNetCore.Configuration.Services
{
    public class MyWork : IWork
    {
        public async Task<string> DoSomeWork()
        {
            await Task.Delay(5000);
            return "work done";
        }
    }
}