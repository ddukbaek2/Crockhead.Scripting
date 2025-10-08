using System.Collections.Generic;


namespace Crockhead.Scripting
{
	/// <summary>
	/// 임의의 호출 표현식.
	/// <para>expr(...)</para>
	/// </summary>
	public sealed class InvokeExpression : IExpression
	{
		/// <summary>
		/// 호출자 표현식 프로퍼티.
		/// </summary>
		public IExpression Callee { get; }

		/// <summary>
		/// 인자 표현식 목록 프로퍼티.
		/// </summary>
		public List<IExpression> Arguments { get; }

		/// <summary>
		/// 생성됨.
		/// </summary>
		public InvokeExpression(IExpression callee, List<IExpression> arguments)
		{
			Callee = callee;
			Arguments = arguments;
		}

		/// <summary>
		/// 평가.
		/// </summary>
		public Variable Evaluate(Context context)
		{
			var calleeVariable = Callee.Evaluate(context);

			// 함수 타입 변수 호출.
			if (calleeVariable.Type == ValueType.Function)
			{
				var functionValue = (FunctionValue)calleeVariable.Value;
				var arguments = new List<Variable>();
				foreach (var argument in Arguments)
					arguments.Add(argument.Evaluate(context));
				return context.Call(functionValue.Value, arguments.ToArray());
			}

			// 구조체 멤버 함수 호출.
			if (Callee is MemberAccessExpression memberAccessExpression)
			{
				var targetVariable = memberAccessExpression.Target.Evaluate(context);
				if (targetVariable.Type == ValueType.Struct)
				{
					var structValue = (StructValue)targetVariable.Value;
					if (structValue.Fields.TryGetValue(memberAccessExpression.Name, out var methodVar) && methodVar.Type == ValueType.Function)
					{
						var functionValue = (FunctionValue)methodVar.Value;
						var arguments = new List<Variable>();
						foreach (var argument in Arguments)
							arguments.Add(argument.Evaluate(context));

						return context.Call(targetVariable, functionValue.Value, arguments.ToArray());
					}
				}
			}

			// 구조체 타입 호출. (생성자)
			if (calleeVariable.Type == ValueType.StructType)
			{
				var st = (StructTypeValue)calleeVariable.Value;
				var dictionary = new Dictionary<string, Variable>(System.StringComparer.OrdinalIgnoreCase);
				foreach (var f in st.Fields)
					dictionary[f] = Variable.Null();
				foreach (var kv in st.Methods)
					dictionary[kv.Key] = Variable.Function(new Function(kv.Value, context.Scope));

				return new Variable(ValueType.Struct, new StructValue(st.Name, dictionary));
			}

			throw new System.Exception("호출할 수 없는 대상");
		}
	}
}
