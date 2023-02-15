Shader "LENY/Toon and Fog" {

    Properties 

    {

        [Header(Color)]
        [Space(10)]
        _MainColor ("Main Color", Color) = (1, 1, 1, 1)
        [NoScaleOffset]_MainTex ("Main Texture", 2D) = "white" {}

        [HideInInspector]_HighlightColor ("Highlight Color", Color) = (1,1,1,1)
        _ShadowColor ("Shadow Color", Color) = (0.3, 0.3, 0.3, 1)
        
        
        [Header(Rim)]
        [Space(10)]
        [Toggle(RimON)]
        _RimON ("Rim ON", Float) = 1
        _RimColor ("Rim Color", Color) = (1,1,1,1)
        _RimPower ("Rim Power", Float) = 0.39
        _RimAmount ("Rim Amount", Range(0,1))= 0.676
        _RimAngle ("Rim Angle", Range(0,1)) = 0
         
        [Header(Specular)]
        [Space(10)]
        _SColor ("Specular Color", Color) = (1,1,1,1)
        _Glossiness ("Specular size", Float) = 32
        _specularsmooth ("Specular smooth", Range(0.01, 0.5)) = 0.2 
     
        
        [Header(Ramp)]
        [Space(10)]
        [NoScaleOffset]_Ramp ("Ramp Texture", 2D) = "White" {}
        
       
        //-----Fog-----------------------------------
       
        [Header(Fog)]
        [Space(10)]
        [Toggle(FogON)]
        _FogON("Fog ON", Float) =1
        _FogColor ("Fog Color", Color) = (0.5, 0.5, 0.5, 1)

        _FogMaxHeight ("Fog Max Height", Float) = 0.0

        _FogMinHeight ("Fog Min Height", Float) = -1.0
         

    }

  
     
       

    SubShader

    {

        Tags  { "RenderType"="Opaque" }

        LOD 200

        Cull Back

        ZWrite On

  

        CGPROGRAM

  

        #pragma surface surf ToonFogColorCustom fullforwardshadows vertex:vert finalcolor:finalcolor 
        #pragma shader_feature FogON
        #pragma shader_feature RimON
        
        sampler2D _MainTex;
        
        float4 _MainColor;
        fixed4 _HighlightColor;
        fixed4 _ShadowColor;
        sampler2D _Ramp;
        
        float4 _SColor;
        float _SpecSize;
        float _Glossiness;
        float _specularsmooth;
            
        float4 _RimColor;
        float _RimPower;
        float _RimAmount;
        float _RimAngle;
        
        float4 _FogColor;
        float _FogMaxHeight;
        float _FogMinHeight;
        
        
        half4 LightingToonFogColorCustom (SurfaceOutput s, half3 lightDir, half3 viewDir, half atten)
        
        {
              #ifndef USING_DIRECTIONAL_LIGHT
              lightDir = normalize(lightDir);
              #endif
              
              s.Normal = normalize(s.Normal);
              fixed ndotl = dot(s.Normal, lightDir) * 0.5 + 0.5;
              
              fixed ndotv = saturate(dot(s.Normal, viewDir));
              _ShadowColor = lerp(_HighlightColor, _ShadowColor, _ShadowColor.a);
              
              //-----Rim------------
              float rimDot = 1-dot(s.Normal, (viewDir+_RimAngle));
              float rimIntensity = rimDot * pow(ndotl, _RimPower);
              rimIntensity = smoothstep(_RimAmount-0.5, _RimAmount +0.5, rimIntensity);
              //float4 rim = rimIntensity * _RimColor;
              
              //-----specular--------
              
              float lightIntensity = smoothstep(0, 0.01, ndotl);
              float3 halfVector = normalize(lightDir + viewDir);
              float NdotH = dot(s.Normal,halfVector);
              
              float specularIntensity = pow(NdotH * lightIntensity, _Glossiness * _Glossiness);
              float specularIntensitySmooth = smoothstep(0.005, _specularsmooth, specularIntensity); // 스펙큘러 테두리를 쨍하게 만들고 싶으면 0,005, 0,01 넣기 
              float4 spec = specularIntensitySmooth * _SColor;
               
              
              
              
              //-----ramp-----------
              fixed3 ramp = tex2D(_Ramp, fixed2(ndotl, ndotv));
              ramp *= atten; // 지우면 그림자 없어짐
              ramp *= _LightColor0;
              ramp = lerp (_ShadowColor.rgb, _HighlightColor.rgb, ramp);
              
              fixed4 c;
              
              #ifdef RimON
              c.rgb = s.Albedo  * (ramp + spec);
              c = lerp(c, _RimColor,rimIntensity);
              #else
              c.rgb = s.Albedo  * (ramp + spec) ;
              #endif
              
              c.a = s.Alpha;
              
                                    
              return c;
              
         
            
        }
  
  
        struct Input 

        {

            float2 uv_MainTex;
            float4 pos;
            
            

        };
        
        
        struct SurfaceOutputCustom
        {
             fixed3 Albedo;
             fixed3 Normal;
             fixed3 Emission;
             fixed Alpha;
             
        
        };
  

        void vert (inout appdata_full v, out Input o) 

        {

            
            o.pos = mul(unity_ObjectToWorld, v.vertex);
            
            o.uv_MainTex = v.texcoord.xy;
            

        }

  

        void surf (Input IN, inout SurfaceOutput o) 

        {

            float4 c = tex2D (_MainTex, IN.uv_MainTex);
            c *= _MainColor;
            
          
            o.Albedo = c.rgb * _MainColor.rgb ;
         
            o.Alpha = c.a * _MainColor.a;

        }

  

        void finalcolor (Input IN, SurfaceOutput o, inout fixed4 color)

        {

            #ifndef UNITY_PASS_FORWARDADD

            float lerpValue = clamp((IN.pos.y - _FogMinHeight) / (_FogMaxHeight - _FogMinHeight), 0, 1);
            fixed a = _FogColor.a;
            
            #ifdef FogON
            color = lerp (_FogColor, color, lerpValue);
            #else
            color;
            #endif
          

            #endif

        }

  

        ENDCG

    }

  

    FallBack "Diffuse"

}


