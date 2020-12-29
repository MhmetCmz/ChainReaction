using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private LevelLoader levelLoader;
    public UnityEvent OnGameWon;
    public UnityEvent OnLevelStart;
    public UnityEvent OnLevelReset;
    public Button startButton;
    public Text lvlText;
    int currentLevel;

    private void OnEnable()
    { 
    }
    void Start()
    {
        currentLevel = PlayerPrefs.GetInt("virtualLevel",0) +1;
        lvlText.text = "level " + currentLevel;
    }
    public void StartButton()
    {
        startButton.interactable = false;
        GameObject.Find("GeneralController").GetComponent<touchCtrl>().RayOn=false;
        GameObject.Find("GeneralController").GetComponent<levelController>().startLevel();
        OnLevelStart?.Invoke();
    }
    public void RestartButton()
    {
        startButton.interactable = true;
        GameObject.Find("GeneralController").GetComponent<touchCtrl>().RayOn = true;
        GameObject.Find("GeneralController").GetComponent<levelController>().resetObjects();
        OnLevelReset?.Invoke();
    } 
}