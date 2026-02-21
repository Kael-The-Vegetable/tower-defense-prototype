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
	[SerializeField] private State _state;
	public UnityEvent<State> StateChanged { get; } = new();
	[SerializeField] private int _collisions = 0;
	public void OnDisable()
	{
		if (InputManager.HasInstance)
		{
			InputManager.Instance.Interact.RemoveListener(OnInteract);
			InputManager.Instance.Pointer.RemoveListener(OnPointerMove);
		}
	}
	[ContextMenu("Placing")]
	public void BeginPlacement()
	{
		_state = State.Placing;
		_collider.isTrigger = true;
		InputManager.Instance.Interact.AddListener(OnInteract);
		InputManager.Instance.Pointer.AddListener(OnPointerMove);
	}
	public void CancelPlacement()
	{
		_state = State.None;
	}

	private void OnInteract()
	{
		if (_state == State.Placing)
		{
			_state = State.Placed;
			_collider.isTrigger = false;
			InputManager.Instance.Interact.RemoveListener(OnInteract);
			InputManager.Instance.Pointer.RemoveListener(OnPointerMove);
		}
	}

	private void OnPointerMove(Vector2 mousePos)
	{
		transform.position = Utils.PointerToWorldXZ(Camera.main, mousePos);
	}
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == gameObject.layer) // on same layer
		{
			_collisions++;
			_state = State.Invalid;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (--_collisions == 0 && _state != State.Placed)
		{
			_state = State.Placing;
		}
	}
}
