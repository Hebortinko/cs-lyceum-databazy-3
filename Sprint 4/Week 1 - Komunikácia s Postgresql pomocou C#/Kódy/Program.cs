// See https://aka.ms/new-console-template for more information


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

// Načítaj
app.Select("studenti");

app.Close();