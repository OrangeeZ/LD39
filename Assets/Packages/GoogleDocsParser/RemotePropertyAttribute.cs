using System;
using UnityEngine;
using System.Collections;

[AttributeUsage( validOn: AttributeTargets.Parameter | AttributeTargets.Field, AllowMultiple = false )]
public class RemotePropertyAttribute : Attribute {

	public readonly string PropertyName;

	public RemotePropertyAttribute( string propertyName ) {
		PropertyName = propertyName;
	}

}
