using Sirenix.OdinInspector; 
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 

[CreateAssetMenu]
public class LevelLoader : ScriptableObject
{
    private const string pref = "level";  
    private const string virtualpref = "virtualLevel";  

    public int CurrentLevelIndex { get; private set; }
    public int VirtualLevelIndex { get; private set; }

    public void LoadFirstLevel()
    {
        
        LoadLevelIndex();
        if (!SceneManager.GetSceneByName($"Level{CurrentLevelIndex + 1}").isLoaded)
            SceneManager.LoadScene($"Level{CurrentLevelIndex + 1}", LoadSceneMode.Additive); 
        
    }

    public void Load()
    {
        SceneManager.LoadScene("Game");
        SceneManager.UnloadSceneAsync(CurrentLevelIndex + 1);
        SceneManager.LoadScene($"Level{CurrentLevelIndex + 2}", LoadSceneMode.Additive); 
    }
[Button(ButtonSizes.Large)]
    public void LoadNewLevel()
    {
        Load();
        IncreaseLevelIndex();
        SaveLevelIndex(); 
    }
     

    private void IncreaseLevelIndex()
    {
        CurrentLevelIndex += 1;
        VirtualLevelIndex += 1;
        if (CurrentLevelIndex >= SceneManager.sceneCountInBuildSettings -1) CurrentLevelIndex = 1; 
    }

   

    private void SaveLevelIndex()
    {
        PlayerPrefs.SetInt(pref, CurrentLevelIndex);
        PlayerPrefs.SetInt(virtualpref,VirtualLevelIndex);
    }

    private void LoadLevelIndex()
    {
        CurrentLevelIndex = PlayerPrefs.GetInt(pref, 0);
        VirtualLevelIndex = PlayerPrefs.GetInt(virtualpref, 0);
        
    }

    #region Editor
    private void DecreaseLevelIndex()
    {
        CurrentLevelIndex -= 1;
    }
    [Button(ButtonSizes.Large)]

    public void LoadPreviousLevel()
    {
        DecreaseLevelIndex();
        SaveLevelIndex();
        Load(); 
    }

    #endregion
}