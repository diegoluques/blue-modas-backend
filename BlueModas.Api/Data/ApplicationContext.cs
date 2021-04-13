using BlueModas.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace BlueModas.Api.Data
{
	public class ApplicationContext : DbContext
	{
		public ApplicationContext(DbContextOptions<ApplicationContext> options) :base(options) { }

		public DbSet<Produto> Produto { get; set; }
		public DbSet<CestaCompra> CestaCompra { get; set; }
		public DbSet<ItemCompra> ItemCompra { get; set; }
		public DbSet<Cliente> Cliente { get; set; }
	}
}