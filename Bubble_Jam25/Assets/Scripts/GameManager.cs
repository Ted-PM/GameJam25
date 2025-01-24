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
        Instance = this;

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
        sceneCount = 0;
        if (sceneCount >= SceneManager.sceneCountInBuildSettings)
        {
            sceneCount = 0;
        }
        PlayerPrefs.SetInt("levelCount", sceneCount);
        SceneManager.LoadScene(sceneCount);
    }

    public void Lose()
    {
        SceneManager.LoadScene(sceneCount);
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
