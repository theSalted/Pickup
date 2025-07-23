using UnityEngine;

namespace PickupPlaceSystem.Examples
{
    /// <summary>
    /// Adapter for QuickOutline package - uncomment and use if you have QuickOutline installed
    /// This bridges the IOutlineEffect interface with the QuickOutline component
    /// </summary>
    public class QuickOutlineAdapter : MonoBehaviour, IOutlineEffect
    {
        /*
        // Uncomment this section if you have QuickOutline installed
        
        private Outline quickOutline;
        
        void Awake()
        {
            quickOutline = GetComponent<Outline>();
            if (quickOutline == null)
            {
                quickOutline = gameObject.AddComponent<Outline>();
            }
        }
        
        public void ShowOutline()
        {
            if (quickOutline != null)
                quickOutline.enabled = true;
        }
        
        public void HideOutline()
        {
            if (quickOutline != null)
                quickOutline.enabled = false;
        }
        
        public void SetOutlineColor(Color color)
        {
            if (quickOutline != null)
                quickOutline.OutlineColor = color;
        }
        */
        
        // Placeholder implementation - remove when uncommenting above
        public void ShowOutline()
        {
            Debug.LogWarning("QuickOutlineAdapter: Please uncomment the implementation section after installing QuickOutline");
        }
        
        public void HideOutline()
        {
            Debug.LogWarning("QuickOutlineAdapter: Please uncomment the implementation section after installing QuickOutline");
        }
        
        public void SetOutlineColor(Color color)
        {
            Debug.LogWarning("QuickOutlineAdapter: Please uncomment the implementation section after installing QuickOutline");
        }
    }
}