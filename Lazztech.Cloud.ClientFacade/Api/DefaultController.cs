using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lazztech.Cloud.ClientFacade.Dal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Lazztech.Cloud.ClientFacade.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class DefaultController : Controller
    {
        private readonly SimpleDataAccess _dataAccess;

        public DefaultController()
        {
            var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
            _dataAccess = new SimpleDataAccess(connectionString);
        }

        // GET: api/Default
        [HttpGet]
        public IEnumerable<string> Get()
        {
            var r = new Random();
            var x = new SimpleValue() { Id = r.Next(), Value = "testvalue" };
            //_dataAccess.AddNewValue(x);
            var id = _dataAccess.AddNewValue("testvalue");

            return _dataAccess.GetAllValues();
        }

        // GET: api/Default/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Default
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Default/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
