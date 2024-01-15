Shader "Adverty/AdShader"
{
    Properties
    {
        [MainColor] _BaseColor("Color", Color) = (1, 1, 1, 1)
        [MainTexture] [NoScaleOffset] _BaseMap("Albedo", 2D) = "white" {}
        [Gamma] _Metallic("Metallic", Range(0.0, 1.0)) = 0.0
        _Smoothness("Smoothness", Range(0.0, 1.0)) = 0.5

        [HideInInspector]_StencilWriteMaskID("Stencil Write Mask ID", Float) = 0
        [HideInInspector]_WatermarkTex("_WatermarkTex (RGB)", 2D) = "white" {}
        [HideInInspector]_WatermarkIsVisible("Watermark is Visible", Float) = 1
        [HideInInspector]_WatermarkUvSize("Watermark UV size", Vector) = (0, 0, 0, 0)
        [HideInInspector]_FadeTexture("_FadeTexture (RGB)", 2D) = "white"{}
        [HideInInspector]_TransitionProgress("Transition Progress", float) = 0
        [HideInInspector]_FadeTexUVFactor("_FadeTextureUVFactor", int) = 0
        [HideInInspector]_MainTexUVFactor("_MainTextureUVFactor", int) = 0
    }

    SubShader
    {
        Tags{"RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline"}

        Pass
        {
            Name "ForwardLit"
            Tags{"LightMode" = "UniversalForward"}

            Blend One Zero
            ZWrite On
            Cull Back

            HLSLPROGRAM

            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0

            #pragma shader_feature ADVERTY_URP_COMPAT
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
            #pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
            #pragma multi_compile _ _SHADOWS_SOFT
            #pragma multi_compile _ _MIXED_LIGHTING_SUBTRACTIVE

            #pragma multi_compile _ DIRLIGHTMAP_COMBINED
            #pragma multi_compile _ LIGHTMAP_ON
            #pragma multi_compile_fog
            #pragma multi_compile_instancing

            #pragma vertex LitPassVertex
            #pragma fragment AdvertyLitPassFragment

            #include "AdvertyURP.hlsl"

#ifdef ADVERTY_URP_COMPAT
            #include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/LitForwardPass.hlsl"

            half4 AdvertyLitPassFragment(Varyings input) : SV_Target
            {
                float2 fadeUV = input.uv.xy;
                fadeUV.y = lerp(fadeUV.y, 1.0f - fadeUV.y, _FadeTexUVFactor);
                input.uv.y = lerp(input.uv.y, 1.0f - input.uv.y, _MainTexUVFactor);

                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

                SurfaceData surfaceData;
                InitializeStandardLitSurfaceData(input.uv, surfaceData);

                InputData inputData;
                InitializeInputData(input, surfaceData.normalTS, inputData);

                half3 albedoColor = lerp(surfaceData.albedo, SAMPLE_TEXTURE2D(_FadeTexture, sampler_FadeTexture, fadeUV).rgb, _TransitionProgress);
                half4 color = UniversalFragmentPBR(inputData, albedoColor, surfaceData.metallic, surfaceData.specular, surfaceData.smoothness, surfaceData.occlusion, surfaceData.emission, surfaceData.alpha);
                color.rgb = MixFog(color.rgb, inputData.fogCoord);
                float2 mainUv = input.uv;
                mainUv.y = lerp(mainUv.y, 1.0f - mainUv.y, _MainTexUVFactor);
                return lerp(color, addWatermark(mainUv, color), _WatermarkIsVisible);
            }
#endif
            ENDHLSL
        }


        Pass
        {
            Name "ShadowCaster"
            Tags{"LightMode" = "ShadowCaster"}

            ZWrite On
            ZTest LEqual
            Cull Back

            HLSLPROGRAM

            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0

            #pragma shader_feature _ALPHATEST_ON
            #pragma shader_feature ADVERTY_URP_COMPAT

            #pragma multi_compile_instancing
            #pragma shader_feature _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A

            #pragma vertex ShadowPassVertex
            #pragma fragment ShadowPassFragment

#ifdef ADVERTY_URP_COMPAT
            #include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/ShadowCasterPass.hlsl"
#else 
            #include "AdvertyURP.hlsl"
#endif
            ENDHLSL
        }

        Pass
        {
            Name "DepthOnly"
            Tags{"LightMode" = "DepthOnly"}

            ZWrite On
            ColorMask 0
            Cull Back

            HLSLPROGRAM
            
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0

            #pragma vertex DepthOnlyVertex
            #pragma fragment DepthOnlyFragment

            #pragma shader_feature _ALPHATEST_ON
            #pragma shader_feature _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
            #pragma shader_feature ADVERTY_URP_COMPAT

            #pragma multi_compile_instancing

#ifdef ADVERTY_URP_COMPAT
            #include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/DepthOnlyPass.hlsl"
#else 
            #include "AdvertyURP.hlsl"
#endif

            ENDHLSL
        }

        Pass
        {
            Name "Meta"
            Tags{"LightMode" = "Meta"}

            Cull Off

            HLSLPROGRAM
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x

            #pragma vertex UniversalVertexMeta
            #pragma fragment UniversalFragmentMeta

            #pragma shader_feature _SPECULAR_SETUP
            #pragma shader_feature _EMISSION
            #pragma shader_feature _METALLICSPECGLOSSMAP
            #pragma shader_feature _ALPHATEST_ON
            #pragma shader_feature _ _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
            #pragma shader_feature _SPECGLOSSMAP
            #pragma shader_feature ADVERTY_URP_COMPAT

#ifdef ADVERTY_URP_COMPAT
            #include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/LitMetaPass.hlsl"
#else 
            #include "AdvertyURP.hlsl"
#endif

            ENDHLSL
        }

        Pass
        {
            Name "Universal2D"
            Tags{ "LightMode" = "Universal2D" }

            Blend[_SrcBlend][_DstBlend]
            ZWrite On
            Cull Back

            HLSLPROGRAM

            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x

            #pragma vertex vert
            #pragma fragment frag

            #pragma shader_feature _ALPHATEST_ON
            #pragma shader_feature _ALPHAPREMULTIPLY_ON
            #pragma shader_feature ADVERTY_URP_COMPAT

#ifdef ADVERTY_URP_COMPAT
            #include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/Utils/Universal2D.hlsl"
#else 
            #include "AdvertyURP.hlsl"
#endif

            ENDHLSL
        }
    }

    SubShader
    {
        Tags{ "RenderType" = "Opaque" "DisableBatching" = "True" }

        Stencil
        {
            Ref 255
            WriteMask[_StencilWriteMaskID]
            Pass Replace
        }

        CGPROGRAM

        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0
        #include "AdvertyCG.cginc"

        half _Metallic;
        half _Smoothness;

        void surf(Input input, inout SurfaceOutputStandard o)
        {
            fixed4 color = GetAdvertyColor(input.uv_BaseMap, 0);
            o.Albedo = color;
            o.Metallic = _Metallic;
            o.Smoothness = _Smoothness;
            o.Alpha = 1.0;
        }

        ENDCG
    }
}