using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private int sceneCount = 0;

    [SerializeField]
    private Timer timer;
    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
            timer.enabled = false;
            DontDestroyOnLoad(this);
            //sceneCount = 0;
            //PlayerPrefs.SetInt("levelCount", 0);
        }
        else
        {
            //sceneCount = PlayerPrefs.GetInt("levelCount");
            Destroy(Instance);
            //Destroy(this);
        }
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.R))
        {
            Lose();
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
            sceneCount = SceneManager.GetActiveScene().buildIndex;
            PlayerPrefs.SetInt("levelCount", sceneCount);
            Debug.Log("sceneCount = " + sceneCount);
            //sceneCount = PlayerPrefs.GetInt("levelCount");
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

        if (sceneCount == 1)
        {
            timer.enabled = true;
        }
        else if (sceneCount == 0 || sceneCount == SceneManager.sceneCountInBuildSettings - 1)
        {
            timer.enabled = false;
        }

        PlayerPrefs.SetInt("levelCount", sceneCount);
        SceneManager.LoadScene(sceneCount);
    }

    public void Restart()
    {
        timer.enabled = false;
        PlayerPrefs.SetInt("levelCount", 0);
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }

    public void Lose()
    {
        Debug.Log("LOSE");

        SceneManager.LoadScene(sceneCount);
    }

    public bool IsOnePhone()
    {
        bool result = true;

        if ((!Application.isMobilePlatform && Application.platform == RuntimePlatform.WebGLPlayer) || Application.platform == RuntimePlatform.WindowsEditor)
        {
            result = false;
        }
        return result;
    }
}
