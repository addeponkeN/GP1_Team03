using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    private Slider _slider;
    private Image _fill;
    
    private void Awake()
    {
        _slider = GetComponentInChildren<Slider>();
        _fill = GetComponentInChildren<Image>();
    }

    public void Slider_ValueChanged()
    {
        _fill.gameObject.SetActive(_slider.value > 0);
    }
    
}

