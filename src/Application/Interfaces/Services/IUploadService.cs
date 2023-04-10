using ArdaManager.Application.Requests;
using System.Threading.Tasks;

namespace ArdaManager.Application.Interfaces.Services
{
    public interface IUploadService
    {
        string Upload(UploadRequest request);
        Task<string> UploadAsync(UploadRequest request);
    }
}