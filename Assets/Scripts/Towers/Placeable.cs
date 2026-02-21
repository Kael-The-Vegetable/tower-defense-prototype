using UnityEngine;
using UnityEngine.Events;

public class Placeable : MonoBehaviour
{
	public enum State
	{
		None,
		Placing,
		Invalid,
		Placed
	}
	[SerializeField] private Collider _collider;
	private State _state;
	public UnityEvent<State> StateChanged { get; } = new();
}
