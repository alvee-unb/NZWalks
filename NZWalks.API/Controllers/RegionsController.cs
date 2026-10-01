using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.CustomActionFilter;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;
using System.Text.Json;

namespace NZWalks.API.Controllers
{
    // https://localhost:portnumber/api/regions
    [Route( "api/[controller]" )]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;
        private readonly ILogger< RegionsController > logger;

        public RegionsController(
            IRegionRepository regionRepository,
            IMapper mapper,
            ILogger< RegionsController > logger )
        {
            this.regionRepository = regionRepository;
            this.mapper = mapper;
            this.logger = logger;
        }
        // GET ALL REGIONS
        // GET: https://localhost:portnumber/api/regions
        [HttpGet]
        //[Authorize( Roles = "Reader,Writer" )]
        public async Task< IActionResult > GetAll()
        {
            try
            {
                // Get data from database - Domain Models.
                var regionsDomain = await regionRepository.GetAllAsync();

                var regionsDto = mapper.Map< List< RegionDto > >( regionsDomain );

                logger.LogInformation( $"Finished GetAllRegions request with data: {JsonSerializer.Serialize( regionsDomain )}" );

                // Return DTOs.
                return Ok( regionsDto );
            }
            catch( Exception ex )
            {
                logger.LogError( ex, ex.Message );
                return BadRequest();
            }

            
        }

        // GET SINGLE REGION (Get Region By ID)
        // GET: https://localhost:portnumber/api/regions/{id}
        [HttpGet]
        [Route( "{id:Guid}" )]
        //[Authorize( Roles = "Reader,Writer" )]
        public async Task< IActionResult > GetById( [FromRoute] Guid id )
        {
            // Get Region Domain Model from Database.
            //var region = dbContext.Regions.Find( id );
            var regionDomain = await regionRepository.GetByIdAsync( id );

            if( regionDomain == null )
            {
                return NotFound();
            }

            // Map/ Convert Region Domain Model to Region DTO.
            var regionDto = mapper.Map< RegionDto >( regionDomain );

            // Return DTO back to client.
            return Ok( regionDto );
        }

        // POST to create new region.
        // POST: https://localhost:portnumber/api/regions
        [HttpPost]
        [ValidateModel]
        //[Authorize( Roles = "Writer" )]
        public async Task< IActionResult > Create( [FromBody] AddRegionRequestDto addRegionRequestDto )
        {
            // Map or convert DTO to Domain Model.
            var regionDomainModel = mapper.Map< Region >( addRegionRequestDto );

            // Use Domain Model to create Region.
            regionDomainModel = await regionRepository.CreateAsync( regionDomainModel );

            // Map the Domain Model back to DTO.
            var regionsDto = mapper.Map< RegionDto >( regionDomainModel );

            return CreatedAtAction( nameof( GetById ), new { id = regionsDto.Id }, regionsDto );
        }

        // Update region.
        // PUT: https://localhost:portnumber/api/regions/{id}
        [HttpPut]
        [Route( "{id:Guid}" )]
        [ValidateModel]
        //[Authorize( Roles = "Writer" )]
        public async Task< IActionResult > Update( [FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto )
        {
            // Map DTO to Domain Model.
            var regionDomainModel = mapper.Map< Region >( updateRegionRequestDto );

            // Update the data.
            regionDomainModel = await regionRepository.UpdateAsync( id, regionDomainModel );

            // Check if it is empty.
            if( regionDomainModel == null )
            {
                return NotFound();
            }

            // Convert Domain Model to DTO.
            var regionDto = mapper.Map< RegionDto >( regionDomainModel );

            // Return DTO.
            return Ok( regionDto );
        }

        // Delete region.
        // DELETE: https://localhost:portnumber/api/regions/{id}
        [HttpDelete]
        [Route( "{id:Guid}" )]
        //[Authorize( Roles = "Writer" )]
        public async Task< IActionResult > Delete( [FromRoute] Guid id )
        {
            var regionDomainModel = await regionRepository.DeleteAsync( id );

            // Check if it is null.
            if( regionDomainModel == null )
            {
                return NotFound();
            }

            // Convert Domain Model to DTO.
            var regionDto = mapper.Map< RegionDto >( regionDomainModel );

            // Return DTO.
            return Ok( regionDto );
        }
    }
}
