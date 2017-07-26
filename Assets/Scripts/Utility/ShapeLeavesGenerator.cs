using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;

#endif

public class ShapeLeavesGenerator : MonoBehaviour {

	public MeshFilter shape;

	public float leafScale = 0.2f;

	public float normalFactor = 0.2f;

	public GameObject leafPrefab;

	public Material leavesMaterial;

	[HideInInspector]
	[SerializeField]
	private GameObject leavesObject;

	private void Reset() {

		shape = GetComponent<MeshFilter>();
	}

#if UNITY_EDITOR
	[ContextMenu( "Save leaves" )]
	private void SaveLeaves() {

		AssetDatabase.CreateAsset( leavesObject.GetComponent<MeshFilter>().sharedMesh, "Assets/" + shape.transform.root.name + "_" + shape.transform.name + "_leaves.asset" );
	}
#endif

	[ContextMenu( "Generate" )]
	private void Generate() {

		if ( leavesObject == null ) {

			leavesObject = transform.OfType<Transform>().Select( _ => _.gameObject ).FirstOrDefault( where => where.name == "Leaves" );
		}

		if ( leavesObject == null ) {

			leavesObject = new GameObject( "Leaves" );
			leavesObject.transform.SetParent( transform, worldPositionStays: false );
		}

		var leavesMesh = new Mesh();

		var mesh = shape.sharedMesh;

		var prefabMeshes = leafPrefab.GetComponentsInChildren<MeshFilter>( includeInactive: true );

		var combineInstances = new List<CombineInstance>();

		for ( var i = 0; i < mesh.vertexCount; i += 1 ) {

			foreach ( var each in prefabMeshes ) {

				var eachMesh = Instantiate( each.sharedMesh );

				eachMesh.normals = eachMesh.normals.Select( _ => -_ * normalFactor + Vector3.up ).ToArray();
				combineInstances.Add( new CombineInstance {
					mesh = eachMesh,
					transform =
						Matrix4x4.TRS( mesh.vertices[i], Quaternion.FromToRotation( Vector3.up, mesh.normals[i] ), Vector3.one * leafScale )
				} );
			}
		}

		leavesMesh.CombineMeshes( combineInstances.ToArray(), mergeSubMeshes: true, useMatrices: true );

		var leavesMeshFilter = leavesObject.GetComponent<MeshFilter>() != null ? leavesObject.GetComponent<MeshFilter>() : leavesObject.AddComponent<MeshFilter>();
		DestroyImmediate( leavesMeshFilter.sharedMesh, allowDestroyingAssets: true );
		leavesMeshFilter.sharedMesh = leavesMesh;

		var leavesMeshRenderer = leavesObject.GetComponent<MeshRenderer>() != null ? leavesObject.GetComponent<MeshRenderer>() : leavesObject.AddComponent<MeshRenderer>();
		leavesMeshRenderer.sharedMaterial = leavesMaterial;
	}

}