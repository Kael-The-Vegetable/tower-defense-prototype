using System;
using UnityEngine;
using UnityEngine.Splines;
using Random = UnityEngine.Random;

[RequireComponent(typeof(SplineAnimate))]
public class TrackFollower : MonoBehaviour
{
    public float MoveSpeed { get => _moveSpeed; set { ChangeSpeed(value); } }

    [SerializeField]
    private float _moveSpeed;
    private Track currentTrack;
    [SerializeField]
    private SplineAnimate splineAnimate;
    public void OnEnable()
    {
        Track.AddToTrack(this);
    }

    public void Start()
    {
        Track.AddToTrack(this);
    }

    public void OnDisable()
    {
        Track.RemoveFromTrack(this);
    }

    public void OnDestroy()
    {
        Track.RemoveFromTrack(this);
    }

    /// <summary>
    /// Adds the folloer to the tracks and makes them move
    /// </summary>
    public void AddToTrack()
    {
        currentTrack = Track.Instance;
        splineAnimate.Container = currentTrack.trackSpline;
        currentTrack.onTrackTick += OnTrackTick;
        splineAnimate.AnimationMethod = SplineAnimate.Method.Speed;
        splineAnimate.Loop = SplineAnimate.LoopMode.Once;
        ChangeSpeed(_moveSpeed);
        splineAnimate.Play();


    }

    private void ChangeSpeed(float speed)
    {

        _moveSpeed = speed;
        float prevProgress = splineAnimate.NormalizedTime;
        splineAnimate.MaxSpeed = _moveSpeed;
        splineAnimate.NormalizedTime = prevProgress;
    }

    private void OnTrackTick(float deltaTime)
    {
        if (splineAnimate.NormalizedTime <= 1)
        {
            Track.Instance.onEndReached?.Invoke(this);
        }
    }
}
