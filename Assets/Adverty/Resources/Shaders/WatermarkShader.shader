Shader "Hidden/WatermarkShader"
{
    Properties
    {

    }
    SubShader
    {
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            struct huvf
            {
                float factor;
                float index;
            };

            sampler2D _BackgroundTex;
            float4 _BackgroundTex_TexelSize;
            fixed4 _BackgroundSlice;
            fixed4 _BackgroundColor;

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;

            fixed4 _TextOffset;
            fixed4 _TextColor;

            sampler2D _FontTex;
            float4 _FontTex_TexelSize;

            int _UvsLength;
            float _BaseLine;

            float4 _Uvs[64];
            float4 _Verts[64];
            float _Rotated[64];

            huvf horizontalUvFactor(int i, float uvx, huvf IN)
            {
                float x = _Verts[i].x * _MainTex_TexelSize.x;
                float z = _Verts[i].z * _MainTex_TexelSize.x;

                float xf = (uvx - x) / z;

                float cond = step(0, xf) * step(xf, 1);

                huvf o;

                o.factor = lerp(IN.factor, xf, cond);
                o.index = lerp(IN.index, i, cond);

                return o;
            }

            float verticalUvFactor(int i, float uvy)
            {
                float y = _Verts[i].y * _MainTex_TexelSize.y;
                float w = _Verts[i].w * _MainTex_TexelSize.y;

                float bl = _BaseLine * _MainTex_TexelSize.y;

                float yf = (uvy - (bl + y)) / w;
                float cond = step(0, yf) * step(yf, 1);

                return lerp(0, yf, cond);
            }

            float2 calcUv(int i, float xf, float yf)
            {
                float2 uv;
                float rotated = _Rotated[i];
                float x = lerp(xf, yf, rotated);
                float y = lerp(yf, xf, rotated);

                uv.x = lerp(_Uvs[i].x, _Uvs[i].z, x);
                uv.y = lerp(_Uvs[i].y, _Uvs[i].w, y);

                return uv;
            }

            float slice(float uv, float aSo, float tw, float ts)
            {
                float aDo = aSo * tw * ts;

                float f = clamp(uv / aDo, 0, 1);
                float s = lerp(0.0, aSo, f);

                f = clamp((uv - aDo) / (1 - (aDo * 2)), 0, 1);
                s = lerp(s, 1 - aSo, f);

                f = clamp((uv - (1 - aDo)) / aDo, 0, 1);
                s = lerp(s, 1, f);

                return s;
            }

            fixed4 drawBackground(fixed2 uv)
            {
                float x = slice(uv.x, _BackgroundSlice.x, _BackgroundTex_TexelSize.z, _MainTex_TexelSize.x);
                float y = slice(uv.y, _BackgroundSlice.y, _BackgroundTex_TexelSize.w, _MainTex_TexelSize.y);

                return tex2D(_BackgroundTex, float2(x, y));
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 bg = drawBackground(i.uv);

                float2 uv;

                uv = i.uv - _TextOffset.xy;

                huvf xf;

                xf.factor = 0;
                xf.index = 0;

                for(int i = 0; i < _UvsLength; i++)
                {
                    xf = horizontalUvFactor(i, uv.x, xf);
                }

                float yf = verticalUvFactor(xf.index, uv.y);
                uv = calcUv(xf.index, xf.factor, yf);

                fixed4 text = tex2D(_FontTex, uv);

                return fixed4(lerp(_BackgroundColor.rgb, _TextColor.rgb, text.a), bg.r);
            }
            ENDCG
        }
    }
}
