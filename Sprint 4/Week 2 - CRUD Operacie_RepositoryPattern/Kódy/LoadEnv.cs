namespace EshopApp;

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