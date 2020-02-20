﻿using UnityEngine;
using VDUnityFramework.BaseClasses;
using VDUnityFramework.UnityExtensions;

public class MyLight : BetterMonoBehaviour
{
	[SerializeField]
	private bool shouldCheckEveryFrame = false;

	private Light[] myLights;
	private IsLit IsLit;
		
	private void Start()
	{
		myLights = GetComponentsInChildren<Light>();
		if (myLights.Length == 0)
		{
			Debug.LogError("The GameObject needs light component");
		}

		ToggleLight(false);

		IsLit = gameObject.EnsureComponent<IsLit>();

		CheckForLightSource();
	}

	private void LateUpdate()
	{
		if (shouldCheckEveryFrame)
		{
			CheckForLightSource();
		}
	}

	private void CheckForLightSource()
	{
		ToggleLight(!IsLit.IsInLight());
	}

	private void ToggleLight(bool enable)
	{
		foreach (Light light in myLights)
		{
			light.enabled = enable;
		}
	}
}
