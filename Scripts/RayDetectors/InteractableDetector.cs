namespace PickupPlaceSystem
{
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractableDetector : MonoBehaviour
{
    public static InteractableDetector Instance { get; private set; }

    public static event Action<GameObject> OnInteractedWithItemEvent;

    private HashSet<GameObject> staredObjects = new HashSet<GameObject>();

    private bool _isInteractable = false;

    public bool isInteractable
    {
        get { return _isInteractable; }
        set
        {
            if (value != _isInteractable)
            {
                _isInteractable = value;
                UpdateReticleManager();
            }
        }
    }

    private string _interactableLabel = "Interact";

    public string interactableLabel
    {
        get { return _interactableLabel; }
        set
        {
            if (value != _interactableLabel)
            {
                _interactableLabel = value;
                UpdateReticleManager();
            }
        }
    }

    void Awake()
    {
        EnsureSingletonInstance();
    }

    void OnEnable()
    {
        // OnEnable - Subscribing to events
        CameraRayController.OnRaycastEvent += OnRaycastReceived;
        CameraRayController.OnInteractEvent += OnInteractReceived;
    }

    void OnDisable()
    {
        // OnDisable - Unsubscribing from events
        CameraRayController.OnRaycastEvent -= OnRaycastReceived;
        CameraRayController.OnInteractEvent -= OnInteractReceived;
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

    private void OnRaycastReceived(Ray ray)
    {
        DetectInteractable(ray);
    }

    private void OnInteractReceived(InputAction.CallbackContext context)
    {
        // Received interact input
        
        // Don't handle interaction if an object is currently being moved
        if (PlayerManager.Instance != null)
        {
            // Check PlayerManager inventory state
            if (PlayerManager.Instance.inventory != null)
            {
                // Ignoring input - object is being moved
                return;
            }
        }
        else
        {
            // PlayerManager.Instance is null
        }
        
        CheckInteract();
    }

    private void DetectInteractable(Ray ray)
    {
        if (!enabled) { return; }

        int layerMask = ~LayerMask.GetMask("Overlay", "Ignore Raycast");
        
        RaycastHit hitData;
        if (Physics.Raycast(ray, out hitData, CameraRayController.Instance.rayLength, layerMask))
        {
            GameObject hitObject = hitData.transform.gameObject;
            Interactable interactable = hitObject.GetComponent<Interactable>();


            interactableLabel = "Interact";

            if (!interactable?.isInteractable ?? true)
            {
                isInteractable = false;
                OnStareExit();
                return;
            }

            // Object is interactable
            isInteractable = true;

            if (!staredObjects.Contains(hitObject))
            {
                interactable?.OnStareEnter();
                staredObjects.Add(hitObject);
            }
            interactable?.OnStare();

            // Update ReticleManager
            ReticleManager.Instance.CurrentReticleState = ReticleManager.ReticleState.Interactive;
            ReticleManager.Instance.Label = interactableLabel;
        }
        else
        {
            OnStareExit();
            isInteractable = false;

            // Update ReticleManager to Normal state
            ReticleManager.Instance.CurrentReticleState = ReticleManager.ReticleState.Normal;
            ReticleManager.Instance.Label = "";
        }
    }

    private void OnStareExit()
    {
        if (!enabled) { return; }

        if (staredObjects.Count != 0)
        {
            foreach (var obj in staredObjects)
            {
                obj.GetComponent<Interactable>()?.OnStareExit();
            }
            staredObjects.Clear();
        }
    }

    private void CheckInteract()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hitData;

        int layerMask = ~LayerMask.GetMask("Overlay", "Ignore Raycast");

        if (Physics.Raycast(ray, out hitData, CameraRayController.Instance.rayLength, layerMask))
        {
            GameObject hitObject = hitData.transform.gameObject;
            Interactable interactable = hitObject.GetComponent<Interactable>();

            // Hit object during interact check

            if (interactable == null || !interactable.isInteractable) 
            { 
                // Cannot interact with object - no component or not interactable
                return; 
            }

            // Calling OnInteract() on object
            interactable?.OnInteract();
            OnInteractedWithItemEvent?.Invoke(hitObject);
        }
        else
        {
            // No object hit by raycast
        }
    }

    private void UpdateReticleManager()
    {
        if (isInteractable)
        {
            ReticleManager.Instance.CurrentReticleState = ReticleManager.ReticleState.Interactive;
            ReticleManager.Instance.Label = interactableLabel;
        }
        else
        {
            ReticleManager.Instance.CurrentReticleState = ReticleManager.ReticleState.Normal;
            ReticleManager.Instance.Label = "";
        }
    }
}}
