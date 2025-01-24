using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LungIndicator : MonoBehaviour
{
    [SerializeField]
    private Transform _lungFill;
    void Start()
    {
        
        //_lungFill
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator DrainLungs()
    {
        yield return null;
    }
}
