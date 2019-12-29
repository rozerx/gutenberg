using GutenbergProjectVBS.Web.Models.ApiModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace GutenbergProjectVBS.Web.Tests
{
    [TestClass]
    public class SearchTest
    {
        [TestMethod]
        public void SearchBook()
        {
            var apiUrl = "http://gutendex.com/books?search=" + "kean";
            // Connect Api
            Uri url = new Uri(apiUrl);
            WebClient webClient = new WebClient();
            webClient.Encoding = System.Text.Encoding.UTF8;

            JObject gutenbergSearch = JObject.Parse(webClient.DownloadString(url));
            List<JToken> results = gutenbergSearch["results"].Children().ToList();
            List<SearchResult> searchResults = new List<SearchResult>();

            foreach(JToken result in results)
            {
                SearchResult searchResult = result.ToObject<SearchResult>();
                searchResults.Add(searchResult);
            }

            Assert.AreNotEqual(0, searchResults.Count);
        }

        [TestMethod]
        public void SearchBookNotFound()
        {
            var apiUrl = "http://gutendex.com/books?search=" + "5555";
            // Connect Api
            Uri url = new Uri(apiUrl);
            WebClient webClient = new WebClient();
            webClient.Encoding = System.Text.Encoding.UTF8;

            JObject gutenbergSearch = JObject.Parse(webClient.DownloadString(url));
            List<JToken> results = gutenbergSearch["results"].Children().ToList();
            List<SearchResult> searchResults = new List<SearchResult>();

            foreach (JToken result in results)
            {
                SearchResult searchResult = result.ToObject<SearchResult>();
                searchResults.Add(searchResult);
            }

            Assert.AreEqual(0, searchResults.Count);
        }
    }
}
