using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    private GameObject gameOverUI;
    private Vector3 startPosition;
    // Start is called before the first frame update
    void Start()
    {
        gameOverUI = GameObject.Find("GameOverUI");
        if (gameOverUI != null)
        {
            startPosition = gameOverUI.transform.position;
            gameOverUI.transform.position = new Vector3(10000,0,0);
        }
    }


    public void gameOver()
    {
        gameOverUI.transform.position = startPosition;

    }

}
