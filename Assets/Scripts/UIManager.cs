using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameManager gameManager;
    public Vector2 moveInput;
    private InputSystem_Actions controls;
    private float time;
    public int selectedButton = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        time = 0f;
    }
    void Awake()
    {
        controls = new InputSystem_Actions();
    }

    public void OnEnable()
    {
        controls.Player.Enable();
        controls.Player.Enter.performed += OnEnterTriggered;
    }

    // Update is called once per frame
    void Update()
    {
        moveInput = controls.Player.Move1.ReadValue<Vector2>();
        if (gameManager.titleScreen.activeInHierarchy == true && time > 0.1f)
        {
            if (moveInput.y < 0)
            {
                selectedButton += 1;
            }
            else if (moveInput.y > 0)
            {
                selectedButton -= 1;
            }
            if (selectedButton == 4)
            {
                selectedButton = 1;
            }
            else if (selectedButton == 0)
            {
                selectedButton = 3;
            }
            time = 0f;
        }
        time += Time.deltaTime;
        if (gameManager.optionsScreen.activeInHierarchy == true && time > 0.1f)
        {
            if (moveInput.y < 0)
            {
                selectedButton += 1;
            }
            else if (moveInput.y > 0)
            {
                selectedButton -= 1;
            }
            if (selectedButton == 3)
            {
                selectedButton = 1;
            }
            else if (selectedButton == 0)
            {
                selectedButton = 2;
            }
            time = 0f;
        }
    }
    private void OnEnterTriggered(InputAction.CallbackContext context)
    {
        if(selectedButton == 1 && gameManager.titleScreen.activeInHierarchy == true)
        {
            gameManager.titleScreen.SetActive(false);
        }
        if (selectedButton == 2 && gameManager.titleScreen.activeInHierarchy == true)
        {
            gameManager.titleScreen.SetActive(false);
            gameManager.optionsScreen.SetActive(true);
            selectedButton = 0;
        }
        if (selectedButton == 3 && gameManager.titleScreen.activeInHierarchy == true)
        {
            gameManager.titleScreen.SetActive(false);
            gameManager.controlsScreen.SetActive(true);
            selectedButton = 0;
        }
        if (selectedButton == 2 && gameManager.optionsScreen.activeInHierarchy == true)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
