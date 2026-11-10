using UnityEngine;

public class AudioManager : MonoBehaviour
{
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
        if(gameManager.gameActive == true)
        {
            battleMusic.mute = false;
            menuMusic.mute = true;
        }
    }
}
