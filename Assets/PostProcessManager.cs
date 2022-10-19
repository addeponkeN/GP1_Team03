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
}

public class PostProcessOverride
{
    public VolumeProfile _profile;
    
    public FilmGrain _grain;
    public Tonemapping _tonemapping;
    public LiftGammaGain _gamma;
    public LiftGammaGain _lens;

    public PostProcessOverride(VolumeProfile profile)
    {
        _profile = profile;
        _profile.TryGet(out _grain);
        _profile.TryGet(out _tonemapping);
        _profile.TryGet(out _gamma);
        _profile.TryGet(out _lens);
    }

    public void Lerp(PostProcessOverride start, PostProcessOverride end, float amount)
    {
        _grain.Lerp(start._grain, end._grain, amount);
    }

}

public class PostProcessManager : MonoBehaviour
{
    private float _prevAmount;
    
    //  followerCount / 50  (clamp 0-1)
    [SerializeField, Range(0f, 1f)] private float _dirtAmount;
    
    [SerializeField] private VolumeProfile _mainProfile;
    [SerializeField] private VolumeProfile _cleanProfile;
    [SerializeField] private VolumeProfile _dirtyProfile;
    
    private PostProcessOverride _clean;
    private PostProcessOverride _dirty;
    private PostProcessOverride _main;

    private bool _isDirty;
    
    private void Start()
    {
        _main = new PostProcessOverride(_mainProfile);
        _clean = new PostProcessOverride(_cleanProfile);
        _dirty = new PostProcessOverride(_dirtyProfile);
    }

    private void Update()
    {
        if(_prevAmount != _dirtAmount) {
            // UpdatePP();
        }
        _prevAmount = _dirtAmount;
    }
}