#ifndef ADVERTY_CG_INCLUDED
#define ADVERTY_CG_INCLUDED

#include "UnityCG.cginc"

//Surface input data
struct Input
{
    float2 uv_BaseMap;
    float2 uv_MainTex;
    float3 worldNormal;
    float3 viewDir;
    fixed isFacing : VFACE;
};

struct v2f
{
    float2 uv : TEXCOORD0;
    float4 vertex : SV_POSITION;
    float3 normal : NORMAL;
    float3 viewT : TEXCOORD1;
};

sampler2D _MainTex;
sampler2D _BaseMap;
sampler2D _WatermarkTex;
sampler2D _FadeTexture;

float2 _WatermarkUvSize;
fixed4 _BaseColor;
fixed _GrayFactor;
fixed _TransitionProgress;
fixed _WatermarkIsVisible;
fixed _FadeTexUVFactor;
fixed _MainTexUVFactor;


fixed4 addWatermark(float2 uv, fixed4 main)
{
    float2 watermarkRepeatFactor = float2(uv.x < 0, uv.y < 0);
    fixed2 watermarkUv = watermarkRepeatFactor / _WatermarkUvSize + uv / _WatermarkUvSize;
    watermarkRepeatFactor += uv;

    float watermarkFactor = all(step(watermarkRepeatFactor, _WatermarkUvSize));
    fixed4 watermark = lerp(main, tex2D(_WatermarkTex, watermarkUv), watermarkFactor);

    return lerp(watermark, main, 1 - watermark.a);
}

fixed4 GetMainColor(float2 uv, fixed mainTex)
{
    if(mainTex)
    {
        return tex2D(_MainTex, uv);
    }

    return tex2D(_BaseMap, uv);
}

fixed4 GetAdvertyColor(float2 uv, fixed useMainTex)
{
    float2 mainUv = uv;
    float2 fadeUV = uv;
#if SHADER_API_VULKAN || SHADER_API_METAL
    fadeUV.y = lerp(fadeUV.y, 1.0f - fadeUV.y, _FadeTexUVFactor);
    mainUv.y = lerp(mainUv.y, 1.0f - mainUv.y, _MainTexUVFactor);
#endif

    fixed4 mainColor = GetMainColor(mainUv, useMainTex);
    fixed4 color = lerp(mainColor, tex2D(_FadeTexture, fadeUV), _TransitionProgress);
    color = lerp(color, addWatermark(mainUv, color), _WatermarkIsVisible);
    color = lerp(color, fixed4(0.5, 0.5, 0.5, 1), _GrayFactor);
    color *= _BaseColor;
    return color;
}

v2f AdvertyVert(appdata_base IN)
{
    v2f OUT;
    OUT.vertex = UnityObjectToClipPos(IN.vertex);
    OUT.uv = IN.texcoord.xy;
    OUT.normal = normalize(IN.normal);
    OUT.viewT = normalize(WorldSpaceViewDir(IN.vertex));
    return OUT;
}

fixed4 AdvertyFrag(v2f i) : SV_TARGET
{
    return GetAdvertyColor(i.uv, 0);
}

fixed4 AdvertyUIFrag(v2f i) : SV_TARGET
{
    return GetAdvertyColor(i.uv, 1);
}

#endif