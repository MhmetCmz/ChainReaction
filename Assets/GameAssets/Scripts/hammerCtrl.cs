using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class hammerCtrl : MonoBehaviour,IInteractableObject
{
    public bool gameOn, restartIt, hammerIsOnPosition;
    public float firstAngle, lastAngel;
    public Vector3 startPos;
    public Quaternion startRot;
    Tween rotateTween;

    void Start()
    {
        startPos = gameObject.transform.position;
        startRot = gameObject.transform.rotation;
        restartIt = false;
        gameOn = false;
        hammerIsOnPosition = true;
    }
    void OnTriggerEnter(Collider co)
    {
        if (hammerIsOnPosition)
        {
            if (gameOn)
            {
                if (co.CompareTag("Tahterevalli"))
                {
                    restartIt = true;
                    rotateTween = transform.DORotate(new Vector3(0, 0, firstAngle), 1.5f).SetRelative().OnComplete(() => goToEnd());
                }
                if (co.CompareTag("Glass"))
                {
                    co.GetComponent<MeshRenderer>().enabled = false;
                    co.GetComponent<ParticleSystem>().Play();
                    co.GetComponent<BoxCollider>().enabled = false;
                }
            }
        }
    } 
    void goToEnd()
    {
        transform.DORotate(new Vector3(0, 0, lastAngel), .5f).SetRelative();
    }
    public void SetDefault()
    {
        gameOn = false;
        rotateTween.Kill(); 
        if (restartIt)
        {
            gameObject.transform.position = startPos;
            gameObject.transform.rotation = startRot;
            restartIt = false;
        }
    }
    public void LevelStart()
    {
        startPos = gameObject.transform.position;
        startRot = gameObject.transform.rotation;
        gameOn = true;
    }
}
