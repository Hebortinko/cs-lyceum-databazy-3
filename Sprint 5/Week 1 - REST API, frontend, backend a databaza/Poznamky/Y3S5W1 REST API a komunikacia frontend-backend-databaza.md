# Poznamky - REST API a komunikacia frontend -> backend -> databaza

---

## Obsah

1. [Preco sa ucime REST API](#1-preco-sa-ucime-rest-api)
2. [Frontend, backend a databaza](#2-frontend-backend-a-databaza)
3. [Co je REST API](#3-co-je-rest-api)
4. [Ako vyzera request a response](#4-ako-vyzera-request-a-response)
5. [Struktura starter projektu](#5-struktura-starter-projektu)
6. [Frontend - odoslanie login requestu](#6-frontend---odoslanie-login-requestu)
7. [Backend - definicia route endpointu](#7-backend---definicia-route-endpointu)
8. [Request a Response triedy v C#](#8-request-a-response-triedy-v-c)
9. [AuthService - validacia a business logika](#9-authservice---validacia-a-business-logika)
10. [UserRepository - citanie usera z databazy](#10-userrepository---citanie-usera-z-databazy)
11. [Database.cs a .env subor](#11-databasecs-a-env-subor)
12. [SQL schema pre tabulku users](#12-sql-schema-pre-tabulku-users)
13. [Cely login flow krok za krokom](#13-cely-login-flow-krok-za-krokom)
14. [Preco to je rozdelene do vrstiev](#14-preco-to-je-rozdelene-do-vrstiev)
15. [Najcastejsie chyby](#15-najcastejsie-chyby)
16. [Zhrnutie](#16-zhrnutie)

---
### [Async vs Sync](https://youtu.be/pcMX9YJ-zQc?si=e40gTBO1atPLOsS1)

## 1. Preco sa ucime REST API

V sprinte 4 sme sa naucili:

- pripojit C# na PostgreSQL
- robit CRUD operacie
- pracovat s `Repository Pattern`
- mapovat riadky z databazy na objekty

Teraz ideme o krok dalej.

Databaza uz nebude samostatna tema. Ukazeme si, ako je zapojena do skutocnej aplikacie.

V realnej webovej appke to vacsinou funguje takto:

```text
frontend -> backend API -> databaza
```

Pouzivatel neklikne priamo na databazu. Klikne na tlacidlo vo frontende, frontend zavola backend a backend az potom komunikuje s databazou.

Prave toto je hlavna myslienka tejto hodiny.

---

## 2. Frontend, backend a databaza

### Frontend

Frontend je cast aplikacie ktoru vidi a ovlada pouzivatel.

Priklady:

- login formular
- zoznam produktov
- tlacidlo `Prihlasit sa`
- formular na rezervaciu

### Backend

Backend je serverova cast aplikacie.

Jeho uloha je:

- prijat request
- skontrolovat vstupne data
- vykonat logiku aplikacie
- nacitat alebo ulozit data do databazy
- vratit odpoved

### Databaza

Databaza je miesto kde sa data trvalo ukladaju.

Priklady:

- pouzivatelia
- produkty
- objednavky
- rezervacie
- hodnotenia

### Jednoducha predstava

```text
Frontend = to co vidi pouzivatel
Backend  = logika a pravidla aplikacie
Databaza = ulozisko dat
```

---

## 3. Co je REST API

REST API je sposob, akym frontend komunikuje s backendom cez HTTP.

Frontend neposiela SQL prikazy do databazy. Namiesto toho vola endpointy backendu.

Priklady endpointov:

- `GET /api/products`
- `GET /api/users/5`
- `POST /api/auth/login`
- `POST /api/orders`
- `DELETE /api/products/12`

### Co je endpoint

Endpoint je konkretna adresa funkcionality na backende.

Priklad:

```text
POST /api/auth/login
```

Toto znamena:

- `POST` = posielame data na server
- `/api/auth/login` = adresa funkcionality pre prihlasenie

### Zakladne HTTP metody

| Metoda | Pouzitie | Priklad |
|--------|----------|---------|
| `GET` | nacitanie dat | zoznam produktov |
| `POST` | odoslanie dat alebo vytvorenie akcie | login, registracia |
| `PUT` | uprava celeho zaznamu | uprava profilu |
| `PATCH` | ciastocna uprava | zmena statusu |
| `DELETE` | zmazanie zaznamu | zmazanie produktu |

### Preco sa pri logine pouziva `POST`

Pri logine posielame data na server:

- email
- heslo

Nechceme ich davat do URL adresy. Preto je `POST` vhodnejsi ako `GET`.

---

## 4. Ako vyzera request a response

### Login request z frontendu

Frontend posle na backend JSON:

```json
{
  "email": "eva@lyceum.sk",
  "password": "tajneheslo"
}
```

Toto je request body.

### Uspešna odpoved backendu

```json
{
  "success": true,
  "message": "Prihlasenie uspesne.",
  "displayName": "Eva",
  "email": "eva@lyceum.sk",
  "role": "student"
}
```

### Neuspesna odpoved backendu

```json
{
  "success": false,
  "message": "Nespravne heslo."
}
```

### Co si treba vsimnut

- frontend a backend si neposielaju HTML ani SQL
- posielaju si JSON data
- odpoved je strukturovana
- backend rozhoduje co sa vrati

---

## 5. Struktura starter projektu

Starter projekt je rozdeleny do viacerych casti:

```text
DatabazyApiStarter/
├── Program.cs
├── LoadEnv.cs
├── Database.cs
├── Models/
│   ├── LoginRequest.cs
│   ├── LoginResponse.cs
│   └── User.cs
├── Repositories/
│   └── UserRepository.cs
├── Services/
│   └── AuthService.cs
├── wwwroot/
│   └── index.html
└── sql/
    ├── 01_create_users.sql
    └── 02_seed_users.sql
```

### Co robi ktora cast

- `wwwroot/index.html` = frontend
- `Program.cs` = spustenie webovej aplikacie a definicia endpointov
- `Models/` = datove triedy
- `Services/AuthService.cs` = logika loginu a validacie
- `Repositories/UserRepository.cs` = komunikacia s databazou
- `Database.cs` = vytvorenie spojenia na PostgreSQL
- `sql/` = SQL skripty pre databazu

---

## 6. Frontend - odoslanie login requestu

V starter projekte frontend posiela login cez `fetch()`.

Kod:

```html
<form id="login-form">
  <label>
    Email
    <input id="email" name="email" type="email" required>
  </label>

  <label>
    Heslo
    <input id="password" name="password" type="password" required>
  </label>

  <button type="submit">Prihlasit sa</button>
</form>
```

Tento formular sam o sebe este backend nevola. Potrebujeme JavaScript:

```javascript
const form = document.getElementById("login-form");
const result = document.getElementById("result");

form.addEventListener("submit", async (event) => {
  event.preventDefault();

  const payload = {
    email: document.getElementById("email").value,
    password: document.getElementById("password").value
  };

  const response = await fetch("/api/auth/login", {
    method: "POST",
    headers: {
      "Content-Type": "application/json"
    },
    body: JSON.stringify(payload)
  });

  const data = await response.json();
  result.textContent = data.message;
});
```

### Co robi tento kod

#### `addEventListener("submit", ...)`

JavaScript sleduje odoslanie formulara.

Ked pouzivatel klikne na tlacidlo, spusti sa tento kod.

#### `event.preventDefault()`

Bez tohto prikazu by sa formular odoslal klasickym sposobom a stranka by sa obnovila.

My ale chceme request odoslat cez JavaScript, nie klasickym reloadom stranky.

#### `payload`

Sem si ukladame data z formulara:

```javascript
const payload = {
  email: document.getElementById("email").value,
  password: document.getElementById("password").value
};
```

Toto je bezny JavaScript objekt.

#### `fetch("/api/auth/login", ...)`

Tymto volame backend endpoint.

Dolezita je adresa:

```javascript
"/api/auth/login"
```

Zacina lomkou `/`, takze request ide na ten isty host a port z ktoreho je otvorena stranka.

Ak je stranka otvorena na:

```text
http://localhost:5000/
```

tak request ide na:

```text
http://localhost:5000/api/auth/login
```

#### `headers: { "Content-Type": "application/json" }`

Backendu hovorime, ze v tele requestu posielame JSON.

#### `body: JSON.stringify(payload)`

JavaScript objekt musime zmenit na JSON text, aby sa mohol poslat cez HTTP.

---

## 7. Backend - definicia route endpointu

Na backende je route definovana v `Program.cs`.

Kod:

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<Database>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<AuthService>();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapPost("/api/auth/login", async (LoginRequest request, AuthService authService) =>
{
    var result = await authService.LoginAsync(request);
    return Results.Ok(result);
});

app.Run();
```

### Co robi tento kod

#### `WebApplication.CreateBuilder(args)`

Vytvori ASP.NET aplikaciu.

#### `builder.Services.AddScoped<AuthService>()`

Registruje `AuthService`, aby ju vedel ASP.NET vytvorit a pouzit.

To iste plati pre `UserRepository` a `Database`.

#### `app.UseDefaultFiles()`

Ked otvorime root adresu `/`, ASP.NET hlada predvoleny subor, typicky `index.html`.

#### `app.UseStaticFiles()`

Povie ASP.NET, aby vedel servirovat staticke subory z priecinka `wwwroot`.

Preto funguje:

```text
wwwroot/index.html -> http://localhost:5000/
```

#### `app.MapPost("/api/auth/login", ...)`

Tu sa vytvara route pre login.

Toto je jedna z najdolezitejsich casti celeho backendu.

Rozpis:

- `MapPost` = tento endpoint reaguje na HTTP metodu `POST`
- `"/api/auth/login"` = URL cesta endpointu
- `LoginRequest request` = data z JSON requestu sa namapuju do C# objektu
- `AuthService authService` = ASP.NET vlozi service triedu ktoru potrebujeme

#### `return Results.Ok(result);`

Vysledok sa vrati klientovi ako HTTP 200 response s JSON telom.

---

## 8. Request a Response triedy v C#

Backend neprijima data ako nahodne stringy. Pouziva datove triedy.

### LoginRequest

```csharp
public class LoginRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
```

Tato trieda reprezentuje data ktore prisli z frontendu.

Frontend posle:

```json
{
  "email": "eva@lyceum.sk",
  "password": "tajneheslo"
}
```

ASP.NET z toho vytvori objekt:

```csharp
request.Email
request.Password
```

### LoginResponse

```csharp
public class LoginResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? DisplayName { get; set; }
    public string? Email { get; set; }
    public string? Role { get; set; }
}
```

Tato trieda reprezentuje odpoved ktoru backend posiela spat.

Vyhoda:

- data maju jasnu strukturu
- frontend vie co moze ocakavat
- kod je citatelnejsi

---

## 9. AuthService - validacia a business logika

`AuthService` obsahuje pravidla loginu.

Kod:

```csharp
public async Task<LoginResponse> LoginAsync(LoginRequest request)
{
    if (string.IsNullOrWhiteSpace(request.Email))
    {
        return CreateError("Email je povinny.");
    }

    if (string.IsNullOrWhiteSpace(request.Password))
    {
        return CreateError("Heslo je povinne.");
    }

    if (!request.Email.Contains('@'))
    {
        return CreateError("Email nema spravny format.");
    }

    var user = await _userRepository.GetByEmailAsync(request.Email.Trim());
    if (user is null)
    {
        return CreateError("Pouzivatel neexistuje.");
    }

    if (!user.IsActive)
    {
        return CreateError("Pouzivatel je neaktivny.");
    }

    if (user.Password != request.Password)
    {
        return CreateError("Nespravne heslo.");
    }

    return new LoginResponse
    {
        Success = true,
        Message = "Prihlasenie uspesne.",
        DisplayName = user.Name,
        Email = user.Email,
        Role = user.Role
    };
}
```

### Preco je tato logika v `AuthService`

`Program.cs` by mal byt co najjednoduchsi.

Endpoint ma len prijat request a posunut pracu dalej.

Samotne rozhodovanie:

- je email vyplneny?
- existuje user?
- je aktivny?
- sedi heslo?

patri do service vrstvy.

### Co je business logika

Business logika znamena pravidla aplikacie.

V nasom pripade napriklad:

- email nesmie byt prazdny
- heslo nesmie byt prazdne
- user musi existovat
- user musi byt aktivny
- heslo sa musi zhodovat

### Poznamka k heslam

V starter projekte sa heslo porovnava ako plain text:

```csharp
if (user.Password != request.Password)
```

To je v poriadku na prve pochopenie flowu.

V realnej aplikacii sa hesla nikdy neukladaju ako plain text. Pouziva sa hashing.

---

## 10. UserRepository - citanie usera z databazy

Repository vrstva komunikuje s databazou.

Kod:

```csharp
public async Task<User?> GetByEmailAsync(string email)
{
    await using var connection = _database.CreateConnection();
    await using var command = new NpgsqlCommand(@"
        SELECT id, name, email, password, is_active, role
        FROM users
        WHERE email = @email
        LIMIT 1;
    ", connection);

    command.Parameters.AddWithValue("email", email);

    await using var reader = await command.ExecuteReaderAsync();
    if (!await reader.ReadAsync())
    {
        return null;
    }

    return new User
    {
        Id = reader.GetInt32(0),
        Name = reader.GetString(1),
        Email = reader.GetString(2),
        Password = reader.GetString(3),
        IsActive = reader.GetBoolean(4),
        Role = reader.GetString(5)
    };
}
```

### Co robi tento kod

#### `CreateConnection()`

Vytvori spojenie s PostgreSQL.

#### `NpgsqlCommand`

Predstavuje SQL prikaz ktory sa ma vykonat.

#### SQL dotaz

```sql
SELECT id, name, email, password, is_active, role
FROM users
WHERE email = @email
LIMIT 1;
```

Tento dotaz:

- cita data z tabulky `users`
- hlada usera podla emailu
- vrati najviac jeden zaznam

#### `@email`

Toto je parameter.

Hodnota sa neprilepuje do SQL stringu rucne. Namiesto toho ju posleme bezpecne cez:

```csharp
command.Parameters.AddWithValue("email", email);
```

Toto je lepsie a bezpecnejsie ako skladat SQL cez string interpolation.

#### `reader.ReadAsync()`

Zisti, ci databaza vratila nejaky riadok.

- ak nie, vratime `null`
- ak ano, vytvorime objekt `User`

### Preco repository vracia objekt `User`

Je to prehladnejsie ako pracovat s polami alebo stringami.

Backend potom vie pisat:

```csharp
user.Name
user.Email
user.Role
```

Namiesto:

```csharp
reader[0]
reader[1]
reader[2]
```

---

## 11. Database.cs a .env subor

Na spojenie s databazou pouzivame triedu `Database`.

Kod:

```csharp
public class Database
{
    private readonly string _connectionString;

    public Database()
    {
        _connectionString = BuildConnectionString();
    }

    public NpgsqlConnection CreateConnection()
    {
        var connection = new NpgsqlConnection(_connectionString);
        connection.Open();
        return connection;
    }
}
```

### Odkial sa berie connection string

Z `.env` suboru:

```env
DB_HOST=localhost
DB_PORT=5432
DB_NAME=webappschool
DB_USER=samuel
DB_PASSWORD=
```

Tieto hodnoty sa nacitaju a pouziju pri skladani connection stringu.

Priklad vysledku:

```text
Host=localhost;Port=5432;Database=webappschool;Username=samuel;Password=;
```

### Preco nepist klientovi heslo do kodu

Keby bolo heslo natvrdo v `Program.cs` alebo `Database.cs`, bolo by:

- viditelne v kode
- rizikove pri pushnuti na GitHub
- horsie zmenitelne

Preto sa konfiguracia drzi mimo kodu.

---

## 12. SQL schema pre tabulku users

Tabulka `users` je vytvorena takto:

```sql
CREATE TABLE IF NOT EXISTS users (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    email VARCHAR(150) NOT NULL UNIQUE,
    password VARCHAR(120) NOT NULL,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    role VARCHAR(50) NOT NULL DEFAULT 'user'
);
```

### Co znamenaju jednotlive casti

#### `id SERIAL PRIMARY KEY`

- `SERIAL` = databaza automaticky generuje cislo
- `PRIMARY KEY` = jednoznacny identifikator riadku

#### `email VARCHAR(150) NOT NULL UNIQUE`

- `VARCHAR(150)` = text do dlzky 150 znakov
- `NOT NULL` = nesmie byt prazdny
- `UNIQUE` = ten isty email nemoze byt v tabulke dvakrat

#### `is_active BOOLEAN`

Boolean znamena `true` alebo `false`.

Pouzivame ho na informaciu, ci je ucet aktivny.

#### `role`

Tu si uchovavame typ pouzivatela, napriklad:

- `student`
- `teacher`
- `admin`

---

## 13. Cely login flow krok za krokom

Toto je najdolezitejsia cast celeho materialu.

```text
1. Pouzivatel vyplni email a heslo vo formulari
2. JavaScript zachyti submit formulara
3. Frontend vytvori objekt payload
4. Frontend posle POST /api/auth/login
5. ASP.NET route prijme request
6. JSON sa namapuje do LoginRequest
7. AuthService spusti validacie
8. AuthService zavola UserRepository
9. Repository vykona SQL SELECT v PostgreSQL
10. Databaza vrati zaznam alebo nic
11. AuthService vytvori LoginResponse
12. Backend vrati JSON odpoved
13. Frontend zobrazi vysledok pouzivatelovi
```

### Diagram toku

```text
Pouzivatel
  -> vyplni formular
Frontend
  -> POST /api/auth/login
Backend route
  -> AuthService.LoginAsync()
Repository
  -> SELECT ... FROM users WHERE email = @email
Databaza
  -> user alebo null
Backend
  -> JSON response
Frontend
  -> vypise spravu
```

---

## 14. Preco to je rozdelene do vrstiev

Mozno by sa dalo vsetko napisat do `Program.cs`, ale nebolo by to dobre riesenie.

### Zodpovednost jednotlivych vrstiev

#### Frontend

Ma:

- zobrat vstup od pouzivatela
- poslat request
- zobrazit odpoved

Nema:

- robit SQL dotazy
- obsahovat heslo do databazy

#### Route endpoint v `Program.cs`

Ma:

- prijat request
- zavolat service
- vratit response

Nema:

- obsahovat vsetku logiku loginu

#### `AuthService`

Ma:

- validovat data
- rozhodovat o vysledku loginu

Nema:

- pisat HTML

#### `UserRepository`

Ma:

- komunikovat s tabulkou `users`
- vykonavat SQL dotaz

Nema:

- rozhodovat o tom, ci je heslo spravne z hladiska aplikacie

### Vyhoda tohto rozdelenia

- kod sa lepsie cita
- kod sa lahsie opravuje
- kazda cast ma jasnu zodpovednost
- da sa na tom stavat vacsia aplikacia

---

## 15. Najcastejsie chyby

### Chyba 1 - otvoreny zly index

Ak otvoris `index.html` ako lokalny subor alebo cez iny server, request moze ist na nespravny port.

Spravne:

```text
http://localhost:5000/
```

Nie:

```text
file:///...
```

### Chyba 2 - backend bezi, ale databaza nie

Vtedy sa nacita stranka, ale login request spadne pri pokuse otvorit DB connection.

### Chyba 3 - chyba v `.env`

Ak chyba:

- `DB_NAME`
- `DB_USER`
- `DB_PORT`

backend sa nevie korektne pripojit do databazy.

### Chyba 4 - tabulka `users` neexistuje

Ak nebol spusteny SQL script, backend nema z coho citat.

### Chyba 5 - zle nazvy fieldov vo frontende

Ak frontend posle:

```json
{
  "userEmail": "eva@lyceum.sk"
}
```

a backend caka:

```json
{
  "email": "eva@lyceum.sk"
}
```

namapovanie do `LoginRequest` nebude fungovat tak, ako ocakavame.

---

## 16. Zhrnutie

V tejto teme je najdolezitejsie pochopit tok dat:

```text
frontend -> backend -> databaza -> backend -> frontend
```

Zapamataj si:

- frontend posiela request
- backend prijima a spracuje request
- repository cita databazu
- service robi logiku a validacie
- backend vracia JSON odpoved

Ak tomu rozumies, tak uz nechapes databazu ako izolovanu temu, ale ako sucast celej aplikacie.

To je hlavny ciel tohto tyzdna.
