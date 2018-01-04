﻿Shader "Unlit/Aura"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Color ("Color", Color) = (1,1,1,1)
		_Size("Aura size", float) = 1.5
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_Scale ("Scale", float) = 1
		_Speed ("Speed", float) = 1
		_Frequency ("Frequency", float) = 1
	}

	SubShader
	{
		Tags
		{
			"RenderType"="Opaque"
			"Queue" = "Transparent"

		}
		LOD 200


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

            sampler2D _MainTex;
            float4 _MainTex_ST;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                return col;
            }
            ENDCG
        }

      Tags
		{
			"RenderType"="Opaque"
			
		}
		LOD 200

        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			uniform fixed4 _Color;
			uniform float _Size;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			half _Glossiness;
			half _Metallic;
			float _Scale, _Speed, _Frequency;

				struct appdata
			{
				float4 vertex : POSITION;
				float4 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float3 normal : NORMAL;
			};
			
			v2f vert (appdata v)
			{
				half offsetvert2 = v.vertex.x + v.vertex.z; //diagonal waves
				float value = _Scale * sin(_Time.w * _Speed * _Frequency + offsetvert2 );
				v.vertex.y += value; //remove for no waves
				v.normal.y += value; //remove for no waves

				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex*_Size);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.normal= v.normal.xyz;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = _Color;
				col.w =  abs(i.normal.x*i.normal.z);
				return col;
			}
			ENDCG
		}
		UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
	}
	
	FallBack "Diffuse"
	
}
