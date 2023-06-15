# Changelog

## Version 2.0.0

This BIG update to Flexalon includes new layouts, interactions, bug fixes, and more!

### NEW FEATURES
 - Align Layout: Align all children to the parent on the specified axes
 - Shape Layout: Position children in a shape formation with a specified number of sides. Great for crowds and unit formations.
 - Flexalon Interactable: Add click and drag interactions which let users add, remove, and swap objects in layouts.
 - Random Modifier: Add to any layout to randomly modify the positions and rotations of the children.
 - Circle Layout: New 'Radius Type' option can modify the radius for each object or for each iteration around the circle.
 - Curve Layout: New Tangent Mode options can automatically generate smooth or corner tangents. New spacing option 'Evenly Connected' can evenly space curves that have connected start and end. New 'In With Roll' and 'Out With Roll' rotation options.

### BEHAVIOR CHANGES
 - Curve Layout's spacing mode 'Evenly' now places the first object at the start of the curve and the last object at the end of the curve.
 - Curve and Lerp animators now operate in world space, which makes it simpler to transition objects between layouts.
 - TransformUpdater interface now requires a PreUpdate method, which is called before layout starts updating transforms. This can be used to capture the current transform state.
 - A Flexible Layout that does not have Wrap checked will now use the full size of the layout when computing the fill size on both of the non-flex axes. When Wrap is checked the behavior is unchanged: the fill size on the wrap axis depends on the size of each line.
- Improved the behavior of a spiral layout with negative spacing.

### FIXES
 - Flexalon will now update automatically when a new layout component is added.
 - Fixed an issue where Flexalon would sometimes update on recompile.
 - Multi-editing objects previously updated Flexalon once for each object. Now it only happens once.
 - Fixed cases where Flexalon Constraint prevented using the Unity transform control.
 - Fixed the layout size calculation of grid layout with hexagonal cell type.
 - Fixed some cases where the Flexalon Object bounding box visual appears in the wrong place.
 - Fixed NaN errors when a curve layout has two points in the same position.
 - Fixed some instances where Flexalon Result component appears in the inspector (it should always be hidden).
 - Readme asset will only be selected the first time Flexalon is imported.

## Version 1.0.2
  - Spiral 'Use Height' property replaced with flex-like behavior.
  - New 'Spiral Spacing' property adds vertical gaps between spiral objects.
  - Improved ability to modify objects with the standard transform tool.
  - Fixed various Undo / Redo bugs.
  - FlexalonObject's offset, rotation, and scale no longer apply when an object is not in a a layout or constraint.

## Version 1.0.1
  - FlexalonConstraint now supports margins.
  - Add help links to Flexalon website documentation.
  - Hide Documentation from Unity by renaming directory to Documentation~
  - Prevent adding multiple layout components to a GameObject.
  - Prevent setting grid rows or columns to 0.

## Version 1.0.0 - Initial Release!