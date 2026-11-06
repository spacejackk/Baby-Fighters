using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
	public float gravity;
	public float speed;
	public float jumpForce;
	public string character;
	public bool blocking;
	public Vector2 moveInput;
	public HealthBar p1HP;
	public HealthBar p2HP;
	public SpriteManager sprite;
	public GameManager gameManager;
	private bool onGround;
	private bool crouched;
	private float prevTime;
	private List<string> inputHistory = new List<string>();
	private List<string> jabCombo;
	private List<string> specialCombo;
	private Rigidbody2D playerRb;
	private BoxCollider2D hitbox;
	private InputSystem_Actions controls;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
		hitbox = GetComponent<BoxCollider2D>();
		Physics.gravity *= gravity;
		onGround = true;
		crouched = false;
		blocking = false;
		if (gameObject.name == "Player 1"){
			jabCombo = new List<string>{"Jab1", "Jab1", "Jab1"};
			specialCombo = new List<string>{"Jab1", "Kick1", "Kick1", "Jab1"};
		}
		if (gameObject.name == "Player 2"){
			sprite.flipSprite(-1);
			jabCombo = new List<string>{"Jab2", "Jab2", "Jab2"};
			specialCombo = new List<string>{"Jab2", "Kick2", "Kick2", "Jab2"};
		}
    }
	
	void Awake(){
		controls = new InputSystem_Actions();
	}

    // Update is called once per frame
    void Update()
    {
		if (gameManager.gameActive == true){
			if (gameObject.name == "Player 1"){
				if (controls.Player.Crouch1.IsPressed()){
					if (crouched == false){
						crouched = true;
						sprite.updateSprite("Crouch1");
						hitbox.size = new Vector2(2.0f, 5.0f);
						hitbox.offset = new Vector2(0f, -1.0f);
					}
				}
				else{
					if (crouched == true){
						crouched = false;
						sprite.updateSprite("Character1.");
						hitbox.size = new Vector2(2.0f, 7.0f);
						hitbox.offset = new Vector2(0f, 0f);
					}
				}
				if (controls.Player.Block1.IsPressed()){
					if (blocking == false){
						blocking = true;
					}
				}
				else{
					if (blocking == true){
						blocking = false;
					}
				}
				moveInput = controls.Player.Move1.ReadValue<Vector2>();
			}
			else if (gameObject.name == "Player 2"){
				if (controls.Player.Crouch2.IsPressed()){
					if (crouched == false){
						crouched = true;
						sprite.updateSprite("Crouch2");
						hitbox.size = new Vector2(2.0f, 5.0f);
						hitbox.offset = new Vector2(0f, -1.0f);
					}
				}
				else{
					if (crouched == true){
						crouched = false;
						sprite.updateSprite("Character1.");
						hitbox.size = new Vector2(2.0f, 7.0f);
						hitbox.offset = new Vector2(0f, 0f);
					}
				}
				if (controls.Player.Block2.IsPressed()){
					if (blocking == false){
						blocking = true;
					}
				}
				else{
					if (blocking == true){
						blocking = false;
					}
				}
				moveInput = controls.Player.Move2.ReadValue<Vector2>();
			}
			sprite.flipSprite(moveInput.x);
			transform.Translate(Vector3.right * moveInput.x * Time.deltaTime * speed);
			if (transform.position.x < -9f){
				transform.position = new Vector3(-9f, transform.position.y, transform.position.z);
			}
			else if (transform.position.x > 9f){
				transform.position = new Vector3(9f, transform.position.y, transform.position.z);
			}
		}
    }
	
	public void OnEnable(){
		controls.Player.Enable();
		InputSystem.actions.FindActionMap("Player").actionTriggered += OnActionTriggered;
		controls.Player.Jump1.performed += OnJump1Triggered;
		controls.Player.Jump2.performed += OnJump2Triggered;
		controls.Player.Jab1.performed += OnJab1Triggered;
		controls.Player.Jab2.performed += OnJab2Triggered;
		controls.Player.Kick1.performed += OnKick1Triggered;
		controls.Player.Kick2.performed += OnKick2Triggered;
	}
	
	private void OnCollisionEnter2D(Collision2D collision){
		if (collision.gameObject.CompareTag("Ground")){
			onGround = true;
		}
	}
	
	private void OnTriggerEnter2D(Collider2D collider){
		if ((collider.CompareTag("Jab2") || collider.CompareTag("Kick2") || collider.CompareTag("Punch2") || collider.CompareTag("Special2")) && gameObject.name == "Player 1"){
			if (blocking == false){
				StartCoroutine(counterWin(collider));
			}
		}
		else if ((collider.CompareTag("Jab1") || collider.CompareTag("Kick1") || collider.CompareTag("Punch1") || collider.CompareTag("Special1")) && gameObject.name == "Player 2"){
			if (blocking == false){
				StartCoroutine(counterWin(collider));
			}
		}
	}
	
	private void OnActionTriggered(InputAction.CallbackContext context){
		if (!(jabCombo.Contains(context.action.name) || specialCombo.Contains(context.action.name))){
			return;
		}
		if (Time.time - prevTime > 0.5f){
            inputHistory.Clear();
        }
		inputHistory.Add(context.action.name);
        prevTime = Time.time;
		bool jabGo = false;
		bool specialGo = false;
		if (inputHistory.Count == jabCombo.Count){
			for (int i = 0; i < jabCombo.Count; i++){
				if (inputHistory[i] == jabCombo[i]){
					jabGo = true;
				}
				else{
					jabGo = false;
				}
			}
		}
		else if (inputHistory.Count == specialCombo.Count){
			for (int i = 0; i < specialCombo.Count; i++){
				if (inputHistory[i] != specialCombo[i]){
					specialGo = true;
				}
				else{
					specialGo = false;
				}
			}
		}
		else if (inputHistory.Count > specialCombo.Count){
			inputHistory.Clear();
		}
		if (jabGo == true){
			if (gameObject.name == "Player 1"){
				StartCoroutine(attack("Punch1"));
			}
			else if (gameObject.name == "Player 2"){
				StartCoroutine(attack("Punch2"));
			}
		}
		else if (specialGo == true){
			if (gameObject.name == "Player 1"){
				StartCoroutine(attack("Special1"));
			}
			else if (gameObject.name == "Player 2"){
				StartCoroutine(attack("Special2"));
			}
		}
	}
	
	private void OnJump1Triggered(InputAction.CallbackContext context){
		if (gameManager.gameActive == true && gameObject.name == "Player 1" && onGround == true){
			playerRb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
			onGround = false;
		}
	}
	
	private void OnJump2Triggered(InputAction.CallbackContext context){
		if (gameManager.gameActive == true && gameObject.name == "Player 2" && onGround == true){
			playerRb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
			onGround = false;
		}
	}
	
	private void OnJab1Triggered(InputAction.CallbackContext context){
		if (gameManager.gameActive == true && gameObject.name == "Player 1"){
			if (onGround == true){
				StartCoroutine(attack("Jab1"));
			}
			else{
				StartCoroutine(attack("JumpJab1"));
			}
		}
	}
	
	private void OnJab2Triggered(InputAction.CallbackContext context){
		if (gameManager.gameActive == true && gameObject.name == "Player 2"){
			if (onGround == true){
				StartCoroutine(attack("Jab2"));
			}
			else{
				StartCoroutine(attack("JumpJab2"));
			}
		}
	}
	
	private void OnKick1Triggered(InputAction.CallbackContext context){
		if (gameManager.gameActive == true && gameObject.name == "Player 1"){
			StartCoroutine(attack("Kick1"));
		}
	}
	
	private void OnKick2Triggered(InputAction.CallbackContext context){
		if (gameManager.gameActive == true && gameObject.name == "Player 2"){
			StartCoroutine(attack("Kick2"));
		}
	}
	
	IEnumerator attack(string type){
        transform.Find(type).gameObject.SetActive(true);
		sprite.updateSprite(type);
        yield return new WaitForSeconds(0.3f);
		transform.Find(type).gameObject.SetActive(false);
		sprite.updateSprite("Character1.");
    }
	
	IEnumerator counterWin(Collider2D collider){
		float timer = 0f;
		while (blocking == false && timer < 0.1f){
			timer += Time.deltaTime;
			yield return null;
		}
		if (blocking == true && gameObject.name == "Player 1"){
			p2HP.hp -= 5;
		}
		if (blocking == true && gameObject.name == "Player 2"){
			p1HP.hp -= 5;
		}
		while (blocking == false && timer < 0.2f){
			timer += Time.deltaTime;
			yield return null;
		}
		if (blocking == false && gameObject.name == "Player 1"){
			if (collider.CompareTag("Punch2")){
				p1HP.hp -= 10;
			}
			else if (collider.CompareTag("Special2")){
				p1HP.hp -= 15;
			}
			else{
				p1HP.hp -= 5;
			}
		}
		if (blocking == false && gameObject.name == "Player 2"){
			if (collider.CompareTag("Punch1")){
				p2HP.hp -= 10;
			}
			else if (collider.CompareTag("Special1")){
				p2HP.hp -= 15;
			}
			else{
				p2HP.hp -= 5;
			}
		}
    }
}
