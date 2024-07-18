using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    private GameObject gameOverUI;
    private TextMeshProUGUI txtGameOver;
    private TextMeshProUGUI txtKillCount;
    private Vector3 startPosition;
    // Start is called before the first frame update
    void Start()
    {
        gameOverUI = GameObject.Find("GameOverUI");
        txtGameOver = gameOverUI.transform.Find("TxtGameOver").GetComponent<TextMeshProUGUI>();
        txtKillCount = gameOverUI.transform.Find("TxtKillCounts").GetComponent<TextMeshProUGUI>();
        if (gameOverUI != null)
        {
            startPosition = gameOverUI.transform.position;
            gameOverUI.transform.position = new Vector3(10000,0,0);
        }
    }
    public void UpdateKillCount(int value)
    {
        txtKillCount.text = "Your total Kills : " + value;
    }
    public void Win(GameObject winner)
    {
        string name = winner.GetComponent<PlayerProps>()?.characterName ?? (winner.GetComponent<PlayerAIProps>().characterName ?? "");
        if (gameOverUI != null)
        {
            txtGameOver.text = $"WINNER \n {name}!";
            gameOverUI.transform.position = startPosition;
        }
    }

    public void gameOver()
    {
        if (gameOverUI != null)
        {
            gameOverUI.transform.position = startPosition;
        }

    }

}
