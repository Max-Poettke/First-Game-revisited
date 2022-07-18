using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] float horizontalSpeed = 10f;
    [SerializeField] float jumpForce = 0.1f;

    public bool isTalking = false;
    public bool isGrounded = false;
    public bool jumping = false;
    public bool wantsToJump = false;
    public bool isDead = false;
    public int availableJumps = 1;
    public float jumpBufferDuration = 10f;
    public float jumpBuffer = 0f;
    public float dashSpeed = 3f;
    public float dashTimer;
    public float maxDash = 0.7f;
    public float savedVelocity;
    public float slideSpeed = 2f;
    public float wallJumpPush = 2f;
    public Animator animator;
    public bool isColliding = false;
    public bool facingRight = true;
    public LayerMask notIgnoredLayer;
    public BoxCollider2D col;
    public bool sliding = false;
    private RaycastHit2D hit1;
    private RaycastHit2D hit2;
    private RaycastHit2D hit3;
    private float deltaY = 0f;
    private float oldY = 0f;
    private float newY = 0f;
    private enum DashState{
        Ready,
        Dashing,
        Cooldown
    };
    private DashState dashState;
    private Rigidbody2D rb;

    GameMaster gm;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        transform.position = gm.lastCheckPointPos;
    }

    // Update is called once per frame
    void Update()
    {      
        if(isDead) return;
        if(Input.GetKeyDown(KeyCode.Space)){
            wantsToJump = true;
            jumpBuffer = jumpBufferDuration;
        }
        if(jumpBuffer < 0){
            wantsToJump = false;
            jumpBuffer = 0f;
        } else {
            jumpBuffer -= 0.1f;
        }

        Walking();
        Jumping();
        Dashing();
        Sliding();

        oldY = newY;
        newY = transform.position.y;
        deltaY = newY - oldY;

        animator.SetFloat("SpeedX", Mathf.Abs(rb.velocity.x));
        animator.SetBool("IsJumping", jumping);
        animator.SetBool("IsColliding", isColliding);
        animator.SetBool("IsSliding", sliding);
        animator.SetBool("IsGrounded", isGrounded);
    }

    void Dashing(){
        switch (dashState)
        {
            case DashState.Ready:
                var isDashkeyDown = Input.GetKeyDown(KeyCode.LeftShift);
                if (isDashkeyDown){
                    savedVelocity = rb.velocity.x;
                    horizontalSpeed = horizontalSpeed * dashSpeed;
                    dashState = DashState.Dashing;
                }
                break;
            case DashState.Dashing:
                dashTimer += Time.deltaTime * 1;
                if (dashTimer >= maxDash) {
                    dashTimer = maxDash;
                    horizontalSpeed = Mathf.Abs(savedVelocity);
                    dashState = DashState.Cooldown;
                }
                break;
            case DashState.Cooldown:
                dashTimer -= Time.deltaTime;
                if (dashTimer <= 0) {
                    dashTimer = 0;
                    dashState = DashState.Ready;
                }
                break; 
        }  
    }

    void Walking(){
        if(isTalking) return;

        if(Input.GetAxisRaw("Horizontal") < 0 && transform.rotation.y == 0){
            facingRight = false;
            transform.Rotate(0,180,0);
        } else if (Input.GetAxisRaw("Horizontal") > 0 && transform.rotation.y < 0){
            transform.Rotate(0,-180,0);
            facingRight = true;
        }

        if(Input.GetKey("d")) {
            rb.velocity = new Vector2(horizontalSpeed, rb.velocity.y);
        } else if(Input.GetKey("a")){ 
            rb.velocity = new Vector2(-horizontalSpeed, rb.velocity.y);
        } else {
            rb.velocity = new Vector2(rb.velocity.x / 5, rb.velocity.y);
            if(rb.velocity.x <  0.001f){
                rb.velocity = new Vector2(0f, rb.velocity.y);
            }
        }
    }

    void Jumping(){
        hit1 = Physics2D.Raycast(transform.position + new Vector3(0f, -col.bounds.size.y / 2f, 0f), Vector2.down, notIgnoredLayer);
        hit2 = Physics2D.Raycast(transform.position - col.bounds.size / 2 + new Vector3(0.1f,0f,0f), Vector2.down, notIgnoredLayer);
        hit3 = Physics2D.Raycast(transform.position + new Vector3(col.bounds.size.x / 2f - 0.1f, -col.bounds.size.y / 2f, 0f), Vector2.down, notIgnoredLayer);
        if(hit1.collider != null || hit2.collider != null || hit3.collider != null){
            if(hit1.distance < 0.2f || hit2.distance < 0.2f || hit3.distance < 0.2f){
                isGrounded = true;
                availableJumps = 1;
            } else {
                //Debug.Log("No longer grounded");
                isGrounded = false;
            }
        } 

        if(isTalking) return;

        if(availableJumps > 0 && wantsToJump){
            wantsToJump = false;
            availableJumps --;
            jumping = true;
            if(isGrounded){
                isGrounded = false;
                availableJumps = 1;
            }
            
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            
            if (sliding && facingRight){
                rb.AddForce(Vector3.left * wallJumpPush);
                availableJumps = 1;
            } else if (sliding && !facingRight){
                rb.AddForce(Vector3.right * wallJumpPush);
                availableJumps = 1;
            }
        }
        if(!Input.GetKey(KeyCode.Space) || rb.velocity.y < 0) jumping = false;
        if (rb.velocity.y < 0 && isColliding == false) {
            rb.velocity += Vector2.up * Physics2D.gravity * (2f) * Time.deltaTime;
        } else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space) && isColliding == false) {
            rb.velocity += Vector2.up * Physics2D.gravity * (10f) * Time.deltaTime;
        } else if (rb.velocity.y > 0 && Input.GetKey(KeyCode.Space) && isColliding == false) {
            rb.velocity += Vector2.up * Physics2D.gravity * (2.5f) * Time.deltaTime;
        }
    }

    void Sliding(){
        if(facingRight){
            hit1 = Physics2D.Raycast(transform.position + new Vector3(col.bounds.size.x / 2f, 0f, 0f), Vector2.right, notIgnoredLayer);                             //top right
            hit2 = Physics2D.Raycast(transform.position + col.bounds.size / 2, Vector2.right, notIgnoredLayer);                                                     //middle right
            hit3 = Physics2D.Raycast(transform.position + new Vector3(col.bounds.size.x / 2f, -col.bounds.size.y / 2f, 0f), Vector2.right, notIgnoredLayer);        //bottom right
            Debug.DrawRay(transform.position + new Vector3(col.bounds.size.x / 2f, -col.bounds.size.y / 2f, 0f), Vector2.right, Color.green);
        } else if (!facingRight){
            hit1 = Physics2D.Raycast(transform.position + new Vector3(- col.bounds.size.x / 2f, 0f, 0f), Vector2.left, notIgnoredLayer);                            //middle left
            hit2 = Physics2D.Raycast(transform.position - col.bounds.size / 2f, Vector2.left, notIgnoredLayer);                                                     //bottom left
            hit3 = Physics2D.Raycast(transform.position + new Vector3(- col.bounds.size.x / 2f, col.bounds.size.y / 2f, 0f), Vector2.left, notIgnoredLayer);        //top left
        }
        if(hit1.collider != null && !jumping) {
            if(hit1.distance < 0.1){
                //Debug.Log("1");
                rb.velocity = new Vector2(rb.velocity.x, -slideSpeed);
                availableJumps = 1;
                sliding = true;
            }
        } 
        if(hit2.collider != null && !jumping){
            if(hit2.distance < 0.1){
                //Debug.Log("2");
                rb.velocity = new Vector2(rb.velocity.x, -slideSpeed);
                availableJumps = 1;
                sliding = true;
            }
        } 
        if(hit3.collider != null && !jumping){
            if(hit3.distance < 0.1){
                //Debug.Log("3");
                rb.velocity = new Vector2(rb.velocity.x, -slideSpeed);
                availableJumps = 1;
                sliding = true;
            }
        }
        if((hit1.distance > 0.1 && hit2.distance > 0.1 && hit3.distance > 0.1) || isGrounded) {
            sliding = false;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.transform.parent.tag == "Ground"){
            isColliding = true;
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if(other.gameObject.transform.parent.tag == "Ground"){
            isColliding = false;
        }    
    }
}
