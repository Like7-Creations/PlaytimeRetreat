//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.3.0
//     from Assets/Scripts/MechanicsControl.inputactions
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

public partial class @MechanicsControl : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @MechanicsControl()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""MechanicsControl"",
    ""maps"": [
        {
            ""name"": ""Pickup&Drop/Throw"",
            ""id"": ""5a512146-4f9b-4683-9d4c-addbe41e86a1"",
            ""actions"": [
                {
                    ""name"": ""PickUp"",
                    ""type"": ""Value"",
                    ""id"": ""e83ac8a0-e50f-48e0-818d-dce97256171a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Charge"",
                    ""type"": ""Value"",
                    ""id"": ""5842b043-9761-47ef-b428-0570d00f43fe"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Throw"",
                    ""type"": ""Value"",
                    ""id"": ""91bd655b-2630-4084-8f7d-0adec49f3251"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""d88fb6df-8d57-42a8-8922-5509c4bb8053"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": ""Tap"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PickUp"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7a404322-6873-4c40-8073-838a0b943509"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": ""Hold"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Charge"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7bf02dcf-eb8d-4345-a912-b25bb47dba6f"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": ""Press(behavior=1)"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Throw"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Trigger"",
            ""id"": ""d620e070-d637-4320-8f72-7e2492d330f3"",
            ""actions"": [
                {
                    ""name"": ""InteractButton"",
                    ""type"": ""Value"",
                    ""id"": ""63439c93-e8ea-4050-ad90-89cd7ce56814"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""InteractTimedButton"",
                    ""type"": ""Value"",
                    ""id"": ""1705e828-7c8e-4418-89ec-87879fd7656d"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""InteractLever"",
                    ""type"": ""Value"",
                    ""id"": ""7b6738fc-fcda-455f-ad26-d6e6be78dce2"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""UseButtons"",
                    ""type"": ""Button"",
                    ""id"": ""d18916de-bdff-41b8-acfa-dfb203fd5fe4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""UseLever"",
                    ""type"": ""Button"",
                    ""id"": ""bc6a99b9-3678-4b7f-95f5-680e8dabd8be"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""b8691f12-e1b7-4965-b3e8-1d4d74f0b5be"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""InteractButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""aaa6847d-4477-4a82-a1b3-d343a1c76df5"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""InteractTimedButton"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2a2c9509-9907-4b63-b811-ba47b738911d"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": ""Press(behavior=2)"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""InteractLever"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""04ce3a80-33ad-4806-8169-c7e7fd882a4c"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""UseButtons"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9e9e25d4-ed07-4c06-8f0d-8ebafd723bb5"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": ""Press(behavior=2)"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""UseLever"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Pickup&Drop/Throw
        m_PickupDropThrow = asset.FindActionMap("Pickup&Drop/Throw", throwIfNotFound: true);
        m_PickupDropThrow_PickUp = m_PickupDropThrow.FindAction("PickUp", throwIfNotFound: true);
        m_PickupDropThrow_Charge = m_PickupDropThrow.FindAction("Charge", throwIfNotFound: true);
        m_PickupDropThrow_Throw = m_PickupDropThrow.FindAction("Throw", throwIfNotFound: true);
        // Trigger
        m_Trigger = asset.FindActionMap("Trigger", throwIfNotFound: true);
        m_Trigger_InteractButton = m_Trigger.FindAction("InteractButton", throwIfNotFound: true);
        m_Trigger_InteractTimedButton = m_Trigger.FindAction("InteractTimedButton", throwIfNotFound: true);
        m_Trigger_InteractLever = m_Trigger.FindAction("InteractLever", throwIfNotFound: true);
        m_Trigger_UseButtons = m_Trigger.FindAction("UseButtons", throwIfNotFound: true);
        m_Trigger_UseLever = m_Trigger.FindAction("UseLever", throwIfNotFound: true);
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

    // Pickup&Drop/Throw
    private readonly InputActionMap m_PickupDropThrow;
    private IPickupDropThrowActions m_PickupDropThrowActionsCallbackInterface;
    private readonly InputAction m_PickupDropThrow_PickUp;
    private readonly InputAction m_PickupDropThrow_Charge;
    private readonly InputAction m_PickupDropThrow_Throw;
    public struct PickupDropThrowActions
    {
        private @MechanicsControl m_Wrapper;
        public PickupDropThrowActions(@MechanicsControl wrapper) { m_Wrapper = wrapper; }
        public InputAction @PickUp => m_Wrapper.m_PickupDropThrow_PickUp;
        public InputAction @Charge => m_Wrapper.m_PickupDropThrow_Charge;
        public InputAction @Throw => m_Wrapper.m_PickupDropThrow_Throw;
        public InputActionMap Get() { return m_Wrapper.m_PickupDropThrow; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PickupDropThrowActions set) { return set.Get(); }
        public void SetCallbacks(IPickupDropThrowActions instance)
        {
            if (m_Wrapper.m_PickupDropThrowActionsCallbackInterface != null)
            {
                @PickUp.started -= m_Wrapper.m_PickupDropThrowActionsCallbackInterface.OnPickUp;
                @PickUp.performed -= m_Wrapper.m_PickupDropThrowActionsCallbackInterface.OnPickUp;
                @PickUp.canceled -= m_Wrapper.m_PickupDropThrowActionsCallbackInterface.OnPickUp;
                @Charge.started -= m_Wrapper.m_PickupDropThrowActionsCallbackInterface.OnCharge;
                @Charge.performed -= m_Wrapper.m_PickupDropThrowActionsCallbackInterface.OnCharge;
                @Charge.canceled -= m_Wrapper.m_PickupDropThrowActionsCallbackInterface.OnCharge;
                @Throw.started -= m_Wrapper.m_PickupDropThrowActionsCallbackInterface.OnThrow;
                @Throw.performed -= m_Wrapper.m_PickupDropThrowActionsCallbackInterface.OnThrow;
                @Throw.canceled -= m_Wrapper.m_PickupDropThrowActionsCallbackInterface.OnThrow;
            }
            m_Wrapper.m_PickupDropThrowActionsCallbackInterface = instance;
            if (instance != null)
            {
                @PickUp.started += instance.OnPickUp;
                @PickUp.performed += instance.OnPickUp;
                @PickUp.canceled += instance.OnPickUp;
                @Charge.started += instance.OnCharge;
                @Charge.performed += instance.OnCharge;
                @Charge.canceled += instance.OnCharge;
                @Throw.started += instance.OnThrow;
                @Throw.performed += instance.OnThrow;
                @Throw.canceled += instance.OnThrow;
            }
        }
    }
    public PickupDropThrowActions @PickupDropThrow => new PickupDropThrowActions(this);

    // Trigger
    private readonly InputActionMap m_Trigger;
    private ITriggerActions m_TriggerActionsCallbackInterface;
    private readonly InputAction m_Trigger_InteractButton;
    private readonly InputAction m_Trigger_InteractTimedButton;
    private readonly InputAction m_Trigger_InteractLever;
    private readonly InputAction m_Trigger_UseButtons;
    private readonly InputAction m_Trigger_UseLever;
    public struct TriggerActions
    {
        private @MechanicsControl m_Wrapper;
        public TriggerActions(@MechanicsControl wrapper) { m_Wrapper = wrapper; }
        public InputAction @InteractButton => m_Wrapper.m_Trigger_InteractButton;
        public InputAction @InteractTimedButton => m_Wrapper.m_Trigger_InteractTimedButton;
        public InputAction @InteractLever => m_Wrapper.m_Trigger_InteractLever;
        public InputAction @UseButtons => m_Wrapper.m_Trigger_UseButtons;
        public InputAction @UseLever => m_Wrapper.m_Trigger_UseLever;
        public InputActionMap Get() { return m_Wrapper.m_Trigger; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(TriggerActions set) { return set.Get(); }
        public void SetCallbacks(ITriggerActions instance)
        {
            if (m_Wrapper.m_TriggerActionsCallbackInterface != null)
            {
                @InteractButton.started -= m_Wrapper.m_TriggerActionsCallbackInterface.OnInteractButton;
                @InteractButton.performed -= m_Wrapper.m_TriggerActionsCallbackInterface.OnInteractButton;
                @InteractButton.canceled -= m_Wrapper.m_TriggerActionsCallbackInterface.OnInteractButton;
                @InteractTimedButton.started -= m_Wrapper.m_TriggerActionsCallbackInterface.OnInteractTimedButton;
                @InteractTimedButton.performed -= m_Wrapper.m_TriggerActionsCallbackInterface.OnInteractTimedButton;
                @InteractTimedButton.canceled -= m_Wrapper.m_TriggerActionsCallbackInterface.OnInteractTimedButton;
                @InteractLever.started -= m_Wrapper.m_TriggerActionsCallbackInterface.OnInteractLever;
                @InteractLever.performed -= m_Wrapper.m_TriggerActionsCallbackInterface.OnInteractLever;
                @InteractLever.canceled -= m_Wrapper.m_TriggerActionsCallbackInterface.OnInteractLever;
                @UseButtons.started -= m_Wrapper.m_TriggerActionsCallbackInterface.OnUseButtons;
                @UseButtons.performed -= m_Wrapper.m_TriggerActionsCallbackInterface.OnUseButtons;
                @UseButtons.canceled -= m_Wrapper.m_TriggerActionsCallbackInterface.OnUseButtons;
                @UseLever.started -= m_Wrapper.m_TriggerActionsCallbackInterface.OnUseLever;
                @UseLever.performed -= m_Wrapper.m_TriggerActionsCallbackInterface.OnUseLever;
                @UseLever.canceled -= m_Wrapper.m_TriggerActionsCallbackInterface.OnUseLever;
            }
            m_Wrapper.m_TriggerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @InteractButton.started += instance.OnInteractButton;
                @InteractButton.performed += instance.OnInteractButton;
                @InteractButton.canceled += instance.OnInteractButton;
                @InteractTimedButton.started += instance.OnInteractTimedButton;
                @InteractTimedButton.performed += instance.OnInteractTimedButton;
                @InteractTimedButton.canceled += instance.OnInteractTimedButton;
                @InteractLever.started += instance.OnInteractLever;
                @InteractLever.performed += instance.OnInteractLever;
                @InteractLever.canceled += instance.OnInteractLever;
                @UseButtons.started += instance.OnUseButtons;
                @UseButtons.performed += instance.OnUseButtons;
                @UseButtons.canceled += instance.OnUseButtons;
                @UseLever.started += instance.OnUseLever;
                @UseLever.performed += instance.OnUseLever;
                @UseLever.canceled += instance.OnUseLever;
            }
        }
    }
    public TriggerActions @Trigger => new TriggerActions(this);
    public interface IPickupDropThrowActions
    {
        void OnPickUp(InputAction.CallbackContext context);
        void OnCharge(InputAction.CallbackContext context);
        void OnThrow(InputAction.CallbackContext context);
    }
    public interface ITriggerActions
    {
        void OnInteractButton(InputAction.CallbackContext context);
        void OnInteractTimedButton(InputAction.CallbackContext context);
        void OnInteractLever(InputAction.CallbackContext context);
        void OnUseButtons(InputAction.CallbackContext context);
        void OnUseLever(InputAction.CallbackContext context);
    }
}
