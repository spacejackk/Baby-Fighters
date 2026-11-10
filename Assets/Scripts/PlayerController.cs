using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
	//attributes
	public float gravity;
	public float speed;
	public float jumpForce;
	public string character;
	public bool blocking;
	public bool hurt;
	public Vector2 moveInput;
	public PlayerController otherPlayer;
	public HealthBar p1HP;
	public HealthBar p2HP;
	public SpriteManager sprite;
	public GameManager gameManager;
	private bool onGround;
	private bool crouched;
	private bool attacking;
	private bool dead;
	private bool jabGo;
	private bool specialGo;
	private float prevTime;
	private float walkTime;
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
		attacking = false;
		hurt = false;
		dead = false;
		walkTime = 0f;
		//combos
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
	
	//controls
	void Awake(){
		controls = new InputSystem_Actions();
	}

    // Update is called once per frame
    void Update()
    {
		if (gameManager.gameActive == true && hurt == false && dead == false){
			if (gameObject.name == "Player 1"){
				//player 1 blocking
				if (controls.Player.Block1.IsPressed()){
					if (blocking == false){
						blocking = true;
						sprite.updateSprite("Block1");
					}
				}
				else{
					if (blocking == true){
						sprite.updateSprite("Character1");
						blocking = false;
					}
				}
				//player 1 crouching
				if (controls.Player.Crouch1.IsPressed() && blocking == false){
					if (crouched == false){
						sprite.updateSprite("Crouch1");
						hitbox.size = new Vector2(2.0f, 5.0f);
						hitbox.offset = new Vector2(0f, -1.0f);
						crouched = true;
					}
				}
				else{
					if (crouched == true){
						sprite.updateSprite("Character1");
						hitbox.size = new Vector2(2.0f, 7.0f);
						hitbox.offset = new Vector2(0f, 0f);
						crouched = false;
					}
				}
				moveInput = controls.Player.Move1.ReadValue<Vector2>();
			}
			else if (gameObject.name == "Player 2"){
				//player 2 blocking
				if (controls.Player.Block2.IsPressed()){
					if (blocking == false){
						sprite.updateSprite("Block2");
						blocking = true;
					}
				}
				else{
					if (blocking == true){
						sprite.updateSprite("Character1");
						blocking = false;
					}
				}
				//player 2 crouching
				if (controls.Player.Crouch2.IsPressed() && blocking == false){
					if (crouched == false){
						sprite.updateSprite("Crouch2");
						hitbox.size = new Vector2(2.0f, 5.0f);
						hitbox.offset = new Vector2(0f, -1.0f);
						crouched = true;
					}
				}
				else{
					if (crouched == true){
						sprite.updateSprite("Character1");
						hitbox.size = new Vector2(2.0f, 7.0f);
						hitbox.offset = new Vector2(0f, 0f);
						crouched = false;
					}
				}
				moveInput = controls.Player.Move2.ReadValue<Vector2>();
			}
			//player movement
			if (blocking == false && crouched == false){
				sprite.flipSprite(moveInput.x);
				transform.Translate(Vector3.right * moveInput.x * Time.deltaTime * speed);
				walkTime += Time.deltaTime;
				//changes sprites to simulate walk cycle
				if (attacking == false){
					if (moveInput.x != 0){
						if (walkTime < 0.2f){
							sprite.updateSprite("Character1");
						}
						else if (walkTime > 0.4f){
							walkTime = 0f;
						}
						else{
							sprite.updateSprite("Moving.");
						}
					}
					else{
						sprite.updateSprite("Character1");
					}
				}
			}
			//bounds
			if (transform.position.x < -9f){
				transform.position = new Vector3(-9f, transform.position.y, transform.position.z);
			}
			else if (transform.position.x > 9f){
				transform.position = new Vector3(9f, transform.position.y, transform.position.z);
			}
		}
    }
	
	//enables controls
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
	
	//disables controls
	public void OnDisable(){
		InputSystem.actions.FindActionMap("Player").actionTriggered -= OnActionTriggered;
		controls.Player.Jump1.performed -= OnJump1Triggered;
		controls.Player.Jump2.performed -= OnJump2Triggered;
		controls.Player.Jab1.performed -= OnJab1Triggered;
		controls.Player.Jab2.performed -= OnJab2Triggered;
		controls.Player.Kick1.performed -= OnKick1Triggered;
		controls.Player.Kick2.performed -= OnKick2Triggered;
		controls.Player.Disable();
	}
	
	//ground collision
	private void OnCollisionEnter2D(Collision2D collision){
		if (collision.gameObject.CompareTag("Ground")){
			onGround = true;
		}
	}
	
	//hurtbox collision
	private void OnTriggerEnter2D(Collider2D collider){
		//player 1
		if ((collider.CompareTag("Jab2") || collider.CompareTag("Kick2") || collider.CompareTag("Punch2") || collider.CompareTag("Special2")) && gameObject.name == "Player 1"){
			if (blocking == false && hurt == false && dead == false){
				StartCoroutine(counterWin(collider));
			}
		}
		//player 2
		else if ((collider.CompareTag("Jab1") || collider.CompareTag("Kick1") || collider.CompareTag("Punch1") || collider.CompareTag("Special1")) && gameObject.name == "Player 2"){
			if (blocking == false && hurt == false && dead == false){
				StartCoroutine(counterWin(collider));
			}
		}
	}
	
	//checks any action
	private void OnActionTriggered(InputAction.CallbackContext context){
		if (context.performed && (jabCombo.Contains(context.action.name) || specialCombo.Contains(context.action.name)) && blocking == false && hurt == false && dead == false){
			//records input history
			if (Time.time - prevTime > 0.5f){
				inputHistory.Clear();
			}
			inputHistory.Add(context.action.name);
			prevTime = Time.time;
			jabGo = false;
			specialGo = false;
			//checks if the right inputs have been done for a combo
			if (inputHistory.Count == jabCombo.Count){
				jabGo = true;
				for (int i = 0; i < jabCombo.Count; i++){
					 jabGo = jabGo && (inputHistory[i] == jabCombo[i]);
				}
			}
			else if (inputHistory.Count == specialCombo.Count){
				specialGo = true;
				for (int i = 0; i < specialCombo.Count; i++){
					 specialGo = specialGo && (inputHistory[i] == specialCombo[i]);
				}
			}
			else if (inputHistory.Count > specialCombo.Count){
				inputHistory.Clear();
			}
			//jab combo
			if (jabGo == true){
				//player 1
				if (gameObject.name == "Player 1"){
					if (onGround == true){
						StartCoroutine(attack("Jab1"));
					}
					else{
						StartCoroutine(attack("JumpJab1"));
					}
					StartCoroutine(attack("Punch1"));
				}
				//player 2
				else if (gameObject.name == "Player 2"){
					if (onGround == true){
						StartCoroutine(attack("Jab2"));
					}
					else{
						StartCoroutine(attack("JumpJab2"));
					}
					StartCoroutine(attack("Punch2"));
				}
			}
			//special combo
			else if (specialGo == true){
				//player 1
				if (gameObject.name == "Player 1"){
					if (onGround == true){
						StartCoroutine(attack("Jab1"));
					}
					else{
						StartCoroutine(attack("JumpJab1"));
					}
					if (character.Contains("Character1")){
						StartCoroutine(attack("Special1"));
					}
					else if (character.Contains("Character2")){
						StartCoroutine(attack("Special2"));
					}
					else if (character.Contains("Character3")){
						StartCoroutine(attack("Special3"));
					}
					else if (character.Contains("Character4")){
						StartCoroutine(attack("Special4"));
					}
				}
				//player 2
				else if (gameObject.name == "Player 2"){
					if (onGround == true){
						StartCoroutine(attack("Jab2"));
					}
					else{
						StartCoroutine(attack("JumpJab2"));
					}
					if (character.Contains("Character1")){
						StartCoroutine(attack("Special1"));
					}
					else if (character.Contains("Character2")){
						StartCoroutine(attack("Special2"));
					}
					else if (character.Contains("Character3")){
						StartCoroutine(attack("Special3"));
					}
					else if (character.Contains("Character4")){
						StartCoroutine(attack("Special4"));
					}
				}
			}
		}
	}
	
	//player 1 jump
	private void OnJump1Triggered(InputAction.CallbackContext context){
		if (gameManager.gameActive == true && gameObject.name == "Player 1" && onGround == true && blocking == false && hurt == false && dead == false){
			playerRb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
			onGround = false;
		}
	}
	
	//player 2 jump
	private void OnJump2Triggered(InputAction.CallbackContext context){
		if (gameManager.gameActive == true && gameObject.name == "Player 2" && onGround == true && blocking == false && hurt == false && dead == false){
			playerRb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
			onGround = false;
		}
	}
	
	//player 1 jab
	private void OnJab1Triggered(InputAction.CallbackContext context){
		if (gameManager.gameActive == true && gameObject.name == "Player 1" && blocking == false && jabGo == false && specialGo == false && hurt == false && dead == false){
			if (onGround == true){
				StartCoroutine(attack("Jab1"));
			}
			else{
				StartCoroutine(attack("JumpJab1"));
			}
		}
	}
	
	//player 2 jab
	private void OnJab2Triggered(InputAction.CallbackContext context){
		if (gameManager.gameActive == true && gameObject.name == "Player 2" && blocking == false && jabGo == false && specialGo == false && hurt == false && dead == false){
			if (onGround == true){
				StartCoroutine(attack("Jab2"));
			}
			else{
				StartCoroutine(attack("JumpJab2"));
			}
		}
	}
	
	//player 1 kick
	private void OnKick1Triggered(InputAction.CallbackContext context){
		if (gameManager.gameActive == true && gameObject.name == "Player 1" && blocking == false && hurt == false && dead == false){
			StartCoroutine(attack("Kick1"));
		}
	}
	
	//player 2 kick
	private void OnKick2Triggered(InputAction.CallbackContext context){
		if (gameManager.gameActive == true && gameObject.name == "Player 2" && blocking == false && hurt == false && dead == false){
			StartCoroutine(attack("Kick2"));
		}
	}
	
	//does attack
	IEnumerator attack(string type){
		yield return new WaitUntil(() => attacking == false);
		//everything turns on
		attacking = true;
        transform.Find(type).gameObject.SetActive(true);
		sprite.updateSprite(type);
		otherPlayer.playerRb.WakeUp();
        yield return new WaitForSeconds(0.3f);
		//everything turns off
		transform.Find(type).gameObject.SetActive(false);
		sprite.updateSprite("Character1");
		attacking = false;
    }
	
	//changes to hurt sprite
	IEnumerator hurtSprite(PlayerController player){
		//everything turns on
		player.hurt = true;
		player.sprite.updateSprite("Hurt.");
		yield return new WaitForSeconds(0.3f);
		//everything turns off
		//checks if player is dead
		if (player.dead == true){
			player.sprite.updateSprite("Down.");
			gameManager.endGame(p1HP);
		}
		else{
			player.sprite.updateSprite("Character1");
			player.hurt = false;
		}
	}
	
	//checks window for a counter
	IEnumerator counterWin(Collider2D collider){
		float timer = 0f;
		//start of counter window
		while (blocking == false && timer < 0.1f){
			timer += Time.deltaTime;
			yield return null;
		}
		//checks if player blocked in time to counter
		//player 1
		if (blocking == true && otherPlayer.hurt == false && gameObject.name == "Player 1"){
			p2HP.hp -= 5.0f;
			StartCoroutine(hurtSprite(otherPlayer));
		}
		//player 2
		if (blocking == true && otherPlayer.hurt == false && gameObject.name == "Player 2"){
			p1HP.hp -= 5.0f;
			StartCoroutine(hurtSprite(otherPlayer));
		}
		//end of counter window
		while (blocking == false && timer < 0.2f){
			timer += Time.deltaTime;
			yield return null;
		}
		//checks if player is blocking
		//player 1
		if (blocking == false && hurt == false && gameObject.name == "Player 1"){
			if (collider.CompareTag("Punch2")){
				p1HP.hp -= 10.0f;
			}
			else if (collider.CompareTag("Special2")){
				p1HP.hp -= 15.0f;
			}
			else if (collider.CompareTag("XSpecial2")){
				p2HP.hp -= 20.0f;
			}
			else{
				p1HP.hp -= 5.0f;
			}
			StartCoroutine(hurtSprite(gameObject.GetComponent<PlayerController>()));
		}
		//player 2
		if (blocking == false && hurt == false && gameObject.name == "Player 2"){
			if (collider.CompareTag("Punch1")){
				p2HP.hp -= 10.0f;
			}
			else if (collider.CompareTag("Special1")){
				p2HP.hp -= 15.0f;
			}
			else if (collider.CompareTag("XSpecial1")){
				p2HP.hp -= 20.0f;
			}
			else{
				p2HP.hp -= 5.0f;
			}
			StartCoroutine(hurtSprite(gameObject.GetComponent<PlayerController>()));
		}
		//checks if player is dead
		//player 1
		if (gameObject.name == "Player 1" && p1HP.hp <= 0){
			dead = true;
			sprite.updateSprite("Down1");
		}
		//player 2
		else if (gameObject.name == "Player 2" && p2HP.hp <= 0){
			dead = true;
			sprite.updateSprite("Down2");
		}
    }
}
