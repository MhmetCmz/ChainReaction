using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hotAirBaloon : MonoBehaviour,IInteractableObject
{
    public float flyingSpeed;
    public bool isItOnFire;
    Rigidbody rb;
    public Vector3 startPos;
    public Quaternion startRot;
    void Start()
    {
        startPos = gameObject.transform.position;
        startRot = gameObject.transform.rotation;
        isItOnFire = true;
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isItOnFire)
        { 
            rb.AddForce(transform.forward * -flyingSpeed * Time.deltaTime);
        }
    }
    public void LevelStart() {
        if (isItOnFire) rb.isKinematic = false;
    }
    public void SetDefault()
    {
        rb.isKinematic = true;
        gameObject.transform.position = startPos;
        gameObject.transform.rotation = startRot;
    }
}
