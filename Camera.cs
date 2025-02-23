using Godot;

public partial class Camera : Camera2D
{
    private float cameraZoomDirection = 0.0f;
    [Export] public float cameraZoomSpeed = 4.0f;
    [Export] public float cameraZoomMin = 25.0f;    // furthest in you can zoom
    [Export] public float cameraZoomMax = 4.0f;     // furthest out you can zoom
    [Export] public float cameraMoveSpeed = 800.0f;
    [Export] public int cameraPanMargin = 16;

    public override void _Process(double delta)
    {
        float dt = (float)delta;
        CameraZoom(dt);
        CameraMove(dt);
        CameraPan(dt);
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsAction("camera_zoom_in"))
        {
            cameraZoomDirection = 1;
        }
        else if (@event.IsAction("camera_zoom_out"))
        {
            cameraZoomDirection = -1;
        }
    }

    private void CameraZoom(float delta)
    {
        float newZoom = Mathf.Clamp(Zoom.X + (cameraZoomSpeed * Zoom.X) * cameraZoomDirection * delta, cameraZoomMax, cameraZoomMin);
        Zoom = new Vector2(newZoom, newZoom);
        cameraZoomDirection *= 0.9f;
    }

    private void CameraMove(float delta)
    {
        Vector2 velocityDirection = Vector2.Zero;

        if (Input.IsActionPressed("camera_forward")) velocityDirection.Y -= 1;
        if (Input.IsActionPressed("camera_backward")) velocityDirection.Y += 1;
        if (Input.IsActionPressed("camera_right")) velocityDirection.X += 1;
        if (Input.IsActionPressed("camera_left")) velocityDirection.X -= 1;

        Position += velocityDirection.Normalized() * (cameraMoveSpeed / Zoom.X) * delta;
    }

    private void CameraPan(float delta)
    {
        Viewport viewportCurrent = GetViewport();
        Vector2 panDirection = new Vector2(-1, -1);
        Rect2 viewportVisibleRectangle = viewportCurrent.GetVisibleRect();
        Vector2 viewportSize = viewportVisibleRectangle.Size;
        Vector2 currentMousePosition = viewportCurrent.GetMousePosition();

        if (currentMousePosition.X < cameraPanMargin || currentMousePosition.X > viewportSize.X - cameraPanMargin)
        {
            if (currentMousePosition.X > viewportSize.X / 2.0f)
            {
                panDirection.X = 1;
            }
            Position += new Vector2(panDirection.X * (cameraMoveSpeed / Zoom.X) * delta, 0);
        }

        if (currentMousePosition.Y < cameraPanMargin || currentMousePosition.Y > viewportSize.Y - cameraPanMargin)
        {
            if (currentMousePosition.Y > viewportSize.Y / 2.0f)
            {
                panDirection.Y = 1;
            }
            Position += new Vector2(0, panDirection.Y * (cameraMoveSpeed / Zoom.X) * delta);
        }
    }
}
