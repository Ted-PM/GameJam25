using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BubblePointer : MonoBehaviour
{
    [SerializeField]
    private Player _player;
    [SerializeField]
    private Transform _lungFill;

    private Color _startColor;

    private SpriteRenderer _spriteRenderer;
    public SpriteRenderer _innerBubbleRenderer;

    private Transform _transform;

    public float maxLungCapacity;
    [HideInInspector]
    public float lungCapacity;
    public float lungToTimeMultiplier;

    private Transform _playerPivot;
    public Transform pivot;

    public float _bubbleRadius {  get; private set; }
    public bool canBlow { private set; get; }
    public bool isBlowing { private set; get; }

    //private Vector3 startMousePos;
    void Start()
    {
        _bubbleRadius = 0f;
        _playerPivot = GetComponentInParent<Transform>();
        lungCapacity = maxLungCapacity;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _startColor = _spriteRenderer.color;
        _transform = GetComponent<Transform>();
        isBlowing = false;
        canBlow = true;
        _spriteRenderer.enabled = false;
        _innerBubbleRenderer.enabled = false;
    }

    void Update()
    {
        //if (canBlow && Input.GetMouseButtonDown(0) && lungCapacity >= 0.1f)

        if (canBlow && Input.GetKeyDown(KeyCode.Space) && lungCapacity >= 0.1f)
        {
            //startMousePos = Input.mousePosition;
            isBlowing = true;
            canBlow = false;
            Debug.Log("start blowing");
            _player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            StartCoroutine(BlowBubble());
        }
        //if ((isBlowing && Input.GetMouseButtonUp(0)) || lungCapacity <= 0.1f || _bubbleRadius >=2.5f)
        if ((isBlowing && Input.GetKeyUp(KeyCode.Space)) || lungCapacity <= 0.1f || _bubbleRadius >=2.5f)
        {
            isBlowing = false;
            //canBlow = true;
            Debug.Log("pop");
            _player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            _player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            Pop();
            _bubbleRadius = 0f;
        }

        
        if (lungCapacity < 0.1)
        {
            GameManager.Instance.Lose();
        }
    }

    private IEnumerator PlayerStatic()
    {
        Vector3 startPos = _player.transform.position;
        //Debug.Log("startPos = " + startPos);
        yield return new WaitForFixedUpdate();

        //float movesSpeed = Mathf.Ceil(Vector3.Distance(_player.transform.position, startPos));
        Vector3 endPos = _player.transform.position;

        //Debug.Log("endPos = " + endPos);
        //if (movesSpeed == 0)
        if (endPos.x == startPos.x && endPos.y == startPos.y)
        {
            canBlow = true;
        }
    }

    private void FixedUpdate()
    {
        //if (isBlowing)
        //{
            PointBubbleAtMouse();
        //}
        if (!canBlow && !isBlowing && lungCapacity >= 0.1f)
        {
            //Debug.Log("check player static");
            StartCoroutine(PlayerStatic());
        }
    }

    private IEnumerator BlowBubble()
    {
        _spriteRenderer.enabled = true;
        _innerBubbleRenderer.enabled = true;
        float maxTime = lungCapacity * lungToTimeMultiplier;
        //float maxTime = 5f;
        float time = 0f;
        float t = 0f;
        float deltaTime = 0f;

        Vector3 startScale = new Vector3(1, 1, 1);
        Vector3 endScale = new Vector3(lungCapacity, lungCapacity, 1);

        _spriteRenderer.color = _startColor;

        while (t<1 && isBlowing)
        {
            deltaTime = Time.deltaTime;
            time += deltaTime;
            t = time / maxTime;
            lungCapacity -= deltaTime / lungToTimeMultiplier;
            _lungFill.localScale = new Vector3(1, (lungCapacity/maxLungCapacity), 0);
            _transform.localScale = Vector3.Lerp(startScale, endScale, t);
            _transform.localScale = _transform.localScale + (t*Random.insideUnitSphere);
            _spriteRenderer.color = new Color(_startColor.r, _startColor.g, _startColor.b, (1 - t));
            _bubbleRadius = _transform.localScale.x / 2;
            yield return null;
        }
    }

    private void Pop()
    {
        Vector3 mousePos = Input.mousePosition;

        Vector3 playerPos = Camera.main.WorldToScreenPoint(pivot.position);
        Vector3 dir = (mousePos - playerPos).normalized;
        _player.ThrowPlayer(_bubbleRadius, dir);
        _spriteRenderer.enabled = false;
        _innerBubbleRenderer.enabled = false;

    }

    private void PointBubbleAtMouse()
    {
        Vector3 mousePos = Input.mousePosition;
        
        Vector3 playerPos = Camera.main.WorldToScreenPoint(pivot.position);
        Vector3 dir = (mousePos - playerPos).normalized;
        _transform.localPosition = dir * 1.5f * _bubbleRadius;
    }
}
