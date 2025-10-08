using System;

namespace Crockhead.Scripting
{
	/// <summary>
	/// 타입 주석이 있는 변수 선언 구문.
	/// <para>var name : Type = expr;</para>
	/// </summary>
	public sealed class TypedVariableDeclarationStatement : IStatement
	{
		/// <summary>
		/// 변수 이름.
		/// </summary>
		private readonly string m_Name;

		/// <summary>
		/// 타입 이름.
		/// </summary>
		private readonly string m_TypeName;

		/// <summary>
		/// 초기화 표현식.
		/// </summary>
		private readonly IExpression m_Initializer;

		/// <summary>
		/// 변수 이름.
		/// </summary>
		public string Name => m_Name;

		/// <summary>
		/// 타입 이름 프로퍼티.
		/// </summary>
		public string TypeName => m_TypeName;

		/// <summary>
		/// 초기화 표현식 프로퍼티.
		/// </summary>
		public IExpression Initializer => m_Initializer;

		/// <summary>
		/// 생성됨.
		/// </summary>
		public TypedVariableDeclarationStatement(string name, string typeName, IExpression initializer)
		{
			m_Name = name;
			m_TypeName = typeName;
			m_Initializer = initializer;
		}

		/// <summary>
		/// 실행.
		/// </summary>
		public void Execute(Context context)
		{
			var variable = default(Variable);

			// 초기화식이 있는 경우 평가.
			if (m_Initializer != null)
			{
				variable = m_Initializer.Evaluate(context);
			}
			else
			{
				variable = Variable.Null();
			}

			// 타입 지정이 구조체 타입일 경우 생성자 호출로 처리.
			if (!string.IsNullOrEmpty(m_TypeName))
			{
				if (context.TryGetVariable(m_TypeName, out var typeVar) && typeVar.Type == ValueType.StructType)
				{
					var st = (StructTypeValue)typeVar.Value;
					var fields = new System.Collections.Generic.Dictionary<string, Variable>(StringComparer.OrdinalIgnoreCase);
					foreach (var f in st.Fields)
						fields[f] = Variable.Null();
					variable = Variable.Struct(st.Name, fields);
				}
			}

			context.Scope.SetLocalVariable(m_Name, variable);
		}
	}
}
