using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject EndingPanel;
    public Text EndingText;

    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public void GameClear()
    {
        EndingText.text = "GAME CLEAR!";
        StartCoroutine(EndingCoroutine());
    }

    public void GameOver()
    {
        EndingText.text = "TRY AGAIN?";
        StartCoroutine(EndingCoroutine());
    }

    public void Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainGame");
    }

    IEnumerator EndingCoroutine()
    {
        EndingPanel.SetActive(true);

        yield return new WaitForSeconds(2f);

        Time.timeScale = 0f;
    }
}
