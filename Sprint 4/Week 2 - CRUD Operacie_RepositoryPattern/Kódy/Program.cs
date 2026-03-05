// See https://aka.ms/new-console-template for more information

using EshopApp;
using EshopApp.Models;
using EshopApp.Repositories;

LoadEnv.LoadFile(".env");

var db = new Database();

db.Execute("DROP TABLE IF EXISTS products");

db.Execute(@"CREATE TABLE IF NOT EXISTS products (
    id      SERIAL PRIMARY KEY,
    name    VARCHAR(100),
    price   DECIMAL(10,2),
    instock INT
)");

var products = new ProductRepository(db);

// CREATE
products.Create(new Product { Name = "Notebook", Price = 999.99m, InStock = 5 });
products.Create(new Product { Name = "Myš", Price = 29.99m, InStock = 50 });
products.Create(new Product { Name = "Klávesnica", Price = 49.99m, InStock = 30 });

// READ ALL
Console.WriteLine("\n--- Všetky produkty ---");
var vsetky = products.GetAll();
foreach (var p in vsetky)
{
    Console.WriteLine($"ID: {p.Id} | {p.Name} | {p.Price}€ | Skladom: {p.InStock}ks");
}

// READ ONE
Console.WriteLine("\n--- Jeden produkt ---");
var jeden = products.GetByID(1);
Console.WriteLine($"Našiel som: {jeden.Name} za {jeden.Price}€");

// UPDATE
Console.WriteLine("\n--- Update ---");
products.Update(1, new Product { Name = "Notebook Pro", Price = 1299.99m, InStock = 3 });


// DELETE
Console.WriteLine("\n--- Delete ---");
products.Delete(2);

// READ ALL znova
Console.WriteLine("\n--- Po zmenách ---");
var poZmenach = products.GetAll();
foreach (var p in poZmenach)
{
    Console.WriteLine($"ID: {p.Id} | {p.Name} | {p.Price}€ | Skladom: {p.InStock}ks");
}

db.Close();