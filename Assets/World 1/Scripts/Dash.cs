using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{   
    public DashState dashState;
    public float dashTimer;
    public float maxDash;
    public Vector2 savedVelocity;
    public float dashSpeed = 3f;
    Rigidbody2D rb;

    public enum DashState{
        Ready,
        Dashing,
        Cooldown
    }

    
        
    
    // Start is called before the first frame update
    void Start() 
    {
        rb = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (dashState)
        {
            case DashState.Ready:
                var isDashkeyDown = Input.GetKeyDown(KeyCode.LeftShift);
                if (isDashkeyDown){
                    float moveBy = rb.velocity.x * dashSpeed;
                    savedVelocity = rb.velocity;
                    rb.velocity = new Vector2(moveBy, rb.velocity.y);
                    dashState = DashState.Dashing;
                }
                break;
            case DashState.Dashing:
                dashTimer += Time.deltaTime * 5;
                if (dashTimer >= maxDash) {
                    dashTimer = maxDash;
                    rb.velocity = savedVelocity;
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

    
}
