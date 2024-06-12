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
        private InputAction swipe;


        private void Awake()
        {
            playerInputs = new PlayerInputs();
            playTap = playerInputs.PlayingMap.PlayTap;
            menuTap = playerInputs.MenuMap.MapTap;
            swipe = playerInputs.MenuMap.Swipe;
        }
        
        private void OnEnable()
        {
            playerInputs.MenuMap.Disable();
            playerInputs.PlayingMap.Enable();       
        }

        private void OnDisable()
        {
            playerInputs.MenuMap.Disable();
            playerInputs.PlayingMap.Disable();
        }


        private void PlayTap(InputAction.CallbackContext context)
        {
            Core.EventManager.Instance.Publish(Core.EventType.OnPlayerTapped);
        }
    }
}
