# Cvičenia – Rozcvička (Hodina 1)

> Precvičíme objekty, listy, slovníky a SQL pred hlavnou látkou.

---

## Časť 1 – Objekty a Metódy

### Cvičenie 1.1 – Trieda Student (🌶️)

Vytvor triedu `Student` s vlastnosťami `Name`, `Age` a `Grade` (známka 1–5).

Pridaj do nej tieto metódy:
- `Greet()` – vypíše pozdrav so menom študenta
- `IsPassing()` – vráti `true` ak je známka 1–3, inak `false`
- `Describe()` – vypíše všetky informácie o študentovi vrátane toho či prospel

Vytvor 3 študentov a zavolaj `Describe()` na každého.

---

### Cvičenie 1.2 – Trieda BankAccount (🌶️🌶️)

Vytvor triedu `BankAccount` s vlastnosťami `Owner` (meno majiteľa) a `Balance` (zostatok, začína na 0).

Pridaj metódy:
- `Deposit(decimal amount)` – pridá sumu na účet
- `Withdraw(decimal amount)` – odčíta sumu z účtu, ale len ak je dostatok peňazí. Ak nie, vypíše chybovú hlášku.
- `PrintBalance()` – vypíše aktuálny zostatok

Otestuj účet – vlož peniaze, vyber časť, skús vybrať viac ako máš.

---

### Cvičenie 1.3 – Komunikácia medzi objektmi (🌶️🌶️)

Vytvor triedu `Product` s vlastnosťami `Name`, `Price`, `InStock`.

Vytvor triedu `ShoppingCart` (košík) ktorá obsahuje `List<Product>` a tieto metódy:
- `AddProduct(Product product)` – pridá produkt do košíka
- `RemoveProduct(string name)` – odstráni produkt podľa názvu
- `GetTotal()` – vráti celkovú cenu všetkých produktov v košíku
- `PrintCart()` – vypíše všetky produkty a celkovú sumu

V `Program.cs` vytvor 4 produkty, pridaj ich do košíka, jeden odstráň a vypíš obsah košíka aj celkovú sumu.

---

### Cvičenie 1.4 – Trieda Classroom (🌶️🌶️🌶️)

Vytvor triedu `Classroom` ktorá obsahuje `List<Student>` (zo cvičenia 1.1) a tieto metódy:
- `AddStudent(Student student)` – pridá študenta do triedy
- `GetPassingStudents()` – vráti `List<Student>` len tých čo prospeli
- `GetFailingStudents()` – vráti `List<Student>` len tých čo neprospeli
- `GetAverageAge()` – vráti priemerný vek triedy
- `PrintSummary()` – vypíše počet študentov, koľko prospelo a koľko neprospelo

Vytvor triedu s aspoň 5 študentmi a zavolaj `PrintSummary()`.

---

## Časť 2 – Listy a Slovníky

### Cvičenie 2.1 – Práca s Listom (🌶️)

Vytvor `List<int>` s číslami: `5, 12, 3, 8, 21, 7, 15, 2`.

Bez použitia vstavaných sort metód napíš vlastný kód ktorý:
1. Nájde najväčšie číslo
2. Nájde najmenšie číslo
3. Vypočíta súčet všetkých čísel
4. Vypíše len párne čísla

---

### Cvičenie 2.2 – Slovník (🌶️🌶️)

Vytvor `Dictionary<string, int>` kde kľúč je názov predmetu a hodnota je počet hodín týždenne.

Pridaj aspoň 5 predmetov. Potom:
1. Vypíš všetky predmety a ich hodinové dotácie
2. Vypíš celkový počet hodín týždenne
3. Vypíš predmet s najväčším počtom hodín
4. Skontroluj či slovník obsahuje predmet "Matematika" – ak áno vypíš jeho hodinový dotáciu, ak nie vypíš "Predmet nenájdený"

---

### Cvičenie 2.3 – Slovník s Poľom (🌶️🌶️🌶️)

Vytvor dátovú štruktúru `Dictionary<string, List<string>>` kde:
- **kľúč** = názov triedy (napr. `"1A"`, `"1B"`, `"2A"`)
- **hodnota** = zoznam mien študentov v tej triede

Naplň slovník aspoň 3 triedami, každá nech má aspoň 3 študentov.

Potom napíš kód ktorý:
1. Vypíše všetky triedy a ich študentov
2. Vypíše celkový počet študentov v škole
3. Nájde a vypíše triedu s najviac študentmi
4. Pridá nového študenta do existujúcej triedy
5. Vypíše či sa v škole nachádza študent menom "Peter Novák"

---

## Časť 3 – SQL Zopakovanie

### Cvičenie 3.1 – Základné príkazy (🌶️)

Napíš SQL príkazy pre tieto situácie:

```
a) Vyber všetky produkty kde cena je medzi 10 a 100
b) Vyber produkty zoradené podľa názvu abecedne
c) Vyber prvých 5 najdrahších produktov
d) Vyber počet produktov ktoré sú na sklade (instock > 0)
e) Vypočítaj priemernú cenu všetkých produktov
```

---

### Cvičenie 3.2 – Agregačné funkcie (🌶️🌶️)

Máš tabuľku `orders` (objednávky):

```
id | customer  | product    | quantity | price
1  | Ján       | Notebook   | 1        | 999.99
2  | Anna      | Myš        | 2        | 29.99
3  | Ján       | Klávesnica | 1        | 49.99
4  | Peter     | Notebook   | 1        | 999.99
5  | Anna      | Monitor    | 1        | 299.99
```

Napíš SQL ktoré:
1. Vypíše celkovú sumu každého zákazníka
2. Vypíše zákazníka ktorý minul najviac
3. Vypíše koľko objednávok má každý zákazník
4. Vypíše len zákazníkov ktorí minuli viac ako 500€

**Hint:** `GROUP BY`, `SUM()`, `COUNT()`, `HAVING`

---

### Cvičenie 3.3 – JOIN (🌶️🌶️)

Máš tieto tabuľky:

```
products:                    categories:
id | name      | cat_id      id | name
1  | Notebook  | 1           1  | Elektronika
2  | Myš       | 1           2  | Oblečenie
3  | Tričko    | 2           3  | Šport
4  | Bežky     | 3
```

Napíš SQL ktoré:
1. Vypíše každý produkt spolu s názvom jeho kategórie
2. Vypíše len produkty z kategórie "Elektronika"
3. Vypíše počet produktov v každej kategórii

---

## Časť 4 – Kostra kódu (Doprogramuj!)

Máš pripravený projekt s `Database`, `LoadEnv` a `Product` triedou. Chýbajú ti dve veci – doprogramuj ich.

**`ProductRepository.cs`** – doprogramuj chýbajúce metódy:

```csharp
public class ProductRepository
{
    private Database _db;

    public ProductRepository(Database db)
    {
        _db = db;
    }

    public void Create(Product product)
    {
        var price = product.Price.ToString(System.Globalization.CultureInfo.InvariantCulture);
        _db.Execute($"INSERT INTO products (name, price, instock) " +
                    $"VALUES ('{product.Name}', {price}, {product.InStock})");
        Console.WriteLine($"Product '{product.Name}' added!");
    }

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

    public void Delete(int id)
    {
        _db.Execute($"DELETE FROM products WHERE id = {id}");
        Console.WriteLine($"Product {id} deleted!");
    }

    // TODO 1: GetByID – vráti jeden produkt podľa id
    public Product GetByID(int id) { }

    // TODO 2: Update – aktualizuje produkt podľa id
    public void Update(int id, Product product) { }

    // TODO 3: GetByPriceRange – vráti produkty kde cena je medzi min a max
    public List<Product> GetByPriceRange(decimal min, decimal max) { }
}
```

**`Program.cs`** – doprogramuj:

```csharp
LoadEnv.LoadFile(".env");
var db = new Database();

db.Execute(@"CREATE TABLE IF NOT EXISTS products (
    id      SERIAL PRIMARY KEY,
    name    VARCHAR(100),
    price   DECIMAL(10,2),
    instock INT
)");

var products = new ProductRepository(db);

// TODO 4: Pridaj 4 produkty rôznych cien

// TODO 5: Vypíš všetky produkty

// TODO 6: Aktualizuj cenu prvého produktu

// TODO 7: Vypíš produkty v cenovom rozmedzí ktoré si zvolíš

// TODO 8: Zmaž jeden produkt a vypíš zostatok

db.Close();
```

---

## Prehľad náročnosti

| Cvičenie | Téma | Náročnosť |
|----------|------|-----------|
| 1.1 | Trieda Student | 🌶️ |
| 1.2 | BankAccount | 🌶️🌶️ |
| 1.3 | ShoppingCart + Product | 🌶️🌶️ |
| 1.4 | Classroom + Student | 🌶️🌶️🌶️ |
| 2.1 | List operácie | 🌶️ |
| 2.2 | Slovník | 🌶️🌶️ |
| 2.3 | Slovník s Listom | 🌶️🌶️🌶️ |
| 3.1 | SQL základy | 🌶️ |
| 3.2 | GROUP BY + agregácie | 🌶️🌶️ |
| 3.3 | JOIN | 🌶️🌶️ |
| 4 | Kostra kódu | 🌶️🌶️ |
