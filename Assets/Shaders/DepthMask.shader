Shader "Custom/VertexColor" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_RimColor ("Rim Color", Color) = (0.26,0.19,0.16,0.0)
      	_RimPower ("Rim Power", Range(0.5,8.0)) = 3.0

	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		Pass {
			
			ColorMask 0
		}
	} 
	FallBack "Diffuse"
}
