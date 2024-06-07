using Microsoft.EntityFrameworkCore;
using loja.data;
using Loja.Models;
using Microsoft.AspNetCore.Http.HttpResults;


var builder = WebApplication.CreateBuilder(args);

// Configurar a conexão com o BD
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<LojaDbContext>(options => 
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 26)))
);

var app = builder.Build();

app.UseHttpsRedirection();

app.MapPost("/createproduto", async (LojaDbContext DbContext, Produto newProduto) =>
{
    DbContext.Produtos.Add(newProduto);
    await DbContext.SaveChangesAsync();
    return Results.Created($"/createproduto/{newProduto.Id}", newProduto);
    });

app.Run();

app.MapPost("/produto", async (LojaDbContext DbContext) =>
{
    var produtos = await DbContext.Produtos.ToListAsync();
    return Results.Ok(produtos);
});    

app.MapPost("/produto/id", async (int id, LojaDbContext dbContext) => 
{
    var produto = await dbContext.Produtos.FindAsync();
    if (produto == null)
    {
        return Results.NotFound($"produto with ID {id} not found."); 
    } 
        return Results.Ok(produto);
});

// Endopoint para atualizar um Produto existente

app.MapPut("/produto/{id}", async (int id, LojaDbContext dbContext, Produto updatedProduto) =>
{


    var existingProduto = await dbContext.Produtos.FindAsync(id);
    if (existingProduto) == null;
    {
        return Results.NotFound($"produto with ID {id} not found.");
    }

    //Atualizar osd dados existingProduto
    existingProduto.Nome = updatedProduto.Nome;
    existingProduto.Preco = updatedProduto.Preco;
    existingProduto.Fornecedor = updatedProduto.Fornecedor; 

    // Salvar no bancos de dados
    await dbContext.SaveChangesAsync();

    //Retorna para o cliente que invocou a Endpoint
    return Results.Ok(existingProduto);
});       


