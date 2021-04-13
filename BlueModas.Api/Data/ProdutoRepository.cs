using BlueModas.Api.Models;
using System.Collections.Generic;

namespace BlueModas.Api.Data
{
	public class ProdutoRepository
	{
		private readonly ApplicationContext _context;

		public ProdutoRepository(ApplicationContext context)
		{
			_context = context;
		}

		public IEnumerable<Produto> ObterTodos()
		{
			return _context.Produto;
		}

		public Produto ObterPorId(int idProduto)
		{
			return _context.Produto.Find(idProduto);
		}

		public Produto Salvar(Produto produto)
		{
			_context.Produto.Add(produto);
			_context.SaveChanges();

			return produto;
		}

		public Produto Atualizar(Produto produto)
		{
			_context.Produto.Update(produto);
			_context.SaveChanges();

			return produto;
		}

		public void Deletar(Produto produto)
		{
			_context.Produto.Remove(produto);
			_context.SaveChanges();
		}
	}
}