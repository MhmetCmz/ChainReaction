using System;
using Sirenix.Utilities;
using UnityEngine;

public class Tutorials : MonoBehaviour
{
    [SerializeField] private GameObject[] tutorials;
    private UIController uiController;
    private void OnEnable()
    {
        uiController = FindObjectOfType<UIController>();
        uiController.OnLevelStart.AddListener(OnLevelStart);
        uiController.OnLevelReset.AddListener(OnLevelReset);
        
    }

    private void OnLevelReset()
    {
        tutorials.ForEach(o => o.SetActive(true));
    }

    private void OnLevelStart()
    {
        tutorials.ForEach(o => o.SetActive(false));
    }

    private void OnDisable()
    {
        uiController.OnLevelStart.RemoveAllListeners();
        uiController.OnLevelReset.RemoveAllListeners();
    }
}
