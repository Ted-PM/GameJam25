using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    public float forceScale;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
    }
}
