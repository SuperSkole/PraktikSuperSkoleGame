using UnityEngine;
using UnityEngine.UI;

public class ChangeColor : MonoBehaviour
{
    [SerializeField] private GameObject gm;
    private ColorCollection colorWheel = new ColorCollection();
    private GameObject playerHead;
    private GameObject playerBody;
    private GameObject playerLegs;

    CharacterController player;

    private void OnEnable()
    {
        GetPlayerPartsInfo();
    }
    public void Awake()   
    {
        for (int i = 1; i <= 16; i++)
        {
            // Construct the name of the child GameObject
            string childName = "Box (" + i + ")";
            // Find the Box GameObject by name
            Transform box = this.transform.Find(childName);

            if (box != null)
            {
                // Find the Img child GameObject of the Box GameObject
                Transform imgChild = box.Find("Img");

                if (imgChild != null)
                {
                    // Get the Image component of the Img child GameObject
                    Image tmp = imgChild.GetComponent<Image>();

                    // Set the color of the Image component
                    if (tmp != null)
                    {
                        System.Drawing.Color systemColor = colorWheel.colorwheel[i - 1];
                        UnityEngine.Color unityColor = colorWheel.ConvertToUnityColor(systemColor);
                        tmp.color = unityColor;

                        Button butTmp = box.GetComponent<Button>();
                        if (butTmp != null)
                        {
                            //Removes old listners if not done, errors will happen.
                            butTmp.onClick.RemoveAllListeners();
                            //adds a listener for specefik body part
                            butTmp.onClick.AddListener(() => changeColorClicked(unityColor));
                        }
                    }
                    else
                    {
                        Debug.LogError("Image component not found on Img child of " + childName);
                    }
                }
                else
                {
                    Debug.LogError("Img child not found on " + childName);
                }
            }
            else
            {
                Debug.LogError("Box not found: " + childName);
            }
        }
    }
    //whichBodyPart goes from 1: Head, 2: Body, 3: Legs
    public void changeColorClicked(Color color)
    {
        try
        {
            playerHead.GetComponent<SpriteRenderer>().color = color;
            gm.GetComponent<NewGame>().headColor = color;
        }
        catch (System.Exception)
        {

            Debug.Log("ChangeColor/changeColorClicked/Cant Save head color because there is no head");
        }
        playerBody.GetComponent<SpriteRenderer>().color = color;
        gm.GetComponent<NewGame>().BodyColor = color;
        playerLegs.GetComponent<SpriteRenderer>().color = color;
        gm.GetComponent<NewGame>().LegColor = color;
    }
    public void GetPlayerPartsInfo()
    {
        GameObject head = GameObject.Find("Head");
        GameObject torso = GameObject.Find("Mainbody");
        GameObject legs = GameObject.Find("Legs");

        if (head != null)
        {
            playerHead = head;
            gm.GetComponent<NewGame>().spriteHead = head;
        }
        else
        {
            Debug.Log("ChangeColors/GetPlayerPartsInfo/No Head was found");
        }
        if (torso != null)
        {
            playerBody = torso;
            gm.GetComponent<NewGame>().spriteBody = torso;

        }
        else
        {
            Debug.Log("ChangeColors/GetPlayerPartsInfo/No Torso was found");
        }
        if (legs != null)
        {
            playerLegs = legs;
            gm.GetComponent<NewGame>().spriteLeg = legs;
        }
        else
        {
            Debug.Log("ChangeColors/GetPlayerPartsInfo/No Legs was found");
        }

    }
    public void GiveInfoToNewGame()
    {
        gm.GetComponent<NewGame>().spriteHead.GetComponent<SpriteRenderer>().color = playerHead.GetComponent<SpriteRenderer>().color;
        gm.GetComponent<NewGame>().spriteBody.GetComponent<SpriteRenderer>().color = playerBody.GetComponent<SpriteRenderer>().color;
        gm.GetComponent<NewGame>().spriteLeg.GetComponent<SpriteRenderer>().color = playerLegs.GetComponent<SpriteRenderer>().color;
    }

}


