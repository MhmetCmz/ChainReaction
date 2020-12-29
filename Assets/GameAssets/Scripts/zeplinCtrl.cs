using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zeplinCtrl : MonoBehaviour,IInteractableObject
{
    public float speed;
    public bool isItWorkOnStart;
    Vector3 startPos;
    Quaternion startRot;
    Rigidbody rb;
    void Start()
    {
        startPos = transform.position;
        startRot = transform.rotation;
        rb = gameObject.GetComponent<Rigidbody>(); 
    }
     
    void Update()
    {
        rb.AddForce(transform.forward * speed * Time.deltaTime); 
    } 
    public void LevelStart()
    {
        if (isItWorkOnStart)
        {
            rb.isKinematic = false;
        }
    }
    public void SetDefault()
    {
        transform.rotation = startRot;
        transform.position = startPos;
        rb.isKinematic = true;
    }
}
