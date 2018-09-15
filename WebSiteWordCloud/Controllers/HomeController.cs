using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Core.Services.Abstract;
using Data.Repositories.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Model;
using Newtonsoft.Json;
using WebSiteWordCloud.Models;

namespace WebSiteWordCloud.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebPageContentService _webPageContentService;
        private readonly IDataRepository _dataRepository;
        private readonly IMemoryCache _memoryCache;

        public HomeController(IWebPageContentService webPageContentService, IDataRepository dataRepository, IMemoryCache memoryCache)
        {
            _webPageContentService = webPageContentService;
            _dataRepository = dataRepository;
            _memoryCache = memoryCache;
        }

        public JsonResult GetWordCloud(string url)
        {
            if (!_memoryCache.TryGetValue<WebPageContent>(url, out WebPageContent content))
            {
                content = _webPageContentService.DownloadWebSitePage(url);
                _memoryCache.Set<WebPageContent>(url, content);
            }
            return Json(content.Words);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Admin()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        public void SaveWordCloud(string url)
        {
            if (!_memoryCache.TryGetValue<WebPageContent>(url, out WebPageContent content))
            {
                content = _webPageContentService.DownloadWebSitePage(url);
            }
            _memoryCache.Remove(url);
            _dataRepository.SaveWords(content.Words);
        }

        public JsonResult GetWordCloudAdmin()
        {
            var content = _dataRepository.GetWordList();
            return Json(content);
        }
    }
}
