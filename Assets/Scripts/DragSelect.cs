using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class DragSelect : MonoBehaviour, IDragHandler
{
	public float speed;
	public Vector2 moveInput;
	public GameObject selected;
	private RectTransform rectTransform;
	private GameManager gameManager;
	private InputSystem_Actions controls;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
		gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }
	
	void Awake(){
		controls = new InputSystem_Actions();
	}

    // Update is called once per frame
    void Update()
    {
		if (gameManager.titleScreen.activeInHierarchy == true){
			if (gameObject.name == "P1 Select"){
				moveInput = controls.Player.Move1.ReadValue<Vector2>();
			}
			else if (gameObject.name == "P2 Select"){
				moveInput = controls.Player.Move2.ReadValue<Vector2>();
			}
			transform.Translate(Vector3.right * moveInput.x * Time.deltaTime * speed);
			transform.Translate(Vector3.up * moveInput.y * Time.deltaTime * speed);
			selected = checkOverlap();
		}
    }
	
	public void OnEnable(){
		controls.Player.Enable();
	}
	
	public void OnDrag(PointerEventData eventData){
        rectTransform.anchoredPosition += eventData.delta;
		selected = checkOverlap();
    }
	
	private GameObject checkOverlap(){
		Bounds target = new Bounds(rectTransform.position, rectTransform.rect.size);
		foreach (GameObject character in gameManager.characters){
			Vector3[] imageCorners = new Vector3[4];
			Bounds imageBounds = new Bounds(character.GetComponent<RectTransform>().position, character.GetComponent<RectTransform>().rect.size);
			if (target.Intersects(imageBounds)){
                return character;
            }
		}
		return null;
	}
}
