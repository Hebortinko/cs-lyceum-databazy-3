# Požiadavky na Projekt – Databázová Časť

---

## 1. Dokumentácia systému

Stručný dokument ktorý popisuje:
- O čom systém je a aký problém rieši
- Zoznam entít (tabuliek) ktoré systém obsahuje
- Zoznam atribútov každej entity s dátovým typom
- Popis vzťahov medzi entitami: `1:1`, `1:N`, `M:N`
- Obmedzenia – čo je unikátne, čo nesmie byť prázdne (`NOT NULL`), čo je povinné

**Hodnotí sa:** či model dáva zmysel pre danú doménu, či sú entity a atribúty jasne a zmysluplne pomenované, či sú vzťahy správne identifikované.

---

## 2. Normalizácia (1NF – 3NF)

Databáza musí spĺňať prvú, druhú a tretiu normálnu formu. V dokumentácii stručne preukáž že tvoj návrh tieto formy spĺňa.

- **1NF** – každý atribút obsahuje atomickú hodnotu, žiadne opakujúce sa skupiny
- **2NF** – každý neklúčový atribút závisí od celého primárneho kľúča
- **3NF** – žiadny neklúčový atribút nezávisí od iného neklúčového atribútu

---

## 3. ERD Diagram

Grafický diagram ktorý zobrazuje celú databázu. Diagram musí:
- Obsahovať všetky entity s ich atribútmi
- Zobrazovať všetky vzťahy medzi entitami vrátane kardinality (`1:1`, `1:N`, `M:N`)
- Mať pomenované vzťahy (napr. *„zákazník skladá objednávku"*)
- Byť prehľadný, konzistentný a čitateľný

**Hodnotí sa:** správnosť vzťahov a kardinality, prehľadnosť a vizuálna kvalita diagramu.

---

## 4. Implementácia Databázy

### Schéma
SQL skript ktorý vytvorí celú databázu – všetky tabuľky, primárne kľúče, cudzie kľúče a obmedzenia.

### CRUD operácie
Implementované pomocou **Repository Pattern** v C# – každá entita má vlastnú Repository triedu s metódami `Create`, `GetAll`, `GetById`, `Update`, `Delete`.

### JOINy
Databáza musí obsahovať **minimálne 3 JOINy** – teda aspoň 3 miesta kde sa dotazuješ na dáta z viacerých tabuliek naraz. Toto prirodzene vyplynie z návrhu v 3NF.

### Testovacie dáta
SQL skript alebo C# kód ktorý naplní databázu reálne vyzerajúcimi dátami. Dát musí byť dostatok na to aby sa dala overiť funkcionalita a integrita.

**Integrita dát** znamená že dáta sú konzistentné a správne – napríklad objednávka nemôže odkazovať na zákazníka ktorý neexistuje, produkt nemôže mať zápornú cenu, email musí byť unikátny. Hodnotí sa či testovacie dáta tieto pravidlá dodržujú a či ich databáza aktívne vynucuje.

---

## 5. Prepojenie s Backendom (C#)

- Aplikácia sa musí úspešne pripojiť na databázu
- CRUD metódy musia byť správne implementované a funkčné
- Pripojenie aj operácie musia reálne fungovať

**Hodnotí sa:** funkčnosť pripojenia, správnosť implementovaných metód, kvalita a čistota kódu.

---

## Prehľad hodnotených oblastí

| Oblasť | Čo sa hodnotí |
|--------|--------------|
| Dokumentácia | Popis domény, entity, atribúty, vzťahy, obmedzenia |
| Normalizácia | Splnenie 1NF, 2NF, 3NF |
| ERD Diagram | Správnosť, kardinalita, vizuálna kvalita |
| Implementácia | Schéma, CRUD, JOINy (min. 3), testovacie dáta |
| Integrita dát | Konzistentnosť a správnosť testovacích dát |
| Backend | Funkčné pripojenie, správne metódy, kvalita kódu |
