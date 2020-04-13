using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backEndTestTask.Attributes;
using backEndTestTask.Exceptions;
using backEndTestTask.Interfaces;
using backEndTestTask.Interfaces.Services;
using backEndTestTask.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace backEndTestTask.Services
{
    public class ImagesThirdPartyService : IImagesThirdPartyService
    {
        private readonly IClient client;
        private readonly IMemoryCache memoryCache;
        private readonly ImagesThirdPartySetting imagesThirdPartySetting;

        private string token;

        public ImagesThirdPartyService(IClient client, IMemoryCache memoryCache, IOptions<ImagesThirdPartySetting> options)
        {
            this.client = client;
            this.memoryCache = memoryCache;
            imagesThirdPartySetting = options.Value;
        }

        public async Task<Images> GetById(string id)
        {
            var result = await InvokeTokenWrapper(async (token) => await GetByIdInternal(id, token));
            return (Images)result;
        }

        public async Task<ImagesPage> GetByPage(int? page)
        {
            var result = await InvokeTokenWrapper(async (token) => await GetByPageInternal((page ?? 0).ToString(), token));
            return (ImagesPage)result;
        }

        public List<Images> SearchInCache(string filter)
        {
            var result = new List<Images>();
            if (memoryCache.TryGetValue("images", out object imagesObject))
            {
                var image = (List<Images>)imagesObject;
                var propsToSearch = GetPropertiesToSearch();
                foreach (var item in image)
                {
                    foreach (var prop in propsToSearch)
                    {
                        var valueToCompare = (string)item.GetType().GetProperty(prop).GetValue(item);
                        if((valueToCompare ?? string.Empty).ToLower().Contains(filter.ToLower()))
                        {
                            result.Add(item);
                            break;
                        }
                    }
                }
            }

            return result;
        }

        private List<string> GetPropertiesToSearch()
        {
            var propertiesToSearch = new List<string>();
            var props = typeof(Images).GetProperties();
            foreach (var prop in props)
            {
                var attrs = prop.GetCustomAttributes(true);
                foreach (var attr in attrs)
                {
                    if (attr is SearchFieldAttribute authAttr)
                    {
                        propertiesToSearch.Add(prop.Name);
                    }
                }
            }
            return propertiesToSearch;
        }

        private async Task<object> InvokeTokenWrapper(Func<string, Task<object>> getter)
        {
            var token = await GetToken();

            try
            {
                return await getter(token);
            }
            catch (UnauthorizedStatusCodeException)
            {
                token = await GetToken(true);
                return await getter(token);
            }
        }

        private async Task<ImagesPage> GetByPageInternal(string page, string token)
        {
            var key = $"page {page}";
            if (memoryCache.TryGetValue(key, out object imagesPage))
                return (ImagesPage)imagesPage;

            imagesPage = await client.GetDataJsonAsync<ImagesPage>($"{imagesThirdPartySetting.Url}/images?page={page}", token);
            SetInCache(key, imagesPage);
            return (ImagesPage)imagesPage;
        }

        private async Task<Images> GetByIdInternal(string id, string token)
        {
            var key = $"id {id}";
            if (memoryCache.TryGetValue(key, out object images))
                return (Images)images;

            images = await client.GetDataJsonAsync<Images>($"{imagesThirdPartySetting.Url}/images/{id}", token);
            SetInCache(key, images);
            return (Images)images;
        }

        private void SetInCache(string key, object value)
        {
            memoryCache.Set(key, value, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(imagesThirdPartySetting.CacheAbsoluteExpirationTime)));
        }

        private async Task<string> GetToken(bool reset = false)
        {
            if (!reset && !string.IsNullOrEmpty(token))
            {
                return token;
            }

            var tokenModel = await client.PostDataAsJsonAsync<TokenModel>($"{imagesThirdPartySetting.Url}/auth", new { apiKey = imagesThirdPartySetting.ApiKey });
            token = tokenModel.Token;
            return token;
        }
    }
}
