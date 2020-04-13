using System.Collections.Generic;
using System.Threading.Tasks;
using backEndTestTask.Interfaces.Services;
using backEndTestTask.Models;

namespace backEndTestTask.Services
{
    public class ImagesService : IImagesService
    {
        private readonly IImagesThirdPartyService imagesThirdPartyService;

        public ImagesService(IImagesThirdPartyService imagesThirdPartyService)
        {
            this.imagesThirdPartyService = imagesThirdPartyService;
        }

        public Task<Images> GetById(string id)
        {
            return imagesThirdPartyService.GetById(id);
        }

        public Task<ImagesPage> GetByPage(int? page)
        {
            return imagesThirdPartyService.GetByPage(page);
        }

        public List<Images> SearchInCache(string filter)
        {
            return imagesThirdPartyService.SearchInCache(filter);
        }
    }
}
