using System.Text.Json.Serialization;

namespace CatalogMinimalAPI.Models
{
    public class Categoria
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public string? Descricao { get; set; }
        [JsonIgnore]
        public ICollection<Produto>? Produtos { get; set; }
    }
}
