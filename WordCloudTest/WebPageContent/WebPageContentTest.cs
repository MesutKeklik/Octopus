using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;
using Model;
using HtmlAgilityPack;
using System.Linq;

namespace WordCloudTest.WebPageContent
{
    public class WebPageContentTest
    {
        private readonly string sampleWebPage = File.ReadAllText("WebPageContent\\sample site.html"); //koalay.com site's source

        [Fact]
        public void IsPageDownloadedCorrectly()
        {
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(sampleWebPage);
            Model.WebPageContent wpc = new Model.WebPageContent(document);

            //in sample html's body section "trafik" word count is 12.
            Assert.Equal(12, wpc.Words.Where(w => w.Word == "trafik").FirstOrDefault().Weight);

        }
    }
}
