using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public GameObject PauseMenuUI;
    public GameObject GameOverMenuUI;
    public GameObject ScoreCountUI;
    private bool isGamePaused;
    public static bool isGameEnded = false;
    // Start is called before the first frame update
    void Start()
    {
        isGamePaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isGamePaused = !isGamePaused;
            if (isGamePaused)
            {
                Pause();
            }
            else
            {
                Resume();
            }
        }
    }

    public void Pause()
    {
        Time.timeScale = 0;
        PauseMenuUI.SetActive(true);
    }

    public void Resume()
    {
        Debug.Log("resume");
        Time.timeScale = 1;
        isGamePaused = false;
        PauseMenuUI.SetActive(false);
    }

    public void LoadMenu()
    {
        Debug.Log("Loading main menu");
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Debug.Log("Quitting application");
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }

    public void StartGame()
    {
        Debug.Log("Starting the game");
        SceneManager.LoadScene(1);
    }

    public void Restart()
    {
        Debug.Log("ReStarting the game");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void GameOver()
    {
        Debug.Log("GameOver");
        if(!isGameEnded)
        {
            ScoreCountUI.SetActive(false);
            GameOverMenuUI.SetActive(true);
            //var scoreText = GameOverMenuUI.GetComponent<TMPro.TextMeshProUGUI>();
            // scoreText.text = scoreText.text + ScoreCount.Score.ToString();
            //foreach (var text in scoreText)
            //{
            //    Debug.Log("Text");
            //    Debug.Log(scoreText.text);
            //}
            var scoreText = GameObject.FindGameObjectWithTag("ScoreText").GetComponent<TMPro.TextMeshProUGUI>();
            scoreText.text = scoreText.text + ScoreCount.Score.ToString();
            Debug.Log(scoreText.text);
            isGameEnded = true;
        }
    }
}
