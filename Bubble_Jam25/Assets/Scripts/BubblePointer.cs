using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.Events;
using System.Threading;
using Unity.VisualScripting;
//using static UnityEditor.Experimental.GraphView.GraphView;

public class BubblePointer : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _head;

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
    private bool _playerStatic;
    public bool canBlow { private set; get; }
    public bool isBlowing { private set; get; }

    [SerializeField]
    private AudioSource _blowSound;
    [SerializeField]
    private AudioSource _pop;
    //private Vector3 startMousePos;
    void Start()
    {

        _playerStatic = false;
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

        _player.GetComponent<Player>().Refill += RefillLungs;
        //_player.gotRefill.AddListener(bubbleAction);
        //_player.GetComponent<UnityEvent>().AddListener(bubbleAction);
        //_player.ev
        //bubbleAction += RefillLungs;
    }

    void Update()
    {

        if (canBlow && Input.GetKeyDown(KeyCode.Space) && lungCapacity >= 0.1f)
        {
            isBlowing = true;
            canBlow = false;
            Debug.Log("start blowing");
            //_player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            StartCoroutine(BlowBubble());
        }
        if ((isBlowing && Input.GetKeyUp(KeyCode.Space)) || lungCapacity <= 0.1f || _bubbleRadius >=2.5f)
        {
            isBlowing = false;
            //canBlow = true;
            StartCoroutine(WaitBeforeBlow());
            Debug.Log("pop");
            _blowSound.Stop();
            //_player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            //_player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            Pop();
            _bubbleRadius = 0f;
        }

        
        if (lungCapacity < 0.1 && !_playerStatic)
        {
            //_playerStatic
            StartCoroutine(PlayerStatic());
            //GameManager.Instance.Lose();
        }
        if (lungCapacity < 0.1 && _playerStatic)
        {
            GameManager.Instance.Lose();
        }
    }

    private IEnumerator WaitBeforeBlow()
    {
        yield return new WaitForSeconds(0.1f);
        canBlow = true;
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
            //canBlow = true;
            _playerStatic = true;
        }
    }

    private void FixedUpdate()
    {
        //if (isBlowing)
        //{
            PointBubbleAtMouse();
        //}
        //if (!canBlow && !isBlowing && lungCapacity >= 0.1f)
        //{
        //    //Debug.Log("check player static");
        //    StartCoroutine(PlayerStatic());
        //}
    }

    private IEnumerator BlowBubble()
    {
        _player.BlowFace();
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
        _blowSound.Play();
        while (t<1 && isBlowing)
        {
            deltaTime = Time.deltaTime;
            time += deltaTime;
            t = time / maxTime;
            lungCapacity -= deltaTime / lungToTimeMultiplier;
            _lungFill.localScale = new Vector3(1, (lungCapacity/maxLungCapacity), 0);
            _transform.localScale = Vector3.Lerp(startScale, endScale, t);
            _transform.localScale = _transform.localScale + (t*Random.insideUnitSphere);
            _head.color = new Color(1-2*t, 1-2*t, 1);
            _blowSound.volume = t;
            _spriteRenderer.color = new Color(_startColor.r, _startColor.g, _startColor.b, (1 - t));
            _bubbleRadius = _transform.localScale.x / 2;
            yield return null;
        }
    }

    private void Pop()
    {
        _pop.volume = _bubbleRadius / 5;
        _pop.Play();
        Vector3 mousePos = Input.mousePosition;

        Vector3 playerPos = Camera.main.WorldToScreenPoint(pivot.position);
        Vector3 dir = (mousePos - playerPos).normalized;
        _player.ThrowPlayer(_bubbleRadius, dir);
        _player.NormalFace();
        _head.color = Color.white;
        _spriteRenderer.enabled = false;
        _innerBubbleRenderer.enabled = false;
        _bubbleRadius = 0f;
        _transform.localScale = new Vector3(1, 1, 1);
        _transform.localPosition = new Vector3(0, 1, 0);
        
    }

    private void PointBubbleAtMouse()
    {
        Vector3 mousePos = Input.mousePosition;
        
        Vector3 playerPos = Camera.main.WorldToScreenPoint(pivot.position);
        Vector3 dir = (mousePos - playerPos).normalized;
        _transform.localPosition = dir *  _bubbleRadius;
    }

    public void RefillLungs()
    {
        lungCapacity = maxLungCapacity;
        _lungFill.localScale = new Vector3(1, 1, 0);
    }
}
