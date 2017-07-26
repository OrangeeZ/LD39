Shader "Custom/WarFogQuadRenderer" {
	Properties {
		_MainTex("Main texture", 2D) = "white" {}
		_Color ("Color", Color) = (1,1,1,1)
	}
	SubShader {
		Tags { "RenderType"="Transparent" "RenderQueue"="Overlay" }
		LOD 200
		ZTest Always
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Lambert keepalpha
		#pragma fragmentoption ARB_precision_hint_fastest
			
		struct Input {
			float2 uv_MainTex;
			float3 viewDir;
		};

		fixed4 _Color;
		uniform sampler2D _MainTex;

		void surf (Input IN, inout SurfaceOutput o) {
			// Albedo comes from a texture tinted by color
			//o.Albedo = _Color.rgb;// float3(_Color.rgb, IN.color.r);//  *_Color;
			o.Alpha = tex2D(_MainTex, IN.uv_MainTex).r;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
