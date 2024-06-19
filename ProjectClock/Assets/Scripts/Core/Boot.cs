using RoadToAAA.ProjectClock.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RoadToAAA.ProjectClock.Core
{
    /*
     * Boots the ground systems for the game, so that other entities can assume they are initialized
     */
    public class Boot : MonoBehaviour
    {
        private void Awake()
        {
            // Load all systems
            SceneManager.LoadScene("CoreSystems", LoadSceneMode.Additive);
#if UNITY_EDITOR
            //SceneManager.LoadScene("DebugSystems", LoadSceneMode.Additive);
#endif
            SceneManager.LoadScene("Game", LoadSceneMode.Additive);

            EventManager.Instance.Publish(EEventType.OnBootSystemsLoaded);
        }

        /*
         * The Start method is necessary to being sure to capture the nasty [Debug Updater] GameObject (URP only)
         */
        private void Start()
        {
            GameObject debugUpdater = GameObject.Find("[Debug Updater]");
            if (debugUpdater != null ) 
            {    
                Destroy(debugUpdater);
            }
            SceneManager.UnloadSceneAsync("Boot");
        }
    }
}