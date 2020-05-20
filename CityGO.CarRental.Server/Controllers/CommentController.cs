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
    public class CommentController : ControllerBase
    {
        //===========================================================//
        [HttpGet]
        [Route("api/comments")]
        public async Task<IActionResult> Get()
        {
            Response.Headers.Add("Access-Control-Allow-Origin", "*");
            Logger.Log("=======================", LogType.Info);
            Logger.Log("Received GET request for method: api/comments", LogType.Info);
            Logger.Log(
                "Received request from: Remote ip: " + Request.HttpContext.Connection.RemoteIpAddress +
                ", Remote port: " + Request.HttpContext.Connection.RemotePort, LogType.Info);

            try
            {
                using var commentService = new CommentService();
                if (Request.Query.Count != 0 && Request.Query.ContainsKey("Mail"))
                {
                    var mail = Request.Query["Mail"].ToString();
                    var comments = await commentService.GetAsync();
                    return Ok(comments.Where(x => x.Mail.Trim() == mail.Trim()));
                }

                return Ok(await commentService.GetAsync());
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Problem();
            }
        }

        //===========================================================//
        [HttpPost]
        [Route("api/comments")]
        public async Task<IActionResult> Set()
        {
            Response.Headers.Add("Access-Control-Allow-Origin", "*");
            Logger.Log("=======================", LogType.Info);
            Logger.Log("Received POST request for method: api/comments", LogType.Info);
            Logger.Log(
                "Received request from: Remote ip: " + Request.HttpContext.Connection.RemoteIpAddress +
                ", Remote port: " + Request.HttpContext.Connection.RemotePort, LogType.Info);

            try
            {
                using var streamReader = new StreamReader(Request.Body);
                using var commentService = new CommentService();
                var comment = JsonConvert.DeserializeObject<Comment>(await streamReader.ReadToEndAsync());
                return comment.Validate() ? Ok(await commentService.SetAsync(comment)) : Ok("Invalid comment!");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Problem();
            }
        }

        //===========================================================//
        [HttpDelete]
        [Route("api/comments")]
        public async Task<IActionResult> Delete()
        {
            Response.Headers.Add("Access-Control-Allow-Origin", "*");
            Logger.Log("=======================", LogType.Info);
            Logger.Log("Received DELETE request for method: api/comments", LogType.Info);
            Logger.Log(
                "Received request from: Remote ip: " + Request.HttpContext.Connection.RemoteIpAddress +
                ", Remote port: " + Request.HttpContext.Connection.RemotePort, LogType.Info);

            try
            {
                var id = Convert.ToInt64(Request.Query["Id"].First());
                using var commentService = new CommentService();
                await commentService.DeleteAsync(id);

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