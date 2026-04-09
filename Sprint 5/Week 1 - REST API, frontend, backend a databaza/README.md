# Week 1 – REST API, frontend, backend a databáza

## Čo sme robili

V prvom týždni sprintu 5 spájame databázy a programovanie do jedného flowu:

`frontend -> REST API -> business logika -> databáza -> odpoveď späť na frontend`

Cieľom je aby študenti chápali, že databáza už nie je izolovaná téma. V reálnej aplikácii frontend neposiela SQL priamo do databázy, ale komunikuje s backendom cez API. Backend validuje vstup, volá databázu a vracia odpoveď vo formáte JSON.

## Čo sme naprogramovali

### `DatabazyApiStarter`
Jednoduchý štartovací projekt v C# ktorý ukazuje:

- statické HTML login UI vo `wwwroot`
- endpoint `POST /api/auth/login`
- vrstvy `Repository` + `Service`
- napojenie na PostgreSQL cez `Npgsql`
- základné validácie pri prihlasovaní

### SQL skripty

V priečinku `sql` je jednoduchá ukážka:

- vytvorenie tabuľky `users`
- seed testovacích dát

Môžeš to použiť ako demonštračné minimum alebo ako štartovaciu šablónu ktorú si študenti upravia podľa vlastného projektu.

## Kľúčové koncepty

- čo je REST API a prečo ho používame
- rozdiel medzi `frontend`, `backend` a `database`
- `GET` vs `POST`
- `request` a `response`
- JSON ako dátový formát medzi frontendom a backendom
- validácia vstupu pred dotazom do databázy
- Repository Pattern aj vo webovej aplikácii
- prečo frontend nikdy nemá komunikovať priamo s databázou

## Odporúčaný priebeh 3 x 45 min

1. blok:
- intro do architektúry `frontend -> backend -> db`
- vysvetlenie REST API, endpointov a JSON
- nakresliť aspoň 2 request flowy na tabuľu

2. blok:
- prejsť si starter kód
- ukázať login request a odpoveď
- vysvetliť `Repository`, `Service`, `Program.cs`

3. blok:
- študenti robia cvičenie
- navrhnú vlastnú tabuľku používateľov alebo upravia existujúcu
- doplnia login logiku a základné kontroly
- pripravia si zadanie pre spoločný projekt databázy + programovanie
