using System;
using System.Collections.Generic;
using System.Text;

namespace Expressions
{
   public partial class Calculator
   {
      public delegate void CalcVariableDelegate(object sender, EventArgs e);
      public event CalcVariableDelegate OnVariableStore;

      Dictionary<string, double> variables;

      public const string AnswerVar = "r";

      private void LoadConstants() {

	      variables = new Dictionary<string, double> {{"pi", Math.PI}, {"e", Math.E}, {AnswerVar, 0}};

	      if (OnVariableStore != null)
            OnVariableStore(this, new EventArgs());
      }

      public Dictionary<string, double> Variables
      {
         get { return variables; }
      }

      public void SetVariable(string name, double val)
      {
		  variables[name] = val;

         if (OnVariableStore != null)
            OnVariableStore(this, new EventArgs());
      }

      public double GetVariable(string name)
      {  // return variable's value // if variable ha push default value, 0

	      var result = 0d;
	      
		  variables.TryGetValue( name, out result );
	      
		  return result;
      }
   }
}
