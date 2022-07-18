using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float rotationSpeed;
    public Transform rotator;
    void Update()
    {
        Rotater();
    }

    void Rotater()
    {
        if (rotator.rotation.z != 180)
        {
            var rotationZ = rotator.rotation.z;
            var rotateBy = rotationSpeed * Time.deltaTime;
            rotator.rotation = new Quaternion(rotator.rotation.x, rotator.rotation.y, rotationZ + rotateBy, rotator.rotation.w);

        } else if (rotator.rotation.z == 180)
        { 
            rotator.rotation = new Quaternion(rotator.rotation.x, rotator.rotation.y, 0, rotator.rotation.w);
        }
    }
}
