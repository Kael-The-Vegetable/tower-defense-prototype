using BasicUtilities;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Actions;
using UnityEngine.InputSystem.Interactions;

public class InputManager : PersistentSingleton<InputManager>, InputSystem_Actions.IPlayerActions
{
	private InputSystem_Actions _actions;

	#region Events
	public UnityEvent<Vector2> Move { get; private set; } = new();
	public UnityEvent<Vector2> Look { get; private set; } = new();
	public UnityEvent<Vector2> Pointer { get; private set; } = new();
	public UnityEvent Interact { get; private set; } = new();
	public UnityEvent<bool> InteractHold { get; private set; } = new();
	public UnityEvent<Vector2> Scroll { get; private set; } = new();
	public UnityEvent<bool> Rotate { get; private set; } = new();
	public UnityEvent Examine { get; private set; } = new();
	public UnityEvent<bool> ExamineHold { get; private set; } = new();
	#endregion

	#region Necessesary Initializations
	protected override void Initialize()
	{
		_actions = new InputSystem_Actions();
		_actions.Player.AddCallbacks(this);
	}
	private void OnEnable()
	{
		_actions.Player.Enable();
	}
	private void OnDisable()
	{
		_actions?.Player.Disable();
	}
	private void OnDestroy()
	{
		_actions.Dispose();
	}
	#endregion

	#region Player
	public void OnMove(InputAction.CallbackContext context)
		=> Move.Invoke(context.ReadValue<Vector2>());

	public void OnLook(InputAction.CallbackContext context)
		=> Look.Invoke(context.ReadValue<Vector2>());

	public void OnPoint(InputAction.CallbackContext context)
		=> Pointer.Invoke(context.ReadValue<Vector2>());

	public void OnInteract(InputAction.CallbackContext context)
		=> TapHoldInteraction(context, Interact, InteractHold);

	public void OnScroll(InputAction.CallbackContext context)
		=> Scroll.Invoke(context.ReadValue<Vector2>());

	public void OnRotate(InputAction.CallbackContext context)
	{
		if (context.performed)
		{
			Rotate.Invoke(true);
		}
		else if (context.canceled)
		{
			Rotate.Invoke(false);
		}
	}

	public void OnExamine(InputAction.CallbackContext context)
		=> TapHoldInteraction(context, Examine, ExamineHold);
	#endregion

	#region Helper Methods
	private void TapHoldInteraction(InputAction.CallbackContext ctx, UnityEvent tap, UnityEvent<bool> hold)
	{
		if (ctx.performed)
		{
			if (ctx.interaction is TapInteraction)
			{
				tap.Invoke();
			}
			else if (ctx.interaction is HoldInteraction)
			{
				hold.Invoke(true);
			}
		}
		else if (ctx.canceled && ctx.interaction is HoldInteraction)
		{
			hold.Invoke(false);
		}
	}
	#endregion
}
