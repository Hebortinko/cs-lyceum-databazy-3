// See https://aka.ms/new-console-template for more information

using EshopApp2;
using EshopApp2.Models;
using EshopApp2.Repositories;

LoadEnv.LoadFile(".env");
var db = new Database();

ProductRepository repo = new ProductRepository(db);

/*
var list = repo.GetAll();
for (int i = 0; i < list.Count; i++)
{
    Console.WriteLine(list[i].Name);
}
*/

var orders = repo.GetOrderDetails();
for (int i = 0; i < orders.Count; i++)
{
    OrderDetail order = orders[i];
    Console.WriteLine($"Zakaznik s menom: {order.CustomerName} si objednal {order.ProductName} s hodnotou {order.Total}");
}