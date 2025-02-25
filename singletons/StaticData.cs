
using Godot;

public partial class StaticData : Node
{
    public Godot.Collections.Dictionary<string, Godot.Collections.Dictionary<string, string>> countriesData = new();

    private string coutriesDataFilePath = "res://data/provinces-attributes3.txt";
    private int lineCount = 0;

    public override void _Ready()
    {
        using var file = FileAccess.Open(coutriesDataFilePath, FileAccess.ModeFlags.Read);
        while (!file.EofReached())
        {
            lineCount++;
            var line = file.GetLine();
            var parts = line.Split(",");


            if (lineCount == 1 || parts == null)
            {
                continue;
            }

            try
            {
                string id = parts[0];
                string country = parts[1];
                string continent = parts[7];

                if (!countriesData.ContainsKey(id))
                {
                    countriesData[id] = new Godot.Collections.Dictionary<string, string>();
                }

                countriesData[id]["id"] = id;
                countriesData[id]["name"] = country;
                countriesData[id]["continent"] = continent;
            }
            catch (System.Exception e)
            {
                GD.PrintErr($"Error parsing line {lineCount}: {line}");
                GD.PrintErr(e.Message);
            }
        }

        GD.Print("countriesData ", countriesData);
    }
}