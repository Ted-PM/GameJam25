using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
//using 

public class Instructions : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _instructions;
    private bool _hasStarted;
    [SerializeField]
    private GameObject _lungHighlight;
    [SerializeField]
    private GameObject _winDoor;
    [SerializeField]
    private GameObject _doorArrow;
    void Start()
    {
        _hasStarted = false;
        _lungHighlight.SetActive(false);
        _winDoor.SetActive(false);
        _doorArrow.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !_hasStarted)
        {
            _hasStarted = true;
            StartCoroutine(DisableText());
        }
        //if (Input.GetKeyUp(KeyCode.Space) && _hasStarted)
        //{
        //    //_instructions.text = "Move your MOUSE to AIM the BUBBLE.";
        //    //_hasStarted = true;
        //    //_instructions.enabled = false;
        //    StartCoroutine(DisableText());
        //}
    }

    private IEnumerator DisableText()
    {
        _instructions.text = "Move your MOUSE to AIM the BUBBLE.";
        yield return new WaitForSeconds(4);
        _instructions.text = "A LARGER BUBBLE will send you FURTHER.";
        yield return new WaitForSeconds(4);
        _instructions.text = "Make sure not to RUN OUT of AIR...";
        _lungHighlight.SetActive(true);
        yield return new WaitForSeconds(2);
        _lungHighlight.SetActive(false);
        yield return new WaitForSeconds(2);
        _instructions.text = "... and get to the EXIT!!";
        _winDoor.SetActive(true);
        _doorArrow.SetActive(true);
        yield return new WaitForSeconds(2);
        _doorArrow.SetActive(false);
        yield return new WaitForSeconds(2);
        _instructions.enabled = false;

    }
}
