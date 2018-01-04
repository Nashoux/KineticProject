Shader "Unlit/Aura"
{
	Properties
	{
	_MainTex ("Texture", 2D) = "white" {}
	_Color ("Color", COLOR) = (1,0,0,1)
	_Size("Aura size", float) = 1.5
	}
	SubShader
	{

	Tags { 	"LightMode"="ForwardBase"	}
		LOD 100		 

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			#include "Lighting.cginc"

            #pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight
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
				 fixed shadow = SHADOW_ATTENUATION(i);
 				fixed3 lighting = i.diff * shadow + i.ambient;				
				fixed4 col = tex2D(_MainTex, i.uv);
				col.rgb *= lighting;
				return col;
			}
			ENDCG
		}

		  Tags
        { 
            "RenderType" = "Transparent" 
            "Queue" = "Transparent+100" 
			"LightMode"="ForwardBase"
        }
		LOD 100

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

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
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
				col.w =  abs(i.normal.x*i.normal.z)/3;
				return col;
			}
			ENDCG
		}
	}
}
