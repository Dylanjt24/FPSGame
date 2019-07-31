using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [Header("Game")]
    [SerializeField] private Player player;
    [SerializeField] GameObject enemyContainer;

    [Header("UI")]
    [SerializeField] private Text healthText;
    [SerializeField] private Text ammoText;
    [SerializeField] private Text enemyText;
    [SerializeField] private Text infoText;

    private float resetTimer = 3f;
    private bool gameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        infoText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Update player health and ammo text
        healthText.text = "Health: " + player.Health;
        ammoText.text = "Ammo: " + player.Ammo;
        // Keep track of number of enemies
        int aliveEnemies = 0;
        foreach (Enemy enemy in enemyContainer.GetComponentsInChildren<Enemy>())
        {
            if (enemy.Killed == false)
                aliveEnemies++;
        }
        enemyText.text = "Enemies: " + aliveEnemies;

        if (aliveEnemies == 0)
        {
            gameOver = true;
            infoText.gameObject.SetActive(true);
            infoText.text = "You win!\nGood job!";
        }

        if (player.Killed)
        {
            gameOver = true;
            infoText.gameObject.SetActive(true);
            infoText.text = "You lose:(\n Try again!";
        }

        if (gameOver)
        {
            resetTimer -= Time.deltaTime;
            if (resetTimer <= 0)
                SceneManager.LoadScene("Menu");
        }
    }
}
