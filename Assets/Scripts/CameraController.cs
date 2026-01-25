using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	private bool _isRotating;

	private bool _isDragging;
	private Vector3 _dragOrigin;
	private Vector3 _origPos;

	private Vector2 _mousePos;

	private void OnEnable()
	{
		InputManager.Instance.ExamineHold.AddListener(OnExamineHold);
		InputManager.Instance.Pointer.AddListener(OnPointer);
		InputManager.Instance.Rotate.AddListener(OnRotate);
	}
	private void OnDisable()
	{
		if (InputManager.HasInstance)
		{
			InputManager.Instance.ExamineHold.RemoveListener(OnExamineHold);
			InputManager.Instance.Pointer.RemoveListener(OnPointer);
			InputManager.Instance.Rotate.RemoveListener(OnRotate);
		}
	}

	#region Event Handlers
	private void OnExamineHold(bool isDragging)
	{
		Debug.Log("Dragging: " + isDragging);
		_isDragging = isDragging;
		if (_isDragging)
		{
			_dragOrigin = Utils.PointerToWorldOnXZPlane(Camera.main, _mousePos);
			_origPos = transform.position;
		}
	}
	public void OnPointer(Vector2 pos)
	{
		_mousePos = pos;
		if (_isDragging)
		{
			Debug.Log("Mouse Pos: " + pos);
			Vector3 difference = Utils.PointerToWorldOnXZPlane(Camera.main, _mousePos) - _dragOrigin - transform.position;
			transform.position = _origPos - difference;
		}
	}
	private void OnRotate(bool isHolding)
	{
		_isRotating = isHolding;
	}
	#endregion
}
