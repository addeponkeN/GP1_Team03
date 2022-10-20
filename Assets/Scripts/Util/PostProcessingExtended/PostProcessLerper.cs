using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace Util.PostProcessingExtended
{
    public class PostProcessLerper : MonoBehaviour
    {
        private float _prevAmount;

        [SerializeField, Range(0f, 1f)] private float _value;

        [SerializeField] private Volume _mainVolume;
        [SerializeField] private VolumeProfile _startProfile;
        [SerializeField] private VolumeProfile _endProfile;

        private PostProcessOverride _clean;
        private PostProcessOverride _dirty;
        private PostProcessOverride _main;

        private bool _isDirty;

        private void Awake()
        {
            _main = new PostProcessOverride(_mainVolume.profile);
            _clean = new PostProcessOverride(_endProfile);
            _dirty = new PostProcessOverride(_startProfile);
        }

        private void Start()
        {
            SetAmount(0f);
        }

        /// <summary>
        /// The amount of dirtiness of the world
        /// </summary>
        /// <param name="amount">Between 0 and 1</param>
        public void SetAmount(float amount)
        {
            _value = Mathf.Clamp(amount, 0f, 1f);
            _main.Lerp(_dirty, _clean, amount);
        }

        //  for testing
        private void Update()
        {
            if(Math.Abs(_prevAmount - _value) > 0.01f)
            {
                SetAmount(_value);
                _prevAmount = _value;
            }
        }
    }
}