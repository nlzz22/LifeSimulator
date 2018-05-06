using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SliderScript : MonoBehaviour {
    private Text _text;
    private Slider _slider;

    private void Start()
    {
        _text = GetComponentInChildren<Text>();
        _slider = GetComponent<Slider>();
        UpdateSliderText();
    }

    public void UpdateSliderText()
    {
        if (_text != null && _slider != null)
        {
            _text.text = _slider.value.ToString();
        }
        
    }
}
