using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
	public bool gameActive;
	public GameObject titleScreen;
	public List<Image> characters;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       Application.targetFrameRate = 60;
	   gameActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	public void startGame(){
		gameActive = true;
		titleScreen.gameObject.SetActive(false);
	}
}
