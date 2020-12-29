using com.adjust.sdk;
using UnityEngine;

namespace DHFramework.Analytics
{
    [CreateAssetMenu(menuName = "Framework/Analytics/AdjustReporter")]
    public class AdjustReporter : ScriptableObject
    {
        [SerializeField] private LevelLoader levelLoader;
        public void ReportLevelStart()
        {
            AdjustEvent adjustEvent = new AdjustEvent("LevelStart");

            adjustEvent.addCallbackParameter("Level", $"{levelLoader.VirtualLevelIndex +1}");

            Adjust.trackEvent(adjustEvent);
        }

        public void ReportLevelSuccess()
        {
            AdjustEvent adjustEvent = new AdjustEvent("LevelSuccess");

            adjustEvent.addCallbackParameter("Level", $"{levelLoader.VirtualLevelIndex +1}");

            Adjust.trackEvent(adjustEvent);
        }

        public void ReportLevelFailed()
        {
            AdjustEvent adjustEvent = new AdjustEvent("LevelFailed");

            adjustEvent.addCallbackParameter("Level", $"{levelLoader.VirtualLevelIndex +1}");

            Adjust.trackEvent(adjustEvent);
        }
    }
}