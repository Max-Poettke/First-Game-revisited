using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;
    public PlayerMovement player;
    public GameObject boss;
    public CanvasGroup parentCanvasGroup;
    Tween tween;
    


    private int index;
    // Start is called before the first frame update
    void Start()
    {
        textComponent.text = string.Empty;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        tween = parentCanvasGroup.DOFade(1f, 3f);
        StartDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)){
            if(textComponent.text == lines[index]){
                NextLine();
            } else {
                StopAllCoroutines();  
                textComponent.text = lines[index];
            }
        }
    }

    void StartDialogue(){
        index = 0;
        player.isTalking = true;
        StartCoroutine(TypeLine());
    }

    void NextLine(){
        if(index < lines.Length - 1){
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        } else {
            gameObject.SetActive(false);
            player.isTalking = false;
            if(boss != null) {
                var bossScript = boss.GetComponent<Boss>();
                bossScript.isStarted = true;
                bossScript.resetting = true;
            }  
        }
    }

    IEnumerator TypeLine(){
        foreach(char c in lines[index].ToCharArray()){
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }
}
