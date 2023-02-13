# Selection Utility

## Description
The Nementic Selection Utility is a lightweight Unity tool which facilitates selecting GameObjects in the Scene view by displaying a context menu with all objects currently under the mouse cursor as a popup list.

## Setup
This tool has no dependencies other than the Unity editor itself and becomes available as soon as it is installed. The minimum required Unity version is 2018.4.0f1 (LTS). If you require the tool to work in an older version of Unity, please contact us at support@nementic.games.

## Usage
Right-click over GameObjects in the SceneView to show a popup list of all objects that can be selected. The list is sorted by depth from front to back and displays icons for each component on a selectable GameObject. Left-click an item in the list to select it.

For a simple search, type the name of a GameObject in the search field. Letter case and whitespace around words is ignored. For an advanced search, type 't:' followed by a simple type name to find only GameObjects with a specific component, e.g. 't:SpriteRenderer' will find all GameObjects that have a UnityEngine.SpriteRenderer component attached to them. A simple and advanced query can be combined by separating them with a space, e.g. 'Foreground_Tree t:SpriteRenderer'.

## Settings
Open the Unity Preferences window to find the Selection Utility settings. All settings are stored per user on the local machine.

Enabled: Turns the tool on and off globally.

Click Dead Zone: The selection popup opens when the right mouse button was clicked down and released over approximately the same position. The threshold defines the distance in pixels how far the mouse pointer may move before the open action is cancelled. Increase this value if you're experiences issues with opening the popup when using e.g. a touchpad.

Show Filter Toolbar: Shows filter buttons for 3D, 2D and UI.

Show Search Field: Turns the search field at the top of the popup list on or off.

Show Duplicate Icons: If the same component type is present on a GameObject, should its icon be drawn multiple times?

Show Missing Script Icons: If a script is missing (or cannot be recompiled for example), its icon is a blank file symbol.

Enable Hover Selection Outline: When enabled, a colored outline will be drawn around 3D and 2D (SpriteRenderer) objects. Only the builtin render pipeline is supported.

Hover Selection Outline Thickness: Increase this value to make the hover selection outline more prominent.

Hover Selection Outline Color: RGB values control the outline color. The alpha value controls the transparency of the opaque overlay on top of objects.

Hidden Icons: Some icons are repetitive and do not add information to the list of selectable GameObjects, for example, every GameObject has a Transform component. To hide such an icon from the popup list, add a new entry to the list of hidden icons and specify the simple type name for the component, e.g. 'MeshFilter' for the UnityEngine.MeshFilter component.

Use Defaults: Reverts all settings back to their default values.

Context-click on any setting: Opens the property context menu with the 'Reset' option to revert this property back to its default value.

## Extensions
It is possible to add custom tabs to the popup filter toolbar.
Assign SelectionPopupExtensions.FilterModifier during InitializeOnLoad (edit time)
or OnEnable (runtime) to change the default filter list. 
You can add, remove, replace or reorder filters in this manner.
See the provided sample scripts for more information: CustomExtension.cs and CustomRuntimeFilter.cs

## Support
If you encounter any issues, have questions or feedback, please get in touch: 
support@nementic.games
