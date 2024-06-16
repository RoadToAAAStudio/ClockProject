//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/Scripts/PlayerInputs.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @PlayerInputs: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInputs()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInputs"",
    ""maps"": [
        {
            ""name"": ""PlayingMap"",
            ""id"": ""82a9036e-6a5b-41b3-896f-4932d8bd02a9"",
            ""actions"": [
                {
                    ""name"": ""PlayTap"",
                    ""type"": ""Button"",
                    ""id"": ""ae0d6c6f-1bda-4a56-82b9-ebf0f2a67287"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""4eb4a27f-4765-466e-aff9-59b30ca9176a"",
                    ""path"": ""<Touchscreen>/primaryTouch/press"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PlayTap"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""MenuMap"",
            ""id"": ""38f2be8f-63bb-409b-ab2a-016b16ef91c4"",
            ""actions"": [
                {
                    ""name"": ""Swipe"",
                    ""type"": ""Value"",
                    ""id"": ""2bffecdd-deb3-4c22-b2bc-a4c8314e7653"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""ced3a7ce-3bf4-4d60-af73-5732b0cca9c6"",
                    ""path"": ""<Touchscreen>/touch0/delta/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Swipe"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // PlayingMap
        m_PlayingMap = asset.FindActionMap("PlayingMap", throwIfNotFound: true);
        m_PlayingMap_PlayTap = m_PlayingMap.FindAction("PlayTap", throwIfNotFound: true);
        // MenuMap
        m_MenuMap = asset.FindActionMap("MenuMap", throwIfNotFound: true);
        m_MenuMap_Swipe = m_MenuMap.FindAction("Swipe", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // PlayingMap
    private readonly InputActionMap m_PlayingMap;
    private List<IPlayingMapActions> m_PlayingMapActionsCallbackInterfaces = new List<IPlayingMapActions>();
    private readonly InputAction m_PlayingMap_PlayTap;
    public struct PlayingMapActions
    {
        private @PlayerInputs m_Wrapper;
        public PlayingMapActions(@PlayerInputs wrapper) { m_Wrapper = wrapper; }
        public InputAction @PlayTap => m_Wrapper.m_PlayingMap_PlayTap;
        public InputActionMap Get() { return m_Wrapper.m_PlayingMap; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayingMapActions set) { return set.Get(); }
        public void AddCallbacks(IPlayingMapActions instance)
        {
            if (instance == null || m_Wrapper.m_PlayingMapActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_PlayingMapActionsCallbackInterfaces.Add(instance);
            @PlayTap.started += instance.OnPlayTap;
            @PlayTap.performed += instance.OnPlayTap;
            @PlayTap.canceled += instance.OnPlayTap;
        }

        private void UnregisterCallbacks(IPlayingMapActions instance)
        {
            @PlayTap.started -= instance.OnPlayTap;
            @PlayTap.performed -= instance.OnPlayTap;
            @PlayTap.canceled -= instance.OnPlayTap;
        }

        public void RemoveCallbacks(IPlayingMapActions instance)
        {
            if (m_Wrapper.m_PlayingMapActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IPlayingMapActions instance)
        {
            foreach (var item in m_Wrapper.m_PlayingMapActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_PlayingMapActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public PlayingMapActions @PlayingMap => new PlayingMapActions(this);

    // MenuMap
    private readonly InputActionMap m_MenuMap;
    private List<IMenuMapActions> m_MenuMapActionsCallbackInterfaces = new List<IMenuMapActions>();
    private readonly InputAction m_MenuMap_Swipe;
    public struct MenuMapActions
    {
        private @PlayerInputs m_Wrapper;
        public MenuMapActions(@PlayerInputs wrapper) { m_Wrapper = wrapper; }
        public InputAction @Swipe => m_Wrapper.m_MenuMap_Swipe;
        public InputActionMap Get() { return m_Wrapper.m_MenuMap; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MenuMapActions set) { return set.Get(); }
        public void AddCallbacks(IMenuMapActions instance)
        {
            if (instance == null || m_Wrapper.m_MenuMapActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_MenuMapActionsCallbackInterfaces.Add(instance);
            @Swipe.started += instance.OnSwipe;
            @Swipe.performed += instance.OnSwipe;
            @Swipe.canceled += instance.OnSwipe;
        }

        private void UnregisterCallbacks(IMenuMapActions instance)
        {
            @Swipe.started -= instance.OnSwipe;
            @Swipe.performed -= instance.OnSwipe;
            @Swipe.canceled -= instance.OnSwipe;
        }

        public void RemoveCallbacks(IMenuMapActions instance)
        {
            if (m_Wrapper.m_MenuMapActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IMenuMapActions instance)
        {
            foreach (var item in m_Wrapper.m_MenuMapActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_MenuMapActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public MenuMapActions @MenuMap => new MenuMapActions(this);
    public interface IPlayingMapActions
    {
        void OnPlayTap(InputAction.CallbackContext context);
    }
    public interface IMenuMapActions
    {
        void OnSwipe(InputAction.CallbackContext context);
    }
}
