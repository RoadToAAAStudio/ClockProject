using UnityEngine;
using UnityEngine.InputSystem;
using RoadToAAA.ProjectClock.Utilities;
using static RoadToAAA.ProjectClock.Managers.GameManager;
using UnityEngine.UI;

namespace RoadToAAA.ProjectClock.Managers
{
    public class InputManager : MonoBehaviour
    {
        private PlayerInputs playerInputs;
        private InputAction playTap;
        private InputAction menuTap;
        private InputAction menuSwipe;


        private void Awake()
        {
            playerInputs = new PlayerInputs();
            playTap = playerInputs.PlayingMap.PlayTap;
            menuSwipe = playerInputs.MenuMap.Swipe;
        }
        
        private void OnEnable()
        {
            playerInputs.MenuMap.Enable();
            playerInputs.PlayingMap.Disable();
            playTap.started += PlayTap;


            EventManager<EGameState, EGameState>.Instance.Subscribe(EEventType.OnGameStateChanged, CheckSwitchActionMap);
        }

        private void OnDisable()
        {

            playTap.started -= PlayTap;

            playerInputs.MenuMap.Disable();
            playerInputs.PlayingMap.Disable();

        }

        private void PlayTap(InputAction.CallbackContext context)
        {
            EventManager.Instance.Publish(EEventType.OnPlayTap);
        }



        private void CheckSwitchActionMap(EGameState oldState, EGameState newState)
        {
            if (newState == EGameState.Playing)
            {
                playerInputs.MenuMap.Disable();
                playerInputs.PlayingMap.Enable();
            }
            else
            {                        
                playerInputs.PlayingMap.Disable();
                playerInputs.MenuMap.Enable();
            }
        }
    }

}
