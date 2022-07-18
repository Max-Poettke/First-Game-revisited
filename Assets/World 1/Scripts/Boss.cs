using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public float maxHealth;
    public float health;
    public float speed;

    public bool isStarted = false;
    public bool isAttacking = false;
    public bool isVulnerable = false;
    public bool resetting = false;

    public GameObject bolt;  
    public GameObject player;
    public GameObject aimer;
    public GameObject resetPosition;

    /* States
        Attacking
        Vulnerable fase
        Moving around between attacks
    */
    public int attackState = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!isStarted) return;
        if(resetting) {
            ResetPosition();
            return;
        }
        if(!isAttacking) return;
        int attackChooser = 1;
        switch (attackChooser) {
            case 1:
                StartCoroutine(Attack1());
                isAttacking = false;
                break;
        }
    }

    void ResetPosition(){
        Debug.Log("Resetting");
        transform.position = Vector3.MoveTowards(transform.position, resetPosition.transform.position, speed);
        if(Vector3.Distance(resetPosition.transform.position, transform.position) < 0.1f) resetting = false;
    }

    IEnumerator Attack1(){
        yield return new WaitForSeconds(1);
        Transform targetTransform = aimer.transform;
        targetTransform.position = player.transform.position;


        var newBolt = Instantiate(bolt);
        var ai = newBolt.GetComponent<BoltAi>();
        ai.targetPos = targetTransform.position + (targetTransform.position - newBolt.transform.position) * 10;
        ai.startSpeed = 10f;

        newBolt = Instantiate(bolt);
        ai = newBolt.GetComponent<BoltAi>();
        targetTransform.RotateAround(gameObject.transform.position, new Vector3(0, 0, 1), -20);
        ai.targetPos = targetTransform.position + (targetTransform.position - newBolt.transform.position) * 10;
        ai.startSpeed = 10f;
        
        newBolt = Instantiate(bolt);
        ai = newBolt.GetComponent<BoltAi>();
        targetTransform.RotateAround(gameObject.transform.position, new Vector3(0, 0, 1), 10);
        ai.targetPos = targetTransform.position + (targetTransform.position - newBolt.transform.position) * 10;
        ai.startSpeed = 10f;

        newBolt = Instantiate(bolt);
        ai = newBolt.GetComponent<BoltAi>();
        targetTransform.RotateAround(gameObject.transform.position, new Vector3(0, 0, 1), 10);
        ai.targetPos = targetTransform.position + (targetTransform.position - newBolt.transform.position) * 10;
        ai.startSpeed = 10f;
        
        newBolt = Instantiate(bolt);
        ai = newBolt.GetComponent<BoltAi>();
        targetTransform.RotateAround(gameObject.transform.position, new Vector3(0, 0, 1), 10);
        ai.targetPos = targetTransform.position + (targetTransform.position - newBolt.transform.position) * 10;
        ai.startSpeed = 10f;
    }
}
