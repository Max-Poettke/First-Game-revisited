using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster1 : MonoBehaviour
{
    public List<Transform> wayPoints;
    public int wayPointIndex = 0;
    public float moveSpeed = 2f; 
    public bool playerTouching = false;
    public GameObject player;

    private void Start()
    {
        transform.position = wayPoints[wayPointIndex].transform.position;
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
            var targetPos = wayPoints[wayPointIndex].transform.position;
            var moveBy = moveSpeed * Time.deltaTime;
            var direction = (targetPos.x - transform.position.x) / Mathf.Abs(targetPos.x - transform.position.x);
            if(player != null && playerTouching){
                player.transform.position = new Vector3(player.transform.position.x + moveBy * direction, player.transform.position.y, player.transform.position.z);
            }
            transform.position = Vector2.MoveTowards(transform.position, targetPos, moveBy);
            if (transform.position == targetPos)
            {
                wayPointIndex++;
            }
            if (wayPointIndex >= wayPoints.Count)
        {
            wayPointIndex = 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.transform.tag == "Player"){
            Debug.Log("player touched");
            playerTouching = true;
            player = other.gameObject;
        }
    }

    void OnCollisionExit2D(Collision2D other){
        if(other.gameObject.transform.tag == "Player"){
            playerTouching = false;
        }
    }

}   
