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
            try
            {
                using (var clientService = new ClientService())
                {
                    return Ok(await clientService.GetAsync());
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
        [Route("api/clients")]
        public async Task<IActionResult> Set()
        {
            try
            {
                using (var streamReader = new StreamReader(Request.Body))
                using (var clientService = new ClientService())
                {
                    var str = await streamReader.ReadToEndAsync();
                    return Ok(await clientService.SetAsync(JsonConvert.DeserializeObject<Client>(str)));
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
        [Route("api/clients")]
        public async Task<IActionResult> Delete()
        {
            try
            {
                var clientId = Convert.ToInt64(Request.Query["id"].First());
                using (var clientService = new ClientService())
                {
                    await clientService.DeleteAsync(clientId);
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