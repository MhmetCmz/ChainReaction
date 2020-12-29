#if GAMEANALYTICS
using GameAnalyticsSDK;
#endif
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

#endif

namespace DHFramework.Analytics
{
    [CreateAssetMenu(menuName = "Framework/Analytics/GameAnalyticsReporter")]
    public class GameAnalyticsReporter : ScriptableObject
    {
        [SerializeField] private LevelLoader levels;

#if UNITY_EDITOR
        private const string gameanalytics = "GAMEANALYTICS";

        [Button]
        void EnableGameAnalyticsForIos()
        {
            string def = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS);
            if (!def.Contains(gameanalytics))
            {
                def += $";{gameanalytics};";
                PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS, def);
            }
        }

        [Button]
        void DisableGameAnalyticsForIos()
        {
            string def = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS);
            if (def.Contains(gameanalytics))
            {
                def = def.Replace(gameanalytics, string.Empty);
                PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS, def);
            }
        }
#endif

        public void ReportLevelStart()
        {
#if GAMEANALYTICS
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, 
                $"Level{levels.VirtualLevelIndex +1}");
#endif
        }

        public void ReportLevelSuccess()
        {
#if GAMEANALYTICS
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete,
                $"Level{levels.VirtualLevelIndex +1}");
#endif
        }

        public void ReportLevelFailed()
        {
#if GAMEANALYTICS
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, $"Level{levels.VirtualLevelIndex +1}");
#endif
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void InitAnalytics()
        {
#if GAMEANALYTICS
            Debug.Log("[Initialize] Initializing GameAnalytics");
            GameAnalytics.Initialize();
#endif
        }
    }
}