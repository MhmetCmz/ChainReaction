using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class defaulter : MonoBehaviour, IInteractableObject
{
    Vector3 startPos,levStartPos;
    Quaternion startRot,levStartRot;
    bool restartIt;
    public bool kinematicCtrl=false;
    void Start()
    {
        restartIt = false;
        startPos = transform.localPosition;
        startRot = transform.rotation;
    } 
    public void SetDefault()
    {
        if (gameObject.GetComponent<canMove>()==null)
        {
            transform.localPosition = startPos;
            transform.rotation = startRot; 
        }
        else if (restartIt)
        {
            restartIt = false;
            transform.localPosition = levStartPos;
            transform.rotation = levStartRot; 
        }
        if (kinematicCtrl) gameObject.GetComponent<Rigidbody>().isKinematic = true;
    }
    public void LevelStart()
    {
        levStartPos = transform.localPosition;
        levStartRot = transform.rotation;
        if (kinematicCtrl) gameObject.GetComponent<Rigidbody>().isKinematic = false;
    }
    void OnCollisionEnter(Collision col)
    {
        restartIt = true;
    }
}
