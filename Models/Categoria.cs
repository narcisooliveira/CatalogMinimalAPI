namespace CatalogMinimalAPI.Models
{
    public class Categoria
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public string? Descricao { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public ICollection<Produto>? Produtos { get; set; }
    }
}
