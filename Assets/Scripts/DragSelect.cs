using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class DragSelect : MonoBehaviour, IDragHandler
{
	private RectTransform rectTransform;
	private GameManager gameManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
		gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	public void OnDrag(PointerEventData eventData){
        rectTransform.anchoredPosition += eventData.delta;
    }
}
