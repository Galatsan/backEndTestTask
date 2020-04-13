using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using backEndTestTask.Interfaces.Services;
using backEndTestTask.Models.Responses;
using Microsoft.AspNetCore.Mvc;

namespace backEndTestTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImagesService imagesService;
        private readonly IMapper mapper;

        public ImagesController(IImagesService imagesService, IMapper mapper)
        {
            this.imagesService = imagesService;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? page = null)
        {
            var result = await imagesService.GetByPage(page);
            var mapped = mapper.Map<ImagesPageResponse>(result);
            return Ok(mapped);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await imagesService.GetById(id);
            var mapped = mapper.Map<ImagesResponse>(result);
            return Ok(mapped);
        }

        [HttpGet("search/{filter}")]
        public IActionResult SearchInCache(string filter)
        {
            var result = imagesService.SearchInCache(filter);
            var mapped = mapper.Map<List<ImagesResponse>>(result);
            return Ok(mapped);
        }
    }
}