void MainLight_float(float3 worldPos, out float3 direction, out float3 color, out float distanceAtten, out float shadowAtten)
{
#ifdef SHADERGRAPH_PREVIEW
	direction = normalize(float3(0.5f, 0.5f, 0.25f));
	color = float3(1.0f, 1.0f, 1.0f);
	distanceAtten = 1.0f;
	shadowAtten = 1.0f;
#else
	float4 shadowCoord = TransformWorldToShadowCoord(worldPos);
	Light mainLight = GetMainLight(shadowCoord);

	direction = mainLight.direction;
	color = mainLight.color;
	distanceAtten = mainLight.distanceAttenuation;
	shadowAtten = mainLight.shadowAttenuation;
#endif
}

void MainLight_half(half3 worldPos, out half3 direction, out half3 color, out half distanceAtten, out half shadowAtten)
{
#ifdef SHADERGRAPH_PREVIEW
	direction = normalize(half3(0.5f, 0.5f, 0.25f));
	color = half3(1.0f, 1.0f, 1.0f);
	distanceAtten = 1.0f;
	shadowAtten = 1.0f;
#else
	half4 shadowCoord = TransformWorldToShadowCoord(worldPos);
	Light mainLight = GetMainLight(shadowCoord);

	direction = mainLight.direction;
	color = mainLight.color;
	distanceAtten = mainLight.distanceAttenuation;
	shadowAtten = mainLight.shadowAttenuation;
#endif
}

void AdditionalLight_float(float3 worldPos, int index, out float3 direction, out float3 color, out float distanceAtten, out float shadowAtten)
{
	direction = normalize(float3(0.5f, 0.5f, 0.25f));
	color = float3(0.0f, 0.0f, 0.0f);
	distanceAtten = 0.0f;
	shadowAtten = 0.0f;

#ifndef SHADERGRAPH_PREVIEW
	int pixelLightCount = GetAdditionalLightsCount();
	if (index < pixelLightCount)
	{
		Light light = GetAdditionalLight(index, worldPos);

		direction = light.direction;
		color = light.color;
		distanceAtten = light.distanceAttenuation;
		shadowAtten = light.shadowAttenuation;
	}
#endif
}

void AdditionalLight_half(half3 worldPos, int index, out half3 direction, out half3 color, out half distanceAtten, out half shadowAtten)
{
	direction = normalize(half3(0.5f, 0.5f, 0.25f));
	color = half3(0.0f, 0.0f, 0.0f);
	distanceAtten = 0.0f;
	shadowAtten = 0.0f;

#ifndef SHADERGRAPH_PREVIEW
	int pixelLightCount = GetAdditionalLightsCount();
	if (index < pixelLightCount)
	{
		Light light = GetAdditionalLight(index, worldPos);

		direction = light.direction;
		color = light.color;
		distanceAtten = light.distanceAttenuation;
		shadowAtten = light.shadowAttenuation;
	}
#endif
}
