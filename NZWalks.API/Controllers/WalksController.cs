using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    // /api/walks
    [Route( "api/[controller]" )]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IWalkRepository walkRepository;

        public WalksController( IMapper mapper, IWalkRepository walkRepository )
        {
            this.mapper = mapper;
            this.walkRepository = walkRepository;
        }

        // CREATE Walk
        // POST: /api/walks
        [HttpPost]
        public async Task< IActionResult > Create( AddWalkRequestDto addWalkRequestDto )
        {
            // Map DTO to Domain Model.
            var walkDomainModel = mapper.Map< Walk >( addWalkRequestDto );

            walkDomainModel = await walkRepository.CreateAsync( walkDomainModel );

            // Map Domain Model to DTO.
            var walkDto = mapper.Map< WalkDto >( walkDomainModel );

            return Ok( walkDto );
        }

        // GET Walks.
        // GET: /api/walks
        [HttpGet]
        public async Task< IActionResult > GetAll()
        {
            var walksDomainModel = await walkRepository.GetAllAsync();

            var walksDto = mapper.Map< List< WalkDto > >( walksDomainModel );

            return Ok( walksDto );
        }
    }
}
