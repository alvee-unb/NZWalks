using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Repositories
{
    public class SQLWalkRepository : IWalkRepository
    {
        private readonly NZWalksDbContext dbContext;

        public SQLWalkRepository( NZWalksDbContext dbContext )
        {
            this.dbContext = dbContext;
        }
        public async Task< Walk > CreateAsync( Walk walk )
        {
            await dbContext.Walks.AddAsync( walk );
            await dbContext.SaveChangesAsync();

            return walk;
        }

        public async Task< List< Walk > > GetAllAsync( string? filterOn = null, string? filterQuery = null, string? sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 1000 )
        {
            var walks = dbContext.Walks
                .Include( "Difficulty" )
                .Include( "Region" )
                .AsQueryable();

            // Filtering by Name only.
            if( string.IsNullOrWhiteSpace( filterOn ) == false &&
                string.IsNullOrWhiteSpace( filterQuery ) == false )
            {
                if( filterOn.Contains( "Name", StringComparison.OrdinalIgnoreCase ) )
                {
                    walks = walks.Where( x => x.Name.Contains( filterQuery ) );
                }
            }

            // Sorting by Name or LengthInKm.
            if( string.IsNullOrWhiteSpace( sortBy ) == false )
            {
                if( sortBy.Equals( "Name", StringComparison.OrdinalIgnoreCase ) )
                {
                    walks = isAscending ? walks.OrderBy( x => x.Name ) : walks.OrderByDescending( x => x.Name );
                }
                else if( sortBy.Equals( "Length", StringComparison.OrdinalIgnoreCase ) )
                {
                    walks = isAscending ? walks.OrderBy( x => x.LengthInKm ) : walks.OrderByDescending( x => x.LengthInKm );
                }
            }

            // Pagination.
            var skipResults = ( pageNumber - 1 ) * pageSize;
            walks = walks.Skip( skipResults ).Take( pageSize);

            // Return Walks.
            return await walks.ToListAsync();
        }

        public async Task< Walk? > GetByIdAsync( Guid id )
        {
            return await dbContext.Walks
                .Include( "Region" )
                .Include( "Difficulty" )
                .FirstOrDefaultAsync( x => x.Id == id );
        }

        public async Task< Walk? > UpdateWalkAsync( Guid id, Walk walk )
        {
            var existingWalk = await dbContext.Walks.FirstOrDefaultAsync( x => x.Id == id );

            if( existingWalk == null )
                return null;

            existingWalk.Name = walk.Name;
            existingWalk.Description = walk.Description;
            existingWalk.LengthInKm = walk.LengthInKm;
            existingWalk.DifficultyId = walk.DifficultyId;
            existingWalk.RegionId = walk.RegionId;

            await dbContext.SaveChangesAsync();

            return existingWalk;
        }

        public async Task< Walk? > DeleteAsync( Guid id )
        {
            var existingWalk = dbContext.Walks.FirstOrDefault( x => x.Id == id );

            if( existingWalk == null )
                return null;

            dbContext.Walks.Remove( existingWalk );
            await dbContext.SaveChangesAsync();
            return existingWalk;
        }
    }
}
