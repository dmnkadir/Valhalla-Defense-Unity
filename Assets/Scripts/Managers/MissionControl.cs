using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MissionControl : MonoBehaviour
{
    public static MissionControl Instance;

    [Header("Operation Status")]
    [SerializeField] protected int currentMoney;    // Player's current money
    [SerializeField] protected int currentHealth;   // Player's current health

    [Header("Economy Settings")]
    [SerializeField] private int passiveIncomeAmount = 2;
    [SerializeField] private float incomeInterval = 1f;

    public bool isGameOver = false;
    protected float gameSpeed = 1f;
    private float incomeTimer = 0f;
    protected string missionName = "Defend Valhalla";

    [Header("Interface (UI) Connections")]
    public GameObject gameOverUI; // Game Over Panel
    public GameObject victoryUI;  // Victory Panel (Add if you want)

    [Header("Speed Button Visuals")]
    public Image speedButtonImage; // You will assign that single button in the hierarchy here
    public Sprite icon1x;
    public Sprite icon2x;
    public Sprite icon05x;

    public bool IsWaveActive { get; private set; }


    void Awake()
    {
        if (Instance == null)
            Instance = this;

        else
            Destroy(gameObject);
        GameLogger.InitLog(missionName, 100, 200);
    }


    void Update()
    {
        // CHECK: If game is not over AND wave is active, give money.
        if (!isGameOver && IsWaveActive)
        {
            incomeTimer += Time.deltaTime;

            if (incomeTimer >= incomeInterval)
            {
                AddMoney(passiveIncomeAmount);
                incomeTimer = 0f;
            }
        }
    }

    public void SetWaveStatus(bool status)
    {
        IsWaveActive = status;

    }

    void Start()
    {
        SetupMission(); // Start the mission
    }



    protected virtual void SetupMission() // This function can be overridden in child classes.
    {
        // Default settings (Starting Values in Documentation)
        currentMoney = 200;
        currentHealth = 100;
        SetGameSpeed(1f);



    }


    public void AddMoney(int amount)
    {
        currentMoney += amount;
    }
    public bool SpendMoney(int amount)
    {
        if (currentMoney >= amount)
        {
            currentMoney -= amount;
            return true;
        }
        return false;
    }
    public void TakeDamage(int damage)
    {
        // If the game is already over (e.g. if you won), don't take damage or die
        if (isGameOver) return;

        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            EndMission();
        }
    }
    void EndMission()
    {

        if (isGameOver) return;

        isGameOver = true;
        SetGameSpeed(0f);
        // Log setup
        GameLogger.Write($"---------------------------------------------------------------------------------------------------------");
        GameLogger.Write($"Ragnarok has arrived! You lost the game. Remaining Health: 0, Total Money: {currentMoney}.");

        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
        }
    }

    public void WinMission()
    {
        // LOCK: If the game is already over (Lose screen is open), do not enter here!
        if (isGameOver) return;

        isGameOver = true;
        SetGameSpeed(0f);

        Debug.Log("You Won!");
        // Add Log
        GameLogger.Write($"---------------------------------------------------------------------------------------------------------");
        GameLogger.Write($"END: All waves cleared. Valhalla is Safe! (Remaining Health: {currentHealth}, Total Money: {currentMoney})");
        PauseMenu pauseScript = FindFirstObjectByType<PauseMenu>();
        if (pauseScript != null)
        {
            pauseScript.enabled = false;
        }

        if (victoryUI != null)
        {
            victoryUI.SetActive(true);
        }
    }

    public void SetGameSpeed(float speedMultiplier)
    {
        Time.timeScale = speedMultiplier;
        gameSpeed = speedMultiplier;


        // If speed is not 0 (meaning game didn't end/freeze), change the image.
        if (speedButtonImage != null && speedMultiplier != 0f)
        {
            if (speedMultiplier == 1f) speedButtonImage.sprite = icon1x;
            else if (speedMultiplier == 2f) speedButtonImage.sprite = icon2x;
            else if (speedMultiplier == 0.5f) speedButtonImage.sprite = icon05x;
        }
    }

    public void CycleGameSpeed()
    {
        // If game is over, don't change speed
        if (isGameOver) return;

        // Cycle: 1x -> 2x -> 0.5x -> 1x
        if (Mathf.Approximately(Time.timeScale, 1f))
        {
            SetGameSpeed(2f);
        }
        else if (Mathf.Approximately(Time.timeScale, 2f))
        {
            SetGameSpeed(0.5f);
        }
        else
        {
            SetGameSpeed(1f);
        }
    }

    public void ToggleSpeed()       // Toggles between 1x and 2x speed
    {
        if (gameSpeed == 1f)
            SetGameSpeed(2f);
        else
            SetGameSpeed(1f);
    }
    public int GetCurrentMoney()    // Returns current money
    {
        return currentMoney;
    }
    public int GetCurrentHealth()   // Returns current health
    {
        return currentHealth;
    }
}