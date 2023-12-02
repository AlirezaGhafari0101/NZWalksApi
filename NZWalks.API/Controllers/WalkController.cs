using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActonFilters;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTOs;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WalkController : ControllerBase
    {
        #region Ctor
        private readonly IMapper mapper;
        private readonly IWalkRepository walkRepository;

        public WalkController(IMapper mapper, IWalkRepository walkRepository)
        {
            this.mapper = mapper;
            this.walkRepository = walkRepository;
        }
        #endregion

        #region Actions
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto)
        {
            var walkDomainModel = mapper.Map<Walk>(addWalkRequestDto);
            await walkRepository.CreateAsync(walkDomainModel);

            return Ok(mapper.Map<WalkDto>(walkDomainModel));
        }

        // api/walk?filterOn=Name&FilterQuery=""
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? FilterQuery,
            [FromQuery] string? sortBy, [FromQuery] bool? isAscending, [FromQuery] int pageNumber = 1, int pageSize = 13)
        {
            var walksDomainModel = await walkRepository.GetAllAsync(filterOn, FilterQuery, sortBy, isAscending, pageNumber, pageSize);

            if (walksDomainModel == null)
            {
                return NotFound();
            }
            var walkDtoModel = mapper.Map<List<WalkDto>>(walksDomainModel);
            return Ok(walkDtoModel);
        }


        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var walkDomainModel = await walkRepository.GetByIdAsync(id);
            if (walkDomainModel == null)
            {
                return NotFound();
            }
            var walkDtoModel = mapper.Map<WalkDto>(walkDomainModel);
            return Ok(walkDtoModel);
        }

        [HttpPut("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateWalkRequestDto updateWalkRequestDto)
        {
            var existedDomainModel = await walkRepository.UpdateAsync(id, mapper.Map<Walk>(updateWalkRequestDto));
            if (existedDomainModel == null)
            {
                return NotFound();
            }
            var walkDtoModel = mapper.Map<WalkDto>(existedDomainModel);
            return Ok(walkDtoModel);
        }

        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var existedDomainModel = await walkRepository.DeleteAsync(id);
            if (existedDomainModel == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<WalkDto>(existedDomainModel));
        }
        #endregion
    }
}
