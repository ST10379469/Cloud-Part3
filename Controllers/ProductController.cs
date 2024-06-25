using KhumaloCrafts1.Models;
using Microsoft.AspNetCore.Mvc;

namespace KhumaloCrafts1.Controllers
{
    public class ProductController : Controller
    {
        public productTable prodtbl = new productTable();



        [HttpPost]
        public ActionResult MyWork(productTable products)
        {
            var result2 = prodtbl.InsertProduct(products);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult MyWork()
        {
            return View(prodtbl);
        }
    }
}