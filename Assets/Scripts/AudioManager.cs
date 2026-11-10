using UnityEngine;

public class AudioManager : MonoBehaviour
{
	//attributes
    public AudioSource menuMusic;
    public AudioSource battleMusic;
    public GameManager gameManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		//switches music
		//in battle
        if (gameManager.gameActive == true){
            battleMusic.mute = false;
            menuMusic.mute = true;
        }
		//out of battle
		else{
			battleMusic.mute = true;
            menuMusic.mute = false;
		}
    }
}
