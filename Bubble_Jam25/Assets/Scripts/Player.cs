using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using static Unity.Collections.AllocatorManager;

public class Player : MonoBehaviour
{

    public UnityEvent gotRefill;
    public event UnityAction Refill;
    private Rigidbody2D rb;
    public float forceScale;

    public GameObject blow;
    public GameObject normal;
    public TextMeshProUGUI face;
    //private Transform _transform;

    //public GameObject Sprite;
    void Start()
    {
        //_transform = GetComponent<Transform>();
        //blow.transform.localScale = new Vector3(2, 2, 1);
        //normal.transform.localScale = new Vector3(2, 2, 1);
        blow.SetActive(false);
        normal.SetActive(true);
        //face.text = "0o";
        rb = GetComponent<Rigidbody2D>();

        if (gotRefill == null )
        {
            gotRefill = new UnityEvent();
            gotRefill.AddListener(Refill);
        }

    }

    public void ThrowPlayer(float bubbleRad, Vector3 dir)
    {
        Vector2 force = new Vector2(-dir.x, -dir.y);
        rb.AddForce(force * (bubbleRad* forceScale), ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "WinDoor")
        {
            GameManager.Instance.Win();
        }
        else if (collision.tag == "Obstacle")
        {
            GameManager.Instance.Lose();
        }
        else if (collision.tag == "Coin" && gotRefill != null)
        {
            gotRefill.Invoke();
        }

       
    }

    private bool onMover = false;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Mover")
        {
            onMover = true;
            StartCoroutine(MovePlayerWithBlock(collision.gameObject));
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Mover")
        {
            onMover = false;
        }
    }

    private IEnumerator MovePlayerWithBlock(GameObject block)
    {
        while (onMover)
        {
            this.transform.position = new Vector3(block.transform.position.x, this.transform.position.y, this.transform.position.z);
            yield return null;
        }
    }

    //private void FixedUpdate()
    //{
    //    Vector3 mousePos = Input.mousePosition;
    //    Sprite.transform.LookAt(mousePos);
    //}
    private void OnDisable()
    {
        //gotRefill.RemoveListener(Refill);
    }

    private void FixedUpdate()
    {
        //blow.transform.position = this.transform.position + new Vector3 (0, 1, 0);
        //normal.transform.position = this.transform.position + new Vector3(0, 1, 0);
        //face.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 1, 0);
        //face.transform.localPosition = new Vector3(_transform.localPosition.x, _transform.localPosition.y + 1, face.transform.localPosition.z);
        //face.rectTransform.transform.position = _transform.position;
        //face.rectTransform.transform.localPosition = new Vector3(_transform.position.x - 14.75f, _transform.position.y - 8.5f + 1, 0);//_transform.position;
    }

    public void BlowFace()
    {
        //face.text = "><";
        blow.SetActive(true);
        normal.SetActive(false);
    }
    public void NormalFace()
    {
        //face.text = "0o";
        blow.SetActive(false);
        normal.SetActive(true);
    }
}
