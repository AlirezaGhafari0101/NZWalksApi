using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class RegionRepository : IRegionRepository
    {
        private readonly NZWalksDbContext ctx;

        public RegionRepository(NZWalksDbContext ctx)
        {
            this.ctx = ctx;
        }
        public async Task<List<Region>> GetAllAsync()
        {
            return await ctx.Regions.ToListAsync();
        }
        public async Task<Region?> GetByIdAsync(Guid id)
        {
            return await ctx.Regions.FirstOrDefaultAsync(r => r.Id == id);
        }
        public async Task<Region> CreateAsync(Region region)
        {
            await ctx.Regions.AddAsync(region);
            await ctx.SaveChangesAsync();
            return region;
        }
        public async Task<Region?> UpdateAsync(Guid id, Region region)
        {
            var existedRegion = await ctx.Regions.FirstOrDefaultAsync(r => r.Id == id);
            if (existedRegion == null)
            {
                return null;
            }

            existedRegion.Name = region.Name;
            existedRegion.Code = region.Code;
            existedRegion.RegionImageUrl = region.RegionImageUrl;
            await ctx.SaveChangesAsync();

            return existedRegion;
        }
        public async Task<Region?> DeleteAsync(Guid id)
        {
            var existedRegion = await ctx.Regions.FirstOrDefaultAsync(r => r.Id == id);
            if (existedRegion == null)
            {
                return null;
            }

            ctx.Regions.Remove(existedRegion);
            await ctx.SaveChangesAsync();

            return existedRegion;
        }
    }
}
