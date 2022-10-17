using PlayerControllers.Controllers;
using UnityEngine;

public class BikeDustParticleController : MonoBehaviour
{
    private ParticleSystem _ps;
    private MovementController _move;

    private void Awake()
    {
        _ps = GetComponent<ParticleSystem>();
    }

    private void Start()
    {
        var player = GetComponentInParent<Player>();
        _move = player.ControllerManager.GetController<MovementController>();
    }

    private void Update()
    {
        if(_move.Speed > 1f)
        {
            if(!_ps.isPlaying)
            {
                _ps.Play();
            }
        }
        else if(_ps.isPlaying)
        {
            _ps.Stop();
        }
    }
}