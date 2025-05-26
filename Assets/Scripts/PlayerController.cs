using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    bool inRange = false;
    public float speed = 1;
    float movX, movY;
    public Rigidbody2D rb;
	private Animator anim;
	public GameObject pickUpTrash, tempTrash, getFish;
	public GameObject[] typeFish;
	public bool inPaper, inCans, inPlastic, inTrash, inPond;
	public static bool isPaused = false;
	public int health = 6, point = 0;
	public static int coins = 0;
	public TMP_Text resultText;
	public TMP_Text successText;
	public TMP_Text coin;
	public Animator resultAnim;
	public GameObject buttonEndGame;
	public GameObject food;

	public RNGTrash trashGame;

	string popUpHome = "Press E to enter the house";
	string popUpTrashGame = "Press E to start the trash game";
	string popUpPond = "Press E to fish";
	GameObject objectDetected;
	private PopUpSystem pop, end;

	public Image currentHealthBar;

	private void Awake(){
		anim = GetComponent<Animator>();
	}

    void Start()
    {
		pop = GameObject.FindGameObjectWithTag("GameManager").GetComponent<PopUpSystem>();
		coins = PlayerPrefs.GetInt("coins", 0);
	}

    // Update is called once per frame
    void FixedUpdate()
    {
        movX = Input.GetAxisRaw("Horizontal");
        movY = Input.GetAxisRaw("Vertical");
		if(movX !=0){
			rb.velocity = new Vector2(movX, 0)*speed;
		}
		else{
			rb.velocity = new Vector2(0, movY)*speed;
		}
    }
    void OnTriggerEnter2D(Collider2D collision){
	
		if(collision.gameObject.tag == "Door"){
			inRange = true;
			pop.PopUp(popUpHome);
		}
		if(collision.gameObject.tag == "TrashGame"){
			objectDetected = collision.gameObject;
			pop.PopUp(popUpTrashGame);
		}
		if(collision.gameObject.tag == "Trash"){
			inTrash = true;
			tempTrash = collision.gameObject;
		}
		if(collision.gameObject.name == "TrashPaper"){
			inPaper = true;
		}
		if(collision.gameObject.name == "TrashCans"){
			inCans = true;
		}
		if(collision.gameObject.name == "TrashPlastic"){
			inPlastic = true;
		}
		if(collision.gameObject.name == "Pond"){
			inPond = true;
			pop.PopUp(popUpPond);
		}
    }
    void OnTriggerExit2D(Collider2D collision){
		if(collision.gameObject.tag == "Door"){
			inRange = false;
		}
		if(collision.gameObject.tag == "TrashGame"){
			objectDetected = null;
		}
		if(!inRange && objectDetected == null){
			if(pop != null){
				pop.close();
			}
		}
		if(collision.gameObject.tag == "Trash"){
			inTrash = false;
			tempTrash = null;
		}
		if(collision.gameObject.name == "TrashPaper"){
			inPaper = false;
		}
		if(collision.gameObject.name == "TrashCans"){
			inCans = false;
		}
		if(collision.gameObject.name == "TrashPlastic"){
			inPlastic = false;
		}
		if(collision.gameObject.name == "Pond"){
			inPond = false;
		}
    }
    void Update(){
		if(isPaused){
			return;
		}
		float horizontalInput = Input.GetAxis("Horizontal");
		float verticalInput = Input.GetAxis("Vertical");
		if(coin){
			coin.text = "Coins: " + coins;
		}
		if(currentHealthBar){
			currentHealthBar.fillAmount = health/6f;
		}
		if(Input.GetKeyDown("f") && food != null){
			Destroy(food);
		}
		if(Input.GetKeyDown("e")){
			if(inPond == true && getFish == null){
				getFish = Instantiate(typeFish[Random.Range(0,typeFish.Length)]);
				getFish.transform.parent = transform;
				getFish.transform.localPosition = new Vector2(0,12);
			}
			if(inRange == true){
				SceneManager.LoadScene("Home");
			}
			else if(objectDetected != null){
				SceneManager.LoadScene("TrashGameMinigame");
			}
			else if(tempTrash != null && pickUpTrash == null){
				pickUpTrash = tempTrash;
				pickUpTrash.GetComponent<BoxCollider2D>().enabled = false;
				pickUpTrash.transform.parent = transform;
				pickUpTrash.transform.localPosition = new Vector2(0,12);
			}
			else if(inCans == true && pickUpTrash.name == "TrashC"){
				point++;
				Destroy(pickUpTrash);
			}
			else if(inCans == true){
				health--;
				Destroy(pickUpTrash);
			}
			else if(inPaper == true && pickUpTrash.name == "TrashPa"){
				point++;
				Destroy(pickUpTrash);
			}
			else if(inPaper == true){
				health--;
				Destroy(pickUpTrash);
			}
			else if(inPlastic == true && pickUpTrash.name == "TrashPl"){
				point++;
				Destroy(pickUpTrash);
			}
			else if(inPlastic == true){
				health--;
				Destroy(pickUpTrash);
			}
		}
		if(health == 0){
			miniGameEnd();
		}
		if(9 == (6-health)+point){
			miniGameEnd();
		}
		if(horizontalInput != 0 && horizontalInput > 0.01f){
			anim.Play("right");
		}
		else if(horizontalInput != 0 && horizontalInput < -0.01f){
			anim.Play("left");
		}
		else if(verticalInput != 0 && verticalInput < -0.01f){
			anim.Play("front");
		}
		else if(verticalInput != 0 && verticalInput > 0.01f){
			anim.Play("back");
		}
		else {
			anim.Play("idle");
		}

		if(trashGame && Input.GetKeyDown(KeyCode.Escape) &&  9 != (6-health)+point && health !=0){
			Debug.Log("testt pause");
			trashGame.pauseGame();
		}
	}
	public void sellFIsh(){
        if(getFish.name == "Fish1"){
            Destroy(getFish);
           	coins = coins + 3;
        }
        else{
            Destroy(getFish);
            coins = coins + 5;
        }
		PlayerPrefs.SetInt("coins", coins);
    }
	public void miniGameEnd(){
		resultAnim = GameObject.FindGameObjectWithTag("PopUpEnd").GetComponent<Animator>();
		int coinCalculate = 0;
		if(9 == (6-health)+point && health !=0){
			successText.text = "Success !!";
			coinCalculate += point * health;
		}else{
			successText.text = "Failed :(";
			coinCalculate += point * 0;
		}
		resultText.text = "Score \t: " + point + "\n" +
						"Life \t\t: " + health + "\n"+ 
						"You get " + coinCalculate + " coins\n";
		resultAnim.SetTrigger("end");
		int wait = 20;
		while(wait > 0){
			wait--;
		}
		if(buttonEndGame){
			buttonEndGame.SetActive(true);
		}
		
	}

	public void backToMainScene(){
		coins = coins + point*health;
		PlayerPrefs.SetInt("coins", coins);
		SceneManager.LoadScene("SampleScene");
	}
	
	public void coinMinus(int minus){
		if(coins - minus < 0){
			Destroy(food);
		}
		else{
			coins = coins - minus;
			food.transform.parent = transform;
			food.transform.localPosition = new Vector2(0,12);
		}
	}
}