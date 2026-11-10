using UnityEngine;
using UnityEngine.UI;

public class ButtonSelected : MonoBehaviour
{
	//attributes
    public GameManager gameManager;
    public int buttonNumber;
    public Image image;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
		//button selected color
        if (gameManager.selectedButton == buttonNumber){
            Color tempColor = image.color;
            tempColor.a = 1f;
            image.color = tempColor;
        }
		//button not selected color
		else if(gameManager.selectedButton != buttonNumber){
            Color tempColor = image.color;
            tempColor.a = 0.5f;
            image.color = tempColor;
        }
    }
}
