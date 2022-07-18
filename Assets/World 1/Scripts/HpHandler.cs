using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HpHandler : MonoBehaviour
{
    [SerializeField] int HP = 1;

    GameMaster gm;
    CameraFollow cm;
    PlayerMovement pm;
    public Rigidbody2D rb;
    public SpriteRenderer sp;
    public ParticleSystem ps;

    void Start(){
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        cm = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>();
        pm = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }

    void OnTriggerEnter2D(Collider2D col){
        if(col.CompareTag("Spike")){
            HP --;
            if(HP == 0){
                if(SceneManager.GetActiveScene().name == "Start Screen"){
                    transform.position = gm.lastCheckPointPos;
                    return;
                }
                sp.enabled = false;
                pm.isDead = true;
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
                StartCoroutine(cm.Screenshake());
            }
        } else if (col.CompareTag("CheckPoint")){
            gm.lastCheckPointPos = col.transform.position;
        }
    }

    public void Die(){
        SceneManager.LoadScene(gm.curScene);
    }
}
