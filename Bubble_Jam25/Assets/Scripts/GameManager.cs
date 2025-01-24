using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private int sceneCount = 0;
    private void Awake()
    {
       
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
            //sceneCount = 0;
            //PlayerPrefs.SetInt("levelCount", 0);
        }
        else
        {
            //sceneCount = PlayerPrefs.GetInt("levelCount");
            Destroy(Instance);
        }
    }
    void Start()
    {
        if (!PlayerPrefs.HasKey("levelCount"))
        {
            PlayerPrefs.SetInt("levelCount", 0);
            sceneCount = 0;
        }
        else
        {
            sceneCount = PlayerPrefs.GetInt("levelCount");
        }
    }
    public void Win()
    {
        Debug.Log("WIN");
        //sceneCount = 0;
        sceneCount++;
        if (sceneCount >= SceneManager.sceneCountInBuildSettings)
        {
            sceneCount = 0;
        }
        PlayerPrefs.SetInt("levelCount", sceneCount);
        SceneManager.LoadScene(sceneCount);
    }

    public void Lose()
    {
        Debug.Log("LOSE");

        SceneManager.LoadScene(sceneCount);
    }
}
