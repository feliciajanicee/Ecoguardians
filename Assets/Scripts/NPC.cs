using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class NPC : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;
    public string[] dialogue;
    private int index = 0;

    public GameObject contButton;
    public float wordSpeed;
    public bool playerIsClose = false;

    public bool first = true;


    // Update is called once per frame
    void Update()
    {
        if((first || Input.GetKeyDown(KeyCode.E) )&& playerIsClose ){
            first = false;
            if(dialoguePanel.activeInHierarchy){
                zeroText();
            }else{
                dialoguePanel.SetActive(true);
                StartCoroutine(Typing());
            }
        }

        if(dialogueText.text == dialogue[index]){
            contButton.SetActive(true);
        }
    }

    IEnumerator Typing(){
        foreach(char letter in dialogue[index].ToCharArray()){
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }
    }

    public void NextLine(){
        contButton.SetActive(false);
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
        index = 0;
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

    
}
