using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlueModas.Api.Models
{
	public class Produto
	{
		public int ProdutoId { get; set; }

		public string NomeProduto { get; set; }
		public decimal PrecoProduto { get; set; }
		public string UrlImage { get; set; }
		 
		public ICollection<ItemCompra> ItensCompra { get; set; }
	}
}