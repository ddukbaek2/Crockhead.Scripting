using System.Collections.Generic;


namespace Crockhead.Scripting
{
	/// <summary>
	/// 호출 표현식.
	/// </summary>
	public sealed class CallExpression : IExpression
	{
		/// <summary>
		/// 이름 프로퍼티. (이 경우는 호출 표현식으로 호출 될 함수 이름)
		/// </summary>
		public string Name { get; }

		/// <summary>
		/// 함수 호출시 전달되는 인자 목록 프로퍼티.
		/// </summary>
		public List<IExpression> Arguments { get; }

		/// <summary>
		/// 생성됨.
		/// </summary>
		public CallExpression(string name, List<IExpression> arguments)
		{
			Name = name;
			Arguments = arguments;
		}

		/// <summary>
		/// 평가.
		/// </summary>
		public Variable Evaluate(ExpressionContext context)
		{
			var evaluatedVariables = new Variable[Arguments.Count];
			for (var i = 0; i < Arguments.Count; ++i)
			{
				evaluatedVariables[i] = Arguments[i].Evaluate(context);
			}

			return context.Call(Name, evaluatedVariables);
		}
	}
}