﻿using UnityEngine;

public static class GridUtil {
    public static bool AddAndPosition(this GridObject gridObject, IGridManager gridManager, Vector2Int position, GridRotation rotation) {
        if (!gridManager.TryAdd(gridObject, position, rotation)) return false;
        gridObject.transform.position = gridManager.GridToWorld(position, gridObject.StaticSize, rotation);
        return true;
    }
}