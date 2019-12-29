using GutenbergProjectVBS.Web.Models.ApiModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Net;

namespace GutenbergProjectVBS.Web.Tests
{
    [TestClass]
    public class BookTest
    {
        [TestMethod]
        public void BookDetail()
        {
            var apiUrl = "http://gutendex.com/books/5555/";
            // Connect Api
            Uri url = new Uri(apiUrl);
            WebClient webClient = new WebClient();
            webClient.Encoding = System.Text.Encoding.UTF8;
            // Get Book
            string getBook = webClient.DownloadString(url);
            SearchResult book = JsonConvert.DeserializeObject<SearchResult>(getBook);
            Assert.IsNotNull(book);
        }
        [TestMethod]
        public void BookDetailNotFound()
        {
            SearchResult book = null;

            try
            {
                var apiUrl = "http://gutendex.com/books/5555555/";
                // Connect Api
                Uri url = new Uri(apiUrl);
                WebClient webClient = new WebClient();
                webClient.Encoding = System.Text.Encoding.UTF8;
                // Get Book
                string getBook = webClient.DownloadString(url);
                book = JsonConvert.DeserializeObject<SearchResult>(getBook);
            }
            catch (WebException ex)
            {
                book = null;
            }
            catch (Exception ex)
            {
                book = null;
            }

            Assert.IsNull(book);
        }

        [TestMethod]
        public void BookViewMethod()
        {

        }

        [TestMethod]
        public void BookSaveMethod()
        {

        }
    }
}
