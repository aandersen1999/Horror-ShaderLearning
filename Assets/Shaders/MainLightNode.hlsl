#ifndef MAINLIGHT_INCLUDED
#define MAINLIGHT_INCLUDED

void GetMainLightData_float(out half3 direction, out half3 color, out half distanceAttenuation, out half shadowAttenuation)
{
#if SHADERGRAPH_PREVIEW
    direction = half3(-0.3, -0.8, 0.6);
    color = half3(1, 1, 1);
    distanceAttenuation = 1.0;
    shadowAttenuation = 1.0;
#else
    Light mainLight = GetMainLight();
    direction = -mainLight.direction;
    color = mainLight.color;
    distanceAttenuation = 1.0;

    shadowAttenuation = 1.0;
#endif
}