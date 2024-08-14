using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

namespace RoadToAAA.ProjectClock.Managers
{
    public class CloudSaveManager : MonoBehaviour
    {
        private async void Awake()
        {
            await UnityServices.InitializeAsync();
            await AuthenticationService.Instance.SignInAnonymouslyAsync();

#if UNITY_EDITOR
            Debug.Log("Cloud save initialization complete.");
#endif
        }
    }
}
