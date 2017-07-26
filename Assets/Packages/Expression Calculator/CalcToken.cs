using System.Collections.Generic;

namespace Expressions
{
   public partial class Calculator
   {
      public enum CalcMode { Numeric, Logic};

      public static class Token
      {
         public const string PRight = ")", PLeft = "(", Power = "^", Divide = "/",
                             Multiply = "*", UnaryMinus = "_", Add = "+", Subtract = "-",
                             Factorial = "!", Mod = "%",
                             Sentinel = "#", End = ";", Store = "=", None = " ",
                             Separator = ",";

         public const string Sine = "sin", Cosine = "cos", Tangent = "tan",
                    ASine = "asin", ACosine = "acos", ATangent = "atan",
                    Log = "log", Log10 = "log10", Ln = "ln", Exp = "exp",
                    Abs = "abs", Sqrt = "sqrt", Root = "rt";

         //BoolAnd = "&", BoolNot = "!", BoolOr = "|", BoolImp = ">", BoolXor = "^",

	      private static HashSet<string> binaryOperators = new HashSet<string>( new[] {
		      
			  Multiply, Divide, Subtract, Add,
		      Power, Log, Root, Mod
	      } );

	      private static HashSet<string> unaryOperators = new HashSet<string>( new[] {
		      
			  Subtract, Sine, Cosine, Tangent, ASine,
		      ACosine, ATangent, Log10, Ln, Exp,
		      Abs, Sqrt
	      } );

		  private static HashSet<string> specialOperators = new HashSet<string>( new[] { Sentinel, End, Store, None, Separator, PRight } );

		  private static HashSet<string> rightSideOperators = new HashSet<string>( new[] { Factorial } );

	      private static HashSet<string> FunctionList = new HashSet<string>( new[] {
		      
			  Sine, Cosine, Tangent, ASine, ACosine,
		      ATangent, Log, Log10, Ln, Exp, Abs,
		      Sqrt, Root
	      } );

	      private static HashSet<string> lastProcessedOperators = new HashSet<string>( new[] { Power } ); // 2^3^4 = 2^(3^4)

         private static int Precedence(string op)
         {

            switch (op)
            {
               case Subtract:    return 4;
               case Add:         return 4;
               case UnaryMinus:  return 8;
               case Multiply:    return 24;
               case Divide:      return 24;
               case Mod:         return 32;
               case Factorial:   return 48;
			   case Power:		 return 56;
               case PLeft:       return 64;
               case PRight:      return 64;
               
               //default: return 0; //operators END, Sentinel, Store
            }

			if ( Token.IsFunction( op ) ) return 64;
	         return 0;
         }

         /// <summary>
         /// 
         /// </summary>
         /// <param name="op1"></param>
         /// <param name="op2"></param>
         /// <returns></returns>
         public static int Compare(string op1, string op2)
         {
            if (op1 == op2 && lastProcessedOperators.Contains( op1 ))
               return -1;
            else
               return Precedence(op1) >= Precedence(op2) ? 1 : -1;
         }

         #region Is... Functions
         public static bool IsBinary(string op)
         {
            return binaryOperators.Contains( op );
         }

         public static bool IsUnary(string op)
         {
            return unaryOperators.Contains( op );
         }

         public static bool IsRightSide(string op)
         {
            return rightSideOperators.Contains( op );
         }

         public static bool IsSpecial(string op)
         {
            return specialOperators.Contains( op );
         }

         public static bool IsName(string token) {

			 foreach ( var each in token ) {

				 if ( ( each < '0' || each > '9' ) && ( each <'a' || each > 'z' ) ) {

					 return false;
				 }
			 }

	         return true;
         }

         public static bool IsDigit(string token) {

			 foreach ( var each in token ) {
				 
				 if ( each < '0' || each > '9' ) {
					 
					 return false;
				 }
			 }

			 return true;
         }

         public static bool IsFunction(string op)
         {
            return FunctionList.Contains( op );
         }

         #endregion

         /// <summary>
         /// Converts operator from expression to driver-comprehensible mode
         /// </summary>
         /// <param name="op">Unary operator</param>
         /// <returns>Converted operator</returns>
         public static string ConvertOperator(string op)
         {
            switch (op)
            {
               case "-": return "_";
               default: return op;
            }
         }

         public static string ToString(string op)
         {
            switch (op)
            {
               case End: return "END";
               default: return op;
            }
         }
      }
   }
}
