using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication98
{
    public class MyController
    {
        [HttpGet("/dan")]
        public string Get()
        {
            return "Hello From Host MVC";
        }
    }
}
