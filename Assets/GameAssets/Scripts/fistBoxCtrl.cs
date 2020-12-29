using UnityEngine;
using DG.Tweening;

public class fistBoxCtrl : MonoBehaviour, IInteractableObject
{ 
    public GameObject glove;
    public Vector3 gloveStartPos;

    void Start()
    {
        gloveStartPos = glove.transform.localPosition;
    }
    void OnTriggerEnter(Collider coll)
    {
        glove.transform.DOLocalMoveY(4,0.1f);
    }
    public void SetDefault()
    {
        glove.transform.localPosition = gloveStartPos;
    }
    public void LevelStart()
    {

    }
} 