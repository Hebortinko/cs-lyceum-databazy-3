# Week 1 – Pripojenie na Databázu v C#

## Čo sme robili

Prvý týždeň sme sa naučili ako pripojiť C# aplikáciu na PostgreSQL databázu a ako správne organizovať prístupové údaje mimo kódu.

## Čo sme naprogramovali

### `.env` + `LoadEnv.cs`
Citlivé údaje (host, port, heslo) nikdy nepíšeme priamo do kódu. Ukladáme ich do `.env` súboru ktorý sa nikdy nenahrá na GitHub. Trieda `LoadEnv` tento súbor načíta a nahrá premenné do systému.

### `Database.cs`
Trieda ktorá sa stará o fyzické pripojenie na PostgreSQL a vykonávanie SQL príkazov. Poskytuje dve metódy:
- `Query()` – na SELECT príkazy ktoré vracajú dáta
- `Execute()` – na INSERT, UPDATE, DELETE, CREATE ktoré dáta nevracajú

### `App.cs`
Prostredník medzi `Program.cs` a `Database`. Poskytuje helper metódy ako `CreateTable()`, `Insert()`, `Select()` ktoré skladajú SQL za nás – vhodné na demonštráciu bez písania SQL.

## Kľúčové koncepty

- Premenné prostredia (`Environment.GetEnvironmentVariable`)
- `private` vs `public` – zapuzdrenie
- `static` metódy – volanie bez vytvorenia objektu
- Rozdiel medzi `ExecuteReader()` a `ExecuteNonQuery()`
- Prečo `.env` patrí do `.gitignore`
