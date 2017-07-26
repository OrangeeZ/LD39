Shader "Hidden/WarFogPostEffect"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_WarFogTexture("Texture", 2D) = "white" {}
	}
		SubShader
	{
		// No culling or depth
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

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				return o;
			}

			sampler2D _MainTex;
			sampler2D _WarFogTexture;
			sampler2D _CameraDepthTexture;
			float4x4 _World2Texture;
			float4x4 _ViewProjectInverse;
			float4x4 _Camera2World;
			fixed _WarFogBrightness;

			fixed4 frag(v2f i) : SV_Target
			{
				float4 color = tex2D(_MainTex, i.uv);
/*
				fixed depth = tex2D(_CameraDepthTexture, i.uv.xy) * 2 - 1;

				if (Linear01Depth(depth) > 0.99)
				{
					return 0;
				}
*/
				fixed4 clipSpacePosition = 0;
				clipSpacePosition.xy = i.uv.xy * 2 - 1;
#if UNITY_UV_STARTS_AT_TOP
				clipSpacePosition.y *= -1;
#endif
				clipSpacePosition.z = 0;
				clipSpacePosition.w = 1;

				fixed4 worldPosition = mul(_ViewProjectInverse, clipSpacePosition);
				worldPosition /= worldPosition.w;

				half2 worldCoords = mul(_World2Texture, worldPosition).xz - half2(0.5, 0.5);

				return lerp(half4(0, 0, 0, 0), color, tex2D(_WarFogTexture, worldCoords).r * _WarFogBrightness);
			}
			ENDCG
		}
	}
}
