using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TitleGum : MonoBehaviour
{
    public GameObject gum;
    public float totalTime;

    public AudioSource blow;
    public AudioSource _pop;
    bool started = false;

    public TextMeshProUGUI title;
    public TextMeshProUGUI PressPlay;
    // Start is called before the first frame update
    void Start()
    {
        gum.SetActive(false);
        title.enabled = false;
        if (GameManager.Instance.IsOnePhone())
        {
            PressPlay.text = "[TAP to BEGIN]";
        }
    }

    private IEnumerator BlowBubble()
    {
        SpriteRenderer _spriteRenderer = gum.GetComponent<SpriteRenderer>();
        Color _startColor = _spriteRenderer.color;
        
        //yield return new WaitForSeconds(3f);
        blow.Play();
        gum.SetActive(true);
        float time = 0f;
        float t = 0f;

        Vector3 startSize = new Vector3(.1f, .1f, 1);
        Vector3 endSize = new Vector3 (20,20,1);

        while (t < 1)
        {
            yield return null;
            time += Time.deltaTime;
            t = time / totalTime;
            if (t < 0.7)
                _spriteRenderer.color = new Color(_startColor.r, _startColor.g, _startColor.b, (1.1f - t));
            gum.transform.localScale = Vector3.Lerp (startSize, endSize, t);

        }
        blow.Stop();
        _pop.Play();
        yield return new WaitForSeconds(.2f);
        gum.SetActive(false);
        //title.GetComponent<TextMeshProUGUI>().color = new Color(255, 86, 228);
        //title.color = new Color(255, 86, 228);
        title.enabled = true;
        //title.vertexc = new Color(255, 86, 228);
        yield return new WaitForSeconds(2.5f);

        GameManager.Instance.Win();
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Space) || Input.touchCount > 0) && !started)
        {
            started = true;
            StartCoroutine(BlowBubble());
        }
    }
}
