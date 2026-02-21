using BasicUtilities;
using UnityEngine;
using UnityEngine.Events;

public class EconomyManager : Singleton<EconomyManager>
{
	[SerializeField] private int _balance;

	public int Balance
	{
		get => _balance;
		private set
		{
			if (value != _balance)
			{
				BalanceChanged.Invoke(value);
				_balance = value;
			}
		}
	}

	/// <summary>
	/// The integer carried over is the NEW balance. The <seealso cref="Balance"/> will show the previous value.
	/// </summary>
	public UnityEvent<int> BalanceChanged { get; } = new();

	protected override void Initialize() {}

	/// <summary>
	/// Use this method to make purchases.
	/// </summary>
	/// <param name="cost"></param>
	/// <returns></returns>
	public bool Purchase(int cost)
	{
		if (_balance >= cost)
		{
			Balance -= cost;
			return true;
		}
		return false;
	}

	/// <summary>
	/// Use this method when being able to go below 0 balance is desired.
	/// </summary>
	/// <param name="cost"></param>
	public void ForcePurchase(int cost) => Balance -= cost;

	/// <summary>
	/// Use this method to deposit to the current balance.
	/// </summary>
	/// <param name="value"></param>
	public void Deposit(int value) => Balance += value;
}
