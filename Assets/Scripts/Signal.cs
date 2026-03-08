using UnityEngine;
using UnityEngine.Events;

public class Signal<T>
{
	private UnityEvent<T> _process = new UnityEvent<T>();
	public void AddListener(UnityAction<T> pListener) => _process.AddListener(pListener);
	public void RemoveListener(UnityAction<T> pListener) => _process.RemoveListener(pListener);
	public void RemoveAllListeners() => _process.RemoveAllListeners();
	public void Emit(T pValue) => _process.Invoke(pValue);
}

public class Signal
{
	private UnityEvent _process = new UnityEvent();
	public void AddListener(UnityAction pListener) => _process.AddListener(pListener);
	public void RemoveListener(UnityAction pListener) => _process.RemoveListener(pListener);
	public void RemoveAllListeners() => _process.RemoveAllListeners();
	public void Emit() => _process.Invoke();
}