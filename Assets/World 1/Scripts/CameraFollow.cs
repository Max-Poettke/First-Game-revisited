using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{   
    public Transform followTransform;
    public BoxCollider2D mapBounds;
    public float xMin, xMax, yMin, yMax;
    public float camX, camY;
    public float xResize = 2f;
    public float smoothSpeed = 0.5f;
    public AnimationCurve curve;
    public float shakeDuration = 1f;
    private float camOrthsize;
    private float cameraRatio;
    private Camera mainCam;
    private Vector3 smoothPos;
    private HpHandler hpH;
    private void Start()
    {
        xMin = mapBounds.bounds.min.x;
        xMax = mapBounds.bounds.max.x;
        yMin = mapBounds.bounds.min.y;
        yMax = mapBounds.bounds.max.y;
        mainCam = GetComponent<Camera>();
        camOrthsize = mainCam.orthographicSize;
        cameraRatio = (xMax + camOrthsize) / xResize;
        hpH = GameObject.FindGameObjectWithTag("Player").GetComponent<HpHandler>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        camY = Mathf.Clamp(followTransform.position.y, yMin + camOrthsize, yMax - camOrthsize);
        camX = Mathf.Clamp(followTransform.position.x, xMin + cameraRatio, xMax - cameraRatio); 
        smoothPos = Vector3.Lerp(this.transform.position, new Vector3(camX, camY, this.transform.position.z), smoothSpeed);
        this.transform.position = smoothPos;
        
    }

    public IEnumerator Screenshake(){
        Vector3 startPos = transform.position;
        float elapsedTime = 0f;
        hpH.ps.Play();

        while (elapsedTime < shakeDuration){
            elapsedTime += Time.deltaTime;
            float strength = curve.Evaluate(elapsedTime/shakeDuration);
            transform.position = startPos + Random.insideUnitSphere * strength;
            yield return null;
        } 
        hpH.ps.Stop();
        hpH.Die();
    }
}
