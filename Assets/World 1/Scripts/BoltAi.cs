using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoltAi : MonoBehaviour
{

    public Vector3 targetPos;
    public float startSpeed;
    public float acceleration;
    private float speed;
    // Start is called before the first frame update
    void Start()
    {
        speed = startSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if(speed < 0.01) speed = 0f;
        if(speed == 0f) Explode();
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        speed -= acceleration;
    }

    void Explode(){
        Destroy(gameObject);
    }
}
