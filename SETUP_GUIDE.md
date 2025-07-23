# Setup Guide - Pickup & Place System

## Prerequisites

1. Unity 2020.3 or newer
2. Unity's New Input System installed
3. An outline effect solution (e.g., QuickOutline from Asset Store)

## Step-by-Step Setup

### 1. Import the Package

- Copy the entire `PickupPlaceSystem` folder to your Unity project's Assets folder
- Unity will compile the scripts automatically

### 2. Configure Project Layers

Add these layers to your project (Edit → Project Settings → Tags and Layers):
- `Overlay` (for carried objects)
- `Ignore Raycast` (if not already present)

### 3. Set Up Input Actions

Create or modify your Input Actions asset:

1. Create an Action Map called "Player"
2. Add an Action called "Interact" 
3. Set binding to Left Mouse Button (or preferred key)
4. Set Action Type to "Button"

### 4. Player Setup

On your Player GameObject:

1. **On the Main Player Object:**
   - Add `PlayerManager` component
   - Add `ReticleManager` component

2. **On the Player's Camera:**
   - Add `CameraRayController` component
   - Add `InteractableDetector` component
   - Add `MovableDetector` component
   - Add `SnapableDetector` component (optional)

3. **Configure Components:**
   - On `InteractableDetector`: Set `maxDistance` (default: 10f)
   - On `ReticleManager`: Assign your reticle UI elements

### 5. Create Pickupable Objects

For each object you want to be pickupable:

1. Add a Collider component
2. Add one of these components:
   - `PickUp` - For basic pickupable objects
   - `Extractable` - For objects that need to be "pulled out" first
   - Or extend `Movable` for custom behavior

### 6. UI Setup

Create a simple reticle:
1. Create a Canvas (if you don't have one)
2. Add an Image as child of Canvas
3. Set to center of screen
4. Assign to `ReticleManager.reticle`

### 7. Handle Outline Effects

Since the outline system depends on your chosen solution:

1. Open `Interactable.cs`
2. Find the `ShowOutline()` and `HideOutline()` methods
3. Replace with your outline system's API calls

Example for QuickOutline:
```csharp
public virtual void ShowOutline()
{
    var outline = GetComponent<Outline>();
    if (outline != null)
        outline.enabled = true;
}
```

## Testing Your Setup

1. Place a cube in your scene
2. Add `PickUp` component to it
3. Play the scene
4. Look at the cube - it should show an outline
5. Click to pick it up
6. Click again to place it down

## Common Issues

**No outline appears:**
- Check that your outline system is properly integrated
- Verify the object has the outline component

**Can't pick up objects:**
- Check Input System is configured correctly
- Verify PlayerManager and detectors are set up
- Ensure object has a Collider

**Objects fall through surfaces:**
- Add Rigidbody to pickupable objects
- Ensure placement surfaces have colliders

## Next Steps

- Customize the visual feedback
- Adjust interaction distances
- Create snap points for precise placement
- Extend Movable class for special object behaviors