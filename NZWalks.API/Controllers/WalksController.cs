using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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

    }
}