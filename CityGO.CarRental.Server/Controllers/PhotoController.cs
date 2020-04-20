using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CityGO.CarRental.Core.Models;
using CityGO.CarRental.Core.Service;
using CityGO.CarRental.Core.Utils;
using Microsoft.AspNetCore.Mvc;

namespace CityGO.CarRental.Server.Controllers
{
    [ApiController]
    public class PhotoController : ControllerBase
    {
        //===========================================================//
        [HttpGet]
        [Route("api/photos")]
        public async Task<IActionResult> Get()
        {
            Console.WriteLine("Received GET request for method: api/photos");
            Logger.Log("Received GET request for method: api/photos", LogType.Info);
            
            Console.WriteLine("Received request from: Local ip: " + Request.HttpContext.Connection.LocalIpAddress +
                              ", Local port: " + Request.HttpContext.Connection.LocalPort + ", Remote ip: " +
                              Request.HttpContext.Connection.RemoteIpAddress + ", Remote port: " +
                              Request.HttpContext.Connection.RemotePort);
            Logger.Log("Received request from: Local ip: " + Request.HttpContext.Connection.LocalIpAddress +
                       ", Local port: " + Request.HttpContext.Connection.LocalPort + ", Remote ip: " +
                       Request.HttpContext.Connection.RemoteIpAddress + ", Remote port: " +
                       Request.HttpContext.Connection.RemotePort, LogType.Info);
            
            using var photoService = new PhotoService();
            return Ok(await photoService.GetAsync());
        }

        //===========================================================//
        [HttpPost]
        [Route("api/photos")]
        public async Task<IActionResult> Set()
        {
            Console.WriteLine("Received POST request for method: api/photos");
            Logger.Log("Received POST request for methos: api/photos", LogType.Info);
            
            Console.WriteLine("Received request from: Local ip: " + Request.HttpContext.Connection.LocalIpAddress +
                              ", Local port: " + Request.HttpContext.Connection.LocalPort + ", Remote ip: " +
                              Request.HttpContext.Connection.RemoteIpAddress + ", Remote port: " +
                              Request.HttpContext.Connection.RemotePort);
            Logger.Log("Received request from: Local ip: " + Request.HttpContext.Connection.LocalIpAddress +
                       ", Local port: " + Request.HttpContext.Connection.LocalPort + ", Remote ip: " +
                       Request.HttpContext.Connection.RemoteIpAddress + ", Remote port: " +
                       Request.HttpContext.Connection.RemotePort, LogType.Info);
            
            var carId = Convert.ToInt64(Request.Query["carid"].First());
            var file = (await Request.ReadFormAsync()).Files.GetFile("photo");
            var filename = DateTime.Now.TimeOfDay.Hours.ToString() +
                                    DateTime.Now.TimeOfDay.Minutes +
                                    DateTime.Now.TimeOfDay.Seconds +
                                    DateTime.Now.TimeOfDay.Milliseconds;
            filename = Path.Combine(AppSettings.PhotoPath, filename) + ".jpg";
            var fileStream = System.IO.File.Create(filename);
            await file.CopyToAsync(fileStream);
            fileStream.Close();

            var photo = new Photo(carId, filename);
            using var photoService = new PhotoService();
            return Ok(await photoService.SetAsync(photo));
        }

        //===========================================================//
        [HttpGet]
        [Route("api/download_photo")]
        public async Task<IActionResult> Download()
        {
            Console.WriteLine("Received GET request for method: api/download_photo");
            Logger.Log("Received GET request for method: api/download_photo", LogType.Info);
            
            Console.WriteLine("Received request from: Local ip: " + Request.HttpContext.Connection.LocalIpAddress +
                              ", Local port: " + Request.HttpContext.Connection.LocalPort + ", Remote ip: " +
                              Request.HttpContext.Connection.RemoteIpAddress + ", Remote port: " +
                              Request.HttpContext.Connection.RemotePort);
            Logger.Log("Received request from: Local ip: " + Request.HttpContext.Connection.LocalIpAddress +
                       ", Local port: " + Request.HttpContext.Connection.LocalPort + ", Remote ip: " +
                       Request.HttpContext.Connection.RemoteIpAddress + ", Remote port: " +
                       Request.HttpContext.Connection.RemotePort, LogType.Info);
            
            var id = Convert.ToInt64(Request.Query["id"].First());
            using var photoService = new PhotoService();
            var photo = (await photoService.GetAsync()).First(x => x.Id == id);
            return File(System.IO.File.Open(photo.Path, FileMode.Open, FileAccess.Read, FileShare.Read), "image/jpeg");
        }
    }
}