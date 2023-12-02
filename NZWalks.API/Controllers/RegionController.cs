using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTOs;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionController : ControllerBase
    {
        #region  Ctor

        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionController(IRegionRepository regionRepository, IMapper mapper)
        {

            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }

        #endregion

        #region  Actions

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var regions = await regionRepository.GetAllAsync();
            return Ok(mapper.Map<List<RegionDto>>(regions));
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var region = await regionRepository.GetByIdAsync(id);
            if (region == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<RegionDto>(region));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto region)
        {
            if (ModelState.IsValid)
            {
                var regionDomain = mapper.Map<Region>(region);

                await regionRepository.CreateAsync(regionDomain);

                var regionDto = mapper.Map<RegionDto>(regionDomain);

                return CreatedAtAction(nameof(GetById), new { id = regionDomain.Id }, regionDto);
            }
            else
            {
                return BadRequest(region);
            }
        }

        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto UpdateRegionRequestDto)
        {

            if (ModelState.IsValid)
            {
                var regionDomain = mapper.Map<Region>(UpdateRegionRequestDto);

                var regionDomainModel = await regionRepository.UpdateAsync(id, regionDomain);

                if (regionDomainModel == null)
                {
                    return NotFound();
                }

                var regionDto = mapper.Map<RegionDto>(regionDomainModel);

                return Ok(regionDto);
            }
            else
            {
                return BadRequest(UpdateRegionRequestDto);
            }
        }

        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var regionDomainModel = await regionRepository.GetByIdAsync(id);

            if (regionDomainModel == null)
            {
                return NotFound();
            }

            await regionRepository.DeleteAsync(id);

            var regionDto = mapper.Map<RegionDto>(regionDomainModel);
            return Ok(regionDto);
        }
        #endregion
    }
}
