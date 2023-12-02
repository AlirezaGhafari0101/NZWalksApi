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
    }
}
