using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TextMesh))]
public class UpdateProgressDisplay : MonoBehaviour
{
    private TextMesh _textMesh;

    // Start is called before the first frame update
    void Start()
    {
        _textMesh = GetComponent<TextMesh>();
    }

    public void UpdateText(float percentage)
    {
        _textMesh.text = (int)(percentage * 100f) + "%";
    }
}
