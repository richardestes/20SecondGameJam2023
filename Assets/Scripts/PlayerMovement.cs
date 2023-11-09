using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D _rigidBody;
    private SpriteRenderer _spriteRenderer;
    [HideInInspector]
    public Vector2 MoveDirection;
    public float MoveSpeed;
    
    [Header("IFrames")] public float invincibilityDuration;
    public float invincibilityTimer;
    public bool isInvincible;
    
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (invincibilityTimer > 0)
        {
            invincibilityTimer -= Time.deltaTime;
            Debug.Log("Invincibility Time: " + invincibilityTimer);
        }
        else if (isInvincible)
        {
            isInvincible = false;
        }
        InputManagement();
    }

    void FixedUpdate() // not dependent on frame rate
    {
        Move();
    }

    void InputManagement()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        MoveDirection = new Vector2(moveX, moveY).normalized;
        
        // Flip sprite when travelling left
        if (moveX < 0) _spriteRenderer.transform.rotation = new Quaternion(0f, 180f, 0f, 0f);
        else if (moveX > 0) _spriteRenderer.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
    }

    void Move()
    {
        _rigidBody.velocity = new Vector2(MoveDirection.x * MoveSpeed, MoveDirection.y * MoveSpeed);
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Environment"))
        {
            GameManager.instance.Die();
        }
    }
}
