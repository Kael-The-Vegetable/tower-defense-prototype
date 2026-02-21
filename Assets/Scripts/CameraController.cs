using System;
using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	private bool _isRotating;

	private bool _isDragging;
	private Vector3 _dragOrigin;

	private Vector2 _mousePos;
	private Vector3 _lookDelta;

	private void OnEnable()
	{
		InputManager.Instance.ExamineHold.AddListener(OnExamineHold);
		InputManager.Instance.Pointer.AddListener(OnPointer);
		InputManager.Instance.Rotate.AddListener(OnRotate);
		InputManager.Instance.Look.AddListener(OnLook);
	}
	private void OnDisable()
	{
		if (InputManager.HasInstance)
		{
			InputManager.Instance.ExamineHold.RemoveListener(OnExamineHold);
			InputManager.Instance.Pointer.RemoveListener(OnPointer);
			InputManager.Instance.Rotate.RemoveListener(OnRotate);
			InputManager.Instance.Look.RemoveListener(OnLook);
		}
	}

	#region Event Handlers
	private void OnExamineHold(bool isDragging)
	{
		_isDragging = isDragging;
		if (_isDragging)
		{
			_dragOrigin = Utils.PointerToWorldXZ(Camera.main, _mousePos);
			_dragOrigin.y = 0;
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
				newPos.y = 0;
				transform.position += _dragOrigin - newPos;
			}
		}
	}
	private void OnRotate(bool isHolding)
	{
		_isRotating = isHolding;
		StartCoroutine(Rotator());
	}
	private void OnLook(Vector2 delta) => _lookDelta = new Vector3(0, delta.x);

	#endregion

	private IEnumerator Rotator()
	{
		while (_isRotating)
		{
			transform.eulerAngles += _lookDelta;
			yield return null;
		}
	}
}
