using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [SerializeField]private List<GameObject>letterCubes = new List<GameObject>();
    private List<LetterCubeManager>letterCubeManagers = new List<LetterCubeManager>();
    private List<LetterCubeManager>activeLetterCubeManagers = new List<LetterCubeManager>();
    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject l in letterCubes){
            LetterCubeManager lCM = l.GetComponent<LetterCubeManager>();
            if (lCM != null){
                letterCubeManagers.Add(lCM);
            }
        }
        newLetters();
        

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R)){
            newLetters();
        }
    }
    private void newLetters(){
        newLetters(Random.Range(1, 10));
    }
    private void newLetters(int count){
        foreach (LetterCubeManager lCM in activeLetterCubeManagers){
            lCM.Deactivate();
        }
        activeLetterCubeManagers.Clear();
        for (int i = 0; i < count; i++){
            string letter = LetterAndWordCollections.GetRandomLetters(1)[0].ToString();
            activeLetterCubeManagers.Add(letterCubeManagers[Random.Range(0, letterCubeManagers.Count)]);
            activeLetterCubeManagers[i].Activate(letter);
        }
    }
}
