using CatalogMinimalAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogMinimalAPI.Context;

public class CatalogContext : DbContext
{
    public CatalogContext(DbContextOptions<CatalogContext> options) : base(options) { }
    public DbSet<Produto>? Produtos { get; set; }
    public DbSet<Categoria>? Categorias { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Categoria
        modelBuilder.Entity<Categoria>().HasKey(c => c.Id);
        modelBuilder.Entity<Categoria>().Property(c => c.Nome).IsRequired().HasMaxLength(50);
        modelBuilder.Entity<Categoria>().Property(c => c.Descricao).IsRequired().HasMaxLength(200);

        // Produto 
        modelBuilder.Entity<Produto>().HasKey(p => p.Id);
        modelBuilder.Entity<Produto>().Property(p => p.Nome).IsRequired().HasMaxLength(50);
        modelBuilder.Entity<Produto>().Property(p => p.Descricao).IsRequired().HasMaxLength(200);
        modelBuilder.Entity<Produto>().Property(p => p.ImagemUrl).IsRequired().HasMaxLength(100);
        modelBuilder.Entity<Produto>().Property(p => p.Preco).HasPrecision(14, 2);

        // Relacionamento
        modelBuilder.Entity<Produto>().HasOne(p => p.Categoria)
                                      .WithMany(c => c.Produtos)
                                      .HasForeignKey(p => p.CategoriaId);

        // Categoria Seed
        modelBuilder.Entity<Categoria>().HasData(
            new Categoria { Id = 1, Nome = "Material Escolar", Descricao = "Material Escolar" });

        // Produto Seed
        modelBuilder.Entity<Produto>().HasData(
        new Produto
        {
            Id = 1,
            Nome = "Caderno",
            Descricao = "Caderno espiral 100 folhas",
            Preco = 7.45M,
            ImagemUrl = "http://www.macoratti.net/Imagens/produtos/caderno.jpg",
            DataCompra = DateTime.Parse("2021-01-01"),
            Quantidade = 10,
            CategoriaId = 1
        },
        new Produto
        {
            Id = 2,
            Nome = "Borracha",
            Descricao = "Borracha branca pequena",
            Preco = 3.75M,
            ImagemUrl = "http://www.macoratti.net/Imagens/produtos/borracha.jpg",
            DataCompra = DateTime.Parse("2021-01-01"),
            Quantidade = 20,
            CategoriaId = 1
        },
        new Produto
        {
            Id = 3,
            Nome = "Estojo",
            Descricao = "Estojo escolar transparente",
            Preco = 5.50M,
            ImagemUrl = "http://www.macoratti.net/Imagens/produtos/estojo.jpg",
            DataCompra = DateTime.Parse("2021-01-01"),
            Quantidade = 15,
            CategoriaId = 1
        },
        new Produto
        {
            Id = 4,
            Nome = "Apontador",
            Descricao = "Apontador com depósito",
            Preco = 4.75M,
            ImagemUrl = "http://www.macoratti.net/Imagens/produtos/apontador.jpg",
            DataCompra = DateTime.Parse("2021-01-01"),
            Quantidade = 30,
            CategoriaId = 1
        },
        new Produto
        {
            Id = 5,
            Nome = "Caneta",
            Descricao = "Caneta esferográfica azul",
            Preco = 1.50M,
            ImagemUrl = "http://www.macoratti.net/Imagens/produtos/caneta.jpg",
            DataCompra = DateTime.Parse("2021-01-01"),
            Quantidade = 40,
            CategoriaId = 1
        });
    }


}
