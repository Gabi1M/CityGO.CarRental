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
            try
            {
                using (var carService = new CarService())
                {
                    return Ok(await carService.GetAsync());
                }
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
            try
            {
                using (var streamReader = new StreamReader(Request.Body))
                using (var carService = new CarService())
                {
                    var str = await streamReader.ReadToEndAsync();
                    return Ok(await carService.SetAsync(JsonConvert.DeserializeObject<Car>(str)));
                }
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
            try
            {
                var carId = Convert.ToInt64(Request.Query["id"].First());
                using (var carService = new CarService())
                {
                    await carService.DeleteAsync(carId);
                }

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