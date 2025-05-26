using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class NPCAxel : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;
    public string[] dialogue;
    private static int index = 0;

    public GameObject contButton, exitButton, enterButton;
    public float wordSpeed;
    public bool playerIsClose = false;

    public static int intro = 1;

    void Start(){
        intro = PlayerPrefs.GetInt("axelIntro", 1);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && playerIsClose ){
            playerIsClose = false;
            if(dialoguePanel.activeInHierarchy){
                zeroText();
            }else{
                dialoguePanel.SetActive(true);
                StartCoroutine(Typing());
            }
        }


        if(dialogueText.text == dialogue[index]){
            if((index == 0 || index == 1)&&Input.GetKeyDown(KeyCode.E)){
                PlayerPrefs.SetInt("axelIntro", 0);
                NextLine();
            }else if(index == 2){
                contButton.SetActive(true);
            }else if((index == 3 || index == 6)&&Input.GetKeyDown(KeyCode.E)){
                zeroText();
                index = 4;
            }else if(index == 4){
                exitButton.SetActive(true);
                enterButton.SetActive(true);
            }else if(index == 5 &&Input.GetKeyDown(KeyCode.E)){
                index = 4;
                SceneManager.LoadScene("TrashGameMinigame");
            }
            
        }
    }

    IEnumerator Typing(){
        Debug.Log(dialogue[index]);
        foreach(char letter in dialogue[index].ToCharArray()){
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }
    }

    public void NextLine(){
        contButton.SetActive(false);
        exitButton.SetActive(false);
        enterButton.SetActive(false);
        if(index < dialogue.Length - 1){
            index++;
            dialogueText.text = "";
            StartCoroutine(Typing());
        }else{
            zeroText();
        }
    }

    public void zeroText(){
        dialogueText.text = "";
        if(intro == 1){
            index = 0;
            intro = 0;
            PlayerPrefs.SetInt("axelIntro", 0);
        }else{
            index = 4;
        }
        
        dialoguePanel.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Player")){
            playerIsClose = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other){
        if(other.CompareTag("Player")){
            playerIsClose = false;
        }
    }

    public void jumpIndex(int add){
        index += add;
        NextLine();
    }

    
}
