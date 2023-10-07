using CityInfoAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfoAPI.Models.DatabaseSessionConnection
{
    public class CityDbContext:DbContext
    {
        public DbSet<City> Cities { get; set; } = null!;
        public DbSet<PointOfInterest> PointOfInterests { get; set; } = null!;


        /* There are 2 ways to create connections string
         * 1. Inside DbContext by using OnConfiguration 
         * 2. Inside Program.cs while defining DbContext as a service
         */

        // 1. Method:
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlite("ConnectionString");
        //    base.OnConfiguring(optionsBuilder);
        //}

        // 2. Method is defined in Program.cs
        public CityDbContext(DbContextOptions<CityDbContext> options):base(options)
        {

        }

        // To Seed data, we need to override OnModelCreating method where we can configure model builder and seed some initial data.
        // This is used to manually construct our model and create initial data in the database
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>().HasData(
                new City()
                {
                    Id = 1,
                    Name = "Kristiansand",
                    Description = "5.biggest city"
                }
                ,
                new City()
                {
                    Id = 2,
                    Name = "Oslo",
                    Description = "Capital of Norway"                  
                },
                new City()
                {
                    Id = 3,
                    Name = "Boston",
                    Description = "Turgut and Marinda's city"               
                },
                new City
                {
                    Id = 4,
                    Name = "Bergen",
                    Description = "The most rainy city"
                });

            modelBuilder.Entity<PointOfInterest>().HasData(
                 new PointOfInterest()
                 {
                     Id = 1,
                     CityId = 1,
                     Name = "Dom Kirke",
                     Description = "Central Church in Kristiansand"
                 },
                 new PointOfInterest()
                 {
                        Id = 2,
                        CityId = 1,
                        Name = "Centrum",
                        Description = "Central Place in Kristiansand"
                 },
                  new PointOfInterest()
                  {
                      Id = 3,
                      CityId = 2,
                      Name = "Viking Museum",
                      Description = "Oldest and fameous museum in Oslo"
                  },
                  new PointOfInterest()
                  {
                        Id = 4,
                        CityId = 2,
                        Name = "Istanbul Khebab",
                        Description = "Best khebab in Oslo"
                  },
                  new PointOfInterest()
                  {
                      Id = 5,
                      CityId = 3,
                      Name = "Harvard University",
                      Description = "Best University in the world"
                  },
                    new PointOfInterest()
                    {
                        Id = 6,
                        CityId = 3,
                        Name = "Central Park",
                        Description = "Central Park in Boston"
                    }); 
            base.OnModelCreating(modelBuilder);
        }
    }
}
