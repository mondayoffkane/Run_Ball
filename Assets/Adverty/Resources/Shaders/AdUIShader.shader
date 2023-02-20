Shader "Adverty/AdUIShader"
{
    Properties
    {
        [NoScaleOffset] [MainTexture] _MainTex("Base (RGB)", 2D) = "white" {}
        [MainColor] _BaseColor("Color", Color) = (1, 1, 1, 1)
        [HideInInspector]_StencilWriteMaskID("Stencil Write Mask ID", Float) = 0
        [HideInInspector]_WatermarkTex("_WatermarkTex (RGB)", 2D) = "white" {}
        [HideInInspector]_WatermarkIsVisible("Watermark is Visible", Float) = 1
        [HideInInspector]_WatermarkUvSize("Watermark UV size", Vector) = (0, 0, 0, 0)
        [HideInInspector]_FadeTexture("_FadeTex (RGB)", 2D) = "white"{}
        [HideInInspector]_TransitionProgress("Transition Progress", float) = 0
        [HideInInspector]_FadeTexUVFactor("_FadeTextureUVFactor", int) = 0
        [HideInInspector]_MainTexUVFactor("_MainTextureUVFactor", int) = 0

        [HideInInspector]_StencilComp("Stencil Comparison", Float) = 8
        [HideInInspector]_Stencil("Stencil ID", Float) = 0
        [HideInInspector]_StencilOp("Stencil Operation", Float) = 0
        [HideInInspector]_StencilWriteMask("Stencil Write Mask", Float) = 255
        [HideInInspector]_StencilReadMask("Stencil Read Mask", Float) = 255
        [HideInInspector]_ColorMask("Color Mask", Float) = 15
    }

        SubShader
        {
            Tags
            {
                "Queue" = "Transparent"
                "IgnoreProjector" = "True"
                "RenderType" = "Transparent"
                "PreviewType" = "Plane"
                "CanUseSpriteAtlas" = "True"
            }

            Stencil
            {
                Ref[_Stencil]
                Comp[_StencilComp]
                Pass[_StencilOp]
                ReadMask[_StencilReadMask]
                WriteMask[_StencilWriteMask]
            }

            Cull Off
            Lighting Off
            ZWrite Off
            ZTest[unity_GUIZTestMode]
            Blend SrcAlpha OneMinusSrcAlpha
            ColorMask[_ColorMask]

            Pass
            {
                Name "Default"
                CGPROGRAM
                #pragma vertex AdvertyVert
                #pragma fragment AdvertyUIFrag
                #pragma target 2.0

                #include "AdvertyCG.cginc"

                ENDCG
            }
        }
}