using BlueModas.Api.Data;
using BlueModas.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace BlueModas.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProdutoController : ControllerBase
	{
		private readonly ProdutoRepository _produtoRepository;
		private readonly CestaCompraRepository _cestaCompraRepository;
		private readonly IConfiguration _configuration;

		public ProdutoController(ProdutoRepository produtoRepository, IConfiguration configuration, CestaCompraRepository cestaCompraRepository)
		{
			_produtoRepository = produtoRepository;
			_configuration = configuration;
			_cestaCompraRepository = cestaCompraRepository;
		}

		[HttpGet]
		public IEnumerable<dynamic> Get()
		{
			return _produtoRepository.ObterTodos();
			//.Select(p => new
			//{
			//	ProdutoId = p.ProdutoId,
			//	NomeProduto = p.NomeProduto,
			//	PrecoProduto = p.PrecoProduto,
			//	UrlImage = GetUrl() + p.UrlImage
			//});
		}

		[HttpGet("{id}")]
		public Produto Get(int id)
		{
			return _produtoRepository.ObterPorId(id);
		}

		public class ProdutoDto
		{
			public string NomeProduto { get; set; }
			public decimal PrecoProduto { get; set; }

			public IFormFile Foto { get; set; }

		}
		public class ProdutoAtualizacaoDto
		{
			public string NomeProduto { get; set; }
			public decimal PrecoProduto { get; set; }
			 
		}

		private string GetUrl()
		{
			return Request.Scheme + "://" + Request.Host.Value + "/";
		}

		[HttpPost()]
		public Produto Post([FromForm] ProdutoDto produtoDto)
		{
			var produto = new Produto();
			PopularProduto(produtoDto, produto);

			return _produtoRepository.Salvar(produto);
		}

		private void PopularProduto(ProdutoDto produtoDto, Produto produto)
		{
			produto.NomeProduto = produtoDto.NomeProduto;
			produto.PrecoProduto = produtoDto.PrecoProduto;

			var bytes = GetBytes(produtoDto.Foto);

			System.IO.File.WriteAllBytes($"Assets\\Images\\{produtoDto.Foto.FileName}", bytes);
			produto.UrlImage = GetUrl() + $"Assets/Images/{produtoDto.Foto.FileName}";

		}

		private byte[] GetBytes(IFormFile file)
		{
			using (var ms = new System.IO.MemoryStream())
			{
				file.CopyTo(ms);
				return ms.ToArray();
			}
		}

		[HttpPut("troca-imagem/{id}")]
		public Produto PutTrocarImagem(int id, [FromForm] ProdutoDto produtoDto)
		{
			var produto = this._produtoRepository.ObterPorId(id);
			ExcluirArquivoDisco(produto);

			PopularProduto(produtoDto, produto);
			produto.ProdutoId = id;

			return _produtoRepository.Atualizar(produto);
		}

		[HttpPut("{id}")]
		public Produto Put(int id, [FromBody] ProdutoAtualizacaoDto produtoDto)
		{
			var produto = this._produtoRepository.ObterPorId(id);

			produto.NomeProduto = produtoDto.NomeProduto;
			produto.PrecoProduto = produtoDto.PrecoProduto;

			return _produtoRepository.Atualizar(produto);
		}

		private void ExcluirArquivoDisco(Produto produto)
		{
			var caminho = produto.UrlImage.Replace(GetUrl(), "");
			var caminhoCompleto = _configuration.GetSection("AppSettings:CurrentDir").Value + "\\" + caminho;

			if (System.IO.File.Exists(caminhoCompleto))
				System.IO.File.Delete(caminhoCompleto);
		}

		[HttpDelete("{id}")]
		public void Delete(int id)
		{
			var produto = this._produtoRepository.ObterPorId(id);

			var possuiCompras = this._cestaCompraRepository.ProdutoFoiVendido(produto.ProdutoId);
			if (possuiCompras) throw new System.Exception("Produto não pode ser apagado pois já foi vendido");

			_produtoRepository.Deletar(produto);
			ExcluirArquivoDisco(produto);
		}
	}
}