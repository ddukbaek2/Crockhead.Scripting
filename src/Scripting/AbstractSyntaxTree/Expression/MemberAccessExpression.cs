namespace Crockhead.Scripting
{
	/// <summary>
	/// 멤버 접근 표현식.
	/// </summary>
	public sealed class MemberAccessExpression : IExpression
	{
		/// <summary>
		/// 대상.
		/// </summary>
		public IExpression Target { get; }

		/// <summary>
		/// 이름.
		/// </summary>
		public string Name { get; }

		/// <summary>
		/// 생성됨.
		/// </summary>
		public MemberAccessExpression(IExpression target, string name)
		{
			Target = target;
			Name = name;
		}

		/// <summary>
		/// 평가.
		/// </summary>
		public Variable Evaluate(Context context)
		{
			var targetVariable = Target.Evaluate(context);
			if (targetVariable.Type == ValueType.Struct)
			{
				var sv = (StructValue)targetVariable.Value;
				if (sv.Fields.TryGetValue(Name, out var v)) return v;
				return Variable.Null();
			}
			return Variable.Null();
		}

		/// <summary>
		/// 구조체에 변수 설정.
		/// </summary>
		public void Set(Context context, Variable value)
		{
			var targetVariable = Target.Evaluate(context);
			if (targetVariable.Type != ValueType.Struct)
				throw new System.Exception("구조체 필드가 아님");

			var structValue = (StructValue)targetVariable.Value;
			structValue.Fields[Name] = value;
		}
	}
}
