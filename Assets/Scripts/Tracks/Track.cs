using BasicUtilities;
using System;
using System.Collections.Generic;
using UltEvents;
using UnityEngine;
using UnityEngine.Splines;

public class Track : Singleton<Track>
{
    public SplineContainer trackSpline;
    public List<TrackFollower> activeFollowers = new List<TrackFollower>();
    public UltEvent<float> onTrackTick;
    public UltEvent<TrackFollower> onEndReached;
    public UltEvent<TrackFollower> onFollowerAdded;
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
    public UltEvent<TrackFollower?> onFollowerRemoved;
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
    protected override void Initialize()
    {
        activeFollowers = new List<TrackFollower>();
    }
    /// <summary>
    /// Adds the TrackFollower to the track, parenting it under the track.
    /// </summary>
    public static void AddToTrack(TrackFollower trackFollower)
    {
        try
        {
            if (instance.activeFollowers != null)
            {
                instance.activeFollowers.Add(trackFollower);
                trackFollower.AddToTrack();
                instance.onFollowerAdded?.Invoke(trackFollower);
            }
        }
        catch(NullReferenceException exception)
        {
            Debug.Log(exception.Message + " | Unable to add to track. instance hasn't been set yet.");
        }
    }

    /// <summary>
    /// Removes the TrackFollower from the track.
    /// </summary>
    /// <param name="trackFollower"></param>
    public static void RemoveFromTrack(TrackFollower trackFollower)
    {
        if (instance.activeFollowers != null && instance.activeFollowers.Contains(trackFollower))
        {
            instance.activeFollowers.Remove(trackFollower);
            instance.onFollowerAdded?.Invoke(trackFollower);
        }
    }

    private void Update()
    {
        if (activeFollowers.Count > 0)
        {
            onTrackTick?.Invoke(Time.deltaTime);
        }
    }

}
