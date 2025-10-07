using System;


namespace Crockhead.Scripting
{
	/// <summary>
	/// 이항 표현식.
	/// </summary>
	public sealed class BinaryExpression : IExpression
	{
		/// <summary>
		/// 좌항 프로퍼티.
		/// </summary>
		public IExpression Left { get; }

		/// <summary>
		/// 좌항 프로퍼티.
		/// </summary>
		public IExpression Right { get; }

		/// <summary>
		/// 연산자.
		/// </summary>
		public string Operator { get; }

		/// <summary>
		/// 생성됨.
		/// </summary>
		public BinaryExpression(IExpression left, IExpression right, string @operator)
		{
			Left = left;
			Right = right;
			Operator = @operator;
		}

		/// <summary>
		/// 평가.
		/// </summary>
		public Variable Evaluate(Context context)
		{
			var left = Left.Evaluate(context);
			var right = Right.Evaluate(context);
			if (Operator == "+" && (left.Type == ValueType.String || right.Type == ValueType.String))
				return Variable.String(left.ToString() + right.ToString());

			var leftIsNumberLike = left.Type == ValueType.Number || left.Type == ValueType.Boolean;
			var rightIsNumberLike = right.Type == ValueType.Number || right.Type == ValueType.Boolean;
			if (!leftIsNumberLike || !rightIsNumberLike)
				throw new Exception($"연산자 '{Operator}' 는 NumberValue 타입에만 허용됩니다.");

			var a = left.ToNumber();
			var b = right.ToNumber();

			switch (Operator)
			{
				case "+": return Variable.Number(a + b);
				case "-": return Variable.Number(a - b);
				case "*": return Variable.Number(a * b);
				case "/": return Variable.Number(a / b);
				case "%": return Variable.Number(Number.Remainder(a, b));
				default: throw new Exception($"지원하지 않는 연산자 '{Operator}'");
			}
		}
	}
}