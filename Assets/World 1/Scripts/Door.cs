using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Door : MonoBehaviour{

    public int levelToLoad;
    public int collisionCheck = 0;
    public UnityEngine.Vector2 doorSpawn;
    public string enterText;
    public Color bottomTextColor;

    private GameMaster gm;

    void Start(){

        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();

    }

    void Update(){
        if(Input.GetKeyDown("e") && collisionCheck == 1){
            gm.lastCheckPointPos = doorSpawn;
            gm.curScene = levelToLoad;
            SceneManager.LoadScene(levelToLoad);
            gm.InputText.text = (" ");
        } 
    }

    void OnTriggerEnter2D(Collider2D col){
        if (col.CompareTag("Player")){
            collisionCheck = 1;
            gm.InputText.text = (enterText);   
            gm.bottomText.color = bottomTextColor;  
        }

    }

    void OnTriggerExit2D(Collider2D col){
        if (col.CompareTag("Player")){
            gm.InputText.text = (" ");
            collisionCheck = 0;
        }
    }
}
