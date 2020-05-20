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
            Response.Headers.Add("Access-Control-Allow-Origin", "*");
            Logger.Log("=======================", LogType.Info);
            Logger.Log("Received GET request for method: api/clients", LogType.Info);
            Logger.Log(
                "Received request from: Remote ip: " + Request.HttpContext.Connection.RemoteIpAddress +
                ", Remote port: " + Request.HttpContext.Connection.RemotePort, LogType.Info);

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
        [HttpGet]
        [Route("api/login")]
        public async Task<IActionResult> Login()
        {
            Response.Headers.Add("Access-Control-Allow-Origin", "*");
            Logger.Log("=======================", LogType.Info);
            Logger.Log("Received GET request for method: api/login", LogType.Info);
            Logger.Log(
                "Received request from: Remote ip: " + Request.HttpContext.Connection.RemoteIpAddress +
                ", Remote port: " + Request.HttpContext.Connection.RemotePort, LogType.Info);

            try
            {
                var mail = Request.Query["Mail"].ToString().Trim();
                var password = Request.Query["Password"].ToString().Trim();
                using var clientService = new ClientService();
                var client = await clientService.LoginAsync(mail, password);
                return Ok(client != null ? JsonConvert.SerializeObject(client) : "FAIL");
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
            Response.Headers.Add("Access-Control-Allow-Origin", "*");
            Logger.Log("=======================", LogType.Info);
            Logger.Log("Received POST request for method: api/clients", LogType.Info);
            Logger.Log(
                "Received request from: Remote ip: " + Request.HttpContext.Connection.RemoteIpAddress +
                ", Remote port: " + Request.HttpContext.Connection.RemotePort, LogType.Info);

            try
            {
                using var streamReader = new StreamReader(Request.Body);
                using var clientService = new ClientService();
                var client = JsonConvert.DeserializeObject<Client>(await streamReader.ReadToEndAsync());
                return client.Validate() ? Ok(await clientService.SetAsync(client)) : Ok("Invalid client!");
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
            Response.Headers.Add("Access-Control-Allow-Origin", "*");
            Logger.Log("=======================", LogType.Info);
            Logger.Log("Received DELETE request for method: api/clients", LogType.Info);
            Logger.Log(
                "Received request from: Remote ip: " + Request.HttpContext.Connection.RemoteIpAddress +
                ", Remote port: " + Request.HttpContext.Connection.RemotePort, LogType.Info);

            try
            {
                var clientId = Convert.ToInt64(Request.Query["Id"].First());
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