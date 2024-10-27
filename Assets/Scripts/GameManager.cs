using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int nextSceneToLoad;

    public float waitingTime;
    public GameObject spawnPoint;
    public GameObject[] enemies;
    public int maxEnemiesOnScreen;
    public int enemiesOnscreen;
    public int totalEnemies; // Total enemies that will be spawned in the game
    public int maxEnemiesPerWave = 5; // New variable to limit enemies per wave
    public int enemiesPerSpawn;
    public int currentGold;
    public Text goldText;
    public Text CurrentWaveText;
    public Text CurrentHealthText;
    public int maxWave;
    public int currentWave;
    public int maxHealth = 3;
    public int currentHealth;
    public GameObject winWindow;
    public GameObject loseWindow;

    private void Awake()
    {
        instance = this;
        currentHealth = maxHealth;
        nextSceneToLoad = SceneManager.GetActiveScene().buildIndex + 1;
    }

    // Update is called once per frame
    void Update()
    {
        goldText.text = currentGold.ToString();
        CurrentWaveText.text = currentWave.ToString() + "/" + maxWave.ToString();
        CurrentHealthText.text = currentHealth.ToString() + "/" + maxHealth.ToString();

        if (currentWave < maxWave && enemiesOnscreen == 0)
        {
            currentWave++;
            totalEnemies += maxEnemiesPerWave; // Increase total enemies by the max per wave
            maxEnemiesOnScreen += maxEnemiesPerWave; // Increase max enemies on screen
            waitingTime -= 0.1f;
            StartCoroutine(Spawn());
        }
        else if (currentWave == maxWave && enemiesOnscreen == 0)
        {
            winWindow.SetActive(true);
            StopAllCoroutines();
        }
    }

    public void AddGold(int amount)
    {
        currentGold += amount;
    }

    public void ReduceGold(int amount)
    {
        currentGold -= amount;
    }

    public void PlayerGetDamage()
    {
        currentHealth--;
        if (currentHealth <= 0)
        {
            loseWindow.SetActive(true);
            currentHealth = 0;
        }
    }

    IEnumerator Spawn()
    {
        // Check if we have less enemies than total enemies for the current wave
        if (enemiesOnscreen < totalEnemies)
        {
            // Loop through the enemies to spawn them
            for (int i = 0; i < enemiesPerSpawn; i++)
            {
                // Check if enemies on screen are less than max enemies on screen
                if (enemiesOnscreen < maxEnemiesOnScreen)
                {
                    // Spawn enemy if the condition above is true
                    GameObject newEnemy = Instantiate(enemies[Random.Range(0, enemies.Length)]); // Use enemies.Length for flexibility
                    newEnemy.transform.position = spawnPoint.transform.position;
                    enemiesOnscreen += 1;
                }
            }
            // Time between spawns
            yield return new WaitForSeconds(waitingTime);
            StartCoroutine(Spawn());
        }
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(nextSceneToLoad);
        AudioManager.instance.PlaySFX(3);
        if (nextSceneToLoad > PlayerPrefs.GetInt("levelAt"))
        {
            PlayerPrefs.SetInt("levelAt", nextSceneToLoad);
        }
    }

    public void ReplyLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        AudioManager.instance.PlaySFX(3);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
        AudioManager.instance.PlaySFX(3);
    }
}
