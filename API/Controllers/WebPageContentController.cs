using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Core.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebSiteWordCloud.Models;

namespace WebSiteWordCloud.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebPageContentService _webPageContentService;
        public HomeController(IWebPageContentService webPageContentService)
        {
            _webPageContentService = webPageContentService;
        }

        public JsonResult GetCloud(string url)
        {
            var content = _webPageContentService.DownloadWebSitePage(url);
            return Json(content.Words);
        }

    }
}
