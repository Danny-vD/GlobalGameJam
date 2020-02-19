﻿using UnityEngine;

public class ShapeGenerator
{
	private ShapeSettings settings;
	private INoiseFilter[] noiseFilters;
    public MinMaxGenerator elevationMinMax;

	public void UpdateSettings (ShapeSettings settings)
	{
		this.settings = settings;
		noiseFilters = new INoiseFilter[settings.noiseLayers.Length];

		for (int i = 0; i < noiseFilters.Length; i++)
		{
			noiseFilters[i] = NoiseFilterFactory.CreateNoiseFilter(settings.noiseLayers[i].noiseSettings);
		}
        elevationMinMax = new MinMaxGenerator();
	}

	public Vector3 CalculatePointOnPlanet(Vector3 pointOnUnitSphere)
	{
		float firstLayerValue = 0;
		float elevation = 0;
		if (noiseFilters.Length > 0)
		{
			firstLayerValue = noiseFilters[0].Evaluate(pointOnUnitSphere);
			if (settings.noiseLayers[0].enabled)
			{
				elevation = firstLayerValue;
			}
		}

		/*Find a way to create a inversed mask of the float firstLayerValue. for underwater distortion
         * See if you can use Mathf.InverseLerp */
		
		for (int i = 1; i < noiseFilters.Length; i++)
		{
			if (settings.noiseLayers[i].enabled)
			{
				float mask = (settings.noiseLayers[i].useFirstLayerAsMask) ? firstLayerValue : 1;
				elevation += noiseFilters[i].Evaluate(pointOnUnitSphere) * mask;
			}
		}
        elevation = settings.planetRadius * (1 + elevation);
        elevationMinMax.AddValue(elevation);
        return pointOnUnitSphere * elevation;
	}
}
