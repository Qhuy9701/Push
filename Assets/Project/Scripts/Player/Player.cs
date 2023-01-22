using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float speed = 250f;
    [SerializeField] private float jumpForce = 350f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Kunai kunaiPrefab;
    [SerializeField] private Transform throwPoint;
    [SerializeField] private GameObject attackArea;


    private float horizontal;
    private int coin;

    private Vector3 savePoint;


    private bool isGrounded = true;
    private bool isJumping = false;
    private bool isAttack = false;
    private bool isDead = false;

    public float distanceGround = 1.02f;


    public void SavePoint()
    {
        savePoint = transform.position;
    }

    private void Update()
    {
        if (isDead)
        {
            return;
        }

        isGrounded = CheckGround();
        horizontal = Input.GetAxisRaw("Horizontal");

        if (isAttack)
        {
            rb.velocity = Vector2.zero;
            return;
        }
        if (isGrounded)
        {
            if (isJumping)
            {
                return;
            }


            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                Jump();
            }


            if (Mathf.Abs(horizontal) > 0.1f)
            {
                ChangeAnim("run");
            }

            if (Input.GetKeyDown(KeyCode.C) && isGrounded && !isAttack)
            {
                Attack();
            }


            if (Input.GetKeyDown(KeyCode.V) && isGrounded && !isAttack)
            {
                Throw();
            }

        }


        if (!isGrounded && rb.velocity.y < 0)
        {
            ChangeAnim("fall");
            isJumping = false;
        }


        if (Mathf.Abs(horizontal) > 0.1f)
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
            transform.rotation = Quaternion.Euler(new Vector3(0, horizontal > 0 ? 0 : 180, 0));
        }

        else if (isGrounded)
        {
            ChangeAnim("idle");
            rb.velocity = Vector2.zero;
        }

    }

    public override void OnInit()
    {
        base.OnInit();
        enabled = true;
        isDead = false;
        isAttack = false;
        transform.position = savePoint;
        ChangeAnim("idle");
        DeActiveAttack();

        SavePoint();
        coin = PlayerPrefs.GetInt("coin", 0);
        UIManager.Instance.SetCoin(coin);

    }


    public override void OnDespawn()
    {
        base.OnDespawn();
        OnInit();
    }

    protected override void OnDeath()
    {
        base.OnDeath();
        isDead = true;
        enabled = false;
    }

    private bool CheckGround()
    {
        Debug.DrawLine(transform.position, transform.position + (Vector3.down * distanceGround), Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.down, distanceGround, groundLayer);
        return hit.collider != null;
    }

    public void Attack()
    {
        if (!enabled) return;
        if (!isGrounded) return;
        if (isAttack) return;
        ChangeAnim("attack");
        isAttack = true;
        Invoke(nameof(ResetAttack), 0.5f);
        ActiveAttack();
        Invoke(nameof(DeActiveAttack), 0.5f);
    }

    public void Throw()
    {
        if (!enabled) return;
        if (!isGrounded) return;
        if (isAttack) return;
        ChangeAnim("throw");
        isAttack = true;
        Invoke(nameof(ResetAttack), 0.5f);

        Instantiate(kunaiPrefab, throwPoint.position, throwPoint.rotation);

    }
    public void Jump()
    {
        if (!enabled) return;
        if (!isGrounded) return;
        if (isAttack) return;
        isJumping = true;
        ChangeAnim("jump");
        rb.AddForce(jumpForce * Vector2.up);
    }

    private void ResetAttack()
    {
        ChangeAnim("idle");
        isAttack = false;
    }

    private void ActiveAttack()
    {
        attackArea.SetActive(true);
    }

    private void DeActiveAttack()
    {
        attackArea.SetActive(false);

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Coin"))
        {
            coin++;
            PlayerPrefs.SetInt("coin", coin);
            UIManager.Instance.SetCoin(coin);
            Destroy(other.gameObject);
        }
        if (other.CompareTag("DeadZone"))
        {
            isDead = true;
            ChangeAnim("die");

            Invoke(nameof(OnInit), 1f);
        }
    }

    public void SetMove(float horizontal)
    {
        this.horizontal = horizontal;
    }
}
