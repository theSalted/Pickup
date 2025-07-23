using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PickupPlaceSystem
{
    public class CameraRayController : MonoBehaviour
    {
        public static CameraRayController Instance { get; private set; }

        [Header("Ray Settings")]
        public InputActionAsset inputActions;
        public string interactActionName = "Interact";
        public float rayLength = 5f;

        private GameObject player;
        private InputAction interact;

    // Events
    public static event Action<Ray> OnRaycastEvent;
    public static event Action<InputAction.CallbackContext> OnInteractEvent;

    void Awake()
    {
        // Awake() called
        EnsureSingletonInstance();
        player = GameObject.FindGameObjectWithTag("Player");
        
        if (player == null)
        {
            // No GameObject with 'Player' tag found
        }
        else
        {
            // Found player GameObject
        }
        
        if (inputActions != null)
        {
            interact = inputActions.FindAction(interactActionName);
            if (interact != null)
            {
                // Found interact action
            }
            else
            {
                // Could not find interact action in Input Actions asset
            }
        }
        else
        {
            // Input Actions asset not assigned
        }
    }

    void OnEnable()
    {
        if (interact != null)
        {
            // OnEnable: Enabling interact action and subscribing to callback
            interact.Enable();
            interact.performed += OnInteractCallback;
            interact.started += OnInteractStarted;
            interact.canceled += OnInteractCanceled;
        }
        else
        {
            // OnEnable: interact action is null
        }
    }

    void OnDisable()
    {
        if (interact != null)
        {
            interact.performed -= OnInteractCallback;
            interact.started -= OnInteractStarted;
            interact.canceled -= OnInteractCanceled;
            interact.Disable();
        }
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        OnRaycastEvent?.Invoke(ray); // Emit the raycast event
        Debug.DrawRay(transform.position, transform.forward * rayLength, Color.red);
        
        // Debug: Log subscriber count only occasionally to avoid spam
        if (OnRaycastEvent != null && Time.frameCount % 60 == 0)
        {
            // Broadcasting ray to subscribers
        }
    }

    public void OnInteractCallback(InputAction.CallbackContext context)
    {
        // OnInteractCallback: Interact button pressed
        
        if (OnInteractEvent != null)
        {
            // OnInteractCallback: Broadcasting to interact subscribers
            OnInteractEvent?.Invoke(context);
        }
        else
        {
            // OnInteractCallback: No subscribers to OnInteractEvent
        }
    }

    public void OnInteractStarted(InputAction.CallbackContext context)
    {
        // OnInteractStarted: Interact button started
    }

    public void OnInteractCanceled(InputAction.CallbackContext context)
    {
        // OnInteractCanceled: Interact button canceled
        
        // Workaround: If the input action is configured as Hold type, trigger interaction on release
        if (OnInteractEvent != null)
        {
            // OnInteractCanceled: Broadcasting to interact subscribers
            OnInteractEvent?.Invoke(context);
        }
        else
        {
            // OnInteractCanceled: No subscribers to OnInteractEvent - may be normal during transitions
        }
    }

    private void EnsureSingletonInstance()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            // Multiple instances detected - destroying duplicate
            Destroy(gameObject);
        }
    }
    }
}