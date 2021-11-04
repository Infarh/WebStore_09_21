using System;
using Microsoft.AspNetCore.Mvc;

namespace WebStore.Controllers.API
{
    [ApiController, Route("api/[controller]")]
    public class ConsoleController : ControllerBase
    {
        [HttpGet("clear")]
        public void Clear() => Console.Clear();

        [HttpGet("WriteLine")]
        public void WriteLine(string Message) => Console.WriteLine(Message);
    }
}
