using System.Data.Entity;
using Campus.Core.Models;
using Campus.Core.Scripts;

namespace Campus.Core
{
    class DataInitializer : DropCreateDatabaseIfModelChanges<DataContext>
    {
        protected override void Seed(DataContext context)
        {
            DbInitializer.GenerateInitData(context);
        }
    }

    public class DataContext : DbContext
    {
        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Dorm> Dorms { get; set; }
        public DbSet<Room> Rooms { get; set; }

        public DataContext() : base("Dorm")
        {
            Database.SetInitializer(new DataInitializer());
        }
    }
}