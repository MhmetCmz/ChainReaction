using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class draggableBomb : MonoBehaviour, IInteractableObject
{
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private Rigidbody[] wallsRb;
    [SerializeField] private ParticleSystem expParticle;
    [SerializeField] private float expForce, expRad;
    private bool oneShot;
    private Vector3 firstPos;
    private Vector3 firstRot;

    void Start()
    {
        firstPos = transform.position;
        firstRot = transform.eulerAngles;
        oneShot = false;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == gameObject.tag)
        {
            if (!oneShot)
            {
                expParticle.Play();
                gameObject.GetComponent<MeshRenderer>().enabled = false;
                transform.GetChild(0).gameObject.SetActive(false);
                foreach (Rigidbody rb in wallsRb)
                {
                    if (rb != null)
                    {
                        rb.gameObject.GetComponent<BoxCollider>().isTrigger = true;
                        rb.isKinematic = false;
                        rb.AddExplosionForce(expForce, transform.position, expRad);
                        Destroy(rb.gameObject, 5f);
                    }
                }

                Destroy(gameObject, 3f);
                oneShot = true;
            }
        }
    }

    public void SetDefault()
    {
        if (gameObject.activeSelf)
        {
            transform.position = firstPos;
            transform.eulerAngles = firstRot;
            rigidbody.isKinematic = true;
        }
    }

    public void LevelStart()
    {
        rigidbody.isKinematic = false;
    }
}