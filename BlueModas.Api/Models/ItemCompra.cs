using System;
using System.ComponentModel.DataAnnotations;

namespace BlueModas.Api.Models
{
	public class ItemCompra
	{
		public int ItemCompraId { get; set; }

		public int CestaCompraId { get; set; }
		public CestaCompra CestaCompra { get; set; }

		public int ProdutoId { get; set; }
		public virtual Produto Produto { get; set; }

		public int Quantidade { get; set; }
		public decimal ValorUnitario { get; set; }

		public decimal ObterValorTotal() => Quantidade * ValorUnitario;

		public void Incrementar()
		{
			this.Quantidade++;
		}
		public void Decrementar()
		{
			if (Quantidade - 1 == 0)
				throw new Exception("A quantidade não pode ficar Zero");

			this.Quantidade--;
		}
	}
}