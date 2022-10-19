using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

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
        bloom.intensity.value = Mathf.Lerp(start.intensity.value, end.intensity.value, value);
        bloom.tint.value = Color.Lerp(start.tint.value, end.tint.value, value);
        bloom.skipIterations.value = (int)Mathf.Lerp(start.skipIterations.value, end.skipIterations.value, value);
    }
}

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

public class PostProcessManager : MonoBehaviour
{
    private float _prevAmount;

    [SerializeField, Range(0f, 1f)] private float _dirtAmount;

    [SerializeField] private Volume _mainVolume;
    
    [SerializeField] private VolumeProfile _cleanProfile;
    [SerializeField] private VolumeProfile _dirtyProfile;

    private PostProcessOverride _clean;
    private PostProcessOverride _dirty;
    private PostProcessOverride _main;

    private bool _isDirty;

    private void Awake()
    {
        _main = new PostProcessOverride(_mainVolume.profile);
        _clean = new PostProcessOverride(_cleanProfile);
        _dirty = new PostProcessOverride(_dirtyProfile);
    }

    /// <summary>
    /// The amount of dirtiness of the world
    /// </summary>
    /// <param name="amount">Between 0 and 1</param>
    public void SetDirtAmount(float amount)
    {
        _dirtAmount = Mathf.Clamp(amount, 0f, 1f);
        _main.Lerp(_dirty, _clean, amount);
    }

    //  for testing
    private void Update()
    {
        if(_prevAmount != _dirtAmount)
        {
            SetDirtAmount(_dirtAmount);
        }
        
        _prevAmount = _dirtAmount;

        // if(Input.GetKey(KeyCode.H))
        // {
        // SetDirtAmount(_dirtAmount - 1f * Time.deltaTime);
        // }
        // else if(Input.GetKey(KeyCode.J))
        // {
        // SetDirtAmount(_dirtAmount + 1f * Time.deltaTime);
        // }
    }
}