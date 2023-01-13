using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Character
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LayerMask groundLayer;  
    [SerializeField] private float speed = 250;

    [SerializeField] private float jumpForce = 350  ;

    private bool isGrounded = true;
    private bool isJumping = false;
    private bool isAttack = false;
    private bool isDeadth = false;

    private float horizontal;


    private int coin = 0;
    private Vector3 savePoint;
    
    // Start is called before the first frame update
    void Start()
    {
        SavePoint();    
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(isDeadth)
        {
            return;
        }
        isGrounded = CheckGrounded();

        horizontal = Input.GetAxisRaw("Horizontal");

        if(isAttack)
        {
            rb.velocity = Vector2.zero;
            return;
        }
        
        if(isGrounded)
        {   
            if(isJumping)
            {
                return;
            }
            
            //JUMP
            if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                Jump();
            }

            if(Mathf.Abs(horizontal) > 0.1f)
            {
                ChangeAnim("run");
            }

            //attack;
            if(Input.GetKeyDown(KeyCode.C) && isGrounded)
            {
                Attack();
            }
          
            //throw;
            if(Input.GetKeyDown(KeyCode.X) && isGrounded)
            {
                Throw();
            }
        }

       
        //check failling
        if(!isGrounded && rb.velocity.y < 0)
        {
            ChangeAnim("fall");
            isJumping = false;
        }       
        
        //MOVE
        if(Mathf.Abs(horizontal) > 0.1f)    
        {
            ChangeAnim("run");
            rb.velocity = new Vector2(horizontal * Time.fixedDeltaTime * speed, rb.velocity.y);
            transform.rotation = Quaternion.Euler(new Vector3(0, horizontal > 0 ? 0 : 180, 0));
        }
        else if(isGrounded)
        {
            ChangeAnim("idle");
            rb.velocity = Vector2.up * rb.velocity.y;   
        }
    }

    public override void OnInit()
    {
        base.OnInit();
        isDeadth = false; 
        isAttack = false;

        transform.position = savePoint; 
        ChangeAnim("idle");
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
    }

    protected override void OnDeath()
    {
        base.OnDeath();
    }

    private bool CheckGrounded()
    {
        Debug.DrawLine(transform.position, transform.position + Vector3.down * 1.1f, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.1f , groundLayer);
        return hit.collider != null;
    }

    private void Attack()
    {
        ChangeAnim("attack");
        isAttack = true;
        Invoke(nameof(ResetAttack), 0.3f);
        //debug resetattack
    }

    private void Throw()
    {
        ChangeAnim("throw");
        isAttack = true;
        Invoke(nameof(ResetAttack), 0.3f);
    }

    private void Jump()
    {
        isJumping = true;
        ChangeAnim("jump");
        rb.AddForce(Vector2.up * jumpForce);
    }

    private void ResetAttack()
    {
        Debug.Log("Reset Attack now");
        isAttack = false;
        ChangeAnim("idle");
    }

    internal void SavePoint()
    {
        savePoint = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag =="Coin")
        {
            coin++;
            Destroy(collision.gameObject);
        }

        if (collision.tag == "DeadZone")
        {
            isDeadth = true;
            ChangeAnim("die");
            Invoke(nameof(OnInit),1f);
        }
    }
}
