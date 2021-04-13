using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlueModas.Api.Models
{
	public class CestaCompra
	{
		public int CestaCompraId { get; set; }
		public   List<ItemCompra> Itens { get; set; }

		public int? ClienteId { get; set; }
		public Cliente Cliente { get; set; }
	}
}