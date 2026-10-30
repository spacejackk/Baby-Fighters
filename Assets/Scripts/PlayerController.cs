using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerController : MonoBehaviour
{
	public float gravity;
	public float speed;
	public float jumpForce;
	public Vector2 moveInput;
	private bool onGround;
	private bool crouched;
	private Rigidbody2D playerRb;
	private GameManager gameManager;
	private InputSystem_Actions controls;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
		Physics.gravity *= gravity;
		onGround = true;
		crouched = false;
		gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
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
					transform.localScale = new Vector3(1f, 1.5f, 1f);
				}
				else{
					if (crouched == true){
						crouched = false;
						transform.position += new Vector3(0f, 0.75f, 0f);
					}
					transform.localScale = new Vector3(1f, 3f, 1f);
				}
				moveInput = controls.Player.Move1.ReadValue<Vector2>();
			}
			else if (gameObject.name == "Player 2"){
				if (controls.Player.Crouch2.IsPressed()){
					if (crouched == false){
						crouched = true;
						transform.position -= new Vector3(0f, 0.75f, 0f);
					}
					transform.localScale = new Vector3(1f, 1.5f, 1f);
				}
				else{
					if (crouched == true){
						crouched = false;
						transform.position += new Vector3(0f, 0.75f, 0f);
					}
					transform.localScale = new Vector3(1f, 3f, 1f);
				}
				moveInput = controls.Player.Move2.ReadValue<Vector2>();
			}
			transform.Translate(Vector3.right * moveInput.x * Time.deltaTime * speed);
		}
    }
	
	public void OnEnable(){
		controls.Player.Enable();
		controls.Player.Jump1.performed += OnJump1Triggered;
		controls.Player.Jump2.performed += OnJump2Triggered;
	}
	
	private void OnCollisionEnter2D(Collision2D collision){
		if (collision.gameObject.CompareTag("Ground")){
			onGround = true;
		}
	}
	
	private void OnJump1Triggered(InputAction.CallbackContext context){
		if (gameObject.name == "Player 1" && onGround == true){
			playerRb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
			onGround = false;
		}
	}
	
	private void OnJump2Triggered(InputAction.CallbackContext context){
		if (gameObject.name == "Player 2" && onGround == true){
			playerRb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
			onGround = false;
		}
	}
}
