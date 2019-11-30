using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InfluxNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TestInfluxClient.Controllers
{
    [Route("test")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IInfluxClient _client;

        public TestController(IInfluxClient client)
        {
            _client = client;
        }

        [HttpGet]
        public async Task<IActionResult> TestStuff()
        {
            await _client.Test();
            return Ok("OK");
        }

    }
}