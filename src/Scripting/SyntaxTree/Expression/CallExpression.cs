using System.Collections.Generic;


namespace Crockhead.Scripting
{
	/// <summary>
	/// 호출 표현식.
	/// </summary>
	public sealed class CallExpression : IExpression
	{
		/// <summary>
		/// 호출자 프로퍼티.
		/// </summary>
		public string Callee { get; }

		/// <summary>
		/// 아규먼트 프로퍼티.
		/// </summary>
		public List<IExpression> Arguments { get; }

		/// <summary>
		/// 생성됨.
		/// </summary>
		public CallExpression(string callee, List<IExpression> arguments)
		{
			Callee = callee;
			Arguments = arguments;
		}

		/// <summary>
		/// 평가.
		/// </summary>
		public Value Evaluate(ExpressionView expressionView)
		{
			var evaluated = new Value[Arguments.Count];
			for (var i = 0; i < Arguments.Count; ++i)
			{
				evaluated[i] = Arguments[i].Evaluate(expressionView);
			}

			return expressionView.CallByNameOrVariable(Callee, evaluated);
		}
	}
}