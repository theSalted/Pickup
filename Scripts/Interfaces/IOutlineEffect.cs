using UnityEngine;

namespace PickupPlaceSystem
{
    /// <summary>
    /// Interface for outline effect implementations
    /// Allows the pickup system to work with any outline solution
    /// </summary>
    public interface IOutlineEffect
    {
        /// <summary>
        /// Show the outline effect
        /// </summary>
        void ShowOutline();
        
        /// <summary>
        /// Hide the outline effect
        /// </summary>
        void HideOutline();
        
        /// <summary>
        /// Set the outline color
        /// </summary>
        /// <param name="color">Color to set</param>
        void SetOutlineColor(Color color);
    }
}