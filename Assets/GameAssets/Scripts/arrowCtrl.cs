using System;
using UniRx;
using UnityEngine;

public class arrowCtrl : MonoBehaviour,IInteractableObject
{ 
    public float arrowSpeed;
    public bool arrowOnPosition;
    Rigidbody rb;
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        arrowOnPosition = true;
    }
    void Update()
    {
        if (arrowOnPosition)
        {
            rb.AddForce(transform.up * arrowSpeed * Time.deltaTime);
        }
    }
    // void OnTriggerEnter(Collider col)
    // {
    //     if (!col.CompareTag("CrossBow") || !col.CompareTag("Ball"))
    //     {
    //         gameObject.GetComponent<Rigidbody>().isKinematic = true;
    //     }
    // }
    public void LevelStart() { }
    public void SetDefault()
    {
        rb.isKinematic = true;
        Observable.Timer(TimeSpan.FromSeconds(0.1f)).Subscribe(l => transform.localEulerAngles = Vector3.zero).AddTo(this);
    }
}
