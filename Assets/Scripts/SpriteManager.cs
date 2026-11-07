using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpriteManager : MonoBehaviour
{
	public List<GameObject> hurtboxes;
	public List<Sprite> sprites;
	private Vector3 offset;
	private SpriteRenderer sprite;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	public void updateSprite(string type){
		sprite.sprite = sprites.Find(item => item.name == type.Remove(type.Length - 1));
		if (type.Contains("Character")){
			transform.localPosition = Vector3.zero;
		}
		else if (type.Contains("Crouch")){
			transform.localPosition = new Vector3(offset.x, -1.0f, offset.z);
		}
		else if (type.Contains("Hurt")){
			transform.localPosition = new Vector3(offset.x * -1.25f, offset.y, offset.z);
		}
		else if (type.Contains("Down")){
			transform.localPosition = new Vector3(offset.x, -2.5f, offset.z);
		}
		else{
			transform.localPosition = offset;
		}
	}
	
	public void flipSprite(float moveInput){
		//right
		if (moveInput > 0){
            sprite.flipX = false;
			offset = new Vector3(0.8f, 0f, 0f);
			foreach (GameObject hurtbox in hurtboxes){
				hurtbox.transform.localPosition = new Vector3(1.0f, hurtbox.transform.localPosition.y, hurtbox.transform.localPosition.z);
			}
        }
		//left
        else if (moveInput < 0){
            sprite.flipX = true;
			offset = new Vector3(-0.8f, 0f, 0f);
			foreach (GameObject hurtbox in hurtboxes){
				hurtbox.transform.localPosition = new Vector3(-1.0f, hurtbox.transform.localPosition.y, hurtbox.transform.localPosition.z);
			}
        }
	}
}
