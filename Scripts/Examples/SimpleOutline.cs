using UnityEngine;

namespace PickupPlaceSystem.Examples
{
    /// <summary>
    /// A simple example implementation of IOutlineEffect using material color changes
    /// This is a basic implementation - replace with your preferred outline solution
    /// </summary>
    public class SimpleOutline : MonoBehaviour, IOutlineEffect
    {
        [Header("Simple Outline Settings")]
        public Color outlineColor = Color.white;
        public float outlineIntensity = 2f;
        
        private Material[] originalMaterials;
        private Material[] outlineMaterials;
        private Renderer objectRenderer;
        private bool isOutlineVisible = false;
        
        void Awake()
        {
            objectRenderer = GetComponent<Renderer>();
            if (objectRenderer != null)
            {
                SetupMaterials();
            }
            else
            {
                Debug.LogWarning($"SimpleOutline: No Renderer found on {gameObject.name}");
            }
        }
        
        void SetupMaterials()
        {
            originalMaterials = objectRenderer.materials;
            outlineMaterials = new Material[originalMaterials.Length];
            
            for (int i = 0; i < originalMaterials.Length; i++)
            {
                // Create a copy of the original material
                outlineMaterials[i] = new Material(originalMaterials[i]);
                
                // Enable emission to create a glow effect
                if (outlineMaterials[i].HasProperty("_EmissionColor"))
                {
                    outlineMaterials[i].EnableKeyword("_EMISSION");
                }
            }
        }
        
        public void ShowOutline()
        {
            if (objectRenderer == null || isOutlineVisible) return;
            
            // Apply outline effect by modifying emission
            for (int i = 0; i < outlineMaterials.Length; i++)
            {
                if (outlineMaterials[i].HasProperty("_EmissionColor"))
                {
                    outlineMaterials[i].SetColor("_EmissionColor", outlineColor * outlineIntensity);
                }
            }
            
            objectRenderer.materials = outlineMaterials;
            isOutlineVisible = true;
        }
        
        public void HideOutline()
        {
            if (objectRenderer == null || !isOutlineVisible) return;
            
            objectRenderer.materials = originalMaterials;
            isOutlineVisible = false;
        }
        
        public void SetOutlineColor(Color color)
        {
            outlineColor = color;
            
            // If outline is currently visible, update it
            if (isOutlineVisible)
            {
                ShowOutline();
            }
        }
        
        void OnDestroy()
        {
            // Clean up created materials
            if (outlineMaterials != null)
            {
                for (int i = 0; i < outlineMaterials.Length; i++)
                {
                    if (outlineMaterials[i] != null)
                    {
                        DestroyImmediate(outlineMaterials[i]);
                    }
                }
            }
        }
    }
}