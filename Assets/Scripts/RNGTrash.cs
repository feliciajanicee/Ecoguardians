using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RNGTrash : MonoBehaviour
{
    public GameObject[] TrashListCans, TrashListPaper, TrashListPlastic;
    public TMP_Text timerTxt;
    public PlayerController playerCon;
    public GameObject pauseMenu;

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
        PlayerController.isPaused = false;
        StartCoroutine(timer());
        for(int i=0;i<3;i++){
            GameObject a = Instantiate(TrashListCans[Random.Range(0,TrashListCans.Length)],transform.position + new Vector3(Random.Range(-15,15),Random.Range(-4,4),0),Quaternion.Euler(0,0,0));
            a.name = "TrashC";
        }
        for(int i=0;i<3;i++){
            GameObject a = Instantiate(TrashListPaper[Random.Range(0,TrashListPaper.Length)],transform.position + new Vector3(Random.Range(-15,15),Random.Range(-4,4),0),Quaternion.Euler(0,0,0));
            a.name = "TrashPa";
        }
        for(int i=0;i<3;i++){
            GameObject a = Instantiate(TrashListPlastic[Random.Range(0,TrashListPlastic.Length)],transform.position + new Vector3(Random.Range(-15,15),Random.Range(-4,4),0),Quaternion.Euler(0,0,0));
            a.name = "TrashPl";
        }
    }

    void update(){

    }
    IEnumerator timer(){
        for(int i=60; i > 0;i--){

            yield return new WaitForSeconds(1);
            timerTxt.text = i.ToString();
            
        }
        playerCon.miniGameEnd();
    }
    public void pauseGame(){
        pauseMenu.SetActive(true);
        Debug.Log("paused");
        Time.timeScale = 0f;
        PlayerController.isPaused = true;
    }

    public void continueGame(){
        pauseMenu.SetActive(false);
        Debug.Log("continue");
        Time.timeScale = 1f;
        PlayerController.isPaused = false;
    }
    
}
