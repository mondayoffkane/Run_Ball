Shader "Adverty/AdUnlitShader"
{
    Properties
    {
        [MainTexture] [NoScaleOffset] _BaseMap("Texture", 2D) = "white" {}
        [MainColor]  _BaseColor("Color", Color) = (1, 1, 1, 1)

        [HideInInspector]_StencilWriteMaskID("Stencil Write Mask ID", Float) = 0
        [HideInInspector]_WatermarkTex("_WatermarkTex (RGB)", 2D) = "white" {}
        [HideInInspector]_WatermarkIsVisible("Watermark is Visible", Float) = 1
        [HideInInspector]_WatermarkUvSize("Watermark UV size", Vector) = (0, 0, 0, 0)
        [HideInInspector]_FadeTexture("_FadeTex (RGB)", 2D) = "white"{}
        [HideInInspector]_TransitionProgress("Transition Progress", float) = 0
        [HideInInspector]_FadeTexUVFactor("_FadeTextureUVFactor", int) = 0
        [HideInInspector]_MainTexUVFactor("_MainTextureUVFactor", int) = 0
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "IgnoreProjector" = "True" "RenderPipeline" = "UniversalPipeline" }

        Blend One Zero
        ZWrite On
        Cull Back

        Pass
        {
            Name "Unlit"
            HLSLPROGRAM

            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x

            #pragma vertex vert
            #pragma fragment frag

            #pragma shader_feature ADVERTY_URP_COMPAT

            #pragma multi_compile_fog
            #pragma multi_compile_instancing
            
            #include "AdvertyURP.hlsl"

#ifdef ADVERTY_URP_COMPAT
            #include "Packages/com.unity.render-pipelines.universal/Shaders/UnlitInput.hlsl"

            struct Attributes
            {
                float4 positionOS       : POSITION;
                float2 uv               : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float2 uv        : TEXCOORD0;
                float fogCoord : TEXCOORD1;
                float4 vertex : SV_POSITION;

                UNITY_VERTEX_INPUT_INSTANCE_ID
                UNITY_VERTEX_OUTPUT_STEREO
            };

            Varyings vert(Attributes input)
            {
                Varyings output = (Varyings)0;

                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_TRANSFER_INSTANCE_ID(input, output);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

                VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
                output.vertex = vertexInput.positionCS;
                output.uv = TRANSFORM_TEX(input.uv, _BaseMap);
                output.fogCoord = ComputeFogFactor(vertexInput.positionCS.z);

                return output;
            }

            half4 GetAdvertyColor(float2 uv)
            {
                float2 mainUv = uv;
                float2 fadeUV = mainUv;
                fadeUV.y = lerp(fadeUV.y, 1.0f - fadeUV.y, _FadeTexUVFactor);
                mainUv.y = lerp(mainUv.y, 1.0f - mainUv.y, _MainTexUVFactor);

                half4 color = lerp(SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, mainUv), SAMPLE_TEXTURE2D(_FadeTexture, sampler_FadeTexture, fadeUV), _TransitionProgress);
                color = lerp(color, addWatermark(mainUv, color), _WatermarkIsVisible);
                color = lerp(color, half4(0.5, 0.5, 0.5, 1), _GrayFactor);
                color *= _BaseColor;
                return color;
            }

            half4 frag(Varyings input) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

                half4 color = GetAdvertyColor(input.uv);
                half3 colorRGB = color.rgb;
                half alpha = color.a;
                colorRGB = MixFog(colorRGB, input.fogCoord);
                return half4(colorRGB, alpha);
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

            #pragma vertex ShadowPassVertex
            #pragma fragment ShadowPassFragment

#ifdef ADVERTY_URP_COMPAT
            #include "Packages/com.unity.render-pipelines.universal/Shaders/UnlitInput.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/ShadowCasterPass.hlsl"
#else 
            #include "AdvertyURP.hlsl"
#endif
            ENDHLSL
        }

        Pass
        {
            Tags{"LightMode" = "DepthOnly"}

            ZWrite On
            ColorMask 0

            HLSLPROGRAM
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0

            #pragma shader_feature _ALPHATEST_ON
            #pragma shader_feature ADVERTY_URP_COMPAT
            
            #pragma multi_compile_instancing
            
            #pragma vertex DepthOnlyVertex
            #pragma fragment DepthOnlyFragment
#ifdef ADVERTY_URP_COMPAT
            #include "Packages/com.unity.render-pipelines.universal/Shaders/UnlitInput.hlsl"
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

            #pragma shader_feature ADVERTY_URP_COMPAT

            #pragma vertex UniversalVertexMeta
            #pragma fragment UniversalFragmentMetaUnlit

#ifdef ADVERTY_URP_COMPAT
            #include "Packages/com.unity.render-pipelines.universal/Shaders/UnlitInput.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/UnlitMetaPass.hlsl"
#else
            #include "AdvertyURP.hlsl"
#endif
            ENDHLSL
        }
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "DisableBatching" = "True"}

        Stencil
        {
            Ref 255
            WriteMask[_StencilWriteMaskID]
            Pass Replace
        }

        Pass
        {
            CGPROGRAM
            #pragma vertex AdvertyVert
            #pragma fragment AdvertyFrag
            #include "AdvertyCG.cginc"
            ENDCG
        }
    }
}
