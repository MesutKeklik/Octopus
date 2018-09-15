using Core.Services.Abstract;
using HtmlAgilityPack;
using Model;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Xml;

namespace Core.Services
{
    public class WebPageContentService : IWebPageContentService
    {
        public WebPageContent DownloadWebSitePage(string url)
        {
            using (WebClient client = new WebClient()) 
            {
                var content = client.DownloadString(url);
                var document = new HtmlDocument();
                document.LoadHtml(content);

                var result = new WebPageContent(document);
                
                return result;
            }
        }


    }
}
