Shader "Custom/VertexColor" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_RimColor ("Rim Color", Color) = (0.26,0.19,0.16,0.0)
      	_RimPower ("Rim Power", Range(0.5,8.0)) = 3.0

	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows
		#pragma fragmentoption ARB_precision_hint_fastest

		half4 _RimColor;
		half _RimPower;

		struct Input {
			float4 color : COLOR;
			float3 viewDir;
		};

		fixed4 _Color;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = _Color;
						
			half gloss = (1 - Luminance(IN.color)) * 0.8;

			o.Albedo = c.rgb * IN.color + gloss;

			o.Metallic = gloss;
			o.Smoothness = gloss;

			half rim = 1.0 - saturate(dot (normalize(IN.viewDir), o.Normal));
          	o.Emission = _RimColor.rgb * pow (rim, _RimPower);

			o.Alpha = c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
