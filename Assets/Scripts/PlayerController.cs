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
	public Vector2 moveInput;
	public List<Sprite> sprites;
	public HealthBar p1HP;
	public HealthBar p2HP;
	public GameManager gameManager;
	private bool onGround;
	private bool crouched;
	private bool hit;
	private Rigidbody2D playerRb;
	private SpriteRenderer sprite;
	private InputSystem_Actions controls;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
		sprite = GetComponent<SpriteRenderer>();
		Physics.gravity *= gravity;
		onGround = true;
		crouched = false;
		hit = false;
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
						transform.position -= new Vector3(0f, 0.75f, 0f);
					}
					transform.localScale = new Vector3(0.5f, 0.25f, 1f);
				}
				else{
					if (crouched == true){
						crouched = false;
						transform.position += new Vector3(0f, 0.75f, 0f);
					}
					transform.localScale = new Vector3(0.5f, 0.5f, 1f);
				}
				moveInput = controls.Player.Move1.ReadValue<Vector2>();
			}
			else if (gameObject.name == "Player 2"){
				if (controls.Player.Crouch2.IsPressed()){
					if (crouched == false){
						crouched = true;
						transform.position -= new Vector3(0f, 0.75f, 0f);
					}
					transform.localScale = new Vector3(0.5f, 0.25f, 1f);
				}
				else{
					if (crouched == true){
						crouched = false;
						transform.position += new Vector3(0f, 0.75f, 0f);
					}
					transform.localScale = new Vector3(0.5f, 0.5f, 1f);
				}
				moveInput = controls.Player.Move2.ReadValue<Vector2>();
			}
			transform.Translate(Vector3.right * moveInput.x * Time.deltaTime * speed);
			if (transform.position.x < -8f){
				transform.position = new Vector3(-8f, transform.position.y, transform.position.z);
			}
			else if (transform.position.x > 8f){
				transform.position = new Vector3(8f, transform.position.y, transform.position.z);
			}
		}
    }
	
	public void OnEnable(){
		controls.Player.Enable();
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
		if ((collider.CompareTag("Jab2") || collider.CompareTag("Kick2")) && gameObject.name == "Player 1"){
			p1HP.hp -= 5;
			StartCoroutine(counterWin());
		}
		else if ((collider.CompareTag("Jab1") || collider.CompareTag("Kick1")) && gameObject.name == "Player 2"){
			p2HP.hp -= 5;
			StartCoroutine(counterWin());
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
			StartCoroutine(attack("Jab1"));
		}
	}
	
	private void OnJab2Triggered(InputAction.CallbackContext context){
		if (gameManager.gameActive == true && gameObject.name == "Player 2"){
			StartCoroutine(attack("Jab2"));
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
	
	private void OnCounter1Triggered(InputAction.CallbackContext context){
		if (hit == true && gameManager.gameActive == true && gameObject.name == "Player 1"){
			StartCoroutine(attack("Counter1"));
		}
	}
	
	private void OnCounter2Triggered(InputAction.CallbackContext context){
		if (hit == true && gameManager.gameActive == true && gameObject.name == "Player 2"){
			StartCoroutine(attack("Counter2"));
		}
	}
	
	IEnumerator attack(string type){
        transform.Find(type).gameObject.SetActive(true);
		sprite.sprite = sprites.Find(item => item.name == type.Remove(type.Length - 1) + "_0");
        yield return new WaitForSeconds(0.3f);
		transform.Find(type).gameObject.SetActive(false);
		sprite.sprite = sprites.Find(item => item.name == "Char1_0");
    }
	
	IEnumerator counterWin(){
        hit = true;
        yield return new WaitForSeconds(0.3f);
		hit = false;
    }
}
