using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadLines : MonoBehaviour
{
#pragma warning disable 649 // 'field' is never assigned to
    [Tooltip("Road lines go in here in any order")] [SerializeField]
    private SpriteRenderer[] _lines;
#pragma warning restore 649 // 'field' is never assigned to

    private float _speed;
    
    // Start is called before the first frame update
    void Start()
    {
        _speed = -1/120f;
    }

    // Update is called once per frame
    void Update()
    {
        for (var i = 0; i < _lines.Length; ++i)
        {
            var pos = _lines[i].transform.position;
            pos.y += _speed;
            if (pos.y < -6)
            {
                pos.y = 6f;
            }
            _lines[i].transform.position = pos;
        }
    }

    public void SetSpeed(float speed)
    {
        _speed = speed;
    }
}
