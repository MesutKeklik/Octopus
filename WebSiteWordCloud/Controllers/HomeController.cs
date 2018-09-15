using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Core.Services.Abstract;
using Data.Repositories.Abstract;
using Microsoft.AspNetCore.Mvc;
using Model;
using Newtonsoft.Json;
using WebSiteWordCloud.Models;

namespace WebSiteWordCloud.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebPageContentService _webPageContentService;
        private readonly IDataRepository _dataRepository;

        public HomeController(IWebPageContentService webPageContentService, IDataRepository dataRepository)
        {
            _webPageContentService = webPageContentService;
            _dataRepository = dataRepository;
        }

        public JsonResult GetWordCloud(string url)
        {
            var content = _webPageContentService.DownloadWebSitePage(url);
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

        public void SaveWordCloud(string url)
        {
            var content = _webPageContentService.DownloadWebSitePage(url);
            _dataRepository.SaveWords(content.Words);
        }

        public JsonResult GetWordCloudAdmin()
        {
            var content = _dataRepository.GetWordList();
            return Json(content);
        }
    }
}
