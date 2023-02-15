Shader "Custom/NewSurfaceShader"
{
    Properties
    {
        _MainColor ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _HightlightColor ("H Color", Color) = (1,1,1,1)
        _ShadowColor ("Shadow Color", Color) = (1,1,1,1)
        _Ramp ("Ramp texture", 2D) = "White" {}
        
    
        _SColor ("Specular Color", Color) = (1,1,1,1)
        _Glossiness ("Specular size", Float) = 32
        _specularsmooth ("Specular smooth", Range(0.01, 0.5)) = 0.2 
        
    }
    
    
    
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf StandardplusCustom fullforwardshadows
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _Ramp;
        
  
        fixed4 _MainColor;
        
        
        fixed4 _HightlightColor;
        fixed4 _ShadowColor;
        
        float4 _SColor;
        float _SpecSize;
        float _Glossiness;
        float _specularsmooth;
        
        
        
        float4 LightingStandardplusCustom (SurfaceOutput s, half3 lightDir, half3 viewDir, half atten)
        
        {
             
              #ifndef USING_DIRECTIONAL_LIGHT
              lightDir = normalize(lightDir);
              #endif
              
             s.Normal = normalize(s.Normal);
             fixed ndotl = dot (s.Normal, lightDir) * 0.5 + 0.5;
             fixed ndotv = saturate(dot(s.Normal, viewDir));
             
             _ShadowColor = lerp(_HightlightColor, _ShadowColor, _ShadowColor.a);
             
             
             fixed3 ramp = tex2D(_Ramp, fixed2(ndotl,ndotv));
             ramp *= atten;
             ramp *= _LightColor0;
             ramp = lerp(_ShadowColor.rgb, _HightlightColor.rgb, ramp);
              
             
             float lightIntensity = smoothstep(0, 0.01, ndotl);
             float3 halfVector = normalize(lightDir + viewDir);
             float NdotH = dot(s.Normal,halfVector);
             
              float specularIntensity = pow(NdotH * lightIntensity, _Glossiness * _Glossiness);
              float specularIntensitySmooth = smoothstep(0.005, _specularsmooth, specularIntensity); // 스펙큘러 테두리를 쨍하게 만들고 싶으면 0,005, 0,01 넣기 
              float4 spec = specularIntensitySmooth * _SColor;
               
             
             fixed4 c;
             
             c.rgb = s.Albedo * (ramp + spec) ;
             c.a = s.Alpha;
             return c;
        
        
        
        
        
        }
        
        struct Input
        {
            float2 uv_MainTex;
        };
        
        struct SurfaceOutputCustom
        {
             fixed3 Albedo;
             fixed3 Normal;
             fixed Alpha;
        
        };

      

       
        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutput o)
        {
            
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _MainColor;
            o.Albedo = c.rgb * _MainColor.rgb;
            //o.Metallic = _Metallic;
            //o.Smoothness = _Glossiness;
            o.Alpha = c.a * _MainColor.a;
            
        }
        ENDCG
    }
    FallBack "Diffuse"
}
