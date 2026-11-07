using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class DragSelect : MonoBehaviour
{
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
	
	void Awake(){
		controls = new InputSystem_Actions();
	}

    // Update is called once per frame
    void Update()
    {
		if (gameManager.charSelect.activeInHierarchy == true){
			if (gameObject.name == "P1 Select"){
				moveInput = controls.Player.Move1.ReadValue<Vector2>();
			}
			else if (gameObject.name == "P2 Select"){
				moveInput = controls.Player.Move2.ReadValue<Vector2>();
			}
			transform.Translate(Vector3.right * moveInput.x * Time.deltaTime * speed);
			transform.Translate(Vector3.up * moveInput.y * Time.deltaTime * speed);
			selected = checkOverlap();
			if (selected != null){
				if (selected.name == "Character1"){
					sprite.sprites = gameManager.char1;
				}
				else if (selected.name == "Character2"){
					sprite.sprites = gameManager.char2;
				}
				else if (selected.name == "Character3"){
					sprite.sprites = gameManager.char3;
				}
				else if (selected.name == "Character4"){
					sprite.sprites = gameManager.char4;
				}
			}
		}
    }
	
	public void OnEnable(){
		controls.Player.Enable();
	}
	
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
