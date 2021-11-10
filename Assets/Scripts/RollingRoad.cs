using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class RollingRoad : MonoBehaviour
{
    private const int NumRoadPieces = 3;
    
#pragma warning disable 649 // 'field' is never assigned to
    [Tooltip("Three road pieces top to bottom. Each road piece must be at least as long as the screen height.")] [SerializeField]
    private GameObject[] roadPieces;

    [FormerlySerializedAs("camera")] [Tooltip("Camera transform")] [SerializeField]
    private Transform trackingCamera;

    [Tooltip("Road piece length with overlap in units")] [SerializeField]
    private float roadLength;
#pragma warning restore 649 // 'field' is never assigned to

    private void Awake()
    {
        var position = trackingCamera.position;
        position.z = 0;
        position.y += roadLength;
        
        for (var i = 0; i < NumRoadPieces; ++i)
        {
            roadPieces[i].transform.position = position;
            position.y -= roadLength;
        }
    }

    private void Update()
    {
        var camPos = trackingCamera.position;
        if (camPos.y >= roadPieces[0].transform.position.y - roadLength / 2)
        {
            for (var i = 0; i < NumRoadPieces; ++i)
            {
                var pos = roadPieces[i].transform.position;
                pos.y += roadLength;
                roadPieces[i].transform.position = pos;
            }
        }
        // You'll want to check for the other way too in case we ever want the road to scroll both ways
    }
}
