using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lazztech.Cloud.ClientFacade.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lazztech.Cloud.ClientFacade.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeedController : ControllerBase
    {
        private readonly IDbSeeder _dbSeeder;

        public SeedController(IDbSeeder dbSeeder)
        {
            _dbSeeder = dbSeeder;
        }

        // GET: api/Seed
        [HttpGet]
        public string Get()
        {
            try
            {
                _dbSeeder.SeedAdminUser();
                return "Seeded";
            }
            catch (Exception e)
            {
                return e.InnerException.ToString();
                throw;
            }
        }

        // GET: api/Seed/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Seed
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Seed/5
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
