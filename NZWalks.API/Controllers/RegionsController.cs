using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;

        public RegionsController(NZWalksDbContext context)
        {
            this.dbContext = context;
        }
        
        [HttpGet]
        public IActionResult GetAll()
        {
            var regionsDbContext = this.dbContext.Regions.ToList();


            return Ok(regionsDbContext);
        }

        //Get a single region by id
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetRegion([FromRoute] Guid id)
        {
            var region = this.dbContext.Regions.FirstOrDefault(region => region.Id == id);

            if (region == null)
            {
                //Return 404 Not Found
                return NotFound();
            }

            return Ok(region);
        }
    }
}