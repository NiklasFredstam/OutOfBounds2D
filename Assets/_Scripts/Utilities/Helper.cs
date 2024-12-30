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

    public static int TransformStatPercentage(int stat, float percentageChange)
    {
        return Mathf.FloorToInt(stat * percentageChange);
    }

    //If lower than one return 1
    public static int TransformStatFlat(int stat, int change)
    {
        int result = stat - change;
        return result < 1 ? 1 : result;

    }

}
