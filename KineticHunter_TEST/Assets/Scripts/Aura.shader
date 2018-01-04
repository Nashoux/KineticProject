Shader "Unlit/Aura"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Color ("Color", Color) = (1,1,1,1)
		_ColorIntensity ("Aura Color Intensity", Range(0,3)) = 0.5
		_NormalCheck ("Aura Normal To Check", Range(-1,1)) = 0.5
		_Size("Aura size", float) = 1.5
		_AuraPower("is the aura important or transp", float) = 1
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
			
			"LightMode"="ForwardBase"
		}
		LOD 200


		Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"
			#include "Lighting.cginc"

<<<<<<< HEAD
			#pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight
=======
			 #pragma multi_compile_forwardadd nolightmap nodirlightmap nodynlightmap novertexlight
			 
>>>>>>> 1d8a273189ce6a0e2762b5ea45d9c46f9af6fa1f
            #include "AutoLight.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
				float4 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
				SHADOW_COORDS(1) //TEXCOORD1
                float4 vertex : SV_POSITION;
				float3 diff : COLOR0;
				fixed3 ambient : COLOR1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				float3 worldNormal = UnityObjectToWorldNormal(v.normal);

                float nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));
                o.diff = nl * _LightColor0.rgb;
                o.ambient = ShadeSH9(half4(worldNormal,1));
                TRANSFER_SHADOW(o);

                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);

				fixed shadow = SHADOW_ATTENUATION(i);
                fixed3 lighting = i.diff * shadow + i.ambient;
                col.rgb *= lighting;
				
                return col;
            }
            ENDCG
        }
		

		Tags
		{
			"RenderType"="Opaque"
<<<<<<< HEAD
=======
			"Queue" = "Transparent"
			
>>>>>>> 1d8a273189ce6a0e2762b5ea45d9c46f9af6fa1f
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
			uniform fixed _ColorIntensity;
			uniform fixed _NormalCheck;
			uniform float _Size;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			half _Glossiness;
			half _Metallic;
			float _Scale, _Speed, _Frequency;
			float _AuraPower;

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
<<<<<<< HEAD
				col.w =  abs(i.normal.x*i.normal.z) * ((i.normal.x > _NormalCheck || i.normal.y > _NormalCheck || i.normal.z > _NormalCheck) ? 0 : _ColorIntensity);
=======
				col.w =  abs(i.normal.x*i.normal.z)*_AuraPower;
>>>>>>> 1d8a273189ce6a0e2762b5ea45d9c46f9af6fa1f
				return col;
			}
			ENDCG
		}
		UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
	}
	
	FallBack "Diffuse"
	
}
