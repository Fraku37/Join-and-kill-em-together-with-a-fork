namespace Jaket.UI;

using UnityEngine;

/// <summary> Structure containing data on the position of the interface element. </summary>
public struct Rect
{
    /// <summary> Position relative to the anchor. </summary>
    public float x, y;
    /// <summary> Constant size in pixels. </summary>
    public float Width, Height;
    /// <summary> Position of the anchor. </summary>
    public Vector2 Min, Max;

    public Rect(float x, float y, float width, float height, Vector2 min, Vector2 max)
    { this.x = x; this.y = y; Width = width; Height = height; Min = min; Max = max; }

    /// <summary> Creates a rect with the default width and anchor of the table. </summary>
    public static Rect Tlw(float y, float height) => new(168f, y, 336f, height, new(0f, 1f), new(0f, 1f));

    /// <summary> Creates a rect with the default size of the button. </summary>
    public static Rect Btn(float x, float y) => new(x, y, 320f, 40f, new(.5f, 1f), new(.5f, 1f));

    /// <summary> Creates a rect with the default size of the icon button. </summary>
    public static Rect Icon(float x, float y) => new(x, y, 40f, 40f, new(.5f, 1f), new(.5f, 1f));

    /// <summary> Creates a rect at the center of the canvas with the given size. </summary>
    public static Rect Size(float width, float height) => new(0f, 0f, width, height, new(.5f, .5f), new(.5f, .5f));
}
