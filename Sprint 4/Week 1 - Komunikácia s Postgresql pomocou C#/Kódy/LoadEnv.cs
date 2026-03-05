namespace ConsoleApp1;

public class LoadEnv
{
    public static bool LoadFile(string path)
    {
        if (!File.Exists(path))
        {
            return false;
        }

        foreach (var line in File.ReadAllLines(path))
        {
            if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#")) continue;
            
            var parts = line.Split('=', 2);
            if (parts.Length == 2)
            {
                Environment.SetEnvironmentVariable(parts[0].Trim(), parts[1].Trim());
            }
        }
        
        return true;
    }
}