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
    public UltEvent<float> trackTick;

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
        }
    }

    private void Update()
    {
        if (activeFollowers.Count > 0)
        {
            trackTick?.Invoke(Time.deltaTime);
        }
    }

}
