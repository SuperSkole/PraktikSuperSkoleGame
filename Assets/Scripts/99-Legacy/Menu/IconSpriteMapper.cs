using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//class manages the mappings between icons and sprites using dictionaries.
public class IconSpriteMapper : MonoBehaviour
{
    // Dictionary to map icons to sprites
    private Dictionary<Sprite, Sprite> iconToSpriteMap = new Dictionary<Sprite, Sprite>();
    private Dictionary<Sprite, Sprite> spriteToIconMap = new Dictionary<Sprite, Sprite>();

    /// <summary>
    /// This method should be called to initialize the mappings
    /// </summary>
    /// <param name="icons"></param>
    /// <param name="sprites"></param>
    public void InitializeMappings(List<Sprite> icons, List<Sprite> sprites)
    {
        if (icons.Count != sprites.Count)
        {
            Debug.LogError("Icons and Sprites lists must be of the same length.");
            return;
        }

        for (int i = 0; i < icons.Count; i++)
        {
            iconToSpriteMap[icons[i]] = sprites[i];
            spriteToIconMap[sprites[i]] = icons[i];
        }
    }

    /// <summary>
    /// Method to get the proper sprite from an icon
    /// </summary>
    /// <param name="icon"></param>
    /// <returns></returns>
    public Sprite GetSpriteFromIcon(Sprite icon)
    {
        if (iconToSpriteMap.TryGetValue(icon, out Sprite sprite))
        {
            return sprite;
        }
        else
        {
            Debug.LogError("Sprite not found for the given icon.");
            return null;
        }
    }

    /// <summary>
    /// Method to get the icon from a proper sprite
    /// </summary>
    /// <param name="sprite"></param>
    /// <returns></returns>
    public Sprite GetIconFromSprite(Sprite sprite)
    {
        if (spriteToIconMap.TryGetValue(sprite, out Sprite icon))
        {
            return icon;
        }
        else
        {
            Debug.LogError("Icon not found for the given sprite.");
            return null;
        }
    }
}
