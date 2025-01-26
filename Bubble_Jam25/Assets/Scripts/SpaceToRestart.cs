using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpaceToRestart : MonoBehaviour
{

    // Update is called once per frame
    public TextMeshProUGUI PressPlay;
    void Update()
    {
        if (GameManager.Instance.IsOnePhone())
        {
            PressPlay.text = "[TAP to RESTART]";
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.touchCount > 0)
        {
            GameManager.Instance.Restart();
        }
    }
}
