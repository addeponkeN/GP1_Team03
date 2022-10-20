using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Util.PostProcessingExtended
{
    public class PostProcessOverride
    {
        private VolumeProfile _profile;

        private FilmGrain _grain;
        private Tonemapping _tonemapping;
        private LiftGammaGain _lgg;
        private ShadowsMidtonesHighlights _smh;
        private Bloom _bloom;
        
        public PostProcessOverride(VolumeProfile profile)
        {
            _profile = profile;
            _profile.TryGet(out _grain);
            _profile.TryGet(out _tonemapping);
            _profile.TryGet(out _lgg);
            _profile.TryGet(out _smh);
            _profile.TryGet(out _bloom);
        }

        public void Lerp(PostProcessOverride start, PostProcessOverride end, float value)
        {
            _grain.Lerp(start._grain, end._grain, value);
            _lgg.Lerp(start._lgg, end._lgg, value);
            _smh.Lerp(start._smh, end._smh, value);
            _bloom.Lerp(start._bloom, end._bloom, value);
        }
    }
}