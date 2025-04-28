using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Helper
{
    //PROBABLY MAKE MULTIPLE hELPERS

    private static readonly int INITIAL_RENDERER_PRIORITY = 10000;


    public static int GetSortingPriorityFromHexPosition(Vector3Int cellPosition)
    {
        //TODO here we can get camera like
        Camera camera = Camera.main;
        //Check rotation of camera, then the priority will change on x/z axis
        return INITIAL_RENDERER_PRIORITY - cellPosition.y;
    }


    public static Vector3 GetMousePosition(Camera mainCamera)
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10;
        mousePos = mainCamera.ScreenToWorldPoint(mousePos);
        return mousePos;
    }

    public static bool IsTileSelectable(SelectState selectState)
    {
        return selectState == SelectState.Grid || selectState == SelectState.Path;
    }

    public static bool IsUnitSelectable(SelectState selectState)
    {
        return selectState == SelectState.Moveable;
    }

    public static bool IsMoveableSelectable(SelectState selectState)
    {
        return selectState == SelectState.Moveable;
    }
}
