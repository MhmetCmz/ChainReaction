using System;
using Lean.Touch;
using UniRx;
using UnityEngine;

public class draggableObj : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 10;
    [SerializeField] private GameObject interactableObj,highlightedObj; 
    [SerializeField] private bool justView;
    private Camera camera;
    private IDisposable disposable;
    private LeanFingerFilter use = new LeanFingerFilter(true);
    private RaycastHit raycastHit;
    private Ray ray;
    private bool isSelected;

    private void OnEnable()
    {
        LeanTouch.OnFingerDown += OnFingerDown;
        LeanTouch.OnFingerUp += OnFingerUp;
        camera = Camera.main;
    }

    private void OnFingerUp(LeanFinger obj)
    {
        disposable?.Dispose();
        isSelected = false;
    }

    private void OnFingerDown(LeanFinger obj)
    {
        disposable = Observable.EveryUpdate().Where(l => isSelected).Subscribe(OnUpdate).AddTo(this);
        ray = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out raycastHit, Mathf.Infinity, LayerMask.GetMask("Default")))
        {
            if (raycastHit.transform.Equals(transform))
                isSelected = true;
        }
    }

    private void OnUpdate(long obj)
    {
        var fingers = use.GetFingers();
        var screenDelta = LeanGesture.GetScreenDelta(fingers);
        if (screenDelta != Vector2.zero) Translate(raycastHit.transform, screenDelta); 
    }

    private void Translate(Transform translateObject, Vector2 screenDelta)
    {
        var screenPoint = camera.WorldToScreenPoint(translateObject.position);
        screenPoint += (Vector3) screenDelta;
        Vector3 position = camera.ScreenToWorldPoint(screenPoint);
        position.z = translateObject.position.z;
        translateObject.position = Vector3.Lerp(translateObject.position, position, movementSpeed);
    }

    private void OnDisable()
    {
        LeanTouch.OnFingerDown -= OnFingerDown;
        LeanTouch.OnFingerUp -= OnFingerUp;
    }
    void OnTriggerEnter(Collider col)
    {
        if (col.tag == gameObject.tag)
        {
            HighlightPlus.HighlightEffect highlight = highlightedObj.GetComponent<HighlightPlus.HighlightEffect>();
            if (highlight != null) Destroy(highlight);
            if (justView)
            {
                hammerCtrl hammerScript = interactableObj.GetComponentInParent<hammerCtrl>();
                arrowCtrl arrowScript = interactableObj.GetComponent<arrowCtrl>();
                if (hammerScript != null) hammerScript.hammerIsOnPosition = true;
                if (arrowScript != null) arrowScript.arrowOnPosition = true;
                interactableObj.GetComponent<MeshRenderer>().enabled = true;
                Destroy(gameObject);
            }
        }
    }
}