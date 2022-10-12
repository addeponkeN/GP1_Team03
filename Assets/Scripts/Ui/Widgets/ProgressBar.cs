using System;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.Widgets
{
    public class ProgressBar : MonoBehaviour
    {
        public float Value
        {
            get => _slider.value;
            set => _slider.value = value;
        }
        
        public event Action<float> ValueChangedEvent;

//  UNITY EDITOR FIELDS

        [Header("Background")] 
        [SerializeField] private Sprite BackgroundImage;
        [SerializeField] private Color BackgroundColor = Color.white;

        [Space(2)] 
        
        [Header("Foreground")] 
        [SerializeField] private Sprite ForegroundImage;
        [SerializeField] private Color ForegroundColor = Color.white;

        [Space(5)]
        
        [SerializeField] private Slider.SliderEvent onValueChanged;


//  CACHED COMPONENTS

        private Slider _slider;
        private Image _foreground;
        private Image _background;

        private void Awake()
        {
            _slider = GetComponentInChildren<Slider>();

            FindAndCacheChildrenImages();
            UpdateComponents();

            _slider.onValueChanged = onValueChanged;
        }

        /// <summary>
        /// Get the images from 'Background' and 'Fill' gameobjects and assign.
        /// </summary>
        private void FindAndCacheChildrenImages()
        {
            var children = gameObject.GetComponentsInChildren<Image>();
            if(children.Length < 2)
            {
                Debug.LogWarning($"No image children found in {gameObject.name}");
                return;
            }

            _background = children[0]; //  Background
            _foreground = children[1]; //  Fill
        }

        private void UpdateComponents()
        {
            _background.sprite = BackgroundImage;
            _background.color = BackgroundColor;

            _foreground.sprite = ForegroundImage;
            _foreground.color = ForegroundColor;
        }

        public void Slider_ValueChanged()
        {
            ValueChangedEvent?.Invoke(_slider.value);
        }

        private void Reset()
        {
            BackgroundColor = Color.white;
            ForegroundColor = new Color(0.75f, 0.75f, 0.75f);

            UpdateComponents();
        }
    }
}