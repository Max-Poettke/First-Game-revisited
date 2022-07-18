using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bossmovement : StateMachineBehaviour
{
    public List<Transform> wayPoints;
    public int wayPointIndex = 0;
    public float moveSpeed = 2f;
    public Transform transform;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        transform.position = wayPoints[wayPointIndex].transform.position;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Move();
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    void Move()
    {
        var targetPos = wayPoints[wayPointIndex].transform.position;
        var moveBy = moveSpeed * Time.deltaTime;

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
}
