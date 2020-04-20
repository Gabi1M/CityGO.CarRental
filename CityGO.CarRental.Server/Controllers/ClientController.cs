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
    public class ClientController : ControllerBase
    {
        //===========================================================//
        [HttpGet]
        [Route("api/clients")]
        public async Task<IActionResult> Get()
        {
            Console.WriteLine("Received GET request for method: api/clients");
            Logger.Log("Received GET request for method: api/clients", LogType.Info);
            
            Console.WriteLine("Received request from: Local ip: " + Request.HttpContext.Connection.LocalIpAddress +
                              ", Local port: " + Request.HttpContext.Connection.LocalPort + ", Remote ip: " +
                              Request.HttpContext.Connection.RemoteIpAddress + ", Remote port: " +
                              Request.HttpContext.Connection.RemotePort);
            Logger.Log("Received request from: Local ip: " + Request.HttpContext.Connection.LocalIpAddress +
                       ", Local port: " + Request.HttpContext.Connection.LocalPort + ", Remote ip: " +
                       Request.HttpContext.Connection.RemoteIpAddress + ", Remote port: " +
                       Request.HttpContext.Connection.RemotePort, LogType.Info);
            
            try
            {
                using var clientService = new ClientService();
                return Ok(await clientService.GetAsync());
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Problem();
            }
        }

        //===========================================================//
        [HttpPost]
        [Route("api/clients")]
        public async Task<IActionResult> Set()
        {
            Console.WriteLine("Received POST request for method: api/clients");
            Logger.Log("Received POST request for method: api/clients", LogType.Info);
            
            Console.WriteLine("Received request from: Local ip: " + Request.HttpContext.Connection.LocalIpAddress +
                              ", Local port: " + Request.HttpContext.Connection.LocalPort + ", Remote ip: " +
                              Request.HttpContext.Connection.RemoteIpAddress + ", Remote port: " +
                              Request.HttpContext.Connection.RemotePort);
            Logger.Log("Received request from: Local ip: " + Request.HttpContext.Connection.LocalIpAddress +
                       ", Local port: " + Request.HttpContext.Connection.LocalPort + ", Remote ip: " +
                       Request.HttpContext.Connection.RemoteIpAddress + ", Remote port: " +
                       Request.HttpContext.Connection.RemotePort, LogType.Info);
            
            try
            {
                using var streamReader = new StreamReader(Request.Body);
                using var clientService = new ClientService();
                return Ok(await clientService.SetAsync(JsonConvert.DeserializeObject<Client>(await streamReader.ReadToEndAsync())));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Problem();
            }
        }

        //===========================================================//
        [HttpDelete]
        [Route("api/clients")]
        public async Task<IActionResult> Delete()
        {
            Console.WriteLine("Received DELETE request for method: api/clients");
            Logger.Log("Received DELETE request for method: api/clients", LogType.Info);
            
            Console.WriteLine("Received request from: Local ip: " + Request.HttpContext.Connection.LocalIpAddress +
                               ", Local port: " + Request.HttpContext.Connection.LocalPort + ", Remote ip: " +
                               Request.HttpContext.Connection.RemoteIpAddress + ", Remote port: " +
                               Request.HttpContext.Connection.RemotePort);
            Logger.Log("Received request from: Local ip: " + Request.HttpContext.Connection.LocalIpAddress +
                       ", Local port: " + Request.HttpContext.Connection.LocalPort + ", Remote ip: " +
                       Request.HttpContext.Connection.RemoteIpAddress + ", Remote port: " +
                       Request.HttpContext.Connection.RemotePort, LogType.Info);
            
            try
            {
                var clientId = Convert.ToInt64(Request.Query["id"].First());
                var clientService = new ClientService();
                await clientService.DeleteAsync(clientId);

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