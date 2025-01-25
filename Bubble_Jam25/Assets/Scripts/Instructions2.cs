using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Instructions2 : MonoBehaviour
{
    [SerializeField]
    private GameObject _text;
    [SerializeField]
    private GameObject _arrow;
    [SerializeField]
    private GameObject _collectable;
    // Start is called before the first frame update
    void Start()
    {
        _arrow.SetActive(false);
        _collectable.SetActive(false);
        StartCoroutine(Stuff());
    }

    private IEnumerator Stuff()
    {
        _text.SetActive(true);
        yield return new WaitForSeconds(1);
        _arrow.SetActive(true);
        yield return new WaitForSeconds(3);
        _arrow.SetActive(false);
        _text.GetComponent<TextMeshProUGUI>().text = "Pick up the INHALER to REFILL your LUNGS.";
        _collectable.SetActive(true);
        yield return new WaitForSeconds(4);
        _collectable.SetActive(false);
        _text.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
