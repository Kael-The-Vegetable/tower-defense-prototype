using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	private bool _isRotating;

	private bool _isDragging;
	private Vector3 _dragOrigin;

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
		_isDragging = isDragging;
		if (_isDragging)
		{
			_dragOrigin = Utils.PointerToWorldXZ(Camera.main, _mousePos);
		}
	}
	public void OnPointer(Vector2 pos)
	{
		_mousePos = pos;
		if (_isDragging)
		{
			Vector3 newPos = Utils.PointerToWorldXZ(Camera.main, _mousePos);
			if (newPos != Vector3.zero)
			{
				transform.position += _dragOrigin - newPos;
			}
		}
	}
	private void OnRotate(bool isHolding)
	{
		_isRotating = isHolding;
	}
	#endregion
}
