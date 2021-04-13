using BlueModas.Api.Data;
using BlueModas.Api.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BlueModas.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProdutoController : ControllerBase
	{
		private readonly ProdutoRepository _produtoRepository;

		public ProdutoController(ProdutoRepository produtoRepository)
		{
			_produtoRepository = produtoRepository;
		}

		[HttpGet]
		public IEnumerable<Produto> Get()
		{
			return _produtoRepository.ObterTodos();
		}

		[HttpGet("{id}")]
		public Produto Get(int id)
		{
			return _produtoRepository.ObterPorId(id);
		}
		
		private byte[] GetBytes(Microsoft.AspNetCore.Http.IFormFile file)
		{
			using (var ms = new System.IO.MemoryStream())
			{
				file.CopyTo(ms);
				return ms.ToArray();
			}
		}

		[HttpPut("{id}")]
		public Produto Put(int id, [FromBody] Produto produto)
		{
			produto.ProdutoId = id;
			return _produtoRepository.Atualizar(produto);
		}

		[HttpDelete("{id}")]
		public void Delete(int id)
		{
			var produto = _produtoRepository.ObterPorId(id);

			_produtoRepository.Deletar(produto);
		}
	}
}