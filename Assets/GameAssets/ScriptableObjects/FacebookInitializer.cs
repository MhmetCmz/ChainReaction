using UnityEngine;

#if FACEBOOK
using Facebook.Unity;
#endif

namespace DHFramework.Analytics
{
    public class FacebookInitializer : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod]
        public static void CheckFBAndCreateIfNeeded()
        {
            FacebookInitializer fbInit = FindObjectOfType<FacebookInitializer>();
            if (fbInit == null)
            {
                new GameObject("FBInitializer").AddComponent<FacebookInitializer>();
            }
        }
    
        void Awake​()
        {
#if FACEBOOK
        FB.Init(FBInitCallback​);
#endif
        }

        private void FBInitCallback​()
        {
#if FACEBOOK
        if (FB.IsInitialized)
        {
            FB.ActivateApp();
        }
#endif
        }

        public void OnApplicationPause​(bool paused)
        {
#if FACEBOOK
        if (!paused)
        {
            if (FB.IsInitialized)
            {
                FB.ActivateApp();
            }
        }
#endif
        }
    }
}