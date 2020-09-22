using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using RestApi.Models;
using Newtonsoft.Json;
using System.Text;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace RestApi.Controllers
{
    public class HomeController : Controller
    {
        //GET request
        //TODO: Display respons in GetResultPage
        public ActionResult GetResult()
        {
            string responseFromServer;
            WebRequest request = WebRequest.Create("http://localhost:49521/api/customers/");
            request.Credentials = CredentialCache.DefaultCredentials;
            WebResponse response = request.GetResponse();
            
            using (Stream dataStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);
                responseFromServer = reader.ReadToEnd();
            }
            var myCustomerList = JsonConvert.DeserializeObject<List<Customer>>(responseFromServer);
            ViewBag.Javascript = "<script language='javascript' type='text/javascript'>alert('Code 201 - succeed');</script>";
            return View(myCustomerList);
        }

        [HttpGet]
        public IActionResult PostResult()
        {
            Customer customer = new Customer();
            return View(customer);
        }

        // POST Request
        //TODO: Display respons in PostResultPage
        [HttpPost]
        public ActionResult PostResult(Customer customer)
        {
            string postData = JsonConvert.SerializeObject(customer);
            byte[] bytes = Encoding.UTF8.GetBytes(postData);
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://localhost:49521/api/customers/");
            httpWebRequest.Method = "POST";
            httpWebRequest.ContentLength = bytes.Length;
            httpWebRequest.ContentType = "application/json";

            using (Stream requestStream = httpWebRequest.GetRequestStream())
            {
                requestStream.Write(bytes, 0, bytes.Count());
            }
            var httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            return RedirectToAction("PostResult");
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetRequest()
        {
            return View();
        }

        public IActionResult PostRequest()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

//[HttpPost]
//public ActionResult Index(string name)
//{
//    ViewBag.Message = string.Format("{Code 201 - succeed}".ToString());
//    return View();
//}