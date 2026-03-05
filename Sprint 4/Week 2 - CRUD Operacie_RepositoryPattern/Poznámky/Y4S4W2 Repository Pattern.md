# Poznámky – CRUD a Repository Pattern v C#

---

## Obsah

1. [Čo je CRUD?](#1-čo-je-crud)
2. [Model trieda](#2-model-trieda)
3. [Repository Pattern](#3-repository-pattern)
4. [Štruktúra projektu](#4-štruktúra-projektu)
5. [Database trieda](#5-database-trieda)
6. [ProductRepository – všetky CRUD operácie](#6-productrepository--všetky-crud-operácie)
7. [Program.cs – skladáme to dokopy](#7-programcs--skladáme-to-dokopy)
8. [Časté chyby a ako ich opraviť](#8-časté-chyby-a-ako-ich-opraviť)
9. [Zhrnutie](#9-zhrnutie)

---

## 1. Čo je CRUD?

CRUD je skratka pre štyri základné operácie s databázou:

| Písmeno | Operácia | SQL príkaz |
|---------|----------|------------|
| **C** | Create – vytvorenie záznamu | `INSERT` |
| **R** | Read – čítanie záznamu | `SELECT` |
| **U** | Update – úprava záznamu | `UPDATE` |
| **D** | Delete – zmazanie záznamu | `DELETE` |

Každá aplikácia ktorá pracuje s databázou tieto štyri operácie používa – či je to e-shop, sociálna sieť alebo banková appka.

---

## 2. Model trieda

Model je **čistá dátová trieda** ktorá reprezentuje jeden riadok z tabuľky. Neobsahuje žiadnu logiku – len vlastnosti (properties).

```csharp
namespace EshopApp.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int InStock { get; set; }
}
```

Každá property zodpovedá jednému stĺpcu v tabuľke:

```
Tabuľka products:          Model Product:
─────────────────          ──────────────
id        (INT)      →     int Id
name      (VARCHAR)  →     string Name
price     (DECIMAL)  →     decimal Price
instock   (INT)      →     int InStock
```

### Prečo Model?

Bez Modelu by sme dáta posielali ako surové stringy alebo slovníky:

```csharp
// Bez Modelu – neprehľadné, ľahko urobíš chybu
Execute("INSERT INTO products VALUES ('Notebook', 999.99, 5)");

// S Modelom – jasné, prehľadné, bezpečné
products.Create(new Product { Name = "Notebook", Price = 999.99m, InStock = 5 });
```

### `{ get; set; }` – čo to znamená?

Každá property má getter (čítanie) a setter (zápis):

```csharp
var p = new Product();
p.Name = "Notebook";   // setter – zapisujeme hodnotu
Console.WriteLine(p.Name);  // getter – čítame hodnotu
```

---

## 3. Repository Pattern

Repository pattern hovorí: **každá tabuľka má svoju vlastnú triedu** ktorá obsahuje všetky CRUD operácie pre ňu.

```
ProductRepository  →  CRUD pre tabuľku products
CustomerRepository →  CRUD pre tabuľku customers
OrderRepository    →  CRUD pre tabuľku orders
```

### Tok dát

```
Program.cs
    ↓  products.Create(new Product { ... })
ProductRepository
    ↓  _db.Execute("INSERT INTO products ...")
Database
    ↓  NpgsqlCommand → ExecuteNonQuery()
PostgreSQL
```

Každá vrstva vie len o vrstve pod ňou:
- `Program.cs` nevie nič o SQL
- `ProductRepository` nevie nič o tom ako sa fyzicky pripája na databázu
- `Database` nevie nič o produktoch

---

## 4. Štruktúra projektu

```
EshopApp/
├── .env                          ← citlivé nastavenia (NIKDY na GitHub!)
├── .gitignore
├── Program.cs                    ← vstupný bod, len volania
├── LoadEnv.cs                    ← načítanie .env súboru
├── Database.cs                   ← pripojenie + raw SQL
├── Models/
│   └── Product.cs                ← dátová trieda
└── Repositories/
    └── ProductRepository.cs      ← CRUD operácie pre produkty
```

Priečinky `Models` a `Repositories` sú **namespaces** – spôsob ako organizovať triedy do logických skupín. Preto na začiatku súborov vidíš:

```csharp
namespace EshopApp.Models;      // trieda patrí do skupiny Models
namespace EshopApp.Repositories; // trieda patrí do skupiny Repositories
```

A pri použití musíš tieto namespaces importovať:

```csharp
using EshopApp.Models;
using EshopApp.Repositories;
```

---

## 5. Database trieda

```csharp
using Npgsql;

namespace EshopApp;

public class Database
{
    private NpgsqlConnection _conn;

    public Database()
    {
        string connString = $"Host={Environment.GetEnvironmentVariable("DB_HOST")};" +
                            $"Port={Environment.GetEnvironmentVariable("DB_PORT")};" +
                            $"Database={Environment.GetEnvironmentVariable("DB_NAME")};" +
                            $"Username={Environment.GetEnvironmentVariable("DB_USER")};" +
                            $"Password={Environment.GetEnvironmentVariable("DB_PASSWORD")};";

        _conn = new NpgsqlConnection(connString);
        _conn.Open();
        Console.WriteLine("Pripojenie OK!");
    }

    // Na SELECT – vracia dáta
    public NpgsqlDataReader Query(string sql)
    {
        var cmd = new NpgsqlCommand(sql, _conn);
        return cmd.ExecuteReader();
    }

    // Na INSERT, UPDATE, DELETE, CREATE – nevracia dáta
    public void Execute(string sql)
    {
        var cmd = new NpgsqlCommand(sql, _conn);
        cmd.ExecuteNonQuery();
    }

    public void Close()
    {
        _conn.Close();
    }
}
```

Táto trieda má jednu úlohu – **spravovať pripojenie a vykonávať SQL**. Nič iné nerieši.

---

## 6. ProductRepository – všetky CRUD operácie

```csharp
using EshopApp.Models;
namespace EshopApp.Repositories;

public class ProductRepository
{
    private Database _db;

    public ProductRepository(Database db)
    {
        _db = db;  // dostaneme pripojenie zvonka
    }

    // CREATE
    public void Create(Product product)
    {
        // InvariantCulture zaručí bodku v desatinnom čísle (nie čiarku)
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
```

### Prečo `InvariantCulture`?

Mac (a Windows) používa podľa jazykového nastavenia rôzne desatinné oddeľovače:

```
Slovenské nastavenie:  999.99m  →  "999,99"   ← PostgreSQL to číta ako dve čísla!
InvariantCulture:      999.99m  →  "999.99"   ← správne ✅
```

Preto vždy keď posielaš desatinné číslo do SQL stringu, použi `InvariantCulture`.

### Ako funguje `GetAll` – reader krok po kroku

```csharp
var reader = _db.Query("SELECT * FROM products");
// reader je ako kurzor – stojí pred prvým riadkom

while (reader.Read())
// Read() posunie kurzor na ďalší riadok, vráti true ak riadok existuje
{
    products.Add(new Product
    {
        Id = reader.GetInt32(0),    // stĺpec 0 = id
        Name = reader.GetString(1), // stĺpec 1 = name
        Price = reader.GetDecimal(2), // stĺpec 2 = price
        InStock = reader.GetInt32(3)  // stĺpec 3 = instock
    });
}
reader.Close(); // vždy zatvori reader!
```

Čísla `0, 1, 2, 3` zodpovedajú poradiu stĺpcov v tabuľke.

---

## 7. Program.cs – skladáme to dokopy

```csharp
using EshopApp;
using EshopApp.Models;
using EshopApp.Repositories;

// 1. Načítaj .env
LoadEnv.LoadFile(".env");

// 2. Pripoj sa
var db = new Database();

// 3. Vytvor tabuľku ak neexistuje
db.Execute(@"CREATE TABLE IF NOT EXISTS products (
    id      SERIAL PRIMARY KEY,
    name    VARCHAR(100),
    price   DECIMAL(10,2),
    instock INT
)");

// 4. Vytvor repository
var products = new ProductRepository(db);

// CREATE
products.Create(new Product { Name = "Notebook", Price = 999.99m, InStock = 5 });
products.Create(new Product { Name = "Myš", Price = 29.99m, InStock = 50 });
products.Create(new Product { Name = "Klávesnica", Price = 49.99m, InStock = 30 });

// READ ALL
Console.WriteLine("\n--- Všetky produkty ---");
var vsetky = products.GetAll();
foreach (var p in vsetky)
    Console.WriteLine($"ID: {p.Id} | {p.Name} | {p.Price}€ | Skladom: {p.InStock}ks");

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
foreach (var p in products.GetAll())
    Console.WriteLine($"ID: {p.Id} | {p.Name} | {p.Price}€ | Skladom: {p.InStock}ks");

db.Close();
```

---

## 8. Časté chyby a ako ich opraviť

### Chyba: `Value cannot be null (Parameter 'Host')`
`.env` súbor sa nenašiel. Skontroluj či je v správnom priečinku a či je nastavený `CopyToOutputDirectory` v `.csproj`.

### Chyba: `INSERT has more expressions than target columns`
Desatinné číslo obsahuje čiarku namiesto bodky. Oprav pomocou `InvariantCulture`:
```csharp
var price = product.Price.ToString(System.Globalization.CultureInfo.InvariantCulture);
```

### Chyba: `column does not exist`
Názov stĺpca v SQL sa nezhoduje s názvom v tabuľke. PostgreSQL je case-sensitive – `inStock` ≠ `instock`.

---

## 9. Zhrnutie

### Čo sme postavili

| Súbor | Úloha |
|-------|-------|
| `.env` | Citlivé nastavenia mimo kódu |
| `LoadEnv.cs` | Načítanie `.env` do Environment premenných |
| `Database.cs` | Pripojenie na PostgreSQL, `Query()` a `Execute()` |
| `Models/Product.cs` | Dátová trieda – jeden produkt = jeden riadok v DB |
| `Repositories/ProductRepository.cs` | Všetky CRUD operácie pre produkty |
| `Program.cs` | Volania – žiadny SQL, len čisté metódy |

### Kľúčové koncepty

| Koncept | Čo to je |
|---------|----------|
| CRUD | Create, Read, Update, Delete |
| Model | Trieda reprezentujúca riadok z tabuľky |
| Repository | Trieda so CRUD operáciami pre jednu entitu |
| `Query()` | Spustí SELECT, vracia dáta cez reader |
| `Execute()` | Spustí INSERT/UPDATE/DELETE/CREATE, nevracia dáta |
| `reader.Read()` | Posunie sa na ďalší riadok výsledku |
| `InvariantCulture` | Zaručí bodku v desatinnom čísle pri konverzii na string |
| `{ get; set; }` | Getter a setter pre čítanie a zápis property |

> ⚠️ `.env` súbor **nikdy nedávaj na GitHub**. Vždy ho pridaj do `.gitignore`.
