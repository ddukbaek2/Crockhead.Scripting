# Crockhead.Scripting

## 개요
.NET Standard 2.1 기반의 경량 스크립트 엔진으로, 단순한 문법과 스택 기반 실행 모델을 제공합니다.   
C#과 스크립트 간의 함수 바인딩을 지원하며 외부 라이브러리에 의존하지 않습니다.   

## 배포
[![NuGet version](https://img.shields.io/nuget/v/Crockhead.Scripting.svg?style=flat&logo=nuget)](https://www.nuget.org/packages/Crockhead.Scripting/)
[![NuGet downloads](https://img.shields.io/nuget/dt/Crockhead.Scripting.svg?style=flat)](https://www.nuget.org/packages/Crockhead.Scripting/)

Lightweight scripting engine for .NET Standard 2.1 with a simple syntax and stack-based execution model.   
Supports function binding between C# and script code with no external dependencies.   

## 설치
~~~bash
dotnet add package Crockhead.Scripting
~~~
~~~powershell
Install-Package Crockhead.Scripting
~~~

## 라이브러리 기반
- netstandard2.1   
- Crockhead.Core   

## 네임스페이스
~~~cs
using Crockhead.Scripting;
~~~

### 언어 규약
- 주석은 한줄 주석을 `//` 을 통해서 사용.
- 문자열은 쌍따옴표 `"` 를 통해서 사용.
- 조건문은 if, else if, else 사용.
- 반복문은 for 사용.
- 분기문은 switch, case, default, break, return 사용.
- 전역 스코프에서 변수 선언이 불가능하며 구조체 정의와 함수 선언만 가능. (외부에서는 전역 함수만 호출 가능)
- 변수는 반드시 함수 안에 선언되는 지역 변수이며, 자신이 선언된 함수 스코프가 종료됨과 동시에 해제됨.
- 함수는 반드시 변수를 반환. (명시하지 않으면 암시적인 null 반환)
- 함수 안에서 하위 함수 선언 및 사용 가능.
- 모든 변수는 논리, 숫자, 문자열, 함수, 널, 구조체 타입 존재.
- 구조체 정의시 멤버로 변수와 함수를 포함하여 정의하며, 이는 다른 언어들과 마찬가지로 필드와 메서드로 명명함.
- 필드는 구조체가 변수 선언이 될 때 같이 선언되며, 구조체 변수가 선언된 함수 스코프가 종료됨과 동시에 필드도 함께 해제됨.
- 메서드 안에서는 this 키워드를 통해 현재 메서드가 포함된 구조체의 변수로 접근 할 수 있으며, this를 사용하지 않아도 동일 구조체의 멤버이므로 바로 접근 가능.
- 함수 자체를 변수에 담을 수 있고, 변수를 함수처럼 호출 할 수 있음. (var f = Func(args); f(args);)
- 변수에 null을 대입할 경우 함수 객체가 선언된 함수 스코프를 벗어나지 않아도 즉시 해제됨.
- 숫자 변수는 실수와 정수를 모두 포함하는 형태이며 대역폭 제약이 없음.
- 변수는 내부적으로 갱신이 있을 때마다 매번 새로 할당됨. (전통적인 메모리 최적화 규칙을 따르지 않음)


### 기본 지원 문법
- 기본 타입 지원. (boolean, number, string, function, null)
- 수식 연산자 지원.
- 비교 연산자 지원.
- 주석 지원. (//)
- 리터럴 문자열 지원. ("str")
- 다중 지역 함수 지원.
- 구조체 지원. (struct)
- 호출식 지원. (변수();)
- 타입 힌팅 옵션 지원. (var num = 0; or var num: Number = 0;)

### 키워드
- var
- null
- true, false
- function
- return
- for
- continue, break
- if, else if, else, or, and
- switch, case, default
- struct, this

## 변수의 값 타입
- Null
- Number
- Boolean
- String
- Function
- Struct

## 연산자
- 수식연산자: +-*/%=
- 비교연산자: ==, !=, <, >, <=, >=, !

### script.txt
~~~text

function doSomething() {
	var num = (1 + 2) * 5 - 3; // 숫자는 실수와 정수를 구분하지 않고 자릿수는 무제한.
	var message = "Hello Scripting: " + num;
	print(message);
}
~~~
### CS
~~~cs
using Crockhead.Scripting;
using System.IO;


public class Application 
{
	public static void Main()
	{
		var script = File.ReadAllText("script.txt");
		var scriptEngine = new ScriptEngine();
		scriptEngine.Load(script);
		scriptEngine.Execute("doSomething");
	}
}
...
~~~