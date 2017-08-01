using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BiteTimer : MonoBehaviour
{
    [SerializeField]
    private Slider _progressBar;

    public void SetValueNormalized(float normalizedValue)
    {
        _progressBar.value = normalizedValue;
    }
}