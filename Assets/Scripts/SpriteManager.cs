using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpriteManager : MonoBehaviour
{
	//attributes
	public int character;
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
	
	//updates sprite
	public void updateSprite(string type){
		//finds sprite in list
		sprite.sprite = sprites.Find(item => item.name == type.Remove(type.Length - 1));
		//determines sprite offset depending on which sprite it is
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
		else if (type.Contains("Special1")){
			transform.localPosition = new Vector3(offset.x, offset.y, offset.z);
		}
		else if (type.Contains("Special3")){
			transform.localPosition = new Vector3(0.9f / offset.x, offset.y, offset.z);
		}
		else{
			transform.localPosition = offset;
		}
	}
	
	//flips sprite and hurtboxes
	public void flipSprite(float moveInput){
		//right
		if (moveInput > 0){
            sprite.flipX = false;
			offset = new Vector3(0.8f, 0f, 0f);
			foreach (GameObject hurtbox in hurtboxes){
				hurtbox.transform.localPosition = new Vector3(1.8f, hurtbox.transform.localPosition.y, hurtbox.transform.localPosition.z);
			}
        }
		//left
        else if (moveInput < 0){
            sprite.flipX = true;
			offset = new Vector3(-0.8f, 0f, 0f);
			foreach (GameObject hurtbox in hurtboxes){
				hurtbox.transform.localPosition = new Vector3(-1.8f, hurtbox.transform.localPosition.y, hurtbox.transform.localPosition.z);
			}
        }
	}
}
