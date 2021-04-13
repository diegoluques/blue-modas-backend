using BlueModas.Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BlueModas.Api.Data
{
	public class CestaCompraRepository
	{
		private readonly ApplicationContext _context;

		public CestaCompraRepository(ApplicationContext context)
		{
			_context = context;
		}
		public dynamic ObterResumo(int id)
		{
			return _context.CestaCompra
				.Include(c => c.Itens)
				.ThenInclude(ic => ic.Produto)
				.Include(c => c.Cliente)
				.Where(c => c.CestaCompraId == id)
				.ToList()
				.Select(c => new
				{
					CestaCompraId = c.CestaCompraId,
					Cliente = new
					{
						ClienteId = c.Cliente.ClienteId,
						NomeCliente = c.Cliente.NomeCliente,
						Email = c.Cliente.Email,
						Telefone = c.Cliente.Telefone
					},
					Itens = c.Itens,
					ValorTotal = c.Itens.Sum(ic => ic.ObterValorTotal())
				})
				.FirstOrDefault();
		}

		public dynamic ObterResumoDaCesta(int id)
		{
			var resumo = _context.CestaCompra
				.Include(c => c.Itens)
				.ThenInclude(ic => ic.Produto)
				.Where(c => c.CestaCompraId == id)
				.ToList()
				.Select(c => new
				{
					CestaCompraId = c.CestaCompraId,
					Itens = c.Itens,
					ValorTotal = c.Itens.Sum(ic => ic.ObterValorTotal())
				})
				.FirstOrDefault(); 
			return resumo;
		}

		public CestaCompra ObterCompraPorId(int id)
		{
			return _context.CestaCompra.Include(c => c.Itens).FirstOrDefault(c => c.CestaCompraId == id);
		}

		public ItemCompra ObterPorItemId(int id)
		{
			return _context.ItemCompra.Find(id);
		}

		public void AtualizarItem(ItemCompra itemCompra)
		{
			_context.ItemCompra.Update(itemCompra);
			_context.SaveChanges();
		}

		public void AtualizarCompra(CestaCompra cestaCompra)
		{
			_context.CestaCompra.Update(cestaCompra);
			_context.SaveChanges();
		}

		public CestaCompra SalvarCesta(CestaCompra cesta)
		{
			_context.CestaCompra.Add(cesta);
			_context.SaveChanges();

			return cesta;
		}

		
		public void SalvarItem(ItemCompra itemCompra)
		{
			_context.ItemCompra.Add(itemCompra);
			_context.SaveChanges();
		}

		public void DeletarItem(ItemCompra itemCompra)
		{
			_context.ItemCompra.Remove(itemCompra);
			_context.SaveChanges();
		}
	}
}