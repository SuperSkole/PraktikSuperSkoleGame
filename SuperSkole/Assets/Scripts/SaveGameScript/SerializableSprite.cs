using UnityEngine;

[System.Serializable]
public class SerializableSprite
{
    public SerializableTexture2D texture;
    public Rect rect;
    public Vector2 pivot;
    public float pixelsPerUnit;

    public SerializableSprite(Sprite sprite)
    {
        texture = new SerializableTexture2D(sprite.texture);
        rect = sprite.rect;
        pivot = sprite.pivot;
        pixelsPerUnit = sprite.pixelsPerUnit;
    }

    public Sprite ToSprite()
    {
        Texture2D texture2D = texture.ToTexture2D();
        return Sprite.Create(texture2D, rect, pivot, pixelsPerUnit);
    }
}
