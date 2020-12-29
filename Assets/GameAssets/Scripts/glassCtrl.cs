using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class glassCtrl : MonoBehaviour, IInteractableObject
{ 
    public void SetDefault()
    {
        gameObject.GetComponent<ParticleSystem>().Stop();
        gameObject.GetComponent<MeshRenderer>().enabled = true; 
        gameObject.GetComponent<BoxCollider>().enabled = true;
    }
    public void LevelStart()
    {

    }
}
