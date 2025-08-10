using UnityEngine;
using UnityEngine.Events;

public class CountdownTimer : MonoBehaviour
{
    [Header("Config")]

    //Duratino of game set 5min
    [SerializeField] private float duration = 300f;
    [SerializeField] private bool useUnscaledTime = false;
    [SerializeField] private bool autoStart = false;

    [Header("Events")]
    public UnityEvent<float> onTick;        // remaining seconds
    public UnityEvent onCompleted;

    public bool IsRunning { get; private set; }
    public bool IsPaused  { get; private set; }
    public float Duration => duration;
    public float Remaining => Mathf.Max(0f, IsRunning ? (float)(EndNow() - Now()) : cachedRemaining);

    private double endTime;

    // used while paused / stopped
    private float cachedRemaining;

    void Start()
    {
        if (autoStart) StartTimer(duration);
    }

    void Update()
    {
        if (!IsRunning || IsPaused) return;

        float remaining = (float)(endTime - Now());
        if (remaining <= 0f)
        {
            IsRunning = false;
            cachedRemaining = 0f;
            onTick?.Invoke(0f);
            onCompleted?.Invoke();
            return;
        }

        onTick?.Invoke(remaining);
    }

    public void StartTimer(float newDuration)
    {
        duration = Mathf.Max(0f, newDuration);
        IsPaused = false;
        IsRunning = true;
        endTime = Now() + duration;
        // emit first tick so UI initializes immediately
        onTick?.Invoke(duration);
    }

    public void StopTimer()
    {
        cachedRemaining = Remaining;
        IsRunning = false;
        IsPaused = false;
    }

    public void Pause()
    {
        if (!IsRunning || IsPaused) return;
        cachedRemaining = Remaining;
        IsPaused = true;
    }

    public void Resume()
    {
        if (!IsRunning || !IsPaused) return;
        IsPaused = false;
        endTime = Now() + cachedRemaining;
    }

    public void AddTime(float delta)
    {
        if (!IsRunning) { cachedRemaining = Mathf.Max(0f, cachedRemaining + delta); return; }
        if (IsPaused)   { cachedRemaining = Mathf.Max(0f, cachedRemaining + delta); return; }
        endTime += delta;
    }

    public void SetUnscaled(bool unscaled)
    {
        // preserve remaining when switching mode
        float rem = Remaining;
        useUnscaledTime = unscaled;
        if (IsRunning && !IsPaused) endTime = Now() + rem;
        else cachedRemaining = rem;
    }

    private double Now() => useUnscaledTime ? Time.unscaledTimeAsDouble : Time.timeAsDouble;
    private double EndNow() => endTime;
}
