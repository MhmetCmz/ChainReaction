using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class golfCtrl : MonoBehaviour,IInteractableObject
{
    public float firstAngle, lastAngle;
    public GameObject sopaGO;
    public Vector3 startPos;
    public Quaternion startRot;
    bool touchCtrl;
    Tween rotateTween;

    void Start()
    {
        touchCtrl = false;
        startPos = sopaGO.transform.localPosition;
        startRot = sopaGO.transform.rotation;
    }
    void OnCollisionEnter(Collision co)
    {
        if (!co.collider.CompareTag("Sopa"))
        {
            if (!touchCtrl)
            {
                touchCtrl = true;
                rotateTween = sopaGO.transform.DORotate(new Vector3(+30, 0, 0), 1f).SetRelative().OnComplete(() => goToEnd());
            }
        }
    } 
    void goToEnd()
    {
        sopaGO.transform.DORotate(new Vector3(-40, 0, 0), .5f).SetRelative();
    }
    public void SetDefault()
    {
        rotateTween.Kill();
        sopaGO.transform.localPosition = startPos;
        sopaGO.transform.rotation = startRot;
        touchCtrl = false;
    }
    public void LevelStart()
    {

    }
}
