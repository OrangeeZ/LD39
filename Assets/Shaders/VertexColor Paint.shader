Shader "Custom/VertexColor Paint" {
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

		struct Input {
		    float4 color : COLOR;
			float3 viewDir;
		};

		half4 _RimColor;
		half _RimPower;
		fixed4 _Color;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = IN.color;

			o.Albedo = Luminance(IN.color) > 0.99 ? _Color.rgb : c.rgb;
			o.Metallic = 0.5;
			
			half rim = 1.0 - saturate(dot (normalize(IN.viewDir), o.Normal));
          	o.Emission = _RimColor.rgb * pow (rim, _RimPower);
			  
			o.Alpha = c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
