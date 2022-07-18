using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fall : MonoBehaviour
{
private Rigidbody2D rb2D;
public float fallDelay;

void Start()
{
rb2D = GetComponent<Rigidbody2D>();
}

void OnCollisionEnter2D(Collision2D col)
{
    if (col.collider.CompareTag("Player"))
    {
        StartCoroutine (fall());
    } else {
        Destroy(gameObject);
    }
    if (col.collider.CompareTag("Spike"))
    {
        Destroy(gameObject);
    }
}

IEnumerator fall()
{
    yield return new WaitForSeconds(fallDelay);
    rb2D.isKinematic = false;
    yield return 0;
}
}

