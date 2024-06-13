using UnityEngine;
using UnityEngine.InputSystem;
using RoadToAAA.ProjectClock.Utilities;

namespace RoadToAAA.ProjectClock.Managers
{
    public class InputManager : MonoBehaviour
    {
        private PlayerInputs playerInputs;
        private InputAction playTap;
        private InputAction menuTap;
        private InputAction menuSwipe;

        //remember to change initial value when completing this class
        private bool isInMenu = false;


        private void Awake()
        {
            playerInputs = new PlayerInputs();
            playTap = playerInputs.PlayingMap.PlayTap;
            menuTap = playerInputs.MenuMap.MapTap;
            menuSwipe = playerInputs.MenuMap.Swipe;
        }
        
        private void OnEnable()
        {            
            playerInputs.PlayingMap.Enable();
            playTap.started += PlayTap;
            menuTap.started += MenuTap;
        }

        private void OnDisable()
        {
            playTap.started -= PlayTap;
            playerInputs.PlayingMap.Disable();
            menuTap.started -= MenuTap;
        }

        private void PlayTap(InputAction.CallbackContext context)
        {
            EventManager.Instance.Publish(EEventType.OnPlayTap);
        }

        private void MenuTap(InputAction.CallbackContext context)
        {
            EventManager.Instance.Publish(EEventType.OnMenuTap);
        }



        private void SwitchActionMap()
        {
            if (isInMenu)
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
