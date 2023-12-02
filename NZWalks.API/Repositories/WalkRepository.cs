using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTOs;

namespace NZWalks.API.Repositories
{
    public class WalkRepository : IWalkRepository
    {
        private readonly NZWalksDbContext ctx;

        public WalkRepository(NZWalksDbContext ctx)
        {
            this.ctx = ctx;
        }
        public async Task<Walk> CreateAsync(Walk walk)
        {
            await ctx.Walks.AddAsync(walk);
            await ctx.SaveChangesAsync();
            var createdModel = await ctx.Walks
                .Include(w => w.Region)
                .Include(w => w.Difficulty)
                .FirstOrDefaultAsync(w => w.Id == walk.Id);
            return walk;
        }

        public async Task<List<Walk>> GetAllAsync()
        {
            return await ctx.Walks
                .Include(w => w.Difficulty)
                .Include(w => w.Region)
                .ToListAsync();
        }

        public async Task<Walk> GetByIdAsync(Guid id)
        {
            return await ctx.Walks
                .Include(w => w.Difficulty)
                .Include(w => w.Region)
                .FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task<Walk?> UpdateAsync(Guid id, Walk Walk)
        {
            var walkExistedModel = await ctx.Walks
                .Include(w => w.Difficulty)
                .Include(w => w.Region)
                .FirstOrDefaultAsync(w => w.Id == id);
            if (walkExistedModel == null)
            {
                return null;
            }
            walkExistedModel.LengthInKm = Walk.LengthInKm;
            walkExistedModel.WalkImageUrl = Walk.WalkImageUrl;
            walkExistedModel.RegionId = Walk.RegionId;
            walkExistedModel.Description = Walk.Description;
            walkExistedModel.DifficultyId = Walk.DifficultyId;
            walkExistedModel.Name = Walk.Name;
            await ctx.SaveChangesAsync();
            return walkExistedModel;
        }

        public async Task<Walk?> DeleteAsync(Guid id)
        {
            var existedWalk = await ctx.Walks
                .Include(w => w.Difficulty)
                .Include(w => w.Region)
                .FirstOrDefaultAsync(w => w.Id == id);

            if (existedWalk == null)
                return null;

            ctx.Walks.Remove(existedWalk);
            await ctx.SaveChangesAsync();
            return existedWalk;
        }
    }
}
