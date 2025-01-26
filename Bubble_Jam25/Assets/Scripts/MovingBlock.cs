using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBlock : MonoBehaviour
{
    public bool isHorizontal;
    public float distance;  // distance in each direction
    public float interval;  // time to go full left - right
    private Transform _transform;

    void Start()
    {
        _transform = GetComponent<Transform>();
        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        Vector3 startPos = _transform.position;
        Vector3 endPos = startPos;
        if (isHorizontal)
        {
            endPos += new Vector3(distance, 0f, 0f);
        }
        else
        {
            endPos += new Vector3(0f, distance, 0f);
        }

        float time = 0f;
        float t = 0f;

        while (t < 1)
        {
            time += Time.deltaTime;
            t = time / interval;

            _transform.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }

        time = 0f;
        t = 0f;

        while (t < 1)
        {
            time += Time.deltaTime;
            t = time / interval;

            _transform.position = Vector3.Lerp(endPos, startPos, t);
            yield return null;
        }

        StartCoroutine(Move());
    }
}
