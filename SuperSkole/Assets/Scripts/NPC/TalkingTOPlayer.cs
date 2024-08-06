using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TalkingTOPlayer : MonoBehaviour
{
    [SerializeField] private GameObject talkingBox;
    [SerializeField] private GameObject imageBox;
    [SerializeField] private GameObject buttons;
    [SerializeField] private GameObject yesButton;
    [SerializeField] private GameObject noButton;
    [SerializeField] private TextMeshProUGUI NameofSpeaker;
    [SerializeField] private TextMeshProUGUI TextField;
    [SerializeField] private float textSpeed;
    private GameObject whichNPC;
    private string nameOfNPC;
    private Sprite imageOfNPC;

    private string[] dialogue;
    private bool startAction;

    //Skips to next line
    [SerializeField] private UnityEvent whichNPCEvent;
    private UnityEvent yesButtonEvent;
    private UnityEvent noButtonEvent;


    [SerializeField] private GameObject soundManager;

    private int index;


    private void Update()
    {
        if (talkingBox != null && talkingBox.activeSelf && PlayerWorldMovement.witchObjCloseTo.name == whichNPC.name)
        {
            if (TextField.text == dialogue[index])
            {
                soundManager.GetComponent<GernalSoundMangement>().StopAllCoroutines();
            }
            //Click to either skip text of move on the the next bit of text.
            if (Input.GetMouseButtonDown(0))
            {
                whichNPCEvent.Invoke();
            }
        }
    }
    public void SkipLineEvent()
    {

        if (TextField.text == dialogue[index])
        {
            NextLine();
        }
        else
        {
            StopAllCoroutines();
            TextField.text = dialogue[index];
        }
    }
    public void TestingEvent()
    {
        Debug.Log("Yes button event works");
    }
    public void TalkingToPlayer(GameObject whichNPC,UnityEvent yesButtonEvent, UnityEvent noButtonEvent, bool startAction, string nameOfNPC, Sprite imageOfNPC, string[] dialogue)
    {
        this.yesButtonEvent = yesButtonEvent;
        this.noButtonEvent = noButtonEvent;
        this.startAction = startAction;
        this.nameOfNPC = nameOfNPC;
        this.imageOfNPC = imageOfNPC;
        this.dialogue = dialogue;
        this.whichNPC = whichNPC;
        TextField.text = string.Empty;
        PlayerWorldMovement.Talking = true;
        PlayerWorldMovement.allowedToMove = false;
        yesButton.GetComponent<Button>().onClick.RemoveAllListeners();
        yesButton.GetComponent<Button>().onClick.AddListener(yesButtonEvent.Invoke);
        noButton.GetComponent<Button>().onClick.RemoveAllListeners();
        noButton.GetComponent<Button>().onClick.AddListener(noButtonEvent.Invoke);


        NameofSpeaker.text = nameOfNPC;
        imageBox.GetComponent<Image>().sprite = imageOfNPC;
        index = 0;
        talkingBox.SetActive(true);
        buttons.SetActive(false);

        // lines = dialogue.lines;

        StartCoroutine(TypeLine());
        soundManager.GetComponent<GernalSoundMangement>().StillSpeaking = true;
        soundManager.GetComponent<GernalSoundMangement>().CallIE();
    }
    public void NextLine()
    {
        if (index < dialogue.Length - 1)
        {
            index++;
            TextField.text = string.Empty;
            soundManager.GetComponent<GernalSoundMangement>().CallIE();
            StartCoroutine(TypeLine());
        }
        else
        {
            soundManager.GetComponent<GernalSoundMangement>().StillSpeaking = false;
            if (startAction)
            {
                buttons.SetActive(true);
            }
            else
            {
                DoneTalkingToPlayer();
            }
        }
    }
    IEnumerator TypeLine()
    {
        foreach (char c in dialogue[index].ToCharArray())
        {
            TextField.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }
    public void DoneTalkingToPlayer()
    {
        talkingBox.SetActive(false);
        buttons.SetActive(false);

        PlayerWorldMovement.allowedToMove = true;
        PlayerWorldMovement.Talking = false;
    }
}
