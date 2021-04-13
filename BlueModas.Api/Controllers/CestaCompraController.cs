using BlueModas.Api.Data;
using BlueModas.Api.Dtos;
using BlueModas.Api.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace BlueModas.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CestaCompraController : ControllerBase
	{
		private readonly CestaCompraRepository _cestaCompraRepository;
		private readonly ProdutoRepository _produtoRepository;
		private readonly ClienteRepository _clienteRepository;

		public CestaCompraController(CestaCompraRepository cestaCompraRepository, ProdutoRepository produtoRepository, ClienteRepository clienteRepository)
		{
			_cestaCompraRepository = cestaCompraRepository;
			_produtoRepository = produtoRepository;
			_clienteRepository = clienteRepository;
		}

		[HttpGet("resumo/{id}")]
		public dynamic ObterResumoCompra(int id)
		{
			return _cestaCompraRepository.ObterResumo(id);
		}

		[HttpGet("resumo-cesta/{id}")]
		public dynamic ObterResumoDaCesta(int id)
		{
			return _cestaCompraRepository.ObterResumoDaCesta(id);
		}

		[HttpPost("adicionar")]
		public int Post([FromBody] AdicionarProdutoDto adicionarProdutoDto)
		{
			if (adicionarProdutoDto.IdCompra.HasValue)
			{
				var cesta = _cestaCompraRepository.ObterCompraPorId(adicionarProdutoDto.IdCompra.Value);
				var itemCompra = cesta.Itens.FirstOrDefault(p => p.ProdutoId == adicionarProdutoDto.IdProduto);
				if (itemCompra != null)
				{
					itemCompra.Incrementar();
					_cestaCompraRepository.AtualizarItem(itemCompra);
				}
				else
				{
					itemCompra = new ItemCompra();
					itemCompra.ProdutoId = adicionarProdutoDto.IdProduto;
					itemCompra.CestaCompraId = cesta.CestaCompraId;
					itemCompra.Quantidade++;

					var produto = _produtoRepository.ObterPorId(itemCompra.ProdutoId);

					itemCompra.ValorUnitario = produto.PrecoProduto;

					_cestaCompraRepository.SalvarItem(itemCompra);
				}

				return cesta.CestaCompraId;
			}
			else
			{
				var cesta = _cestaCompraRepository.SalvarCesta(new CestaCompra());

				var itemCompra = new ItemCompra();
				itemCompra.ProdutoId = adicionarProdutoDto.IdProduto;
				itemCompra.CestaCompraId = cesta.CestaCompraId;
				itemCompra.Quantidade++;

				var produto = _produtoRepository.ObterPorId(itemCompra.ProdutoId);

				itemCompra.ValorUnitario = produto.PrecoProduto;

				_cestaCompraRepository.SalvarItem(itemCompra);

				return cesta.CestaCompraId;
			}
		}

		[HttpPut("finalizar-compra/{idCompra}")]
		public int FinalizarCompra(int idCompra, [FromBody] Cliente cliente)
		{
			if (string.IsNullOrEmpty(cliente.NomeCliente))
				throw new Exception("O nome do cliente é obrigatório");

			if (string.IsNullOrEmpty(cliente.Email))
				throw new Exception("O E-mail do cliente é obrigatório");

			if (string.IsNullOrEmpty(cliente.Telefone))
				throw new Exception("O Telefone do cliente é obrigatório");

			var compra = _cestaCompraRepository.ObterCompraPorId(idCompra);

			if (compra.ClienteId.HasValue)
				throw new Exception("Compra já finalizada!");

			int idCliente;
			var clienteResult = _clienteRepository.ObterPorEmail(cliente.Email);

			if (clienteResult == null)
			{
				idCliente = _clienteRepository.Salvar(cliente).ClienteId;
			}
			else
			{
				idCliente = clienteResult.ClienteId;
			}

			compra.ClienteId = idCliente;

			_cestaCompraRepository.AtualizarCompra(compra);

			return idCompra;
		}

		[HttpDelete("deletar-item/{id}")]
		public void DeletarItem(int id)
		{
			var itemCompra = _cestaCompraRepository.ObterPorItemId(id);
			_cestaCompraRepository.DeletarItem(itemCompra);
		}

		[HttpPut("incrementar/{id}")]
		public void IncrementarItem(int id)
		{
			var itemCompra = _cestaCompraRepository.ObterPorItemId(id);
			itemCompra.Incrementar();
			_cestaCompraRepository.AtualizarItem(itemCompra);
		}

		[HttpPut("decrementar/{id}")]
		public void DecrementarItem(int id)
		{
			var itemCompra = _cestaCompraRepository.ObterPorItemId(id);
			itemCompra.Decrementar();
			_cestaCompraRepository.AtualizarItem(itemCompra);
		}

	}
}