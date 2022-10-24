using UnityEngine;

public class FollowPoint : System.IEquatable<FollowPoint>
{
	private float _speed = 0;
	private bool _occupied = false;
	private bool _grounded = false;
	private Vector3 _position = Vector3.zero;
	
	public bool Occupied {
		get => _occupied;
		set => _occupied = value;
	}
	public float Speed => _speed;
	public bool Grounded => _grounded;
	public Vector3 Position => _position;

	public FollowPoint(float speed, bool grounded, Vector3 position){
		_speed = speed;
		_grounded = grounded;
		_position = position;
	}

	public bool Equals(FollowPoint other){
		return _position == other.Position;
    }
}