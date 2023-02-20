Shader "Adverty/AdSpriteUnlitShader"
{
    Properties
    {
        [PerRendererData] [MainTexture] _BaseMap ("Texture", 2D) = "white" {}
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
            "Queue"="Transparent"
	        "IgnoreProjector"="True"
	        "RenderType"="Opaque"
	        "PreviewType"="Plane"
	        "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off

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
            #pragma fragment AdvertySpriteFrag
            #include "AdvertyCG.cginc"

            fixed4 AdvertySpriteFrag(v2f i, fixed isFacing : VFACE) : SV_TARGET
            {
                float2 uvData = i.uv;
                uvData.x = lerp(uvData.x, 1.0f - uvData.x, isFacing < 0);
                return GetAdvertyColor(uvData, 0);
            }

            ENDCG
        }
    }
}
