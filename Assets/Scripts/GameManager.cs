using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
	public bool gameActive;
	public GameObject titleScreen;
	public GameObject start;
	public PlayerController p1;
	public PlayerController p2;
	public DragSelect p1Select;
	public DragSelect p2Select;
	public List<GameObject> characters;
	private InputSystem_Actions controls;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       Application.targetFrameRate = 60;
	   gameActive = false;
    }
	
	void Awake(){
		controls = new InputSystem_Actions();
	}

    // Update is called once per frame
    void Update()
    {
		if (titleScreen.activeInHierarchy == true){
			if (p1Select.selected != null && p2Select.selected != null){
				start.gameObject.SetActive(true);
			}
			else{
				start.gameObject.SetActive(false);
			}
		}
    }
	public void OnEnable(){
		controls.Player.Enable();
		controls.Player.Enter.performed += OnEnterTriggered;
	}
	
	private void OnEnterTriggered(InputAction.CallbackContext context){
		if (start.activeInHierarchy == true){
			gameActive = true;
			titleScreen.gameObject.SetActive(false);
			start.gameObject.SetActive(false);
			p1.character = p1Select.selected.name;
			p2.character = p2Select.selected.name;
		}
	}
}
