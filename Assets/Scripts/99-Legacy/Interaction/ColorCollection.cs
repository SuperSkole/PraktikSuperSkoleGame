using UnityEngine;

public class ColorCollection
{
    public System.Drawing.Color[] colorwheel = new System.Drawing.Color[]
    {
        System.Drawing.Color.Navy, System.Drawing.Color.HotPink,
        System.Drawing.Color.YellowGreen, System.Drawing.Color.Violet,
        System.Drawing.Color.Aqua, System.Drawing.Color.Salmon, System.Drawing.Color.Bisque,
        System.Drawing.Color.Green, System.Drawing.Color.SkyBlue, System.Drawing.Color.Wheat,
        System.Drawing.Color.Orange, System.Drawing.Color.Brown, System.Drawing.Color.MistyRose,
        System.Drawing.Color.Crimson, System.Drawing.Color.Cyan, System.Drawing.Color.Red
    };

    public UnityEngine.Color ConvertToUnityColor(System.Drawing.Color color)
    {
        return new UnityEngine.Color(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f);
    }
}
