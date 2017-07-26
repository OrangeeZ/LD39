using UniRx;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.Collections;

namespace Expressions {

	public class CalculatorExpression : PropertyAttribute {

	}

#if UNITY_EDITOR
	[CustomPropertyDrawer( typeof ( CalculatorExpression ) )]
	public class CalculatorExpressionPropertyDrawer : PropertyDrawer {

		private const int resultStringSize = 70;

		public override void OnGUI( Rect rect, SerializedProperty property, GUIContent label ) {

			var calculator = new Calculator();

			var result = 0d;
			var isValid = true;

			try {

				result = calculator.Evaluate( GetStringValue( property ) );
			}
			catch {

				isValid = false;
			}

			var resultString = string.Format( " = {0}", result );

			GUI.color = isValid ? Color.white : Color.red;

			var contentPosition = EditorGUI.PrefixLabel( rect, label );
			contentPosition.width -= resultStringSize;

			SetStringValue( property, EditorGUI.TextField( contentPosition, GetStringValue( property ) ) );

			GUI.color = Color.white;

			contentPosition.x += contentPosition.width;
			contentPosition.width = resultStringSize;

			EditorGUI.LabelField( contentPosition, resultString );
		}

		private string GetStringValue( SerializedProperty property ) {

			var targetValue = fieldInfo.GetValue( property.serializedObject.targetObject );

			if ( targetValue is string ) {

				return property.stringValue;
			}

			if ( targetValue is IReactiveProperty<string> ) {

				return property.FindPropertyRelative( "value" ).stringValue;
			}

			return "Unsupported value";
		}

		private void SetStringValue( SerializedProperty property, string value ) {

			var targetValue = fieldInfo.GetValue( property.serializedObject.targetObject );

			if ( targetValue is string ) {

				property.stringValue = value;
			}

			if ( targetValue is IReactiveProperty<string> ) {

				property.FindPropertyRelative( "value" ).stringValue = value;
			}

			property.serializedObject.ApplyModifiedProperties();
		}

	}

	[CustomPropertyDrawer( typeof ( ReactiveCalculator ) )]
	public class ReactiveCalculatorPropertyDrawer : PropertyDrawer {

		private const int resultStringSize = 70;

		public override void OnGUI( Rect rect, SerializedProperty property, GUIContent label ) {

			property.serializedObject.Update();

			var targetObject = property.serializedObject.targetObject;
			var target = fieldInfo.GetValue( targetObject ) as ReactiveCalculator;

			var result = target.Result.Value;
			var resultString = string.Format( " = {0}", result );

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