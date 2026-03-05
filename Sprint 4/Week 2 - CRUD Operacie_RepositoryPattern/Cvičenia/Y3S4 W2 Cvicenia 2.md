# Cvičenia – Week 2 (Bonus)

> Tieto cvičenia nadväzujú na základné cvičenia z hodiny. Predpokladá sa že už máš funkčný `ProductRepository` s CRUD operáciami.

---

## Cvičenie 1 – Filtrovanie (🌶️)

Pridaj do `ProductRepository` metódu `GetByName(string name)` ktorá vráti všetky produkty kde sa názov zhoduje.

```csharp
// Použitie:
var results = products.GetByName("Notebook");
```

**Hint:** Použi `WHERE name = '{name}'` v SQL.

**Bonus:** Uprav metódu aby vracala produkty kde názov **obsahuje** hľadaný výraz, nie len presná zhoda. SQL operátor `LIKE` a `%` ti pomôžu.

---

## Cvičenie 2 – Zoradenie (🌶️)

Pridaj metódu `GetAllSortedByPrice()` ktorá vráti všetky produkty zoradené od najlacnejšieho po najdrahší.

```csharp
var sorted = products.GetAllSortedByPrice();
```

**Bonus:** Pridaj parameter `bool descending = false` aby si vedel prepínať smer zoradenia:
```csharp
products.GetAllSortedByPrice();             // lacnejšie prvé
products.GetAllSortedByPrice(descending: true); // drahšie prvé
```

---

## Cvičenie 3 – Skladová logika (🌶️🌶️)

Pridaj metódu `GetOutOfStock()` ktorá vráti všetky produkty kde `instock = 0`.

Potom pridaj metódu `Restock(int id, int amount)` ktorá **zvýši** aktuálny stav skladu o zadané množstvo – bez toho aby si musel poznať aktuálnu hodnotu.

```csharp
products.Restock(1, 10); // pridá 10 kusov k aktuálnemu stavu
```

**Hint:** SQL vie robiť matematiku priamo: `SET instock = instock + {amount}`

---

## Cvičenie 4 – Počítanie a štatistiky (🌶️🌶️)

Pridaj do `ProductRepository` tieto metódy:

- `Count()` – vráti počet všetkých produktov ako `int`
- `GetTotalValue()` – vráti celkovú hodnotu skladu (`price * instock` pre každý produkt, spočítané)
- `GetMostExpensive()` – vráti jeden produkt s najvyššou cenou

```csharp
Console.WriteLine($"Počet produktov: {products.Count()}");
Console.WriteLine($"Hodnota skladu: {products.GetTotalValue()}€");
var top = products.GetMostExpensive();
Console.WriteLine($"Najdrahší: {top.Name} za {top.Price}€");
```

**Hint:** SQL funkcie `COUNT(*)`, `SUM(price * instock)`, `ORDER BY price DESC LIMIT 1`

---

## Cvičenie 5 – Druhá entita od nuly (🌶️🌶️)

Vytvor kompletnú štruktúru pre entitu `Category` (kategórie produktov):

1. Tabuľka `categories` so stĺpcami `id`, `name`, `description`
2. `Models/Category.cs`
3. `Repositories/CategoryRepository.cs` s plným CRUD

Potom v `Program.cs` vytvor 3 kategórie (napr. Elektronika, Príslušenstvo, Počítače) a vypíš ich.

---

## Cvičenie 6 – Ošetrenie chýb (🌶️🌶️🌶️)

Čo sa stane keď zavoláš `GetByID(999)` a produkt s týmto id neexistuje? Program spadne alebo vráti `null`.

Uprav `Program.cs` aby ošetril tento prípad:

```csharp
var product = products.GetByID(999);

if (product == null)
{
    Console.WriteLine("Produkt nenájdený!");
}
else
{
    Console.WriteLine($"Našiel som: {product.Name}");
}
```

**Bonus:** Uprav `GetByID` aby namiesto `null` vyhodil vlastnú výnimku:
```csharp
throw new Exception($"Produkt s ID {id} neexistuje.");
```

A v `Program.cs` ju odchyť pomocou `try/catch`.

---

## Cvičenie 7 – Mini inventárny systém (🌶️🌶️🌶️)

Spoj všetky metódy dohromady do malého scenára v `Program.cs`:

1. Vytvor tabuľku a vlož 5 produktov
2. Vypíš všetky produkty zoradené podľa ceny
3. Vypíš produkty ktoré sú vypredané (`instock = 0`)
4. Doplň sklad pre vypredané produkty pomocou `Restock()`
5. Vypíš celkovú hodnotu skladu
6. Zmaž najlacnejší produkt
7. Vypíš finálny stav skladu

Výsledok by mal vyzerať ako skutočný výpis inventárneho systému.
