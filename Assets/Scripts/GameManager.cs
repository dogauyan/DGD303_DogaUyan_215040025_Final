using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        Menu,
        Loading,
        Play,
        ESC,
        End
    }
    static public GameState state;
    static public bool Ended;
    public GameState gameState;
    public PlayerShipController Player;
    public GameObject ESC;
    public GameObject credits;
    public GameObject main;
    public GameObject winScreen;
    public GameObject loseScreen;

    // Start is called before the first frame update
    void Start()
    {
        state = gameState;
        Ended = false;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case GameState.Menu:
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    Application.Quit();
                }
                break;
            case GameState.Loading:
                break;
            case GameState.Play:
                if (Ended || Player == null)
                {
                    StartCoroutine(Ending(Player != null));
                    state = GameState.End;
                    return;
                }

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    state = GameState.ESC;
                    ESC.SetActive(true);
                    Time.timeScale = 0;
                }
                break;
            case GameState.ESC:
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    state = GameState.Play;
                    ESC.SetActive(false);
                    Time.timeScale = 1;
                }
                break;
            case GameState.End:
                break;
            default:
                break;
        }
    }

    public void Exit()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
    public void Play()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }
    public void Credits(bool bo)
    {
        if (bo)
        {
            main.SetActive(false);
            credits.SetActive(true);
        }
        else
        {
            main.SetActive(true);
            credits.SetActive(false);
        }
    }

    IEnumerator Ending(bool win)
    {
        yield return new WaitForSeconds(1);

        if (win)
        {
            winScreen.SetActive(true);
        }
        else
        {
            loseScreen.SetActive(true);
        }

        Time.timeScale = 0;
    }
}
