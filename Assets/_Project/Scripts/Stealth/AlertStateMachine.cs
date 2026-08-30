using UnityEngine;

public class AlertStateMachine : MonoBehaviour
{
    [SerializeField] VisionSensor visionSensor;
    [SerializeField] Transform target;
    [SerializeField] float riseRate = 0.35f;
    [SerializeField] float decayRate = 0.2f;
    [SerializeField] float suspiciousThreshold = 0.34f;

    IConcealmentProvider concealment;

    public AlertLevel CurrentState { get; private set; }
    public float Awareness01 { get; private set; }

    void Awake()
    {
        if (target == null)
            target = GameObject.FindGameObjectWithTag("Player")?.transform;
        concealment = target != null ? target.GetComponent<IConcealmentProvider>() : null;
    }

    void Update()
    {
        bool seen = target != null && visionSensor.CanSee(target);
        Awareness01 = Mathf.Clamp01(Awareness01 + DeltaFor(seen) * Time.deltaTime);
        CurrentState = StateFor(Awareness01);
    }

    float DeltaFor(bool seen)
    {
        if (!seen) return -decayRate;
        return riseRate * (concealment?.RiseRateMultiplier ?? 1f);
    }

    AlertLevel StateFor(float awareness)
    {
        if (awareness >= 1f) return AlertLevel.Alert;
        return awareness >= suspiciousThreshold ? AlertLevel.Suspicious : AlertLevel.Unaware;
    }
}
