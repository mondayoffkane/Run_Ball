Shader "Adverty/AdSpriteLitShader"
{
    Properties
    {
        [MainTexture] [NoScaleOffset] _BaseMap("Sprite Texture", 2D) = "white" {}
        [MainColor] _BaseColor("Color", Color) = (1, 1, 1, 1)
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
        Tags 
        { 
            "RenderType" = "Opaque"
            "RenderPipeline" = "UniversalPipeline"
            "UniversalMaterialType" = "SimpleLit"
            "IgnoreProjector" = "True"
        }

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }

            Blend One Zero
            ZWrite On
            Cull Off

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

            #pragma vertex LitPassVertexSimple
            #pragma fragment AdvertyLitPassFragmentSimple

            #include "AdvertyURP.hlsl"

#ifdef ADVERTY_URP_COMPAT
            #include "Packages/com.unity.render-pipelines.universal/Shaders/SimpleLitInput.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/SimpleLitForwardPass.hlsl"

            half4 AdvertyLitPassFragmentSimple(Varyings input) : SV_TARGET
            {
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

                float2 fadeUV = input.uv.xy;
                #if defined(SHADER_API_METAL) || defined(SHADER_API_VULKAN)
                    fadeUV.y = lerp(fadeUV.y, 1.0f - fadeUV.y, _FadeTexUVFactor);
                    input.uv.y = lerp(input.uv.y, 1.0f - input.uv.y, _MainTexUVFactor);
                #endif
                
                half3 normal = half3(1.0f, 1.0f, 1.0f);
                half4 specular = half4(1.0f, 1.0f, 1.0f, 1.0f);
                half smoothness = 0.5f;
                half3 emission = half3(0.0f, 0.0f, 0.0f);
                half alpha = 1.0f;
                
                InputData inputData;
                InitializeInputData(input, normal, inputData);

                float2 mainUV = input.uv;
                
                //Uncomment to use with URP 12.1.7 (Unity 2021.3.0+)
                //bool backFace = !(dot(inputData.normalWS, inputData.viewDirectionWS) > 0);
                bool backFace = !(dot(input.normal, input.viewDir) > 0);

                mainUV.x = lerp(mainUV.x, 1.0f - mainUV.x, backFace);
                fadeUV.x = lerp(fadeUV.x, 1.0f - fadeUV.x, backFace);
                inputData.normalWS.z *= lerp(1.0, -1.0, backFace);
                inputData.normalWS.x *= lerp(1.0, -1.0, backFace);
                half3 diffuseColor = lerp(SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, mainUV).rgb, SAMPLE_TEXTURE2D(_FadeTexture, sampler_FadeTexture, fadeUV).rgb, _TransitionProgress);
                
                //Uncomment to use with URP 12.1.7 (Unity 2021.3.0+)
                //half4 color = UniversalFragmentBlinnPhong(inputData, diffuseColor, specular, smoothness, emission, alpha, normal);
                half4 color = UniversalFragmentBlinnPhong(inputData, diffuseColor, specular, smoothness, emission, alpha);
                
                color.rgb = MixFog(color.rgb, inputData.fogCoord);
                return lerp(color, addWatermark(mainUV, color), _WatermarkIsVisible);
            }
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
            #pragma shader_feature _GLOSSINESS_FROM_BASE_ALPHA
            #pragma shader_feature ADVERTY_URP_COMPAT

            #pragma multi_compile_instancing

#ifdef ADVERTY_URP_COMPAT
            #include "Packages/com.unity.render-pipelines.universal/Shaders/SimpleLitInput.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/DepthOnlyPass.hlsl"
#else 
            #include "AdvertyURP.hlsl"
#endif
            ENDHLSL
        }

        Pass
        {
            Name "Meta"
            Tags{ "LightMode" = "Meta" }

            Cull Off

            HLSLPROGRAM

            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x

            #pragma vertex UniversalVertexMeta
            #pragma fragment UniversalFragmentMetaSimple

            #pragma shader_feature _EMISSION
            #pragma shader_feature _SPECGLOSSMAP
            #pragma shader_feature ADVERTY_URP_COMPAT

#ifdef ADVERTY_URP_COMPAT
            #include "Packages/com.unity.render-pipelines.universal/Shaders/SimpleLitInput.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/SimpleLitMetaPass.hlsl"
#else 
            #include "AdvertyURP.hlsl"
#endif

            ENDHLSL
        }
        Pass
        {
            Name "Universal2D"
            Tags{ "LightMode" = "Universal2D" }
            Tags{ "RenderType" = "Transparent" "Queue" = "Transparent" }

            HLSLPROGRAM

            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x

            #pragma vertex vert
            #pragma fragment frag
            #pragma shader_feature _ALPHATEST_ON
            #pragma shader_feature _ALPHAPREMULTIPLY_ON
            #pragma shader_feature ADVERTY_URP_COMPAT

#ifdef ADVERTY_URP_COMPAT
            #include "Packages/com.unity.render-pipelines.universal/Shaders/SimpleLitInput.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/Utils/Universal2D.hlsl"
#else 
            #include "AdvertyURP.hlsl"
#endif
            ENDHLSL
        }
    }

    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType" = "Opaque"
            "PreviewType" = "Plane"
            "CanUseSpriteAtlas" = "True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Stencil
        {
            Ref 255
            WriteMask[_StencilWriteMaskID]
            Pass Replace
        }

        CGPROGRAM

        #pragma surface surf Lambert nofog nolightmap nodynlightmap noinstancing
        #pragma target 3.0
        #include "AdvertyCG.cginc"

        void surf(Input input, inout SurfaceOutput o)
        {
            float2 uvData = input.uv_BaseMap;
            uvData.x = lerp(uvData.x, 1.0f - uvData.x, input.isFacing < 0);
            fixed4 color = GetAdvertyColor(uvData, 0);
            o.Albedo = color.rgb;
            o.Alpha = 1.0;
            o.Normal.z *= saturate(input.isFacing) * 2.0 - 1.0;
        }

        ENDCG
    }
}
