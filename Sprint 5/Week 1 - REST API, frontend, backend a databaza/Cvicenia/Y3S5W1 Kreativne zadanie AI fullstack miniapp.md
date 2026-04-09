# Y3S5W1 - Cvicenie navyse: login + druhy databazovy call

## Ciel

Po jednoduchom logine spravit este jeden dalsi call do databazy a zobrazit viac udajov na stranke.

Toto cvicenie je stale male. Nemate robit velku aplikaciu.

---

## Zadanie

Po uspesnom logine sprav este jednu funkcionalitu navyse.

Ta ma fungovat tak, ze po prvom uspesnom requeste nacitas z databazy dalsie udaje a zobrazis ich vo frontende.

Mozes ist jednou z tychto ciest:

- nacitat detail prihlaseneho pouzivatela
- nacitat kratky zoznam pouzivatelov
- nacitat stav uctu alebo rolu pouzivatela
- nacitat niekolko dalsich stlpcov z tabulky a zobrazit ich ako kartu

Konkretny navrh si vyber sam.

---

## Podmienky

Riesenie musi obsahovat:

1. druhy endpoint alebo druhu funkcionalitu na backende
2. druhy databazovy dotaz
3. odpoved s viac ako jednou hodnotou
4. zobrazenie vysledku vo frontende

---

## Odporucany scope

Drz sa niecoho maleho.

Dobre riesenie je naprklad:

- po logine zobrazit meno, email, rolu a stav uctu

alebo:

- po logine nacitat zoznam 3 az 5 pouzivatelov a vypisat ich na stranke

alebo:

- po zadani emailu zobrazit profilovu kartu pouzivatela

---

## Co si mate navrhnut sami

Nevypisujeme vam presne:

- nazov endpointu
- nazov metody
- nazov SQL dotazu
- tvar JSON odpovede

Toto je sucast ulohy.

Mate sami rozhodnut:

- ake data chcete vratit
- ako ich nacitate
- ako ich zobrazite

---

## Minimalne odovzdanie

- fungujuci login
- jedna dalsia funkcionalita po logine
- druhy call do databazy
- zobrazenie aspon 3 udajov na stranke

---

## AI je povolene

AI mozete pouzit ako pomoc pri:

- hladani chyby
- navrhu jednoducheho SQL
- vysvetleni kodu

Ale na konci musite vediet sami vysvetlit:

- odkial prisli data
- ktora cast kodu vola databazu
- co vracia backend

---

## Bonus

Ak stihnes, sprav to tak, aby sa druha cast stranky zobrazila iba po uspesnom logine.
