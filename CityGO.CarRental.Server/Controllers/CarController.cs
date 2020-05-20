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
    public class CarController : ControllerBase
    {
        //===========================================================//
        [HttpGet]
        [Route("api/cars")]
        public async Task<IActionResult> Get()
        {
            Response.Headers.Add("Access-Control-Allow-Origin", "*");
            Logger.Log("=======================", LogType.Info);
            Logger.Log("Received GET request for method: api/cars", LogType.Info);
            Logger.Log(
                "Received request from: Remote ip: " + Request.HttpContext.Connection.RemoteIpAddress +
                ", Remote port: " + Request.HttpContext.Connection.RemotePort, LogType.Info);
            
            try
            {
                using var carService = new CarService();
                return Ok(await carService.GetAsync());
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Problem();
            }
        }

        //===========================================================//
        [HttpPost]
        [Route(("api/cars"))]
        public async Task<IActionResult> Set()
        {
            Response.Headers.Add("Access-Control-Allow-Origin", "*");
            Logger.Log("=======================", LogType.Info);
            Logger.Log("Received POST request for method: api/cars", LogType.Info);
            Logger.Log(
                "Received request from: Remote ip: " + Request.HttpContext.Connection.RemoteIpAddress +
                ", Remote port: " + Request.HttpContext.Connection.RemotePort, LogType.Info);

            try
            {
                using var streamReader = new StreamReader(Request.Body);
                using var carService = new CarService();
                var car = JsonConvert.DeserializeObject<Car>(await streamReader.ReadToEndAsync());
                return car.Validate() ? Ok(await carService.SetAsync(JsonConvert.DeserializeObject<Car>(await streamReader.ReadToEndAsync()))) : Ok("Invalid car!");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Problem();
            }
        }

        //===========================================================//
        [HttpDelete]
        [Route("api/cars")]
        public async Task<IActionResult> Delete()
        {
            Response.Headers.Add("Access-Control-Allow-Origin", "*");
            Logger.Log("=======================", LogType.Info);
            Logger.Log("Received DELETE request for method: api/cars", LogType.Info);
            Logger.Log(
                "Received request from: Remote ip: " + Request.HttpContext.Connection.RemoteIpAddress +
                ", Remote port: " + Request.HttpContext.Connection.RemotePort, LogType.Info);

            try
            {
                var carId = Convert.ToInt64(Request.Query["Id"].First());
                using var carService = new CarService();
                await carService.DeleteAsync(carId);

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