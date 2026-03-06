using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class WaveSpawner : MonoBehaviour
{

    // This holds the info of "Which path, what type, how many".
    [System.Serializable]
    public class WaveGroup
    {
        public GameObject enemyPrefab; // Which enemy?
        public int count;              // How many?
        public float rate;             // At what rate?

        [Tooltip("Which path should this group follow? (0, 1, 2)")]
        public int pathIndex;          // Which path?
    }

    // Now there can be multiple "Groups" within a single wave.
    [System.Serializable]
    public class Wave
    {
        public string name;            // Wave Name (Ex: "Great Attack")
        public WaveGroup[] groups;     // LIST OF GROUPS TO SPAWN SIMULTANEOUSLY
    }

    [Header("Wave List")]
    public Wave[] waves;

    [Header("Paths")]
    public Waypoints[] paths; // You will drag your 3 Paths here

    [Header("Settings")]
    public float timeBetweenWaves = 5f;
    private float countdown = 2f;
    private int waveIndex = 0;

    [Header("UI & Bonus Settings")]
    public Image waveTimerRing;    // (Filled Image) The outer decreasing bar
    public Button startButton;     // The Dragon Button in the middle
    public int rewardPerSecond = 3; // Per-second reward for early calling

    private bool isSpawning = false;

    private bool hasGameStarted = false;
    void Start()
    {
        // Set countdown to wait time at start so UI looks correct
        countdown = timeBetweenWaves;

        // Ensure button is active at start
        if (startButton != null) startButton.interactable = true;
    }

    void Update()
    {
        // GAME OVER CHECK
        if (waveIndex >= waves.Length)
        {
            if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
            {
                if (MissionControl.Instance != null)
                    MissionControl.Instance.WinMission();

                if (startButton != null) startButton.interactable = false;
                if (waveTimerRing != null) waveTimerRing.fillAmount = 0;

                this.enabled = false;
            }
            return;
        }

        // WAIT IF ENEMIES ARE SPAWNING
        if (isSpawning)
        {
            if (startButton != null) startButton.interactable = false;
            if (waveTimerRing != null) waveTimerRing.fillAmount = 0;
            return;
        }


        // If the game hasn't been started by pressing the "Start" button yet:
        if (!hasGameStarted)
        {
            // Keep button clickable
            if (startButton != null) startButton.interactable = true;

            // Let the ring appear full (meaning Ready)
            if (waveTimerRing != null) waveTimerRing.fillAmount = 1f;

            // Do not countdown, exit Update.
            return;
        }



        if (startButton != null) startButton.interactable = true;

        if (countdown <= 0f)
        {
            StartCoroutine(SpawnWave());
            countdown = timeBetweenWaves;
            return;
        }

        countdown -= Time.deltaTime;

        if (waveTimerRing != null)
        {
            waveTimerRing.fillAmount = countdown / timeBetweenWaves;
        }
    }

    public void SkipWave()
    {
        // If already spawning, do nothing
        if (isSpawning) return;


        // If game hasn't started yet (First Wave)
        if (!hasGameStarted)
        {
            hasGameStarted = true; // Mark game as started
            countdown = 0f;        // Reset counter so Update starts the wave immediately

            // IMPORTANT: We do not call MissionControl.AddMoney here!
            Debug.Log("Game Started! (No bonus given for first wave)");
            return; // Exit function
        }

        // If game has already started (Early Call Bonus)
        if (countdown > 0f)
        {
            int bonus = Mathf.FloorToInt(countdown) * rewardPerSecond;

            if (MissionControl.Instance != null)
            {
                MissionControl.Instance.AddMoney(bonus);
                Debug.Log($"Wave called early! Bonus: {bonus}");
            }

            countdown = 0f; // Start wave immediately
        }
    }

    //
    IEnumerator SpawnWave()
    {
        isSpawning = true;
        Wave currentWave = waves[waveIndex];
        // WAVE START LOG
        GameLogger.Write($"Wave {waveIndex + 1} Started. (Scenario: {currentWave.name})");
        Debug.Log($"--- {currentWave.name} STARTING ---");

        if (MissionControl.Instance != null)
            MissionControl.Instance.SetWaveStatus(true);

        // Find the duration of the longest group
        float longestGroupDuration = 0f;

        // Start ALL groups in the wave simultaneously!
        foreach (WaveGroup group in currentWave.groups)
        {
            // Run a separate "worker" (Coroutine) for each group
            StartCoroutine(SpawnGroupRoutine(group));

            // Duration calculation: (Count / Rate)
            float duration = group.count / group.rate;
            if (duration > longestGroupDuration)
            {
                longestGroupDuration = duration;
            }
        }

        // Wait for the longest group to finish
        yield return new WaitForSeconds(longestGroupDuration);

        // Prepare for the next wave when all are finished
        waveIndex++;

        if (MissionControl.Instance != null)
            MissionControl.Instance.SetWaveStatus(false);

        isSpawning = false;
    }


    // Spawns a single group.
    IEnumerator SpawnGroupRoutine(WaveGroup group)
    {
        for (int i = 0; i < group.count; i++)
        {
            SpawnEnemy(group.enemyPrefab, group.pathIndex);
            yield return new WaitForSeconds(1f / group.rate);
        }
    }

    void SpawnEnemy(GameObject _enemyPrefab, int pathIndex)
    {
        // Path Error Check
        if (pathIndex >= paths.Length || pathIndex < 0)
        {
            Debug.LogError($"ERROR: Path {pathIndex} not found! Path 0 is being used.");
            pathIndex = 0;
        }

        Waypoints selectedPath = paths[pathIndex];

        // Spawn and Assign Path
        GameObject enemyObj = Instantiate(_enemyPrefab, selectedPath.points[0].position, _enemyPrefab.transform.rotation);
        EnemyMovement movement = enemyObj.GetComponent<EnemyMovement>();


        // --- LOGGING ---
        // Get enemy name
        string rawName = _enemyPrefab.name;
        // Generate ID (Ex: Draugr-ID001)
        string uniqueID = GameLogger.GetNewID(rawName);
        enemyObj.name = uniqueID;
        // Get enemy properties (HP, Armor)
        Enemy enemyScript = enemyObj.GetComponent<Enemy>();
        float startHP = enemyScript.BaseHealth;

        GameLogger.Write($"Enemy '{uniqueID}' (HP: {startHP}/{startHP}) entered the map.");

        if (movement != null)
        {
            movement.SetPath(selectedPath);
        }
    }
}