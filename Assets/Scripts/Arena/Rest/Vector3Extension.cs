using UnityEngine;

public static class Vector3Extension
{
    public static Vector3 MousePosition()
    {
        var mouse = Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
        mouse.z = 0f;
        return mouse;
    }

    public static Vector3 CalculateDirectionTowardsMouse(Vector3 playerPosition)
    {
        var mouse = Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
        mouse.z = 0f;
        var direction = playerPosition - mouse;
        direction.Normalize();
        return -direction;
    }

    public static float DistanceBetweenPlayerMouse(Vector3 player, Vector3 mouse)
    { 
        var distance = Vector3.Distance(player, mouse);
        return distance;
    }

}
