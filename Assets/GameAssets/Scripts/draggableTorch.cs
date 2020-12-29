using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class draggableTorch : MonoBehaviour,IInteractableObject
{
    [SerializeField] private ParticleSystem fireParticle;
    public bool gameOnCtrl;
    void Start()
    {
        gameOnCtrl = false;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider col)
    { 
        if (col.tag == gameObject.tag)
        {
            fireParticle.Play();
            col.gameObject.GetComponent<hotAirBaloon>().isItOnFire = true;
            if (gameOnCtrl) col.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            Destroy(gameObject, 1f);
        }
    }
    public void LevelStart()
    {
        gameOnCtrl = true;
    }
    public void SetDefault()
    {
        gameOnCtrl = false;
    }
}
