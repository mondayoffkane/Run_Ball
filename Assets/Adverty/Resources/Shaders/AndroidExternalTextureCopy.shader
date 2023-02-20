Shader "Hidden/AndroidExternalTextureCopy"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			GLSLPROGRAM

			#pragma only_renderers gles gles3
			#extension GL_OES_EGL_image_external : require
			#extension GL_OES_EGL_image_external_essl3 : enable

			#ifdef VERTEX

			varying vec2 textureCoordinates;

			void main()
			{
				textureCoordinates = gl_MultiTexCoord0.xy;
				gl_Position = gl_ModelViewProjectionMatrix * gl_Vertex;
			}

			#endif

			#ifdef FRAGMENT

			varying vec2 textureCoordinates;
			uniform samplerExternalOES _MainTex;

			void main()
			{
				vec2 mainUv = textureCoordinates;
				mainUv.y = 1.0f - mainUv.y;
				gl_FragColor = texture(_MainTex, mainUv);
			}

			#endif

			ENDGLSL
		}
	}
}
