using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class WarFogPostEffectRenderer : MonoBehaviour {

	public static WarFogPostEffectRenderer Instance { get; private set; }

	[SerializeField]
	private Shader _shader;

	[SerializeField]
	private BlurOptimized _blurOptimized;

	private Material _material;
	private Texture2D _warFogTexture;

	private float _brightness = 1f;

	private void Awake() {

		Instance = this;
	}

	// Use this for initialization
	private void Start() {

		_material = new Material( _shader );
		SetBrightness( 1f );
	}

	private void OnRenderImage( RenderTexture src, RenderTexture dest ) {

		//var blurredWarFog = _blurOptimized.BlurTexture( _warFogTexture );

		//_material.SetTexture( "_WarFogTexture", blurredWarFog );


		Graphics.Blit( src, dest, _material );
		//Graphics.Blit( blurredWarFog, dest, _material );

		//RenderTexture.ReleaseTemporary( blurredWarFog );
	}

	public void SetTexture( WarFogSpaceMap spaceMap, Texture2D warFogTexture ) {

		_warFogTexture = warFogTexture;

		_material.SetTexture( "_WarFogTexture", _warFogTexture );

		_material.SetFloat( "_WarFogBrightness", _brightness );

		var spaceMapBounds = spaceMap.GetBounds();
		_material.SetMatrix( "_World2Texture", Matrix4x4.TRS( Vector3.zero, Quaternion.identity, new Vector3( 1f / spaceMapBounds.size.x, 0, 1f / spaceMapBounds.size.z ) ) );

		var inverseViewProjectionMatrix = ( Camera.main.projectionMatrix * Camera.main.worldToCameraMatrix ).inverse;
		_material.SetMatrix( "_ViewProjectInverse", inverseViewProjectionMatrix );

		_material.SetMatrix( "_Camera2World", Camera.main.cameraToWorldMatrix );
	}

	public void SetBrightness( float value ) {

		_brightness = value;
	}
}