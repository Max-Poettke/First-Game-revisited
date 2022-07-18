using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HealthBar : MonoBehaviour
{
    public Image healthBarInner;
    public Boss boss;
    Tween tween;
    public CanvasGroup parentCanvasGroup;

    void Start(){
        boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss>();
    }
    // Update is called once per frame
    void Update()
    {   
        if(boss.health <= 0f){
            boss.health = 0.001f;
            StartCoroutine(removeHealthBar());
        }
        float duration = 0.75f * (boss.health / boss.maxHealth);
        healthBarInner.DOFillAmount(boss.health / boss.maxHealth, duration);
    }

    IEnumerator removeHealthBar(){
        if(tween != null){
            tween.Kill();
        }
        tween = parentCanvasGroup.DOFade(0f, 3f);
        yield return new WaitForSeconds(3);
        gameObject.SetActive(false);
        StopAllCoroutines();
    }
}
