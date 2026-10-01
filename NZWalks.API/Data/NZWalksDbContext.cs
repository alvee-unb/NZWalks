using Microsoft.EntityFrameworkCore;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Data
{
    public class NZWalksDbContext : DbContext
    {
        public NZWalksDbContext( DbContextOptions< NZWalksDbContext > dbContextOptions ): base( dbContextOptions )
        {
            
        }

        public DbSet< Difficulty > Difficulties { get; set; }
        public DbSet< Region > Regions { get; set; }
        public DbSet< Walk > Walks { get; set; }

        public DbSet< Image > Images { get; set; }

        protected override void OnModelCreating( ModelBuilder modelBuilder )
        {
            base.OnModelCreating( modelBuilder );

            // Seed the data for difficulties.
            // Easy, Medium, Hard.
            var difficulties = new List<Difficulty>
            {
                new Difficulty
                {
                    Id = Guid.Parse( "b90bef48-ee7a-4642-a325-edfc1e937652" ),
                    Name = "Easy",
                },
                new Difficulty
                {
                    Id = Guid.Parse( "80b45334-5da6-4ac0-87ab-38f3aa21e028" ),
                    Name = "Medium",
                },
                new Difficulty
                {
                    Id = Guid.Parse( "bfdad830-43e7-46fb-a747-606598e966a8" ),
                    Name = "Hard",
                }
            };

            modelBuilder.Entity< Difficulty >().HasData( difficulties );


            // Seed data for Regions.
            var regions = new List<Region>
            {
                new Region
                {
                    Id = Guid.Parse( "2f63bce6-6180-4e46-86a9-e8d520d361b0" ),
                    Name = "Auckland",
                    Code = "AKL",
                    RegionImageUrl = "https://test-img.com"
                },
                new Region
                {
                    Id = Guid.Parse( "c527d5da-5ff5-4357-b41a-39020a5dee03" ),
                    Name = "Bay of Plenty",
                    Code = "BOP",
                    RegionImageUrl = "https://bop.jpg"
                },
                new Region
                {
                    Id = Guid.Parse( "ab64dc99-dcb0-44bf-96de-807c77ce3430" ),
                    Name = "Wellington",
                    Code = "WGN",
                    RegionImageUrl = "https://wgn.jpg"
                },
                new Region
                {
                    Id = Guid.Parse( "8a50f623-e511-4d5d-aaa9-e13664b6bfe1" ),
                    Name = "Nelson",
                    Code = "NSN",
                    RegionImageUrl = "https://nsn.jpg"
                },
                new Region
                {
                    Id = Guid.Parse( "6e87f11f-f820-4420-9f4e-73b77f04598f" ),
                    Name = "Southland",
                    Code = "STL",
                    RegionImageUrl = null
                },
            };

            modelBuilder.Entity< Region >().HasData( regions );
        }
    }
}
