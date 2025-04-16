using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

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

    public static List<Unit> GetLivingUnits(List<Unit> units)
    {
        if(units == null)
        {
            return new List<Unit>();
        }
        return units.Where(unit => unit != null && unit.IsAlive(false)).ToList();
    }

    public static Vector3 GetMousePosition(Camera mainCamera)
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10;
        mousePos = mainCamera.ScreenToWorldPoint(mousePos);
        return mousePos;
    }

    public static IEnumerator MoveToPosition(UnityEngine.Transform transformToMove, Vector3 targetPosition, float duration)
    {
        Vector3 startPosition = transformToMove.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            transformToMove.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the position is exactly the target position at the end
        transformToMove.position = targetPosition;
    }

    public static Vector3Int GetSourceDirection(Vector3Int source, Vector3Int target)
    {
        // Get the difference between source and target
        Vector3Int difference = target - source;

        Vector3Int normalizedDirection = NormalizeHexDirection(difference);

        return normalizedDirection; // Return the closest match (even if it's not an exact match)
    }

    public static Vector3Int GetTargetDirection(Vector3Int source, Vector3Int target)
    {
        // Get the difference (this is essentially reversing the source and target)
        return GetSourceDirection(target, source);
    }

    private static Vector3Int NormalizeHexDirection(Vector3Int difference)
    {
        // A list of all possible hex directions
        Vector3Int[] possibleDirections = HexDirectionVectors.ToArray();

        // Find the most appropriate direction by comparing the difference with each possible direction
        Vector3Int bestDirection = Vector3Int.zero;
        int minDotProduct = int.MaxValue;

        foreach (Vector3Int dir in possibleDirections)
        {
            // Calculate the dot product between the difference and each direction
            int dotProduct = DotProduct(difference, dir);

            // We want the direction that is closest to the difference, which corresponds to the highest dot product
            if (dotProduct > minDotProduct)
            {
                minDotProduct = dotProduct;
                bestDirection = dir;
            }
        }

        return bestDirection;
    }
    private static int DotProduct(Vector3Int a, Vector3Int b)
    {
        return a.x * b.x + a.y * b.y + a.z * b.z;
    }

    private static readonly List<Vector3Int> HexDirectionVectors = new List<Vector3Int>
    {
        new (1, 0, -1),  // Right
        new (1, -1, 0),  // Bottom-Right
        new (0, -1, 1),  // Bottom-Left
        new (-1, 0, 1),  // Left
        new (-1, 1, 0),  // Top-Left
        new (0, 1, -1)   // Top-Right
    };

    public static bool IsActive(GameObject gameObject)
    {
        return gameObject != null && gameObject.activeSelf;
    }

}
