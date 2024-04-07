namespace ocicat.Input;

// Xbox style controller mapping
public struct GamePadState
{
    public bool IsConnected;
    
    public Vector2 LeftStick = new(0, 0);
    public Vector2 RightStick = new(0, 0);
    public bool LeftStickPressed, RightStickPressed;

    public float LeftTrigger;
    public bool LeftBumper, LeftThumb;
    public float RightTrigger;
    public bool RightBumper, RightThumb;
    
    public bool Guide, Start, Back;

    public bool A, B, X, Y;
    public bool DpadUp, DpadDown, DpadLeft, DpadRight;

    // Why is this necessary C#?
    public GamePadState() { }
}