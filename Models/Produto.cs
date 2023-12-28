using System.Text.Json.Serialization;

namespace CatalogMinimalAPI.Models
{
    public class Produto
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public string? Descricao { get; set; }
        public decimal Preco { get; set; }
        public string? ImagemUrl { get; set; }
        public DateTime DataCompra { get; set; }
        public int Quantidade { get; set; }
        public int CategoriaId { get; set; }
        [JsonIgnore]
        public Categoria? Categoria { get; set; }
    }
}
