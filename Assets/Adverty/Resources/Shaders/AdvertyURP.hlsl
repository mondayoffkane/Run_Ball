#ifndef ADVERTY_URP_HLSL
#define ADVERTY_URP_HLSL

#ifdef ADVERTY_URP_COMPAT

#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"

    TEXTURE2D(_FadeTexture); SAMPLER(sampler_FadeTexture);
    TEXTURE2D(_WatermarkTex); SAMPLER(sampler_WatermarkTex);

    float2 _WatermarkUvSize;
    half _GrayFactor;
    float _FadeTexUVFactor;
    float _MainTexUVFactor;
    float _TransitionProgress;
    half _WatermarkIsVisible;

    half4 addWatermark(float2 uv, half4 main)
    {
        float2 watermarkRepeatFactor = float2(uv.x < 0, uv.y < 0);  
        float2 watermarkUv = watermarkRepeatFactor / _WatermarkUvSize + uv / _WatermarkUvSize;
        watermarkRepeatFactor += uv;
        float watermarkFactor = all(step(watermarkRepeatFactor, _WatermarkUvSize));
        float4 watermark = lerp(main, SAMPLE_TEXTURE2D(_WatermarkTex, sampler_WatermarkTex, watermarkUv), watermarkFactor);
        return lerp(watermark, main, 1 - watermark.a);
    }

#else
        //Unity URP fallback logic.
        #include "UnityShaderVariables.cginc"

        struct Attributes
        {
            float4 positionOS : POSITION;
        };

        struct Varyings
        {
            float4 vertex : SV_POSITION;
        };

        float4 GetDefaultColor()
        {
            return float4(0.37f, 0.08f, 0.5f, 1.0f);
        }

        float4 GetVertexPosition(float3 positionOS)
        {
            return mul(UNITY_MATRIX_VP, float4(mul(UNITY_MATRIX_M, float4(positionOS, 1.0f)).xyz, 1.0f));
        }

        Varyings GetVaryings(Attributes inData)
        {
            Varyings outData;
            outData.vertex = GetVertexPosition(inData.positionOS);
            return outData;
        }

        Varyings vert(Attributes inData)
        {
            return GetVaryings(inData);
        }

        half4 frag(Varyings input) : SV_TARGET
        {
            return GetDefaultColor();
        }

        Varyings UniversalVertexMeta(Attributes inData)
        {
            return GetVaryings(inData);
        }

        half4 UniversalFragmentMetaSimple(Varyings input) : SV_TARGET
        {
            return GetDefaultColor();
        }

        half4 UniversalFragmentMeta(Varyings input) : SV_TARGET
        {
            return GetDefaultColor();
        }

        half4 UniversalFragmentMetaUnlit(Varyings input) : SV_TARGET
        {
            return GetDefaultColor();
        }

        Varyings DepthOnlyVertex(Attributes inData)
        {
            return GetVaryings(inData);
        }

        half4 DepthOnlyFragment(Varyings input) : SV_TARGET
        {
            return GetDefaultColor();
        }

        Varyings ShadowPassVertex(Attributes inData)
        {
            return GetVaryings(inData);
        }

        half4 ShadowPassFragment(Varyings input) : SV_TARGET
        {
            return GetDefaultColor();
        }

        Varyings LitPassVertex(Attributes inData)
        {
            return GetVaryings(inData);
        }

        half4 AdvertyLitPassFragment(Varyings input) : SV_TARGET
        {
            return GetDefaultColor();
        }
        
        Varyings LitPassVertexSimple(Attributes inData)
        {
            return GetVaryings(inData);
        }

        half4 AdvertyLitPassFragmentSimple(Varyings input) : SV_TARGET
        {
            return GetDefaultColor();
        }

    #endif

#endif