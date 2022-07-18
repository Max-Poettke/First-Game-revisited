/*
INCLUDES:
	Movement with WSAD and Arrows
    Turning head and body
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerMovement3D : MonoBehaviour
{
    public Rigidbody rb;
    public float speed = 6.0f;
    public float jumpSpeed = 8.0f;
    public float wallRunSpeed;
    public bool wallRunning;
    public int availableJumps = 0;
    public bool jumpSubtracted = false;
    public bool isGrounded = false;
    public bool touchingWall = false;
    public float gravity = 20.0f;
	public float turnSensitivity = 4;
	public Transform head;
    public CapsuleCollider col;
    public LayerMask notIgnoredLayer;

    private Vector3 moveDirection = Vector3.zero;
	private Vector3 curEuler = Vector3.zero;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        DetectIfGrounded();
        DetectIfTouchingWall();
            
        moveDirection = transform.TransformDirection(new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical")));
        moveDirection *= speed;
    
        if (Input.GetButtonDown("Jump") && availableJumps > 0)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpSpeed, rb.velocity.z);
            availableJumps --;
        }
		
        if(Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0){
            rb.velocity = new Vector3((transform.forward.x * Input.GetAxis("Vertical") + transform.right.x * Input.GetAxis("Horizontal")) * speed,
            rb.velocity.y,
            (transform.forward.z * Input.GetAxis("Vertical") + transform.right.z * Input.GetAxis("Horizontal")) * speed);
        }

		//rotate head on x-axis (Up and down)
		float XturnAmount = Input.GetAxis("Mouse Y") * Time.deltaTime * turnSensitivity;
		curEuler = Vector3.right * Mathf.Clamp( curEuler.x - XturnAmount, -90f, 90f);
		head.localRotation = Quaternion.Euler(curEuler);
		
		//rotate body on y-axis (Sideways)
		float YturnAmount = Input.GetAxis("Mouse X") * Time.deltaTime * turnSensitivity;
		transform.Rotate(Vector3.up * YturnAmount);
    }

    void DetectIfGrounded(){
        Debug.DrawRay(transform.position + new Vector3(0f, -col.bounds.size.y / 2f, 0f), Vector3.down, Color.blue);
        Debug.DrawRay(transform.position + new Vector3(-col.bounds.size.x / 2f + 0.1f, -col.bounds.size.y / 2f + 0.2f, 0f), Vector3.down, Color.green);
        Debug.DrawRay(transform.position + new Vector3(col.bounds.size.x / 2f - 0.1f, -col.bounds.size.y / 2f + 0.2f, 0f), Vector2.down, Color.red);
        Debug.DrawRay(transform.position + new Vector3(0f, -col.bounds.size.y / 2f + 0.2f, -col.bounds.size.x + 0.6f), Vector2.down, Color.black);
        Debug.DrawRay(transform.position + new Vector3(0f, -col.bounds.size.y / 2f + 0.2f, col.bounds.size.x - 0.6f), Vector2.down, Color.cyan);
        
        if(Physics.Raycast(transform.position + new Vector3(0f, -col.bounds.size.y / 2f + 0.05f, 0f), Vector2.down, out RaycastHit hit1, 0.1f, notIgnoredLayer)
        || Physics.Raycast(transform.position + new Vector3(-(col.bounds.size.x / 2f - 0.1f), -col.bounds.size.y / 2f + 0.05f, 0f), Vector2.down, out RaycastHit hit2, 0.1f, notIgnoredLayer)
        || Physics.Raycast(transform.position + new Vector3(col.bounds.size.x / 2f - 0.1f, -col.bounds.size.y / 2f + 0.05f, 0f), Vector2.down, out RaycastHit hit3, 0.1f, notIgnoredLayer)
        || Physics.Raycast(transform.position + new Vector3(0f, -col.bounds.size.y / 2f + 0.05f, -col.bounds.size.x + 0.6f), Vector2.down, out RaycastHit hit4, 0.1f, notIgnoredLayer)
        || Physics.Raycast(transform.position + new Vector3(0f, -col.bounds.size.y / 2f + 0.05f, col.bounds.size.x - 0.6f), Vector2.down, out RaycastHit hit5, 0.1f, notIgnoredLayer))
        {
            isGrounded = true;
            availableJumps = 1;
        } else {
            isGrounded = false;
        }
    }

    void DetectIfTouchingWall(){

    }

    }