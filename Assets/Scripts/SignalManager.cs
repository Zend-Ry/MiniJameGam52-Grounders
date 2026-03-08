using UnityEngine;
using UnityEngine.InputSystem;

public static class SignalManager
{
	public static class Input
	{
		//public static Signal<Vector2> OnTapStarted = new TypedSignal<Vector2>();
		//public static Signal<Vector2> OnTapEnded = new TypedSignal<Vector2>();
		//public static Signal<Vector2> OnDrag = new TypedSignal<Vector2>();
	}
	
	//public static Signal OnInstantiateNode = new Signal();

	//public static Signal OnIntroSkipped = new Signal();
	//public static Signal OnLogoCompletedSequence = new Signal();
	//public static Signal OnFullscreenButtonPressed = new Signal();
	//public static Signal OnCameraMovedIntoGamePosition = new Signal();

	//public static Signal OnGameStart = new Signal();

	//public static TypedSignal<float> OnSwipeValue = new TypedSignal<float>();
	//public static TypedSignal<Sheet> OnSheetGained = new TypedSignal<Sheet>();


	// Debug
	public static Signal WhistleBlown = new Signal();
	public static Signal<string> SetDebugText = new Signal<string>();

	public static Signal<int> TestTypedSignal = new Signal<int>();
	public static Signal TestSignal = new Signal();

}
