using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using RoadToAAA.ProjectClock.Core;

namespace RoadToAAA.ProjectClock.Manager
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
        }

        private void PlayTap(InputAction.CallbackContext context)
        {
            Core.EventManager.Instance.Publish(Core.EventType.OnPlayerTapped);
        }

        private void MenuTap(InputAction.CallbackContext context)
        {
        
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
