using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories.IRepositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WalksController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IWalkRepository walkRepository;

        public WalksController(IMapper mapper, IWalkRepository walkRepository)
        {
            this.mapper = mapper;
            this.walkRepository = walkRepository;
        }

        //Create a new walk
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> CreateWalk([FromBody] AddWalkRequestDto walkRequest)
        {
            //Map the DTO to a domain model
            var walkDomain = mapper.Map<Walk>(walkRequest);

            await walkRepository.CreateWalkAsync(walkDomain);

            //Map the domain model back to a DTO
            var walkResponse = mapper.Map<WalkDto>(walkDomain);

            //Return 201 Created
            return CreatedAtAction(nameof(CreateWalk), new { id = walkResponse.Id }, walkResponse);
        }

        // Get all walks
        // GET: api/walks?difficulty=1&region=1
        [HttpGet]
        public async Task<IActionResult> GetAllWalks([FromQuery] string? filterOn, [FromQuery] string? filterQuery, [FromQuery] string? sortBy,
        [FromQuery] bool? isAscending, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000)
        {
            var walksDomain = await walkRepository.GetAllWalksAsync(filterOn, filterQuery, sortBy, isAscending ?? true);

            //Map the domain model to a DTO
            var walksDto = mapper.Map<IEnumerable<WalkDto>>(walksDomain);

            return Ok(walksDto);
        }

        //Get walk by id
        [HttpGet("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> GetWalkByIdAsync([FromRoute] Guid id)
        {
            var walkDomainModel = await walkRepository.GetWalkByIdAsync(id);

            if (walkDomainModel == null)
            {
                return NotFound();
            }

            //Map the domain model to a DTO
            var walkDto = mapper.Map<WalkDto>(walkDomainModel);

            return Ok(walkDto);
        }

        //Update walk by id
        [HttpPut("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> UpdateWalk([FromRoute] Guid id, [FromBody] UpdateWalkRequestDto walkRequest)
        {
            var walkDomainModel = mapper.Map<Walk>(walkRequest);

            walkDomainModel = await walkRepository.UpdateWalkAsync(id, walkDomainModel);

            if (walkDomainModel == null)
            {
                return NotFound();
            }

            //Map the domain model to a DTO
            return Ok(mapper.Map<WalkDto>(walkDomainModel));
        }

        //Delete walk by id
        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> DeleteWalk([FromRoute] Guid id)
        {
            var deletedWalkDomainModel = await walkRepository.DeleteWalkAsync(id);

            if (deletedWalkDomainModel == null)
            {
                return NotFound();
            }

            //Map the domain model to a DTO
            return Ok(mapper.Map<WalkDto>(deletedWalkDomainModel));
        }

    }
}