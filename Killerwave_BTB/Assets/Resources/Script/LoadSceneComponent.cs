using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LoadSceneComponent : MonoBehaviour
{
    float timer = 0;
    public string loadThisScene;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.GetComponentInChildren<ScoreManager>().ResetScore();
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > 3)
        {
            SceneManager.LoadScene(loadThisScene);
        }
    }
}
