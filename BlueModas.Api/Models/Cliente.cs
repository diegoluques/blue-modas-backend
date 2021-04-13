using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlueModas.Api.Models
{
	public class Cliente
	{
		public int ClienteId { get; set; }
		public string NomeCliente { get; set; }
		public string Email { get; set; }
		public string Telefone { get; set; }

		public ICollection<CestaCompra> CestasCompra { get; set; }
	}
}