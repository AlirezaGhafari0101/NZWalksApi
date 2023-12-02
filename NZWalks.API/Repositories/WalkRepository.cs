using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

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
    }
}
