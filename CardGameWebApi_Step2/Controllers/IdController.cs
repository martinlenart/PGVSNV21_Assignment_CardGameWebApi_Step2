using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using CardGameWebApi;
using CardGameWebApi.Models;
using DbAppWebApi.Controllers;

namespace CardGameWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]     //All actions (methods) in the controller will be named Id. They must differentiate in Url
    public class IdController : ControllerBase
    {
        private ILogger<LogController> _logger;

        ///id
        [HttpGet()]
        [ProducesResponseType(200, Type = typeof(IEnumerable<string>))]
        public IEnumerable<string> Id()
        {
            return new string[] { AppConfig.ConfigurationRoot.GetAppTitle(),
                AppConfig.ConfigurationRoot.GetAppVersion().ToString(),
                AppConfig.ConfigurationRoot.GetAppDeveloper(),
                "An example of how to implement a CardGame as a WebApi"};
        }

        public IdController(ILogger<LogController> logger)
        {
            _logger = logger;
            _logger.LogInformation($"IdController started: {AppConfig.ConfigurationRoot.GetAppId()}");
        }
    }
}

