using UnityEngine;
using System.Collections;

namespace Expressions {

	public partial class Calculator {

		private const string anonymousVariableFormat = "x{0}";

		public double Evaluate( string expression, params double[] variables ) {

			var index = 1;
			foreach ( var each in variables ) {

				SetVariable( string.Format( anonymousVariableFormat, index++ ), each );
			}

			return Evaluate( expression );
		}
	}
}