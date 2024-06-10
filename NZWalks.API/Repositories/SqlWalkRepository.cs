using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Repositories.IRepositories;

namespace NZWalks.API.Repositories
{
    public class SqlWalkRepository : IWalkRepository
    {
        private readonly NZWalksDbContext dbContext;

        public SqlWalkRepository(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Walk> CreateWalkAsync(Walk walk)
        {
            await this.dbContext.Walks.AddAsync(walk);
            await this.dbContext.SaveChangesAsync();

            return walk;
        }

        public async Task<Walk> DeleteWalkAsync(Guid id)
        {
            var walk = await this.dbContext.Walks.FirstOrDefaultAsync(w => w.Id == id);

            if (walk != null)
            {
                this.dbContext.Walks.Remove(walk);
                await this.dbContext.SaveChangesAsync();
            }

            return walk;
        }

        public async Task<List<Walk>> GetAllWalksAsync(string? filterOn = null, string? filterQuery = null, string? sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 1000)
        {
            var walks = this.dbContext.Walks.Include(x => x.Difficulty).Include(x => x.Region).AsQueryable();

            //filter walks based on the filterOn and filterQuery parameters
            if (!string.IsNullOrWhiteSpace(filterOn) && !string.IsNullOrWhiteSpace(filterQuery))
            {
                if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = walks.Where(w => w.Name.Contains(filterQuery));
                }
            }

            //sort walks based on the sortBy and isAscending parameters
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending ? walks.OrderBy(w => w.Name) : walks.OrderByDescending(w => w.Name);
                }
                else if (sortBy.Equals("Length", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending ? walks.OrderBy(w => w.LengthInKm) : walks.OrderByDescending(w => w.LengthInKm);
                }
            }

            //paginate the walks
            var skipAmount = pageSize * (pageNumber - 1);

            return await walks.Skip(skipAmount).Take(pageSize).ToListAsync();
        }

        public async Task<Walk?> GetWalkByIdAsync(Guid id)
        {
            return await this.dbContext.Walks
                                        .Include(w => w.Difficulty)
                                        .Include(w => w.Region).
                                        FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task<Walk> UpdateWalkAsync(Guid id, Walk walk)
        {
            var existingWalk = await this.dbContext.Walks.FirstOrDefaultAsync(w => w.Id == id);

            if (existingWalk != null)
            {
                existingWalk.Name = walk.Name;
                existingWalk.Description = walk.Description;
                existingWalk.LengthInKm = walk.LengthInKm;
                existingWalk.WalkImageUrl = walk.WalkImageUrl;
                existingWalk.DifficultyId = walk.DifficultyId;
                existingWalk.RegionId = walk.RegionId;

                await this.dbContext.SaveChangesAsync();
            }

            return existingWalk;
        }
    }
}