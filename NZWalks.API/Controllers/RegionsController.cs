using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IActionResult> GetAll()
        {
            //Get all regions from the database
            var regionsDomain = await this.dbContext.Regions.ToListAsync();

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
        public async Task<IActionResult> GetRegion([FromRoute] Guid id)
        {
            //Get the region domain model from the database
            var regionDomain = await this.dbContext.Regions.FirstOrDefaultAsync(region => region.Id == id);

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

        //Create a new region
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto regionRequestDto)
        {
            //Map or convert the DTO to a domain model
            var regionDomainModel = new Region
            {
                Code = regionRequestDto.Code,
                Name = regionRequestDto.Name,
                RegionImageUrl = regionRequestDto.RegionImageUrl
            };

            //Use domain model to add a new region to the database
            await this.dbContext.Regions.AddAsync(regionDomainModel);
            await this.dbContext.SaveChangesAsync();

            //Map domain model to DTO
            var regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };

            //Return 201 Created status code
            return CreatedAtAction(nameof(GetRegion), new { id = regionDomainModel.Id }, regionDto);
        }

        //Update an existing region
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            if (id == Guid.Empty)
            {
                return BadRequest();
            }

            //Get the region domain model from the database
            var regionDomain = await this.dbContext.Regions.FirstOrDefaultAsync(region => region.Id == id);

            if (regionDomain == null)
            {
                return NotFound();
            }

            //Map DTO to domain model
            regionDomain.Code = updateRegionRequestDto.Code;
            regionDomain.Name = updateRegionRequestDto.Name;
            regionDomain.RegionImageUrl = updateRegionRequestDto.RegionImageUrl;

            //Update the region in the database
            await this.dbContext.SaveChangesAsync();

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

        //Delete an existing region
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest();
            }

            //Get the region domain model from the database
            var regionDomainModel = await this.dbContext.Regions.FirstOrDefaultAsync(region => region.Id == id);

            if (regionDomainModel == null)
            {
                return NotFound();
            }

            //Delete the region from the database
            this.dbContext.Regions.Remove(regionDomainModel);
            await this.dbContext.SaveChangesAsync();

            //Map domain model to DTO   
            var regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };

            return Ok(regionDto);
        }

    }
}