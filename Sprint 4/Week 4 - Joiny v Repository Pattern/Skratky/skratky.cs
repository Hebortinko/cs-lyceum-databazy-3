
// TOTO
public class OrderDetail
{
    public int OrderId { get; set; }
    public string CustomerName { get; set; }
    public string ProductName { get; set; }
    public decimal Total { get; set; }
}


// Je len skrateny zapis pre toto 
public class OrderDetail
{
    private int _orderId;
    private string _customerName;
    private string _productName;
    private decimal _total;

    public int OrderId
    {
        get { return _orderId; }
        set { _orderId = value; }
    }

    public string CustomerName
    {
        get { return _customerName; }
        set { _customerName = value; }
    }

    public string ProductName
    {
        get { return _productName; }
        set { _productName = value; }
    }

    public decimal Total
    {
        get { return _total; }
        set { _total = value; }
    }
}



//TOTO
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

//Je len skrateny zapis tohto 
public List<Product> GetAll()
{
    var reader = _db.Query("SELECT * FROM products");
    var products = new List<Product>();

    while (reader.Read())
    {
        Product product = new Product();
        
        product.Id = reader.GetInt32(0);
        product.Name = reader.GetString(1);
        product.Price = reader.GetDecimal(2);
        product.InStock = reader.GetInt32(3);
        products.Add(product);
    }
    reader.Close();
    return products;
}