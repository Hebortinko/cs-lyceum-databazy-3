# Poznámky – PostgreSQL a C# (Úvod do databáz)

---

## Obsah

1. [Čo je databáza?](#1-čo-je-databáza)
2. [Čo je PostgreSQL?](#2-čo-je-postgresql)
3. [Ako C# komunikuje s databázou?](#3-ako-c-komunikuje-s-databázou)
4. [Prvé pripojenie – bez tried](#4-prvé-pripojenie--bez-tried)
5. [Premenné prostredia a .env súbor](#5-premenné-prostredia-a-env-súbor)
6. [Trieda LoadEnv](#6-trieda-loadenv)
7. [Trieda Database](#7-trieda-database)
8. [Trieda App](#8-trieda-app)
9. [Prečo takto? – OOP myslenie](#9-prečo-takto--oop-myslenie)
10. [Finálna štruktúra projektu](#10-finálna-štruktúra-projektu)

---

## 1. Čo je databáza?

Databáza je miesto kde **trvalo ukladáme dáta**. Na rozdiel od premenných v C# ktoré zmiznú keď program skončí – dáta v databáze zostanú aj po vypnutí počítača.

Databázu si môžeš predstaviť ako Excel:

| id | meno       | email            |
|----|------------|------------------|
| 1  | Ján Novák  | jan@skola.sk     |
| 2  | Anna Kováč | anna@skola.sk    |

- **Databáza** = celý Excel súbor
- **Tabuľka** = jeden hárok (sheet)
- **Stĺpec** = jeden typ informácie (meno, email...)
- **Riadok** = jeden záznam (jeden študent)

---

## 2. Čo je PostgreSQL?

PostgreSQL je **databázový server** – program ktorý beží na počítači a spravuje databázy. Tvoj C# program sa naň pripojí, pošle mu príkaz (SQL) a server vykoná čo mu povieš.

```
Tvoj C# program  →  posiela SQL príkazy  →  PostgreSQL server  →  ukladá/číta dáta
```

**SQL** (Structured Query Language) je jazyk ktorým hovoríme databáze čo má robiť:

```sql
-- Načítaj všetkých študentov
SELECT * FROM studenti;

-- Pridaj nového študenta
INSERT INTO studenti (meno, email) VALUES ('Ján', 'jan@skola.sk');

-- Vytvor tabuľku
CREATE TABLE studenti (
    id    SERIAL PRIMARY KEY,
    meno  VARCHAR(100),
    email VARCHAR(100)
);
```

---

## 3. Ako C# komunikuje s databázou?

Na komunikáciu s PostgreSQL z C# používame knižnicu **Npgsql**. Nainštalujeme ju cez terminál:

```bash
dotnet add package Npgsql
```

Npgsql zabezpečí všetku komunikáciu medzi naším programom a PostgreSQL serverom – nemusíme riešiť ako to technicky funguje "pod kapotou".

---

## 4. Prvé pripojenie – bez tried

Predtým než sme začali organizovať kód do tried, pozrime sa ako základné pripojenie funguje:

```csharp
using Npgsql;

// 1. Connection string – "adresa" databázy
string connString = "Host=localhost;Port=5432;Database=webappschool;Username=samuel;Password=;";

// 2. Vytvor objekt pripojenia
var conn = new NpgsqlConnection(connString);

// 3. Otvor pripojenie (fyzicky sa pripoj na server)
conn.Open();
Console.WriteLine("Pripojenie OK!");

// 4. Zatvor pripojenie keď skončíš
conn.Close();
```

### Connection string – čo znamenajú jednotlivé časti?

```
Host=localhost       → server beží na tomto počítači
Port=5432            → port na ktorom PostgreSQL počúva (default)
Database=webappschool → názov databázy ku ktorej sa pripájame
Username=samuel      → meno používateľa
Password=            → heslo (prázdne = žiadne heslo)
```

### SELECT – čítanie dát

```csharp
// Vytvor SQL príkaz
var cmd = new NpgsqlCommand("SELECT * FROM studenti", conn);

// Spusti príkaz a získaj reader (čítač výsledkov)
var reader = cmd.ExecuteReader();

// Čítaj riadok po riadku kým sú nejaké výsledky
while (reader.Read())
{
    Console.WriteLine($"Meno: {reader["meno"]}, Email: {reader["email"]}");
}

reader.Close();
```

`reader.Read()` vráti `true` ak existuje ďalší riadok, `false` ak sme prečítali všetko. Preto ho dávame do `while` cyklu.

### INSERT – vkladanie dát

```csharp
var cmd = new NpgsqlCommand(
    "INSERT INTO studenti (meno, email) VALUES ('Peter', 'peter@skola.sk')",
    conn
);

// ExecuteNonQuery = spusti príkaz ktorý NEvracia dáta
cmd.ExecuteNonQuery();
Console.WriteLine("Záznam pridaný!");
```

Rozdiel medzi `ExecuteReader()` a `ExecuteNonQuery()`:
- `ExecuteReader()` – použiješ pri **SELECT** kde očakávaš dáta späť
- `ExecuteNonQuery()` – použiješ pri **INSERT, UPDATE, DELETE, CREATE** kde dáta späť nečakáš

---

## 5. Premenné prostredia a .env súbor

### Problém

Connection string obsahuje citlivé údaje – meno, heslo, názov databázy. Keby sme ho nechali priamo v kóde a dali projekt na GitHub, každý by videl naše heslo.

### Riešenie – .env súbor

`.env` je jednoduchý textový súbor kde ukladáme citlivé nastavenia **mimo kódu**:

```
DB_HOST=localhost
DB_PORT=5432
DB_NAME=webappschool
DB_USER=samuel
DB_PASSWORD=
```

Tento súbor pridáme do `.gitignore` – Git ho bude ignorovať a nikdy ho nenahrá na GitHub:

```
# .gitignore
.env
```

### Premenné prostredia (Environment Variables)

Premenné prostredia sú **premenné ktoré existujú na úrovni operačného systému**, nie len v našom programe. C# má vstavanú triedu `Environment` na prácu s nimi:

```csharp
// Nastav premennú
Environment.SetEnvironmentVariable("DB_HOST", "localhost");

// Načítaj premennú
string host = Environment.GetEnvironmentVariable("DB_HOST");
// host = "localhost"
```

Premenná existuje len počas behu programu. Preto ich pri každom spustení načítame z `.env` súboru.

---

## 6. Trieda LoadEnv

Táto trieda má jedinú úlohu – prečítať `.env` súbor a nahrať všetky premenné do `Environment`.

```csharp
namespace ConsoleApp1;

public class LoadEnv
{
    public static bool LoadFile(string path)
    {
        // Ak súbor neexistuje, vráť false
        if (!File.Exists(path))
        {
            return false;
        }

        // Prejdi každý riadok súboru
        foreach (var line in File.ReadAllLines(path))
        {
            // Preskočí prázdne riadky a komentáre (# poznámka)
            if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#")) continue;

            // Rozdeľ "DB_HOST=localhost" na ["DB_HOST", "localhost"]
            // číslo 2 = maximálne 2 časti (kvôli heslám kde môže byť '=' v hodnote)
            var parts = line.Split('=', 2);

            if (parts.Length == 2)
            {
                Environment.SetEnvironmentVariable(parts[0].Trim(), parts[1].Trim());
            }
        }

        return true;
    }
}
```

### Prečo `static`?

Metóda `LoadFile` je `static` čo znamená že ju môžeme volať **bez vytvárania objektu**:

```csharp
// Bez static by sme museli písať:
var loader = new LoadEnv();
loader.LoadFile(".env");

// So static stačí:
LoadEnv.LoadFile(".env");
```

Dáva to zmysel – `LoadEnv` nemá žiadny stav (žiadne premenné ktoré si pamätá), len vykonáva jednu akciu. Pre takéto prípady je `static` ideálne.

### Prečo `Split('=', 2)`?

Bez čísla `2` by heslo ako `moje=heslo=123` rozdelilo na 3 časti a stratili by sme časť hesla:

```csharp
"DB_PASSWORD=moje=heslo".Split('=')
// výsledok: ["DB_PASSWORD", "moje", "heslo"]  ❌

"DB_PASSWORD=moje=heslo".Split('=', 2)
// výsledok: ["DB_PASSWORD", "moje=heslo"]  ✅
```

---

## 7. Trieda Database

Táto trieda sa stará o **fyzické pripojenie na databázu** a vykonávanie SQL príkazov.

```csharp
using Npgsql;

namespace ConsoleApp1;

public class Database
{
    // Súkromná premenná – pripojenie na databázu
    private NpgsqlConnection _conn;

    // Konštruktor – spustí sa pri "new Database()"
    public Database()
    {
        string connString = GetConnectionString();
        _conn = CreateDatabaseConnection(connString);
        OpenDatabaseConnection();
        Console.WriteLine("Pripojenie OK!");
    }

    // Poskladá connection string z premenných prostredia
    private string GetConnectionString()
    {
        return $"Host={Environment.GetEnvironmentVariable("DB_HOST")};" +
               $"Port={Environment.GetEnvironmentVariable("DB_PORT")};" +
               $"Database={Environment.GetEnvironmentVariable("DB_NAME")};" +
               $"Username={Environment.GetEnvironmentVariable("DB_USER")};" +
               $"Password={Environment.GetEnvironmentVariable("DB_PASSWORD")};";
    }

    // Vytvorí objekt pripojenia
    private NpgsqlConnection CreateDatabaseConnection(string connString)
    {
        return new NpgsqlConnection(connString);
    }

    // Otvorí pripojenie
    private void OpenDatabaseConnection()
    {
        _conn.Open();
    }

    // Na SELECT príkazy – vracia dáta
    public NpgsqlDataReader Query(string sql)
    {
        var cmd = new NpgsqlCommand(sql, _conn);
        return cmd.ExecuteReader();
    }

    // Na CREATE, INSERT, UPDATE, DELETE – nevracia dáta
    public void Execute(string sql)
    {
        var cmd = new NpgsqlCommand(sql, _conn);
        cmd.ExecuteNonQuery();
        Console.WriteLine("OK!");
    }

    // Zatvorí pripojenie
    public void Close()
    {
        _conn.Close();
    }
}
```

### Prečo `private` a `public`?

- `private` – metóda/premenná je dostupná **len vnútri tejto triedy**
- `public` – metóda/premenná je dostupná **odkiaľkoľvek**

```csharp
var db = new Database();

db.Query("SELECT ...");    // ✅ public – dostupné zvonka
db.Execute("INSERT ...");  // ✅ public – dostupné zvonka
db._conn;                  // ❌ private – zvonka nedostupné
db.GetConnectionString();  // ❌ private – zvonka nedostupné
```

Prečo skrývame `_conn` a `GetConnectionString()`? Lebo zvonka nikto nepotrebuje vedieť ako presne sa pripájame – stačí im vedieť že `Query()` a `Execute()` fungujú. Toto sa volá **zapuzdrenie (encapsulation)** – jeden zo základných princípov OOP.

### Prečo `_conn` (s podčiarkovníkom)?

Konvencia v C# – premenné triedy (fields) ktoré sú `private` píšeme s `_` na začiatku. Takto na prvý pohľad vieš že ide o premennú triedy a nie o lokálnu premennú metódy.

---

## 8. Trieda App

`App` je **prostredník** medzi `Program.cs` a `Database`. Poskytuje jednoduchšie metódy ktoré skladajú SQL za nás.

```csharp
namespace ConsoleApp1;

public class App
{
    private Database _db;

    public App()
    {
        // Načítaj .env a vytvor pripojenie
        bool loaded = LoadEnv.LoadFile(".env");
        if (!loaded)
        {
            Console.WriteLine("Could not find .env file");
        }
        _db = new Database();
    }

    // RAW metóda – pošle akýkoľvek SELECT
    public void Query(string sql)
    {
        var reader = _db.Query(sql);

        while (reader.Read())
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                Console.Write($"{reader.GetName(i)}: {reader[i]}  ");
            }
            Console.WriteLine();
        }
    }

    // RAW metóda – pošle akýkoľvek SQL príkaz
    public void Execute(string sql)
    {
        _db.Execute(sql);
    }

    // BUILDER – vytvorí tabuľku bez písania SQL
    public void CreateTable(string tableName, Dictionary<string, string> columns)
    {
        var cols = string.Join(", ", columns.Select(c => $"{c.Key} {c.Value}"));
        Execute($"CREATE TABLE IF NOT EXISTS {tableName} ({cols})");
    }

    // BUILDER – vloží záznam bez písania SQL
    public void Insert(string tableName, Dictionary<string, string> data)
    {
        var cols = string.Join(", ", data.Keys);
        var vals = string.Join(", ", data.Values.Select(v => $"'{v}'"));
        Execute($"INSERT INTO {tableName} ({cols}) VALUES ({vals})");
    }

    // BUILDER – vypíše všetky záznamy z tabuľky
    public void Select(string tableName)
    {
        Query($"SELECT * FROM {tableName}");
    }

    public void Close()
    {
        _db.Close();
    }
}
```

### Ako fungujú builder metódy?

**`CreateTable`** – rozbor riadok po riadku:

```csharp
// Vstup:
// tableName = "studenti"
// columns = { "id" → "SERIAL PRIMARY KEY", "meno" → "VARCHAR(100)" }

var cols = string.Join(", ", columns.Select(c => $"{c.Key} {c.Value}"));
```

`columns.Select(c => ...)` prejde každý pár kľúč-hodnota a spojí ich:
```
"id" + " " + "SERIAL PRIMARY KEY"  →  "id SERIAL PRIMARY KEY"
"meno" + " " + "VARCHAR(100)"      →  "meno VARCHAR(100)"
```

`string.Join(", ", ...)` spojí všetky výsledky čiarkou:
```
"id SERIAL PRIMARY KEY, meno VARCHAR(100)"
```

Výsledný SQL:
```sql
CREATE TABLE IF NOT EXISTS studenti (id SERIAL PRIMARY KEY, meno VARCHAR(100))
```

---

**`Insert`** – rozbor:

```csharp
// Vstup:
// tableName = "studenti"
// data = { "meno" → "Ján Novák", "email" → "jan@skola.sk" }

var cols = string.Join(", ", data.Keys);
// cols = "meno, email"

var vals = string.Join(", ", data.Values.Select(v => $"'{v}'"));
// každú hodnotu obalíme do úvodzoviek (SQL to vyžaduje pre text)
// vals = "'Ján Novák', 'jan@skola.sk'"
```

Výsledný SQL:
```sql
INSERT INTO studenti (meno, email) VALUES ('Ján Novák', 'jan@skola.sk')
```

### Čo je lambda (`=>`)?

Lambda je **skrátený zápis funkcie**. Číta sa ako "pre každé X urob Y":

```csharp
c => $"{c.Key} {c.Value}"
// "pre každé c, vráť string zložený z c.Key, medzery a c.Value"

v => $"'{v}'"
// "pre každé v, obaľ ho do úvodzoviek"
```

Je to to isté ako keby si napísal celú metódu, len kratšie a priamo na mieste.

---

## 9. Prečo takto? – OOP myslenie

### Princíp jednej zodpovednosti (Single Responsibility)

Každá trieda má **jednu konkrétnu úlohu**:

| Trieda | Zodpovednosť |
|--------|-------------|
| `LoadEnv` | Načítať `.env` súbor |
| `Database` | Spravovať pripojenie a vykonávať SQL |
| `App` | Poskytnúť jednoduché metódy pre prácu s DB |
| `Program` | Spustiť aplikáciu a volať metódy |

Prečo je to dôležité? Predstav si že chceš zmeniť databázu z PostgreSQL na niečo iné. Ak máš všetko v jednom súbore, musíš prerobiť celý program. Ak máš `Database.cs` oddelene, zmeníš len ten jeden súbor.

### Zapuzdrenie (Encapsulation)

Skrývame detaily implementácie za jednoduché metódy:

```csharp
// Bez zapuzdrenia – zvonka vidíš všetko, môžeš omylom niečo pokaziť
conn.Open();
var cmd = new NpgsqlCommand(sql, conn);
cmd.ExecuteNonQuery();

// So zapuzdrením – jednoduchá metóda, detaily sú skryté
db.Execute(sql);
```

### Znovupoužiteľnosť

`Database` a `LoadEnv` môžeš použiť v akomkoľvek inom projekte bez zmeny. Skopíruješ súbory a fungujú.

---

## 10. Finálna štruktúra projektu

```
ConsoleApp1/
├── .env                ← citlivé nastavenia (NIKDY na GitHub!)
├── .gitignore          ← .env je tu zapísaný
├── Program.cs          ← vstupný bod, len volania
├── App.cs              ← builder metódy, prostredník
├── Database.cs         ← pripojenie + raw SQL
└── LoadEnv.cs          ← načítanie .env súboru
```

### Program.cs – takto vyzerá finálne použitie

```csharp
using ConsoleApp1;

var app = new App();

// Vytvor tabuľku
app.CreateTable("studenti", new Dictionary<string, string>
{
    { "id", "SERIAL PRIMARY KEY" },
    { "meno", "VARCHAR(100)" },
    { "email", "VARCHAR(100)" }
});

// Vlož dáta
app.Insert("studenti", new Dictionary<string, string>
{
    { "meno", "Ján Novák" },
    { "email", "jan@skola.sk" }
});

// Načítaj dáta
app.Select("studenti");

app.Close();
```

`Program.cs` je teraz čistý a čitateľný – vidíš čo program robí bez toho aby si musel rozumieť ako to funguje vnútri. Toto je cieľ dobrého OOP kódu.

---

## Zhrnutie – čo sme sa naučili

| Koncept | Čo to je |
|---------|----------|
| PostgreSQL | Databázový server kde ukladáme dáta |
| Connection string | "Adresa" databázy pre pripojenie |
| `NpgsqlConnection` | Objekt reprezentujúci pripojenie |
| `ExecuteReader()` | Spustí SELECT a vráti dáta |
| `ExecuteNonQuery()` | Spustí INSERT/CREATE/DELETE bez návratovej hodnoty |
| `.env` | Súbor s citlivými nastaveniami mimo kódu |
| Environment variables | Premenné dostupné počas behu programu |
| `private` / `public` | Kontrola čo je viditeľné zvonka triedy |
| `static` | Metóda dostupná bez vytvorenia objektu |
| Lambda `=>` | Skrátený zápis funkcie |
| Single Responsibility | Každá trieda má jednu úlohu |
| Encapsulation | Skrývanie detailov za jednoduché metódy |

---

> ⚠️ **Dôležité:** `.env` súbor **nikdy nedávaj na GitHub**. Vždy ho pridaj do `.gitignore`. Obsahuje heslá a prístupové údaje ktoré musia zostať súkromné.
