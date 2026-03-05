# Week 2 – CRUD a Repository Pattern

## Čo sme robili

Druhý týždeň sme sa naučili ako správne organizovať databázový kód pomocou Repository Pattern – štandardného prístupu ktorý sa používa v reálnych projektoch.

## Čo sme naprogramovali

### `Models/Product.cs`
Trieda ktorá reprezentuje jeden riadok z tabuľky `products`. Každá property zodpovedá jednému stĺpcu v databáze. Model neobsahuje žiadnu logiku – len dáta.

### `Repositories/ProductRepository.cs`
Trieda so všetkými CRUD operáciami pre tabuľku `products`:
- `Create(Product product)` – vloží nový záznam
- `GetAll()` – vráti všetky záznamy ako `List<Product>`
- `GetByID(int id)` – vráti jeden záznam podľa id
- `Update(int id, Product product)` – aktualizuje záznam
- `Delete(int id)` – zmaže záznam

### `Program.cs`
Vstupný bod ktorý neobsahuje žiadny SQL – len volá metódy Repository. Výsledok čistého kódu kde každá vrstva má svoju zodpovednosť.

## Kľúčové koncepty

- **CRUD** – Create, Read, Update, Delete
- **Model** – trieda mapujúca riadok z tabuľky na C# objekt
- **Repository Pattern** – každá tabuľka má svoju triedu s CRUD operáciami
- **Single Responsibility** – každá trieda má jednu úlohu
- `reader.GetInt32()`, `reader.GetString()`, `reader.GetDecimal()` – čítanie dát podľa typu
- `InvariantCulture` – správne formátovanie desatinných čísel v SQL
