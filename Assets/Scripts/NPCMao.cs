using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class NPCMao : MonoBehaviour
{
    public GameObject dialoguePanel, shopInter;
    public TMP_Text dialogueText;
    public string[] dialogue;
    private static int index = 0;

    public GameObject contButton, exitButton, enterButton;
    public GameObject[] typeFood;
    public float wordSpeed;
    public bool playerIsClose = false;

    public static int intro = 1;

    public PlayerController playerCon;

    void Start(){
        intro = PlayerPrefs.GetInt("maoIntro", 1);
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
            if((index == 1 || index == 2 || index == 3)&&Input.GetKeyDown(KeyCode.E)){
                NextLine();
            }else if(index == 0){
                contButton.SetActive(true);
            }else if((index == 4 || index == 6)&&Input.GetKeyDown(KeyCode.E)){
                zeroText();
                index = 5;
            }else if(index == 5){
                exitButton.SetActive(true);
                enterButton.SetActive(true);
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
        dialoguePanel.SetActive(true);
        contButton.SetActive(false);
        exitButton.SetActive(false);
        enterButton.SetActive(false);
        shopInter.SetActive(false);
        if(index < dialogue.Length - 1){
            index++;
            dialogueText.text = "";
            StartCoroutine(Typing());
        }else{
            zeroText();
        }
    }

    public void zeroText(){
        PlayerPrefs.SetInt("maoIntro", 0);
        dialogueText.text = "";
        if(intro == 1){
            index = 0;
            intro = 0;
        }else{
            index = 5;
        }
        
        dialoguePanel.SetActive(false);
    }

    public void shopUI(){
        exitButton.SetActive(false);
        enterButton.SetActive(false);
        dialoguePanel.SetActive(false);
        shopInter.SetActive(true);
    }

    public void food1(){
        if(playerCon.food == null){
            playerCon.food = Instantiate(typeFood[0]);
            playerCon.coinMinus(20);
            PlayerPrefs.SetInt("coins", PlayerController.coins);
            NextLine();
        }
        else{
            NextLine();
        }
    }

    public void food2(){
        if(playerCon.food == null){
            playerCon.food = Instantiate(typeFood[1]);
            playerCon.coinMinus(10);
            PlayerPrefs.SetInt("coins", PlayerController.coins);
            NextLine();
        }
        else{
            NextLine();
        }
    }

    public void food3(){
        if(playerCon.food == null){
            playerCon.food = Instantiate(typeFood[2]);
            playerCon.coinMinus(5);
            PlayerPrefs.SetInt("coins", PlayerController.coins);
            NextLine();
        }
        else{
            NextLine();
        }
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
