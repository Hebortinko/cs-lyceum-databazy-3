# Cvičenia – CRUD a Repository Pattern

---

## Cvičenie 1 – Model (10 min)

Vytvor Model triedu pre zákazníka.

Tabuľka `customers` má tieto stĺpce:
- `id` – celé číslo, primárny kľúč
- `name` – text, max 100 znakov
- `email` – text, max 100 znakov
- `city` – text, max 100 znakov

**Úloha:** Vytvor súbor `Models/Customer.cs` s triedou `Customer` ktorá má rovnaké properties ako tabuľka.

---

## Cvičenie 2 – CREATE tabuľky (10 min)

V `Program.cs` pridaj SQL príkaz ktorý vytvorí tabuľku `customers` ak neexistuje.

**Hint:** Použi `db.Execute()` s `CREATE TABLE IF NOT EXISTS`.

**Bonus:** Pridaj aj `DROP TABLE IF EXISTS customers` pred CREATE aby si mal vždy čistú tabuľku pri testovaní.

---

## Cvičenie 3 – CustomerRepository (20 min)

Vytvor súbor `Repositories/CustomerRepository.cs`.

Implementuj tieto metódy:
- `Create(Customer customer)` – vloží nového zákazníka
- `GetAll()` – vráti `List<Customer>` všetkých zákazníkov
- `Delete(int id)` – zmaže zákazníka podľa id

**Hint:** Použi `ProductRepository.cs` ako vzor – štruktúra je rovnaká, len zmeníš názvy stĺpcov a tabuľky.

**Pozor:** Ak `city` môže byť `null`, použi `reader.IsDBNull(3) ? null : reader.GetString(3)` namiesto `reader.GetString(3)`.

---

## Cvičenie 4 – Použitie v Program.cs (10 min)

V `Program.cs` použi `CustomerRepository` aby si:

1. Vytvoril 3 zákazníkov
2. Vypísal všetkých zákazníkov
3. Zmazal jedného zákazníka podľa id
4. Znova vypísal všetkých zákazníkov

---

## Cvičenie 5 – GetByID (10 min) ⭐ Bonus

Pridaj do `CustomerRepository` metódu `GetByID(int id)` ktorá vráti jedného zákazníka podľa id.

Použi ju v `Program.cs` – načítaj zákazníka s id = 1 a vypíš jeho meno a email.

**Hint:** Čo sa stane ak zákazník s týmto id neexistuje? Ošetri tento prípad pomocou `if (jeden != null)`.

---

## Cvičenie 6 – UPDATE (15 min) ⭐⭐ Bonus

Pridaj do `CustomerRepository` metódu `Update(int id, Customer customer)` ktorá aktualizuje zákazníka.

Otestuj ju – zmeň email zákazníka s id = 1 a vypíš všetkých zákazníkov aby si overil zmenu.
