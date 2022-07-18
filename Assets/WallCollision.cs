using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCollision : MonoBehaviour
{
    PlayerMovement3D pm;

    void Start(){
        pm = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement3D>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Ground"){
            pm.touchingWall = true;
        }
    }
    private void OnTriggerExit(Collider other){
        if(other.tag == "Ground"){
            pm.touchingWall = false;
        }
    }
}
