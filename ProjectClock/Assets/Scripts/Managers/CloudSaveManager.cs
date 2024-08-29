using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using Unity.Services.Core;
using UnityEngine;

namespace RoadToAAA.ProjectClock.Managers
{
    public class CloudSaveManager : MonoBehaviour
    {
        private async void Awake()
        {
            await UnityServices.InitializeAsync();

            AuthenticationService.Instance.SignedIn += SignInSuccess;
            AuthenticationService.Instance.SignInFailed += SignInFailed;

            await AuthenticationService.Instance.SignInAnonymouslyAsync();

#if UNITY_EDITOR && LOGGER
            Debug.Log("Cloud save initialization complete.");
#endif

            //List<int> ints = new List<int>();
            //ints.Add(1);
            //ints.Add(2);
            //ints.Add(3);
            //var data = new Dictionary<string, object> { { "MySaveKey", ints } };
            //await CloudSaveService.Instance.Data.Player.SaveAsync(data);
        }

        private void SignInSuccess()
        {
#if UNITY_EDITOR && LOGGER
            Debug.Log("<color=green>SignIn Success</color>");
#endif
        }

        private void SignInFailed(RequestFailedException exception)
        {
#if UNITY_EDITOR && LOGGER
            Debug.Log("<color=red>SignIn Failed</color>");
#endif
        }
    }
}
