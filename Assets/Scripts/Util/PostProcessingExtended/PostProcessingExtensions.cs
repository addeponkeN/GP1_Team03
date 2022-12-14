using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Util.PostProcessingExtended
{
    public static class PostProcessingExtensions
    {
        public static void Lerp(this FilmGrain grain, FilmGrain start, FilmGrain end, float value)
        {
            grain.intensity.value = Mathf.Lerp(start.intensity.value, end.intensity.value, value);
            grain.response.value = Mathf.Lerp(start.response.value, end.response.value, value);
        }

        public static void Lerp(this LiftGammaGain lgg, LiftGammaGain start, LiftGammaGain end, float value)
        {
            lgg.gain.value = Vector4.Lerp(start.gain.value, end.gain.value, value);
        }

        public static void Lerp(this ShadowsMidtonesHighlights mid,
            ShadowsMidtonesHighlights start, ShadowsMidtonesHighlights end,
            float value)
        {
            mid.shadows.value = Vector4.Lerp(start.shadows.value, end.shadows.value, value);
            mid.midtones.value = Vector4.Lerp(start.midtones.value, end.midtones.value, value);
            mid.highlights.value = Vector4.Lerp(start.highlights.value, end.highlights.value, value);
        }

        public static void Lerp(this Bloom bloom, Bloom start, Bloom end, float value)
        {
            bloom.threshold.value = Mathf.Lerp(start.threshold.value, end.threshold.value, value);
            bloom.intensity.value = Mathf.Lerp(start.intensity.value, end.intensity.value, value);
            bloom.scatter.value = Mathf.Lerp(start.scatter.value, end.scatter.value, value);
            bloom.tint.value = Color.Lerp(start.tint.value, end.tint.value, value);
            bloom.clamp.value = Mathf.Lerp(start.clamp.value, end.clamp.value, value);
            bloom.skipIterations.value = (int)Mathf.Lerp(start.skipIterations.value, end.skipIterations.value, value);
        }

        public static void Lerp(this ColorAdjustments ca, ColorAdjustments start, ColorAdjustments end, float value)
        {
            ca.saturation.value = Mathf.Lerp(start.saturation.value, end.saturation.value, value);
            ca.contrast.value = Mathf.Lerp(start.contrast.value, end.contrast.value, value);
            
        }
    }
}