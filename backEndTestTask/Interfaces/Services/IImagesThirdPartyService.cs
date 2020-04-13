using System.Collections.Generic;
using System.Threading.Tasks;
using backEndTestTask.Models;

namespace backEndTestTask.Interfaces.Services
{
    public interface IImagesThirdPartyService
    {
        Task<ImagesPage> GetByPage(int? page);

        Task<Images> GetById(string id);

        List<Images> SearchInCache(string filter);
    }
}
