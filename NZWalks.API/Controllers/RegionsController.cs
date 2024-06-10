using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories.IRepositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController(NZWalksDbContext context, IRegionRepository regionRepository, IMapper mapper)
        {
            this.dbContext = context;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            //Get all regions from the database
            var regionsDomain = await this.regionRepository.GetAllAsync();

            //Map domain models to DTOs
            var regionsDto = mapper.Map<List<RegionDto>>(regionsDomain);

            //Return DTOs to the client
            return Ok(regionsDto);
        }

        //Get a single region by id
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetRegion([FromRoute] Guid id)
        {
            //Get the region domain model from the database
            var regionDomain = await this.regionRepository.GetByIdAsync(id);

            if (regionDomain == null)
            {
                //Return 404 Not Found
                return NotFound();
            }

            //Map domain model to DTO
            return Ok(mapper.Map<RegionDto>(regionDomain));
        }

        //Create a new region
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto regionRequestDto)
        {
            //Map or convert the DTO to a domain model
            var regionDomainModel = mapper.Map<Region>(regionRequestDto);

            //Use domain model to add a new region to the database
            await this.regionRepository.CreateAsync(regionDomainModel);

            //Map domain model to DTO
            var regionDto = mapper.Map<RegionDto>(regionDomainModel);

            //Return 201 Created status code
            return CreatedAtAction(nameof(GetRegion), new { id = regionDomainModel.Id }, regionDto);
        }

        //Update an existing region
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            if (id == Guid.Empty)
            {
                return BadRequest();
            }

            //Get the region domain model from the database
            var regionDomain = mapper.Map<Region>(updateRegionRequestDto);

            //Update the region in the database and check if the update was successful
            regionDomain = await this.regionRepository.UpdateAsync(id, regionDomain);

            if (regionDomain == null)
            {
                return NotFound();
            }

            //Map domain model to DTO
            var regionDto = mapper.Map<RegionDto>(regionDomain);

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
            var regionDomainModel = await this.regionRepository.DeleteAsync(id);

            if (regionDomainModel == null)
            {
                return NotFound();
            }

            //Map domain model to DTO
            return Ok(mapper.Map<RegionDto>(regionDomainModel));
        }
    }
}