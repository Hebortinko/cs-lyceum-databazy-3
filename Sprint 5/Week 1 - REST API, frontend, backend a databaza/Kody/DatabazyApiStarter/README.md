# DatabazyApiStarter

Jednoduchy starter projekt na hodinu o flowe:

`frontend -> REST API -> databaza`

## Co projekt obsahuje

- staticky login frontend vo `wwwroot/index.html`
- endpoint `POST /api/auth/login`
- `UserRepository` pre nacitanie usera z DB
- `AuthService` pre validaciu a login logiku
- SQL skripty pre tabulku `users`

## Ako to pouzit

1. skopiruj `.env.example` na `.env`
2. dopln prihlasovacie udaje do PostgreSQL
3. spusti SQL skripty z priecinka `sql`
4. spusti projekt cez `dotnet run`
5. otvor v prehliadaci adresu ktoru vypise ASP.NET

## Co maju studenti dorobit

- upravit tabulku `users` podla vlastneho projektu
- doplnit dalsie validacie
- upravit response
- doplnit dalsie endpointy
- navrhnut dalsie tabulky a ich vztahy

## Poznamka

Heslo sa v starteri porovnava ako plain text len kvoli jednoduchosti prveho vysvetlenia.

V realnej aplikacii musi byt heslo hashovane.
