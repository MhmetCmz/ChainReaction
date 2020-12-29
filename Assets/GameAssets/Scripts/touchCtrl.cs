using UnityEngine;
using Lean.Touch;
using DG.Tweening;

public class touchCtrl : MonoBehaviour
{
    public bool RayOn = true;
    public bool levelCompleted = false;
    bool dragging; 
    private Camera cam;
    Vector2 draggerFingPos;

    private void OnEnable()
    {
        LeanTouch.OnFingerDown += OnFingerDown;
        LeanTouch.OnFingerUp += OnFingerUp;
        cam = Camera.main;
        RayOn = false;
    } 
    private void OnFingerUp(LeanFinger obj)
    { 
    }

    private void OnFingerDown(LeanFinger obj)
    {
        var ray = cam.ScreenPointToRay(obj.ScreenPosition);
        RaycastHit rcHit; 
        if (Physics.Raycast(ray, out rcHit))
        {
            var trnsfrm = rcHit.collider.transform;
            canMove move = trnsfrm.GetComponent<canMove>();
            if (move != null)
            {
                if (RayOn)
                {
                    if (move.RotateAxis.Equals(RotateAxis.X))
                    {
                        RayOn = false;
                        RotateObject(trnsfrm, new Vector3(+90, 0, 0));
                    }
                    else if (move.RotateAxis.Equals(RotateAxis.Y))
                    {
                        RayOn = false;
                        RotateObject(trnsfrm, new Vector3(0, +90, 0));
                    }
                    else if (move.RotateAxis.Equals(RotateAxis.Z))
                    {
                        RayOn = false;
                        RotateObject(trnsfrm, new Vector3(0, 0, +90));
                    }
                }
            }
        }
    }
    private void OnDisable()
    {
        LeanTouch.OnFingerDown -= OnFingerDown;
        LeanTouch.OnFingerUp -= OnFingerUp;
    }

    private void RotateObject(Transform obj, Vector3 rotateEndValue)
    {
        obj.DORotate(rotateEndValue, 0.5f).OnComplete(() => RayOn = true).SetRelative();
    } 
}

public enum RotateAxis
{
    X,
    Y,
    Z
}