using KhumaloCrafts1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Azure.Search.Models;
using Microsoft.Azure.Search;
using System.Diagnostics;

namespace KhumaloCrafts1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(int userID)
        {
            // Retrieve all products from the database
            List<productTable> products = productTable.AllProducts;

            // Pass products and userID to the view
            ViewData["Products"] = products;
            ViewData["UserID"] = userID;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult MyWork()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public string SearchServiceName = "kheni";
        public string searchServiceAdminAPIKey = "OuMB41x2pB2vAVtrxrpS4SDaN0nTEmFGx1sCdor4wnAzSeDuqVth";
        public string indexName = "azuresql-index";
        public IActionResult Search(string searchData)
        {
            if (String.IsNullOrEmpty(searchData))
                searchData = "*";


            SearchServiceClient serviceClient = new SearchServiceClient(SearchServiceName, new SearchCredentials(searchServiceAdminAPIKey));

            ISearchIndexClient indexClient = serviceClient.Indexes.GetClient(indexName);

            SearchParameters searchParameters = new SearchParameters();
            searchParameters.HighlightFields = new List<string> { "merged_content" };
            searchParameters.HighlightPreTag = "<b>";
            searchParameters.HighlightPostTag = "</b>";

            IList<SearchResultViewModel> searchResults = new List<SearchResultViewModel>();

            var result = indexClient.Documents.SearchAsync(searchData, searchParameters).Result;
            foreach (var data in result.Results)
            {
                SearchResultViewModel currentData = new SearchResultViewModel();
                currentData.fileName = data.Document["metadata_storage_name"].ToString();

                var path = data.Document["metadata_storage_path"].ToString();
                path = path.Substring(0, path.Length - 1);

                var byteData = WebEncoders.Base64UrlDecode(path);
                currentData.filePath = System.Text.ASCIIEncoding.ASCII.GetString(byteData);

                currentData.fileText = data.Document["merged_content"].ToString();

                if (data.Highlights != null)
                {
                    foreach (var high in data.Highlights["merged_content"].ToString())
                    {
                        currentData.highlightedText += high;
                    }
                }
                searchResults.Add(currentData);
            }
            ViewBag.SearchData = searchResults;

            return View();
        }
    }
}
    

