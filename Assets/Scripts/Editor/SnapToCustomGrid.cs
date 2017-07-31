using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class SnapToCustomGrid
{
    private static GridSettings _gridSettings;
    
    [MenuItem("Tools/Snap to custom grid %g")]
    private static void Snap()
    {
        _gridSettings = _gridSettings != null ? _gridSettings : Resources.Load<GridSettings>("Grid Settings");
        
        var selectedGameObjects = Selection.gameObjects;
        foreach (var each in selectedGameObjects)
        {
            var position = each.transform.position;
            position.x -= position.x % _gridSettings.CellSize;
            position.y -= position.y % _gridSettings.CellSize;

            each.transform.position = position;
        }
    }
}