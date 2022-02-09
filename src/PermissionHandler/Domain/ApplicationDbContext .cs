using Domain.Mappings;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Domain
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{
		}

		public DbSet<Server> Servers { get; set; }
		public DbSet<Controller> Controllers { get; set; }
		public DbSet<Action> Actions { get; set; }
		public DbSet<Property> Properties { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfiguration(new BaseMapping<Server>());
			modelBuilder.ApplyConfiguration(new BaseMapping<Controller>());
			modelBuilder.ApplyConfiguration(new BaseMapping<Action>());
			modelBuilder.ApplyConfiguration(new BaseMapping<Property>());
		}
	}
}
