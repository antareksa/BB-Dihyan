using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlaceGameField : MonoBehaviour
{
    private Pose PlacementPose;
    private ARRaycastManager aRRaycastManager;
    private bool placementPoseIsValid = false;

    public GameObject game;
    public bool gamePlaced;

    void Start()
    {
        
    }


    void Update()
    {
        UpdatePlacementPose();
        UpdatePlacementIndicator();
    }

    void UpdatePlacementIndicator()
    {
        if (placementPoseIsValid)
        {
            if (!gamePlaced)
            {
                game.transform.SetPositionAndRotation(PlacementPose.position, PlacementPose.rotation);
                gamePlaced = true;
            }

            else
            {
                //placementIndicator.SetActive(false);
            }
        }
    }


    void UpdatePlacementPose()
    {
        try
        {
            var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
            var hits = new List<ARRaycastHit>();
            aRRaycastManager.Raycast(screenCenter, hits, TrackableType.Planes);

            placementPoseIsValid = hits.Count > 0;
            if (placementPoseIsValid)
            {
                PlacementPose = hits[0].pose;
            }
        }
        catch (NullReferenceException ex)
        {
            //Debug.Log("Ignore this");
        }


    }
}
