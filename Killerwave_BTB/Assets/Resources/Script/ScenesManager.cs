using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScenesManager : MonoBehaviour
{
    float gameTimer = 0;
    float[] endLevelTimer = { 30, 30, 45 };
    int currentSceneNumber = 0;
    bool gameEnding = false;

    public MusicMode musicMode;
    public enum MusicMode
    {
        noSound, fadeDown, musicOn
    }

    Scenes scenes;

    public enum Scenes
    {
        bootUp,
        title,
        shop,
        level1,
        level2,
        level3,
        gameOver
    }
    public void ResetScene()
    {
        StartCoroutine(MusicVolume(MusicMode.noSound));
        gameTimer = 0;
        SceneManager.LoadScene(GameManager.currentScene);
    }
    void NextLevel()
    {
        StartCoroutine(MusicVolume(MusicMode.musicOn));
        gameEnding = false;
        gameTimer = 0;
        SceneManager.LoadScene(GameManager.currentScene + 1);
    }
    public void BeginGame(int gameLevel)
    {
        SceneManager.LoadScene(gameLevel);
    }
    public void GameOver()
    {
        Debug.Log("ENDSCORE: " + GameManager.Instance.GetComponent<ScoreManager>().PlayersScore);
        SceneManager.LoadScene("gameOver");
    }
    public void BeginGame()
    {
        SceneManager.LoadScene("testLevel1");
    }
    void GetScene()
    {
        scenes = (Scenes)currentSceneNumber;
    }
    void GameTimer()
    {
        switch (scenes)
        {
            case Scenes.level1: case Scenes.level2: case Scenes.level3:
                {
                    if (GetComponentInChildren<AudioSource>().clip == null)
                    {
                        AudioClip lvlMusic = Resources.Load<AudioClip> ("Sound/lvlMusic") as AudioClip;
                        GetComponentInChildren<AudioSource>().clip = lvlMusic;
                        GetComponentInChildren<AudioSource>().Play();
                    }
                    if (gameTimer < endLevelTimer[currentSceneNumber-3])
                    {
                        gameTimer += Time.deltaTime;
                    }
                    else
                    {
                        if (!gameEnding)
                        {
                            gameEnding = true;
                            if (SceneManager.GetActiveScene().name != "level3")
                            {
                                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerTransition>().LevelEnds = true;
                                StartCoroutine(MusicVolume(MusicMode.fadeDown));
                            }
                            else
                            {
                                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerTransition>().GameCompleted = true;

                            }

                            void SendInJsonFormat(string lastLevel)
                            {
                                if (lastLevel == "level3")
                                {
                                    GameStats gameStats = new GameStats();
                                    gameStats.livesLeft = GameManager.playerLives;
                                    gameStats.completed = System.DateTime.Now.ToString();
                                    gameStats.score = ScoreManager.playerScore;
                                    string json = JsonUtility.ToJson(gameStats, true);
                                    Debug.Log(json);

                                    Debug.Log(Application.persistentDataPath + "/GameStatsSaved.json");
                                    System.IO.File.WriteAllText(Application.persistentDataPath + "/GameStatsSaved.json", json);
                                }
                            }

                            Invoke("NextLevel", 4);

                        }
                    }

                    break;
                }
            default:
                {
                    GetComponentInChildren<AudioSource>().clip = null;
                    break;
                }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(MusicVolume(MusicMode.musicOn));
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene aScene, LoadSceneMode aMode)
    {
        StartCoroutine(MusicVolume(MusicMode.musicOn));

        GetComponent<GameManager>().SetLivesDisplay(GameManager.playerLives);
        if(GameObject.Find("score"))
        {
            GameObject.Find("score").GetComponent<Text>().text = ScoreManager.playerScore.ToString();
            Debug.Log("Test");
        }
    }
     IEnumerator MusicVolume (MusicMode musicMode)
    {
        switch (musicMode)
        {
            case MusicMode.noSound:
                {
                    GetComponentInChildren<AudioSource>().Stop();
                    break;
                }
            case MusicMode.fadeDown:
                {
                    GetComponentInChildren<AudioSource>().volume -= Time.deltaTime / 3;
                    break;
                }
            case MusicMode.musicOn:
                {
                    if (GetComponentInChildren<AudioSource>().clip != null)
                    {
                        GetComponentInChildren<AudioSource>().Play();
                        GetComponentInChildren<AudioSource>().volume = 1;
                    }
                    break;
                }
        }
        yield return new WaitForSeconds(0.1f);
    }
    // Update is called once per frame
    void Update()
    {
        if (currentSceneNumber != SceneManager.GetActiveScene().buildIndex)
        {
            currentSceneNumber = SceneManager.GetActiveScene().buildIndex;
            GetScene();
        }
        GameTimer();
    }
}
