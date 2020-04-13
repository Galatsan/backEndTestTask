using AutoMapper;
using backEndTestTask.Models;
using backEndTestTask.Models.Responses;

namespace backEndTestTask.Mappings
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<Images, ImagesResponse>();
            CreateMap<ImagesPage, ImagesPageResponse>();
            CreateMap<Pictures, PicturesResponse>();
        }
    }
}
