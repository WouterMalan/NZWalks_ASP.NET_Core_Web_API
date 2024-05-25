using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

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
            //Get all regions from the database
            var regionsDomain = this.dbContext.Regions.ToList();

            //Map domain models to DTOs
            var regionsDto = new List<RegionDto>();

            foreach (var regionDomain in regionsDomain)
            {
                regionsDto.Add(new RegionDto
                {
                    Id = regionDomain.Id,
                    Code = regionDomain.Code,
                    Name = regionDomain.Name,
                    RegionImageUrl = regionDomain.RegionImageUrl
                });
            }

            //Return DTOs to the client
            return Ok(regionsDto);
        }

        //Get a single region by id
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetRegion([FromRoute] Guid id)
        {
            //Get the region domain model from the database
            var regionDomain = this.dbContext.Regions.FirstOrDefault(region => region.Id == id);

            if (regionDomain == null)
            {
                //Return 404 Not Found
                return NotFound();
            }

            //Map domain model to DTO
            var regionDto = new RegionDto
            {
                Id = regionDomain.Id,
                Code = regionDomain.Code,
                Name = regionDomain.Name,
                RegionImageUrl = regionDomain.RegionImageUrl
            };

            return Ok(regionDto);
        }
    }
}