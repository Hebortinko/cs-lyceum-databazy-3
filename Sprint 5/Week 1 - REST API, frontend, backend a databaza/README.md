# Week 1 - REST API, frontend, backend a databaza

## O com je tento tyzden

V tomto tyzdni prepajame databazy a programovanie do jedneho celeho flowu:

```text
frontend -> backend API -> databaza -> backend -> frontend
```

Dolezite je pochopit, ze frontend vacsinou nekomunikuje priamo s databazou.

Pouzivatel pracuje s formularom alebo tlacidlom vo frontende, frontend posle request na backend, backend spravi logiku a az potom cita alebo zapisuje data do databazy.

---

## Co sme sa ucili

V tomto weeku si ukazujeme:

- co je REST API
- co je endpoint
- ako frontend odosiela request
- ako backend prijima data
- ako backend cita udaje z databazy
- ako sa odpoved vracia spat vo formate JSON

Ako ukazku pouzivame jednoduchy login flow a jeden alebo dva male databazove cally.

---

## Co je v tomto priecinku

### `Poznamky`

Tu su vysvetlenia pojmov a celeho toku:

- frontend
- backend
- databaza
- request
- response
- route endpoint
- service vrstva
- repository vrstva

### `Cvicenia`

Tu su jednoduche zadania na precvicenie:

- login od nuly
- dalsi jednoduchy call do databazy a zobrazenie viacerych udajov

### `Kody`

Tu je starter projekt v C#, na ktorom je ukazane:

- jednoduche HTML rozhranie
- API endpoint
- pripojenie na PostgreSQL
- citanie usera z databazy
- vratenie JSON odpovede

---

## Co si vsimni v starter projekte

V kode sa sustred hlavne na tieto casti:

- `wwwroot/index.html`
- `Program.cs`
- `Services/AuthService.cs`
- `Repositories/UserRepository.cs`
- `Database.cs`
- `sql/01_create_users.sql`

Ak porozumies tymto suborom, budes vediet vysvetlit hlavnu myslienku celeho tyzdna.

---

## Hlavna myslienka

Najdolezitejsie je chapat rozdiel medzi vrstvami:

- frontend zbiera vstup a zobrazuje vysledok
- backend prijima request a robi logiku
- databaza uchovava data

Ak sa pouzivatel prihlasuje, frontend neposiela SQL do databazy.

Frontend posle data backendu, backend vyhodnoti situaciu, zavola databazu a vrati odpoved.

---

## Reflexia

Po prejdeni tohto weeku by si mal vediet odpovedat na tieto otazky:

- Viem vysvetlit rozdiel medzi frontendom, backendom a databazou?
- Viem najst v kode miesto, kde frontend vola API?
- Viem najst v kode miesto, kde backend cita data z databazy?
- Viem vysvetlit, preco frontend nema komunikovat priamo s databazou?
- Viem popisat login flow od kliknutia na tlacidlo az po odpoved na stranke?

Ak vies odpovedat na tieto otazky vlastnymi slovami, ciel tohto tyzdna je splneny.
