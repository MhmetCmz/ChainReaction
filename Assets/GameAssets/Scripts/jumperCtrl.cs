using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class jumperCtrl : MonoBehaviour,IInteractableObject
{
    public GameObject obj; 
    public float jumpForce;
    public Vector3 objStartPos;

    void Start()
    {
        objStartPos = obj.transform.localPosition;
    }
    void OnTriggerStay(Collider coll)
    {
        if (coll.CompareTag("Ball")||coll.CompareTag("torch"))
        {
            Rigidbody rb = coll.gameObject.GetComponent<Rigidbody>();
            rb.velocity = transform.up * -jumpForce * Time.deltaTime * 10; 
            Debug.Log(rb.velocity);
            obj.transform.DOLocalMoveY(4,0.3f);
        }
    }
    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Ball"))
        {  
            col.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0); 
        }
    }
    public void SetDefault()
    {
        obj.transform.localPosition = objStartPos;
    }
    public void LevelStart()
    {

    }
}
