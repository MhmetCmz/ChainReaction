using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class levelController : MonoBehaviour
{ 
    private IEnumerable<IInteractableObject> interactableObjects;  
    DOTweenAnimation[] doTWAnim;
    HighlightPlus.HighlightEffect[] highlightEffect;
    private void OnEnable()
    {
        Time.timeScale = 2f;
        interactableObjects = FindObjectsOfType<MonoBehaviour>().OfType<IInteractableObject>();

    }
    void Start()
    {
        doTWAnim = DOTweenAnimation.FindObjectsOfType(typeof(DOTweenAnimation)) as DOTweenAnimation[];
    }

    public void resetObjects()
    {
        highlightEffect = null;
        highlightEffect = HighlightPlus.HighlightEffect.FindObjectsOfType(typeof(HighlightPlus.HighlightEffect)) as HighlightPlus.HighlightEffect[];
        interactableObjects = FindObjectsOfType<MonoBehaviour>().OfType<IInteractableObject>();
        foreach (DOTweenAnimation dtanim in doTWAnim)
        {
            dtanim.DOPause();
        } 
        foreach (var interactableObject in interactableObjects)
        {
            interactableObject.SetDefault();
        }
        foreach (HighlightPlus.HighlightEffect highEff in highlightEffect)
        {
            highEff.highlighted = true;
        }
    }
    public void startLevel()
    {
        highlightEffect = null;
        highlightEffect = HighlightPlus.HighlightEffect.FindObjectsOfType(typeof(HighlightPlus.HighlightEffect)) as HighlightPlus.HighlightEffect[];
        foreach (DOTweenAnimation dtanim in doTWAnim)
        {
            dtanim.DOPlay();
        }
        foreach (var iObject in interactableObjects)
        {
            iObject.LevelStart();
        }
        foreach (HighlightPlus.HighlightEffect highEff in highlightEffect)
        {
            highEff.highlighted = false;
        }
    }  
}