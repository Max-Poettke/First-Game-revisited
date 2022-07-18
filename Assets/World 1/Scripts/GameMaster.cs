using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour{

    public static GameMaster instance;
    public Vector2 lastCheckPointPos;
    public Text InputText;
    public Canvas Canvas;
    public Transform playerSpawn;
    public Transform player;
    private Door d;
    public int curScene;
    public GameObject music;
    public Text bottomText;
    public string currSceneName;
    public string lastSceneName;
    public bool changedScene = false;
    public bool alreadyLoaded = false;
    private void Start()
    {
        d = GameObject.FindGameObjectWithTag("Door").GetComponent<Door>();
    }

    void Awake(){
        if (instance == null){
        instance = this;
        music = Instantiate(music);
        DontDestroyOnLoad(instance);                
        DontDestroyOnLoad(Canvas);
        DontDestroyOnLoad(playerSpawn);
        DontDestroyOnLoad(music);
        } else {
            Destroy(gameObject);                            
        }

    }


}
