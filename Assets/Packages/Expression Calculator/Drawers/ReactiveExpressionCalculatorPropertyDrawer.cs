using UnityEngine;

#if UNITY_EDITOR
    using UnityEditor; 
#endif

using System.Collections;

namespace Expressions {

#if UNITY_EDITOR
    public class ReactiveExpressionAttribute : PropertyAttribute { }

	[CustomPropertyDrawer( typeof( ReactiveExpressionAttribute ) )]
	public class ReactiveExpressionCalculatorPropertyDrawer : PropertyDrawer {

		private const int resultStringSize = 70;

		public override void OnGUI( Rect rect, SerializedProperty property, GUIContent label ) {

			property.serializedObject.Update();

			var targetObject = property.serializedObject.targetObject;
			var target = fieldInfo.GetValue( targetObject ) as ReactiveCalculator;

			var result = target.Result.Value;
			var resultString = string.Format( " = {0}", result );
			//var resultStringSize = GUI.skin.label.CalcSize( new GUIContent( resultString ) ) * 2;

			GUI.color = target.IsValid ? Color.white : Color.red;

			var contentPosition = EditorGUI.PrefixLabel( rect, label );
			contentPosition.width -= resultStringSize;

			UpdateDirtyProperties( target, property );

			target.Expression = EditorGUI.TextField( contentPosition, target.Expression );

			GUI.color = Color.white;

			contentPosition.x += contentPosition.width;
			contentPosition.width = resultStringSize;

			EditorGUI.LabelField( contentPosition, resultString );
		}

		private void UpdateDirtyProperties( ReactiveCalculator target, SerializedProperty property ) {

			var currentValue = target.Expression;
			property.FindPropertyRelative( "_expression" ).stringValue = "";
			property.FindPropertyRelative( "_expression" ).stringValue = currentValue;

			property.serializedObject.ApplyModifiedProperties();
		}
	}

#endif
}