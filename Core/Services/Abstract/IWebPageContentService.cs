using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Services.Abstract
{
    public interface IWebPageContentService
    {
        WebPageContent DownloadWebSitePage(string url);
    }
}
