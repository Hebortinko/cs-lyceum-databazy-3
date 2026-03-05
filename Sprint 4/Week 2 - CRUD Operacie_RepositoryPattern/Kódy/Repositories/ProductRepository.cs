using EshopApp.Models;
namespace EshopApp.Repositories;

public class ProductRepository
{
    private Database _db;

    public ProductRepository(Database db)
    {
        _db = db;
    }
    
    // CREATE
    public void Create(Product product)
    {
        var price = product.Price.ToString(System.Globalization.CultureInfo.InvariantCulture);

        _db.Execute($"INSERT INTO products (name, price, instock) " +
                    $"VALUES ('{product.Name}', {price}, {product.InStock})");
        Console.WriteLine($"Product '{product.Name}' added!");
    }
    // READ ALL
    public List<Product> GetAll()
    {
        var reader = _db.Query("SELECT * FROM products");
        var products = new List<Product>();

        while (reader.Read())
        {
            products.Add(new Product
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Price = reader.GetDecimal(2),
                InStock = reader.GetInt32(3)
            });
        }
        reader.Close();
        return products;
    }
    
    // READ ONE
    public Product GetByID(int id)
    {
        var reader = _db.Query($"SELECT * FROM products WHERE id = {id}");

        if (reader.Read())
        {
            var product = new Product
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Price = reader.GetDecimal(2),
                InStock = reader.GetInt32(3)
            };
            reader.Close();
            return product;
        }
        reader.Close();
        return null;
    }
    
    // UPDATE
    public void Update(int id, Product product)
    {
        var price = product.Price.ToString(System.Globalization.CultureInfo.InvariantCulture);
    
        _db.Execute($"UPDATE products SET name = '{product.Name}', " +
                    $"price = {price}, instock = {product.InStock} " +
                    $"WHERE id = {id}");
        Console.WriteLine($"Product {id} updated!");
    }
    
    // DELETE
    public void Delete(int id)
    {
        _db.Execute($"DELETE FROM products WHERE id = {id}");
        Console.WriteLine($"Product {id} deleted!");
    }
}