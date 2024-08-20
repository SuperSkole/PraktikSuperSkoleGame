using System.Collections;
using TMPro;
using UnityEngine;

public class MoveFloatingNumbers : MonoBehaviour
{
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform xpEndPoint;
    [SerializeField] private Transform goldEndPoint;
    [SerializeField] private GameObject xpAmount;
    [SerializeField] private GameObject goldAmount;
    [SerializeField] private GeneralManagement valueInfo;

    private float moveDuration = 1.5f; // Duration of the movement

    private void Start()
    {
        xpAmount.SetActive(false);
        goldAmount.SetActive(false);
    }

    public void MoveXp(int xpValue)
    {
        xpAmount.transform.position = startPoint.position;
        xpAmount.SetActive(true);
        xpAmount.GetComponent<TextMeshProUGUI>().text = xpValue.ToString();

        // Start the movement coroutine
        StartCoroutine(MoveToEndPoint("xp", xpAmount.transform, xpEndPoint.position, moveDuration));

    }
    public void MoveGold(int goldValue)
    {
        goldAmount.transform.position = startPoint.position;
        goldAmount.SetActive(true);
        goldAmount.GetComponent<TextMeshProUGUI>().text = goldValue.ToString();

        // Start the movement coroutine
        StartCoroutine(MoveToEndPoint("gold", goldAmount.transform, goldEndPoint.position, moveDuration));

    }

    private IEnumerator MoveToEndPoint(string type, Transform objectTransform, Vector3 endPoint, float duration)
    {
        Vector3 startPoint = objectTransform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            objectTransform.position = Vector3.Lerp(startPoint, endPoint, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        objectTransform.position = endPoint;

        if (type == "gold")
        {
            valueInfo.UseXP();
            //Play effect
        }
        else
        {
            valueInfo.UseGold();
            //play xp effect
        }
        objectTransform.gameObject.SetActive(false);
        //Gets called twice, so keep crossed for now
    }
}
