using GutenbergProjectVBS.Web.Models.ApiModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace GutenbergProjectVBS.Web.Models
{
    public class SearchApi
    {
        public List<SearchResult> searchData(string key)
        {
            var apiUrl = "http://gutendex.com/books?search=" + key;
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

            return searchResults;
        }

        public SearchResult bookDetail(int ebookNo)
        {
            SearchResult book = null;

            try
            {
                var apiUrl = "http://gutendex.com/books/" + ebookNo;
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

            return book;
        }
    }
}