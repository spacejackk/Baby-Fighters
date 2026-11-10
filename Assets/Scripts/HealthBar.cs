using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
	//attributes
	public float drain;
	public float hp;
	public float maxHP;
	private float fill;
	private Image image;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
		image = GetComponent<Image>();
		hp = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
		//keeps hp bar filled to the hp value
		fill = Mathf.Clamp01(hp / maxHP);
        if (image.fillAmount != fill){
            image.fillAmount = Mathf.Lerp(image.fillAmount, fill, Time.deltaTime * drain);
        }
    }
	
}
