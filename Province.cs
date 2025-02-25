using Godot;

public partial class Province : Area2D
{
    static StringName _inputMouseLeft = new StringName("mouse_left");
    private StaticData _staticData;

    private Godot.Collections.Dictionary<string, string> countryData;

    public override void _Ready()
    {
        _staticData = GetNode<StaticData>("/root/StaticData");
        var countriesData = _staticData.countriesData;
        countryData = countriesData[Name]; // Name is the shapeId, which is the same as the country id
        
    }

    public void OnInputEventSignal(Node viewport, InputEvent @event, int shapeIdx)
    {
        if (@event is InputEventMouseButton mouseButton)
        {
            if (mouseButton.IsActionPressed(_inputMouseLeft))
            {
                GD.Print($"Province clicked: [id: {Name}]");
                GD.Print($"Country: {countryData}");

                //  Reset Color of all polygons
                foreach (Node node in GetParent().GetChildren())
                {
                    if (node is Area2D area)
                    {
                        foreach (Node subnode in area.GetChildren())
                        {
                            if (subnode is Polygon2D polygon)
                            {
                                polygon.Modulate = new Color(0, 0, 0, 0f);
                            }
                        }
                    }
                }

                //  Set Color of clicked province
                foreach (Node subnode in GetChildren())
                {
                    if (subnode is Polygon2D polygon)
                    {
                        polygon.Modulate = new Color(1, 0, 0, 0.7f);    // TODO: Change color to Country specific color
                    }
                }
            }
        }
    }
}
