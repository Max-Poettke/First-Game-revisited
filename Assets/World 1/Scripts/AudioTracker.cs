using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class AudioTracker : MonoBehaviour
{
    public AudioSource [] narration;
    int narrationTracker = 0;
    bool narrating = true;
    GameMaster gm;

    void OnEnable(){
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        gm.currSceneName = SceneManager.GetActiveScene().name;
        if(gm.currSceneName != gm.lastSceneName || gm.lastSceneName == null){
            gm.changedScene = true;
        } else {
            gm.changedScene = false;
        }
        gm.lastSceneName = gm.currSceneName;
        if(!gm.changedScene || (gm.currSceneName == "Start Screen" && gm.alreadyLoaded == true)) return;
        if(gm.alreadyLoaded == false && gm.currSceneName == "Start Screen") gm.alreadyLoaded = true;
        narration = GameObject.FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
        narration = narration.Skip(1).ToArray();
        SortArrayByNames(narration);
        if(narration.Length > 0) StartCoroutine(Narration());
    }

    //Sorting works by naming the audioSnippets by their corresponding order. E.g. The first Audio snippet may be named "0"
    void SortArrayByNames(AudioSource[] arr) {
        var helper = new AudioSource[arr.Length];
        for(int i = 0; i < helper.Length; i ++){
            helper[i] = narration[i];
        }
        foreach (AudioSource snippet in helper){
            arr[int.Parse(snippet.name)] = snippet;
        }
    }

    IEnumerator Narration(){
        narrationTracker = 0;
        narrating = true;
        while(narrating){
            if(narrationTracker == narration.Length - 1 || narration.Length == 0) {
                narrating = false; 
                StopAllCoroutines(); 
                Debug.Log("narration finished");
                yield break;
            }
            Debug.Log("Snippet " + narrationTracker + " is playing"); 
            narration[narrationTracker].Play();
            narrationTracker ++;
            yield return new WaitForSeconds(30);
        }
    }
}
