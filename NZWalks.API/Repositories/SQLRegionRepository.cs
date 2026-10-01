using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class SQLRegionRepository : IRegionRepository
    {
        private readonly NZWalksDbContext dbContext;

        public SQLRegionRepository( NZWalksDbContext dbContext )
        {
            this.dbContext = dbContext;
        }

        public async Task< List< Region > > GetAllAsync()
        {
            return await dbContext.Regions.ToListAsync();
        }

        public async Task< Region? > GetByIdAsync( Guid id )
        {
            return await dbContext.Regions.FirstOrDefaultAsync( x => x.Id == id );
        }

        public async Task< Region > CreateAsync( Region region )
        {
            await dbContext.Regions.AddAsync( region );
            await dbContext.SaveChangesAsync();
            return region;
        }

        public async Task< Region? > UpdateAsync( Guid id, Region region )
        {
            // Get existing region.
            var existingRegion = await dbContext.Regions.FirstOrDefaultAsync( x => x.Id == id );

            // Check if region exists.
            if( existingRegion == null )
            {
                return null;
            }

            // Update region.
            existingRegion.Code = region.Code;
            existingRegion.Name = region.Name;
            existingRegion.RegionImageUrl = region.RegionImageUrl;
            
            // Save changes.
            await dbContext.SaveChangesAsync();

            return region;
        }

        public async Task< Region? > DeleteAsync( Guid id )
        {
            // Get existing region.
            var existingRegion = await dbContext.Regions.FirstOrDefaultAsync( x => x.Id == id );

            // Check if region exists.
            if( existingRegion == null )
            {
                return null;
            }

            // Delete region.
            dbContext.Regions.Remove( existingRegion );
            await dbContext.SaveChangesAsync();

            return existingRegion;
        }
    }
}
