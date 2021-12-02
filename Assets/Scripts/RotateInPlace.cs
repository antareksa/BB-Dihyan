using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateInPlace : MonoBehaviour
{
    public float speedRotate = 100;

    void Update()
    {
        this.transform.Rotate(Vector3.up * speedRotate * Time.deltaTime);
    }
}
