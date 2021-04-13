using BlueModas.Api.Models;
using System.Linq;

namespace BlueModas.Api.Data
{
	public class ClienteRepository
	{
		private readonly ApplicationContext _context;

		public ClienteRepository(ApplicationContext context)
		{
			_context = context;
		}

		public Cliente ObterPorEmail(string email)
		{
			return _context.Cliente.FirstOrDefault(c => c.Email == email);
		}

		public Cliente Salvar(Cliente cliente)
		{
			_context.Cliente.Add(cliente);
			_context.SaveChanges();

			return cliente;
		}
	}
}