using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallRollingToTarget : MonoBehaviour
{
    public Transform target;
    public Rigidbody rb;
    public float speed;

    void Start()
    {
        
    }

    
    void Update()
    {
        Vector3 targetDirection = target.position - transform.position;
        rb.AddForce(targetDirection * speed * Time.deltaTime);

        if(Vector3.Distance(target.position, transform.position) < 0.1f)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}
