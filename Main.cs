using Godot;
using System.Collections.Generic;
using System.Globalization;

//  Code based on https://github.com/Thomas-Holtvedt/GrandStrategy2D_RealData

public partial class Main : Node2D
{
    private string filePath = "res://provinces-nodes.txt";
    private PackedScene _provinceScene = GD.Load<PackedScene>("res://province.tscn");

    public override void _Ready()
    {
        GD.Print("Main ready");

        Dictionary<int, List<List<Vector2>>> polygons = new Dictionary<int, List<List<Vector2>>>();
        int lineCount = 0;

        using var file = FileAccess.Open(filePath, FileAccess.ModeFlags.Read);
        while (!file.EofReached())
        {
            lineCount++;
            string line = file.GetLine();
            string[] parts = line.Split(",");

            if (parts.Length < 4)
            {
                // Should be 4 parts (shapeId, partId, x, y)
                GD.PrintRich($"[i]Invalid line ({lineCount}): {line}[/i]");
                GD.PrintErr("Expected 4 parts, got " + parts.Length);
                continue;
            }

            int shapeId = int.Parse(parts[0]);
            int partId = int.Parse(parts[1]);
            float x = float.Parse(parts[2], CultureInfo.InvariantCulture);
            float y = -float.Parse(parts[3], CultureInfo.InvariantCulture); // Invert Y axis because qgis uses a different coordinate system

            if (!polygons.ContainsKey(shapeId))
            {
                polygons[shapeId] = new List<List<Vector2>>();
            }

            while (polygons[shapeId].Count <= partId)
            {
                polygons[shapeId].Add(new List<Vector2>());
            }

            polygons[shapeId][partId].Add(new Vector2(x, y));

        }

        foreach (var kvp in polygons)
        {
            CreateProvince(kvp.Key, kvp.Value);
        }
    }

    private void CreateProvince(int shapeId, List<List<Vector2>> parts)
    {
        Province province = _provinceScene.Instantiate<Province>();
        province.Name = shapeId.ToString();
        AddChild(province);

        for (int partid = 0; partid < parts.Count; partid++)
        {
            CreatePart(province, partid, parts[partid]);
        }
    }

    private void CreatePart(Node2D parentNode, int partid, List<Vector2> points)
    {
        Polygon2D polygon = new Polygon2D();
        polygon.Polygon = points.ToArray();
        polygon.Name = partid.ToString();
        polygon.Modulate = new Color(1, 1, 1, 1);   // TOOD: Change to country color

        CollisionPolygon2D collisionPolygon = new CollisionPolygon2D();
        collisionPolygon.Polygon = points.ToArray();
        collisionPolygon.Name = partid.ToString();

        Line2D line = new Line2D();
        line.Points = points.ToArray();
        line.Name = partid.ToString();
        line.Modulate = new Color(0, 0, 0, 0.5f);   // TOOD: Change to country border color 
        line.Width = 0.3f;
        line.Closed = true;

        parentNode.AddChild(polygon);
        parentNode.AddChild(collisionPolygon);
        parentNode.AddChild(line);
    }
}
