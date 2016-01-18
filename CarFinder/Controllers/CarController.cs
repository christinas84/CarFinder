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

namespace CarFinder.Controllers
{
    /// <summary>
    /// Car Finder.
    /// </summary>
    public class CarController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        /// <summary>
        /// Get all cars by year.
        /// </summary>
        /// <returns>This method will return from the stored procedure GetModelYears all the years of vehicles in the database in descending order.</returns>
        public IHttpActionResult GetModelYears()
        {

            var retval = db.Database.SqlQuery<string>(
                "EXEC GetModelYears").ToList();

            return Ok(retval);
        }
        /// <summary>
        /// Get makes by year
        /// </summary>
        /// <param name="model_year"></param>
        /// <returns>This method will return from the stored procedure GetMakesByYear vehicle makes by year selected.</returns>
        public IHttpActionResult GetMakesByYear(string model_year)
        {
            var yearParam = new SqlParameter("@model_year", model_year);
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

        public IHttpActionResult GetModelsByYearMake(string model_year, string make)
        {
            var carmake = new SqlParameter("@make", make);
            var yearParam = new SqlParameter("@model_year", model_year);
            var retval = db.Database.SqlQuery<string>(
            "EXEC GetModelsByYearMake @model_year, @make", yearParam, carmake).ToList();

            return Ok(retval);
        }
        /// <summary>
        /// Get trims by year, make and model
        /// </summary>
        /// <param name="make"></param>
        /// <param name="model_year"></param>
        /// <param name="model_name"></param>
        /// <returns>This method returns from the stored procedure GetTrimsByYearMakeModel vehicle trims by make, model year and model name.</returns>
        public IHttpActionResult GetTrimsByYearMakeModel(string make, string model_year, string model_name)
        {
            var carmake = new SqlParameter("@make", make);
            var yearParam = new SqlParameter("@model_year", model_year);
            var carname = new SqlParameter("@model_name", model_name);
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
        public IHttpActionResult GetCarsByYearMakeModelTrim(string model_year, string make, string model_name, string model_trim)
        {
            var carmake = new SqlParameter("@make", make ?? "");
            var yearParam = new SqlParameter("@model_year", model_year ?? "");
            var carname = new SqlParameter("@model_name", model_name ?? "");
            var cartrim = new SqlParameter("model_trim", model_trim ?? "");
            var retval = db.Database.SqlQuery<Car>(
            "EXEC GetCarsByYearMakeModelTrim @model_year, @make, @model_name, @model_trim", yearParam, carmake, carname, cartrim).ToList();
            return Ok(retval);
        }
        public async Task<IHttpActionResult> getCar(int Id)
        {
            HttpResponseMessage response;
            string content = "";
            var Car = db.Cars.Find(Id);
            var Recalls = "";
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
            Recalls = content;

            var image = new BingSearchContainer(new Uri("https://api.datamarket.azure.com/Bing/search/"));

            image.Credentials = new NetworkCredential("accountKey", "9DVmvKlSSAnTU1On117Lu4rMsXdLgjDI+sUWSdxdzIc");   //"dwmFt1J2rM45AQXkGTk4ebfcVLNcytTxGMHK6dgMreg"
            var marketData = image.Composite(
                "image",
                Car.model_year + " " + Car.make + " " + Car.model_name + " " + Car.model_trim,
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
                null,
                null,
                null
                ).Execute();

            Image = marketData.First().Image.First().MediaUrl;
            return Ok(new { car = Car, recalls = Recalls, image = Image });
           // return Ok(Recalls);
        }
    }
}