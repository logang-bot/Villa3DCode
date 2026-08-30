using UnityEngine;

public class VisionSensor : MonoBehaviour
{
    [SerializeField] Transform eye;
    [SerializeField] float viewAngle = 100f;
    [SerializeField] float viewDistance = 12f;
    [SerializeField] float targetHeightOffset = 0.5f;
    [SerializeField] LayerMask obstructionMask = ~0;

    Transform Eye => eye != null ? eye : transform;

    public bool CanSee(Transform target)
    {
        Vector3 eyePos = Eye.position;
        Vector3 targetPoint = target.position + Vector3.up * targetHeightOffset;
        Vector3 toTarget = targetPoint - eyePos;

        if (toTarget.magnitude > viewDistance) return false;
        if (Vector3.Angle(Eye.forward, toTarget) > viewAngle * 0.5f) return false;
        return HasLineOfSight(eyePos, targetPoint, target);
    }

    bool HasLineOfSight(Vector3 eyePos, Vector3 targetPoint, Transform target)
    {
        Vector3 offset = targetPoint - eyePos;
        float distance = offset.magnitude;
        bool blocked = Physics.Raycast(eyePos, offset / distance, out RaycastHit hit,
            distance, obstructionMask, QueryTriggerInteraction.Ignore);
        return !blocked || hit.transform == target || hit.transform.IsChildOf(target);
    }
}
