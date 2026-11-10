using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class DragSelect : MonoBehaviour
{
	//attributes
	public float speed;
	public Vector2 moveInput;
	public GameObject selected;
	public SpriteManager sprite;
	public GameManager gameManager;
	private RectTransform rectTransform;
	private InputSystem_Actions controls;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
		selected = null;
    }
	
	//controls
	void Awake(){
		controls = new InputSystem_Actions();
	}

    // Update is called once per frame
    void Update()
    {
		//checks if character select screen is on
		if (gameManager.charSelect.activeInHierarchy == true){
			//player 1
			if (gameObject.name == "P1 Select"){
				moveInput = controls.Player.Move1.ReadValue<Vector2>();
			}
			//player 2
			else if (gameObject.name == "P2 Select"){
				moveInput = controls.Player.Move2.ReadValue<Vector2>();
			}
			transform.Translate(Vector3.right * moveInput.x * Time.deltaTime * speed);
			transform.Translate(Vector3.up * moveInput.y * Time.deltaTime * speed);
			//checks which character the selector is hovering over
			selected = checkOverlap();
			if (selected != null){
				if (selected.name == "Character1"){
					sprite.sprites = gameManager.char1;
					sprite.character = 1;
				}
				else if (selected.name == "Character2"){
					sprite.sprites = gameManager.char2;
					sprite.character = 2;
				}
				else if (selected.name == "Character3"){
					sprite.sprites = gameManager.char3;
					sprite.character = 3;
				}
				else if (selected.name == "Character4"){
					sprite.sprites = gameManager.char4;
					sprite.character = 4;
				}
			}
		}
    }
	
	//enables controls
	public void OnEnable(){
		controls.Player.Enable();
	}
	
	//disables controls
	public void OnDisable(){
		controls.Player.Disable();
	}
	
	//checks which character the selector is hovering over
	private GameObject checkOverlap(){
		Bounds target = new Bounds(rectTransform.rect.center, rectTransform.rect.size);
		foreach (GameObject character in gameManager.characters){
			Vector3[] imageCorners = new Vector3[4];
			Bounds imageBounds = new Bounds(rectTransform.InverseTransformPoint(character.GetComponent<RectTransform>().position), character.GetComponent<RectTransform>().rect.size);
			if (target.Intersects(imageBounds)){
                return character;
            }
		}
		return null;
	}
}
