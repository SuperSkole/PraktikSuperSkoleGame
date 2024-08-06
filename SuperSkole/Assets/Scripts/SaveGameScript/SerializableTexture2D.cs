using System;
using UnityEngine;

[System.Serializable]
public class SerializableTexture2D
{
    public int width;
    public int height;
    public byte[] pixelData;
    public TextureFormat format;
    public bool mipmap;

    public SerializableTexture2D(Texture2D texture)
    {
        width = texture.width;
        height = texture.height;
        format = texture.format;
        mipmap = texture.mipmapCount > 1;
        pixelData = texture.GetRawTextureData();
    }

    public Texture2D ToTexture2D()
    {
        Texture2D texture = new Texture2D(width, height, format, mipmap);
        texture.LoadRawTextureData(pixelData);
        texture.Apply();
        return texture;
    }
}
