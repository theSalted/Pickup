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
        EnsureSingletonInstance();
        player = GameObject.FindGameObjectWithTag("Player");
        
        if (inputActions != null)
        {
            interact = inputActions.FindAction(interactActionName);
        }
    }

    void OnEnable()
    {
        if (interact != null)
        {
            interact.Enable();
            interact.performed += OnInteractCallback;
        }
    }

    void OnDisable()
    {
        if (interact != null)
        {
            interact.performed -= OnInteractCallback;
            interact.Disable();
        }
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        OnRaycastEvent?.Invoke(ray); // Emit the raycast event
        Debug.DrawRay(transform.position, transform.forward * rayLength, Color.red);
    }

    public void OnInteractCallback(InputAction.CallbackContext context)
    {
        OnInteractEvent?.Invoke(context);
    }

    private void EnsureSingletonInstance()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Debug.LogError("Multiple instances of CameraRayController detected. Destroying duplicate.");
            Destroy(gameObject);
        }
    }
    }
}