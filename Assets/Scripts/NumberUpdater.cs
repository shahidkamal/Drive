using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NumberUpdater : MonoBehaviour
{
#pragma warning disable 649 // 'field' is never assigned to
    [Tooltip("Number to use in text field")] [SerializeField]
    private FloatVariable value;

    [Tooltip("Scale Factor")] [SerializeField]
    private float scaleFactor;
#pragma warning restore 649 // 'field' is never assigned to
    
    private TextMeshProUGUI text;
    
    // Start is called before the first frame update
    private void Start()
    {
        text = gameObject.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (text != null)
        {
            var total = value * scaleFactor;
            text.SetText(((int)total).ToString());
        }
    }
}
