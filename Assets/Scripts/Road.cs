using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{
    [Tooltip("Lane Allocation Buffer Size")] 
#pragma warning disable 649 // 'field' is never assigned to
    [SerializeField]
    private int _laneBufferDize;
#pragma warning restore 649 // 'field' is never assigned to
    
    private List<int> _laneIndexes = new List<int>();
    
    private void Awake()
    {
        // TODO: This is stupid and broken, don't use the results
        for (var i = 0; i < _laneBufferDize * Game.NumLanes; i += Game.NumLanes)
        {
            for (var j = 0; j < Game.NumLanes; ++j)
            {
                _laneIndexes.Add(j);
            }
        }
        
        _laneIndexes.Shuffle();
    }
}
