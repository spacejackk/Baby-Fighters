using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	//attributes
	public bool gameActive;
	public int selectedButton;
	public List<GameObject> characters;
	public List<GameObject> victory;
	public GameObject titleScreen;
	public GameObject charSelect;
	public GameObject controlsScreen;
	public GameObject pauseScreen;
	public GameObject start;
	public GameObject gameUI;
	public GameObject win;
    public PlayerController p1;
	public PlayerController p2;
	public DragSelect p1Select;
	public DragSelect p2Select;
	public List<Sprite> char1;
	public List<Sprite> char2;
	public List<Sprite> char3;
	public List<Sprite> char4;
	private float time;
	private int controller;
	private Vector2 moveInput;
	private InputSystem_Actions controls;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
	   Cursor.visible = false;
	   gameActive = false;
	   time = 0f;
	   selectedButton = 1;
	   controller = 1;
    }
	
	//controls
	void Awake(){
		controls = new InputSystem_Actions();
	}

    // Update is called once per frame
    void Update()
    {
		//to decide which player controls menus
		if (controller == 1){
			moveInput = controls.Player.Move1.ReadValue<Vector2>();
		}
		else if (controller == 2){
			moveInput = controls.Player.Move2.ReadValue<Vector2>();
		}
		//decides which button is currently selected + which ones are available depending on the screen
		//controls screen
		if (controlsScreen.activeInHierarchy == true){
			selectedButton = 4;
		}
		//victory screen
		else if (win.activeInHierarchy == true){
			selectedButton = 3;
		}
		//title screen
		else if (titleScreen.activeInHierarchy == true && time > 0.1f){
			if (moveInput.y < 0){
				selectedButton += 1;
            }
            else if (moveInput.y > 0){
				selectedButton -= 1;
            }
			if (selectedButton == 3){
                selectedButton = 1;
            }
            else if (selectedButton == 0){
                selectedButton = 2;
            }
			time = 0f;
		}
		//pause screen
		else if (pauseScreen.activeInHierarchy == true && time > 0.1f){
			if (moveInput.y < 0){
				selectedButton += 1;
            }
            else if (moveInput.y > 0){
				selectedButton -= 1;
            }
			if (selectedButton == 4){
                selectedButton = 2;
            }
            else if (selectedButton == 1){
                selectedButton = 3;
            }
			time = 0f;
		}
		//character select screen
		else if (charSelect.activeInHierarchy == true){
			if (p1Select.selected != null && p2Select.selected != null){
				start.gameObject.SetActive(true);
			}
			else{
				start.gameObject.SetActive(false);
			}
		}
		time += Time.unscaledDeltaTime;
    }
	
	//ends the game when a player dies
	public void endGame(HealthBar p1HP){
		gameActive = false;
		gameUI.gameObject.SetActive(false);
		win.gameObject.SetActive(true);
		if (p1HP.hp > 0f){
			victory.Find(item => item.CompareTag(p1.character)).SetActive(true);
		}
		else{
			victory.Find(item => item.CompareTag(p2.character)).SetActive(true);
		}
	}
	
	//enables controls
	public void OnEnable(){
		controls.Player.Enable();
		controls.Player.Enter.performed += OnEnterTriggered;
		controls.Player.Quit.performed += OnQuitTriggered;
		controls.Player.Pause1.performed += OnPause1Triggered;
		controls.Player.Pause2.performed += OnPause2Triggered;
	}
	
	//disables controls
	public void OnDisable(){
		controls.Player.Enter.performed -= OnEnterTriggered;
		controls.Player.Quit.performed -= OnQuitTriggered;
		controls.Player.Pause1.performed -= OnPause1Triggered;
		controls.Player.Pause2.performed -= OnPause2Triggered;
		controls.Player.Disable();
	}
	
	private void OnEnterTriggered(InputAction.CallbackContext context){
		//starts game
		if (start.activeInHierarchy == true){
			gameActive = true;
			charSelect.gameObject.SetActive(false);
			start.gameObject.SetActive(false);
			gameUI.gameObject.SetActive(true);
			p1.character = p1Select.selected.name;
			p2.character = p2Select.selected.name;
			//stats change depending on character
			if (p1.character.Contains("Character3")){
				p1.jumpForce = 9;
			}
			if (p1.character.Contains("Character4")){
				p1.speed = 12;
			}
			if (p2.character.Contains("Character3")){
				p2.jumpForce = 9;
			}
			if (p2.character.Contains("Character4")){
				p2.speed = 12;
			}
		}
		//play button on title screen
		else if (selectedButton == 1 && titleScreen.activeInHierarchy == true){
            titleScreen.SetActive(false);
			charSelect.gameObject.SetActive(true);
        }
		//controls button
		else if (selectedButton == 2){
			//in pause menu
			if (pauseScreen.activeInHierarchy == true){
				pauseScreen.gameObject.SetActive(false);
				controlsScreen.gameObject.SetActive(true);
			}
			//on title screen
			else if (titleScreen.activeInHierarchy == true){
				controlsScreen.gameObject.SetActive(true);
			}
        }
		//reset button
		else if (selectedButton == 3){
			Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
		//back button
		else if (selectedButton == 4 && controlsScreen.activeInHierarchy == true){
			//on title screen
            if (gameActive == false){
				controlsScreen.gameObject.SetActive(false);
				selectedButton = 1;
			}
			//in pause menu
			if (gameActive == true){
				pauseScreen.gameObject.SetActive(true);
				controlsScreen.gameObject.SetActive(false);
				selectedButton = 2;
			}
        }
	}
	
	//player 1 pause
	private void OnPause1Triggered(InputAction.CallbackContext context){
		if (gameActive == true){
			//on
			if (pauseScreen.activeInHierarchy == false){
				pauseScreen.gameObject.SetActive(true);
				selectedButton = 2;
				Time.timeScale = 0f;
			}
			//off
			else{
				pauseScreen.gameObject.SetActive(false);
				Time.timeScale = 1f;
			}
			controller = 1;
		}
	}
	
	//player 2 pause
	private void OnPause2Triggered(InputAction.CallbackContext context){
		if (gameActive == true){
			//on
			if (pauseScreen.activeInHierarchy == false){
				pauseScreen.gameObject.SetActive(true);
				selectedButton = 2;
				Time.timeScale = 0f;
				controller = 2;
			}
			//off
			else{
				pauseScreen.gameObject.SetActive(false);
				Time.timeScale = 1f;
				controller = 1;
			}
		}
	}
	
	//quits game
	private void OnQuitTriggered(InputAction.CallbackContext context){
		Application.Quit();
	}
}
