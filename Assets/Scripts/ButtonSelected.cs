using UnityEngine;
using UnityEngine.UI;

public class ButtonSelected : MonoBehaviour
{
    public UIManager UIManager;
    public int buttonNumber;
    public Image myImage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myImage = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if(UIManager.selectedButton == buttonNumber)
        {
            Color tempColor = myImage.color;
            tempColor.a = 0.5f;
            myImage.color = tempColor;
        } else if(UIManager.selectedButton != buttonNumber)
        {
            Color tempColor = myImage.color;
            tempColor.a = 1f;
            myImage.color = tempColor;
        }
    }
}
