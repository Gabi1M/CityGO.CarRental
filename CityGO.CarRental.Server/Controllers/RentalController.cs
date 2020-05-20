using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CityGO.CarRental.Core.Models;
using CityGO.CarRental.Core.Service;
using CityGO.CarRental.Core.Utils;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CityGO.CarRental.Server.Controllers
{
    [ApiController]
    public class RentalController : ControllerBase
    {
        //===========================================================//
        [HttpGet]
        [Route("api/rentals")]
        public async Task<IActionResult> Get()
        {
            Response.Headers.Add("Access-Control-Allow-Origin", "*");
            Logger.Log("=======================", LogType.Info);
            Logger.Log("Received GET request for method: api/rentals", LogType.Info);
            Logger.Log(
                "Received request from: Remote ip: " + Request.HttpContext.Connection.RemoteIpAddress +
                ", Remote port: " + Request.HttpContext.Connection.RemotePort, LogType.Info);

            try
            {
                using var rentalService = new RentalService();
                if (Request.Query.Count != 0 && Request.Query.ContainsKey("CarId"))
                {
                    var carId = Convert.ToInt64(Request.Query["CarId"]);
                    var rentals = await rentalService.GetAsync();
                    return Ok(rentals.Where(x => x.CarId == carId));
                }

                if (Request.Query.Count != 0 && Request.Query.ContainsKey("ClientId"))
                {
                    var clientId = Convert.ToInt64(Request.Query["ClientId"]);
                    var rentals = await rentalService.GetAsync();
                    return Ok(rentals.Where(x => x.ClientId == clientId));
                }

                return Ok(await rentalService.GetAsync());
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Problem();
            }
        }

        //===========================================================//
        [HttpPost]
        [Route("api/rentals")]
        public async Task<IActionResult> Set()
        {
            Response.Headers.Add("Access-Control-Allow-Origin", "*");
            Logger.Log("=======================", LogType.Info);
            Logger.Log("Received POST request for method: api/rentals", LogType.Info);
            Logger.Log(
                "Received request from: Remote ip: " + Request.HttpContext.Connection.RemoteIpAddress +
                ", Remote port: " + Request.HttpContext.Connection.RemotePort, LogType.Info);

            try
            {
                using var streamReader = new StreamReader(Request.Body);
                using var rentalService = new RentalService();
                return Ok(await rentalService.SetAsync(JsonConvert.DeserializeObject<Rental>(await streamReader.ReadToEndAsync())));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Problem();
            }
        }

        //===========================================================//
        [HttpDelete]
        [Route("api/rentals")]
        public async Task<IActionResult> Delete()
        {
            Response.Headers.Add("Access-Control-Allow-Origin", "*");
            Logger.Log("=======================", LogType.Info);
            Logger.Log("Received DELETE request for method: api/rentals", LogType.Info);
            Logger.Log(
                "Received request from: Remote ip: " + Request.HttpContext.Connection.RemoteIpAddress +
                ", Remote port: " + Request.HttpContext.Connection.RemotePort, LogType.Info);

            try
            {
                var id = Convert.ToInt64(Request.Query["Id"].First());
                using var rentalService = new RentalService();
                await rentalService.DeleteAsync(id);

                return Ok();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Problem();
            }
        }
    }
}