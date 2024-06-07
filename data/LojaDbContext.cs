using Loja.Models;
using Microsoft.EntityFrameworkCore;
using SeuProjeto.Models;


namespace loja.data{
    public class LojaDbContext : DbContext{

        public LojaDbContext(DbContextOptions<LojaDbContext> options) : base(options){}
        public DbSet<Produto> Produtos {get;set;}
        public DbSet<Fornecedor> Fornecedores { get; set; }

        public DbSet<Cliente> Clientes { get; set; }

    }
}
