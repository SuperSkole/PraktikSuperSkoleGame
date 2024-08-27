using UnityEngine;

public class ReadMeQuickINFO
{
    /*
     * ----------- Read for quick guide to different things that are hard to remember -----------
     * http://quill18.com/unity_tutorials/ Maybe contains some good stuff.
     * 
     * SkinShop, CharCuz og ChoosePlayer GO skal alle være aktive når start spillet ellers virker det ikke
     * De bliver set.Active(true) i NewGame som er på GameManger.
     * De bliver set.Active(false) i PlayerWorldMovement Skal ske der ellers ender vi med problemer med den ikke kan finde GO
     * 
     * GetPlayerPartsInfo ved SkinShop og ShowCusBodyParts bliver kaldt ved knappen i CharCreate
     * 
     * PlayerPrefs can only set (int,float or string) can't be used to save complex data
     * 
     * The SaveGame, when saving the skins, if there is a missing skin in the inspector on any of the players then an error will happen when saving the game, and it will not save
     * 
     * Inde i PlayerWorldMovement under FindNPC metoden mangler vi at gøre så man ikke skal åbne Wardrope før man kan åbne shoppen
     * 
     * When loading the game, the IconSpriteMapper gets called in order to get the correct sprite, the class has 2 Dictionaries one for icons to sprite and the other way.
     * These Dictionaries gets filled in ShopSkinManagement where there are 2 list, these lists needs to be filled out in the same order  so Index 0 from the list Icons is the is equal to index 0 from then list sprites
     * When buying skins, the IconSpriteMapper needs to be called in order to not change the players sprites into an icon.
     */
}
