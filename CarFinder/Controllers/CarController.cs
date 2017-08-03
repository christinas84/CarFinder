using CarFinder.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Bing;
using Newtonsoft.Json;

namespace CarFinder.Controllers
{
    /// <summary>
    /// Car Finder.
    /// </summary>
    //[RoutePrefix("api/Car/")]
    public class CarController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public class Selected 
        {
            public string year { get; set; }
            public string make { get; set; }
            public string model { get; set; }
            public string trim { get; set; }

        }
        public class idClass
        {
            public int id { get; set; }
        }
        /// <summary>
        /// Get all cars by year.
        /// </summary>
        /// <returns>This method will return from the stored procedure GetModelYears all the years of vehicles in the database in descending order.</returns>
        [HttpPost]
        public IHttpActionResult GetAllYears()
        {

            var retval = db.Database.SqlQuery<string>(
                "EXEC GetAllYears").ToList();

            return Ok(retval);
        }
        /// <summary>
        /// Get makes by year
        /// </summary>
        /// <param name="selected.year"></param>
        /// <returns>This method will return from the stored procedure GetMakesByYear vehicle makes by year selected.</returns>
        [HttpPost]
        public IHttpActionResult GetMakesByYear(Selected selected)
        {
            var yearParam = new SqlParameter("@model_year", selected.year);
            var retval = db.Database.SqlQuery<string>(
            "EXEC GetMakesByYear @model_year", yearParam).ToList();

            return Ok(retval);
        }
        /// <summary>
        /// Get the models by the year and make
        /// </summary>
        /// <param name="make"></param>
        /// <param name="model_year"></param>
        /// <returns>This method returns from the stored procedure GetModelsByYearMake vehicle models by model year and make.</returns>
        [HttpPost]
        public IHttpActionResult GetModelsByYearMake(Selected selected)
        {
            var carmake = new SqlParameter("@make", selected.make);
            var yearParam = new SqlParameter("@model_year", selected.year);
            var retval = db.Database.SqlQuery<string>(
            "EXEC GetModelsByYearMake @model_year, @make", yearParam, carmake).ToList();

            return Ok(retval);
        }
        /// <summary>
        /// Get trims by year, make and model. This method returns from the stored procedure GetTrimsByYearMakeModel vehicle trims by make, model year and model name.
        /// </summary>
        /// <param name="make">Parameter name is make</param>
        /// <param name="model_year">Parameter name is model_year</param>
        /// <param name="model_name">Parameter name is model_name</param>
        /// <returns>This method returns from the stored procedure GetTrimsByYearMakeModel vehicle trims by make, model year and model name.</returns>
        [HttpPost]
        public IHttpActionResult GetTrimsByYearMakeModel(Selected selected)
        {
            var carmake = new SqlParameter("@make", selected.make);
            var yearParam = new SqlParameter("@model_year", selected.year);
            var carname = new SqlParameter("@model_name", selected.model);
            var retval = db.Database.SqlQuery<string>(
            "EXEC GetTrimsByYearMakeModel @make, @model_year, @model_name", carmake, yearParam, carname).ToList();

            return Ok(retval);
        }
        /// <summary>
        /// Get cars by model year, make, model name and model trim 
        /// </summary>
        /// <param name="model_year"></param>
        /// <param name="make"></param>
        /// <param name="model_name"></param>
        /// <param name="model_trim"></param>
        /// <returns>This method will return from the stored procedure GetCarsByYearMakeModelTrim vehicles by the year, by the year and make, cars by year make and model and cars by year make model and trim.</returns>
        [HttpPost]
        public IHttpActionResult GetCarsByYearMakeModelTrim(Selected selected)
        {
            var carmake = new SqlParameter("@make", selected.make ?? "");
            var yearParam = new SqlParameter("@model_year", selected.year ?? "");
            var carname = new SqlParameter("@model_name", selected.model ?? "");
            var cartrim = new SqlParameter("model_trim", selected.trim ?? "");
            var retval = db.Database.SqlQuery<Car>(
            "EXEC GetCarsByYearMakeModelTrim @model_year, @make, @model_name, @model_trim", yearParam, carmake, carname, cartrim).ToList();
            return Ok(retval);
        }
        //[Route ("details")]
        [HttpPost]
        public async Task<IHttpActionResult> getCar(idClass id)
        {
            HttpResponseMessage response;
            string content = "";
            var Car = db.Cars.Find(id.id);

            var Image = "";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://www.nhtsa.gov/");
                try
                {
                    response = await client.GetAsync("webapi/api/Recalls/vehicle/modelyear/" + Car.model_year +
                                                                                    "/make/" + Car.make +
                                                                                    "/model/" + Car.model_name + "?format=json");
                    content = await response.Content.ReadAsStringAsync();
                }
                catch (Exception e)
                {
                    return InternalServerError(e);
                }
            }

            dynamic Recalls = JsonConvert.DeserializeObject(content);


            var image = new BingSearchContainer(new Uri("https://api.datamarket.azure.com/Bing/search/v1/Image"));

            image.Credentials = new NetworkCredential("accountKey", "9DVmvKlSSAnTU1On117Lu4rMsXdLgjDI+sUWSdxdzIc");   //"dwmFt1J2rM45AQXkGTk4ebfcVLNcytTxGMHK6dgMreg"
            var marketData = image.Composite(
                "image",
                Car.model_year + " " + Car.make + " " + Car.model_name + " " + Car.model_trim + " " + "NOT ebay",
                null,
                null,
                //"Size:Small+Aspect:Square",
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null
                ).Execute();

            //Image = marketData.First().Image.First().MediaUrl;
            //return Ok(new { car = Car, recalls = Recalls, image = Image });
            //return JSON.parse(new { car = Car, recalls = Recalls, image = Image });
            // return Ok(Recalls);
            var Images = marketData.FirstOrDefault()?.Image;
            //int imgCnt = Images.Count();
            foreach (var Img in Images)
            {

                if (UrlCtrl.IsUrl(Img.MediaUrl))
                {
                    Image = Img.MediaUrl;
                    break;
                }
                else
                {
                    continue;
                }


            }
            if (string.IsNullOrWhiteSpace(Image))
            {
                Image = "images/carnotfound.png";
            }
            return Ok(new { car = Car, recalls = Recalls, image = Image });
        }
            public static class UrlCtrl
        {
            public static bool IsUrl(string path)
            {
                HttpWebResponse response = null;
                var request = (HttpWebRequest)WebRequest.Create(path);
                request.Method = "HEAD";
                bool result = true;

                try
                {
                    response = (HttpWebResponse)request.GetResponse();
                }
                catch (WebException ex)
                {
                    /* A WebException will be thrown if the status of the response is not `200 OK` */
                    result = false;
                }
                finally
                {
                    // Don't forget to close your response.
                    if (response != null)
                    {
                        response.Close();
                    }

                }

                return result;




                //    Uri uriResult;
                //    bool result = Uri.TryCreate(str, UriKind.Absolute, out uriResult) &&
                //    (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
                //bool result = File.Exists(path);
                //return result;
            }
            //public static bool Exists(
            //    string path
            //);
        }
    }
    }
