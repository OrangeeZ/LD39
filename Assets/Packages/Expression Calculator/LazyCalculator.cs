using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Expressions;

namespace Expressions {

	[Serializable]
	public class LazyCalculator {

		public double this[string key] {

			set {

				calculator.SetVariable( key, value );
			}
		}

		private Calculator calculator = new Calculator();

		private double? value;

		private string expression;

		public void SetExpression( string expression ) {

			if ( this.expression != expression ) {

				calculator.Clear();
				this.expression = expression;
				value = null;
			}
		}

		public double Evaluate() {

			if ( value == null ) {

				value = calculator.Evaluate( expression );
			}

			return value.Value;
		}

		public void SetVariable( string name, double value ) {

			calculator.SetVariable( name, value );
		}

		//public void SetVariable( Expression<Func<double>> memberExpression ) {

		//	calculator.SetVariable( VariableExtensions.GetMemberName( memberExpression ), memberExpression.Compile()() );
		//}

		public void SetVariables( IDictionary<string, double> variables ) {

			foreach ( var each in variables ) {

				calculator.SetVariable( each.Key, each.Value );
			}
		}
	}
}
