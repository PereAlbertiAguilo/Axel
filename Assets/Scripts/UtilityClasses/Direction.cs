using UnityEngine;

public class Direction
{
    public static Vector3 Normalized(Vector3 targetPos, Vector3 currentPos)
    {
        Vector3 heading = targetPos - currentPos;
        Vector3 direction = heading / heading.magnitude;

        return direction;
    }

    public static Quaternion Rotation(Vector3 targetPos, Vector3 currentPos)
    {
        Vector3 heading = targetPos - currentPos;
        var angle = (Mathf.Atan2(heading.y, heading.x) * Mathf.Rad2Deg) + 90;
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);

        return targetRotation;
    }
}
