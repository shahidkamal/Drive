using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadLines : MonoBehaviour
{
#pragma warning disable 649 // 'field' is never assigned to
    [Tooltip("Player Speed")] [SerializeField]
    private FloatVariable playerSpeed;
    
    [Tooltip("Road lines go in here in any order")] [SerializeField]
    private SpriteRenderer[] _lines;
#pragma warning restore 649 // 'field' is never assigned to
    
    // Update is called once per frame
    private void Update()
    {
        for (var i = 0; i < _lines.Length; ++i)
        {
            var pos = _lines[i].transform.position;
            pos.y -= playerSpeed / 60;
            if (pos.y < -6)
            {
                pos.y = 6f;
            }
            _lines[i].transform.position = pos;
        }
    }
}
