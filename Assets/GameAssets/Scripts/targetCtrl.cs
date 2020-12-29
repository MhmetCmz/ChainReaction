using System;
using UnityEngine;
using DG.Tweening;
using UniRx;

public class targetCtrl : MonoBehaviour
{
    [SerializeField] private ParticleSystem confettiParticle;
    bool touch = false;

    void OnTriggerEnter(Collider coll)
    { 
        if (!coll.CompareTag("Bomb"))
	    {
            if (!touch)
	        {
              touch=true;
              coll.gameObject.GetComponent<Rigidbody>().isKinematic=true;
              if (!coll.CompareTag("Arrow"))
              {
                  transform.DORotate(new Vector3(0, 0, 90), .5f).SetRelative();
              }
              confettiParticle.Play(true);
              Observable.Timer(TimeSpan.FromSeconds(1)).Subscribe(l => FindObjectOfType<UIController>().OnGameWon?.Invoke()).AddTo(this);
            }
        }
    }
}