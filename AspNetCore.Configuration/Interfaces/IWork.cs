using System.Threading.Tasks;

namespace AspNetCore.Configuration.Interfaces
{
    public interface IWork
    {
        Task<string> DoSomeWork();
    }
}