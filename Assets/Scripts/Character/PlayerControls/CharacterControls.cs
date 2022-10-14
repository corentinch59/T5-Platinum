//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.4.2
//     from Assets/Scripts/Character/PlayerControls/CharacterControls.inputactions
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

public partial class @CharacterControls : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @CharacterControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""CharacterControls"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""f85837f0-ff07-42c3-ac9e-809ccf15e78c"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""67d3ff45-f782-42c8-84e1-6a819aeec05a"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""5ccefa9e-183a-41e9-b7aa-39a94f34fce7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Dash"",
                    ""type"": ""Button"",
                    ""id"": ""e7be9c4a-f55f-4234-89b6-ec47aeea6552"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""e98a4543-3f3e-460a-b9ab-397dc1ce87b6"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ce07c395-c795-4884-ba69-df72f0dabd08"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""dc01e3b7-8144-492d-ac27-d2bd521fc452"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""SelectionScreenControl"",
            ""id"": ""8a54639e-965d-4ebf-a512-0bca53662b6a"",
            ""actions"": [
                {
                    ""name"": ""ChangeLeft"",
                    ""type"": ""Button"",
                    ""id"": ""8e17399d-219d-4361-8e40-5d0cb8612fac"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""ChangeRight"",
                    ""type"": ""Button"",
                    ""id"": ""b1334139-407d-4107-86fc-9c849ba2cae9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Ready"",
                    ""type"": ""Button"",
                    ""id"": ""c3909571-cddd-4d00-a05f-e2dfc4ff82c8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""UnReady"",
                    ""type"": ""Button"",
                    ""id"": ""ef91c638-039c-45dc-a7c1-36e036ea2ef2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""73107d7d-f1a2-422f-aebf-8daa2e338e9f"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ChangeLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""db46a877-07cf-403a-92ef-003efa791ed0"",
                    ""path"": ""<Gamepad>/dpad/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ChangeLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3df8caad-e30b-4010-a868-c107f7f07c14"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ChangeRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""71642d6e-2557-4cc6-995b-798d6c672322"",
                    ""path"": ""<Gamepad>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ChangeRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6deda0c0-f84d-4038-ad14-7e865ad0e1ee"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Ready"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6fdfac58-daa3-457d-ad80-9b9ccfad9cc2"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""UnReady"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""PlayerProto3"",
            ""id"": ""aba416ab-22d4-4cd7-873b-ef7a19e0c3ae"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""c3f7bec8-09dd-46df-a3ad-86e7041874a0"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Action"",
                    ""type"": ""Value"",
                    ""id"": ""5da23c4d-cd08-4633-ab9c-e332688df955"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Hold(duration=1,pressPoint=0.1)"",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""4487a69f-99f4-4c5d-b913-8b06581aee78"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""942107ea-7548-4c06-979b-27ac32f16bae"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Action"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_Move = m_Player.FindAction("Move", throwIfNotFound: true);
        m_Player_Interact = m_Player.FindAction("Interact", throwIfNotFound: true);
        m_Player_Dash = m_Player.FindAction("Dash", throwIfNotFound: true);
        // SelectionScreenControl
        m_SelectionScreenControl = asset.FindActionMap("SelectionScreenControl", throwIfNotFound: true);
        m_SelectionScreenControl_ChangeLeft = m_SelectionScreenControl.FindAction("ChangeLeft", throwIfNotFound: true);
        m_SelectionScreenControl_ChangeRight = m_SelectionScreenControl.FindAction("ChangeRight", throwIfNotFound: true);
        m_SelectionScreenControl_Ready = m_SelectionScreenControl.FindAction("Ready", throwIfNotFound: true);
        m_SelectionScreenControl_UnReady = m_SelectionScreenControl.FindAction("UnReady", throwIfNotFound: true);
        // PlayerProto3
        m_PlayerProto3 = asset.FindActionMap("PlayerProto3", throwIfNotFound: true);
        m_PlayerProto3_Move = m_PlayerProto3.FindAction("Move", throwIfNotFound: true);
        m_PlayerProto3_Action = m_PlayerProto3.FindAction("Action", throwIfNotFound: true);
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

    // Player
    private readonly InputActionMap m_Player;
    private IPlayerActions m_PlayerActionsCallbackInterface;
    private readonly InputAction m_Player_Move;
    private readonly InputAction m_Player_Interact;
    private readonly InputAction m_Player_Dash;
    public struct PlayerActions
    {
        private @CharacterControls m_Wrapper;
        public PlayerActions(@CharacterControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Player_Move;
        public InputAction @Interact => m_Wrapper.m_Player_Interact;
        public InputAction @Dash => m_Wrapper.m_Player_Dash;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                @Interact.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInteract;
                @Interact.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInteract;
                @Interact.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnInteract;
                @Dash.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDash;
                @Dash.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDash;
                @Dash.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnDash;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Interact.started += instance.OnInteract;
                @Interact.performed += instance.OnInteract;
                @Interact.canceled += instance.OnInteract;
                @Dash.started += instance.OnDash;
                @Dash.performed += instance.OnDash;
                @Dash.canceled += instance.OnDash;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);

    // SelectionScreenControl
    private readonly InputActionMap m_SelectionScreenControl;
    private ISelectionScreenControlActions m_SelectionScreenControlActionsCallbackInterface;
    private readonly InputAction m_SelectionScreenControl_ChangeLeft;
    private readonly InputAction m_SelectionScreenControl_ChangeRight;
    private readonly InputAction m_SelectionScreenControl_Ready;
    private readonly InputAction m_SelectionScreenControl_UnReady;
    public struct SelectionScreenControlActions
    {
        private @CharacterControls m_Wrapper;
        public SelectionScreenControlActions(@CharacterControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @ChangeLeft => m_Wrapper.m_SelectionScreenControl_ChangeLeft;
        public InputAction @ChangeRight => m_Wrapper.m_SelectionScreenControl_ChangeRight;
        public InputAction @Ready => m_Wrapper.m_SelectionScreenControl_Ready;
        public InputAction @UnReady => m_Wrapper.m_SelectionScreenControl_UnReady;
        public InputActionMap Get() { return m_Wrapper.m_SelectionScreenControl; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(SelectionScreenControlActions set) { return set.Get(); }
        public void SetCallbacks(ISelectionScreenControlActions instance)
        {
            if (m_Wrapper.m_SelectionScreenControlActionsCallbackInterface != null)
            {
                @ChangeLeft.started -= m_Wrapper.m_SelectionScreenControlActionsCallbackInterface.OnChangeLeft;
                @ChangeLeft.performed -= m_Wrapper.m_SelectionScreenControlActionsCallbackInterface.OnChangeLeft;
                @ChangeLeft.canceled -= m_Wrapper.m_SelectionScreenControlActionsCallbackInterface.OnChangeLeft;
                @ChangeRight.started -= m_Wrapper.m_SelectionScreenControlActionsCallbackInterface.OnChangeRight;
                @ChangeRight.performed -= m_Wrapper.m_SelectionScreenControlActionsCallbackInterface.OnChangeRight;
                @ChangeRight.canceled -= m_Wrapper.m_SelectionScreenControlActionsCallbackInterface.OnChangeRight;
                @Ready.started -= m_Wrapper.m_SelectionScreenControlActionsCallbackInterface.OnReady;
                @Ready.performed -= m_Wrapper.m_SelectionScreenControlActionsCallbackInterface.OnReady;
                @Ready.canceled -= m_Wrapper.m_SelectionScreenControlActionsCallbackInterface.OnReady;
                @UnReady.started -= m_Wrapper.m_SelectionScreenControlActionsCallbackInterface.OnUnReady;
                @UnReady.performed -= m_Wrapper.m_SelectionScreenControlActionsCallbackInterface.OnUnReady;
                @UnReady.canceled -= m_Wrapper.m_SelectionScreenControlActionsCallbackInterface.OnUnReady;
            }
            m_Wrapper.m_SelectionScreenControlActionsCallbackInterface = instance;
            if (instance != null)
            {
                @ChangeLeft.started += instance.OnChangeLeft;
                @ChangeLeft.performed += instance.OnChangeLeft;
                @ChangeLeft.canceled += instance.OnChangeLeft;
                @ChangeRight.started += instance.OnChangeRight;
                @ChangeRight.performed += instance.OnChangeRight;
                @ChangeRight.canceled += instance.OnChangeRight;
                @Ready.started += instance.OnReady;
                @Ready.performed += instance.OnReady;
                @Ready.canceled += instance.OnReady;
                @UnReady.started += instance.OnUnReady;
                @UnReady.performed += instance.OnUnReady;
                @UnReady.canceled += instance.OnUnReady;
            }
        }
    }
    public SelectionScreenControlActions @SelectionScreenControl => new SelectionScreenControlActions(this);

    // PlayerProto3
    private readonly InputActionMap m_PlayerProto3;
    private IPlayerProto3Actions m_PlayerProto3ActionsCallbackInterface;
    private readonly InputAction m_PlayerProto3_Move;
    private readonly InputAction m_PlayerProto3_Action;
    public struct PlayerProto3Actions
    {
        private @CharacterControls m_Wrapper;
        public PlayerProto3Actions(@CharacterControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_PlayerProto3_Move;
        public InputAction @Action => m_Wrapper.m_PlayerProto3_Action;
        public InputActionMap Get() { return m_Wrapper.m_PlayerProto3; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerProto3Actions set) { return set.Get(); }
        public void SetCallbacks(IPlayerProto3Actions instance)
        {
            if (m_Wrapper.m_PlayerProto3ActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_PlayerProto3ActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_PlayerProto3ActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_PlayerProto3ActionsCallbackInterface.OnMove;
                @Action.started -= m_Wrapper.m_PlayerProto3ActionsCallbackInterface.OnAction;
                @Action.performed -= m_Wrapper.m_PlayerProto3ActionsCallbackInterface.OnAction;
                @Action.canceled -= m_Wrapper.m_PlayerProto3ActionsCallbackInterface.OnAction;
            }
            m_Wrapper.m_PlayerProto3ActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Action.started += instance.OnAction;
                @Action.performed += instance.OnAction;
                @Action.canceled += instance.OnAction;
            }
        }
    }
    public PlayerProto3Actions @PlayerProto3 => new PlayerProto3Actions(this);
    public interface IPlayerActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
        void OnDash(InputAction.CallbackContext context);
    }
    public interface ISelectionScreenControlActions
    {
        void OnChangeLeft(InputAction.CallbackContext context);
        void OnChangeRight(InputAction.CallbackContext context);
        void OnReady(InputAction.CallbackContext context);
        void OnUnReady(InputAction.CallbackContext context);
    }
    public interface IPlayerProto3Actions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnAction(InputAction.CallbackContext context);
    }
}
