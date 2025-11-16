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
- 키워드
	- var
	- if
	- else
	- or (||)
	- and (&&)
	- for
	- continue
	- break
	- switch
	- case
	- default
	- null
	- true
	- false
	- struct
	- function

- 수식 연산자
	- +, -, *, /, %
- 비교 연산자
	- ==, !=, !{var}, &&, ||
- 주석
	- //
- 리터럴.
	- var flag = true; 
	- var num = 1.5;
	- var str = "abc";
	- var arr = [1,2,3,4,5];
	- var tuple = (1, "abc");
- 계층적 구조체, 함수 정의.
	- 호출식
		- 변수();
- 타입 힌팅. (옵션)
	- var num = 0;
	- var num: Number = 0;

## 값 타입 (대입시 복사)
- number: 숫자. (리터럴로 대입 생성, 정수와 실수 통합, 대역폭 무제한, 수식 연산자에 의해 값을 만들어 다시 대입)
- boolean: 논리. (리터럴로 대입 생성, true, false, 논리 연산자에 의한 비교문 기능)
- string: 문자열. (리터럴로 대입 생성, 길이 고정, 논리 연산자에 의한 문자열 조작 기능)
- array: 배열. (리터럴로 대입 생성, 길이 고정, 색인 연산자에 의한 요소 접근 기능)
- tuple: 튜플. (리터럴로 대입 생성, 요소 갯수 고정, 색인 연산자에 의한 요소 접근 기능)

## 선언타입 (스크립트 선언)
- function
- struct

## 참조 타입 (대입시 참조)
- callable: 함수 객체. (호출식을 참조하는 객체)
- objectable: 구조체 객체. (정의된 구조체 틀로 할당된 객체를 참조하는 객체)
- nullable: 변수 무효화 객체. (대입된 변수를 null로 재할당하여 내부의 원래 값을 무효화, 실제 할당된 값과 객체는 스코프 벗어나기전까지 스코프에 존재)

## 구조체 기반 타입 (참조 타입과 동일)
- System.Collections.Dictionary
- System.Collections.List
- System.Collections.LinkedList
- System.Collections.Set
- System.Collections.Queue
- System.Collections.Stack
- System.Collections.String
	- Length()
 	- Clear()
	- Set(var str: string)
	- At()
 	- Get() -> String:
	- GetRange(var start: number, var end: number) -> String:
	- Add(var str: String)
	- Subtract(var other: String) -> String:
	- Replace()
	- Remove()
	- Join()


## 연산자
- 수식연산자: +-*/%=
- 비교연산자: ==, !=, <, >, <=, >=, !, &&, ||

## 스크립트 특징
- static, const 구조체, 변수, 함수 없음.
- 모든 변수는 형변환 없음.
- 모든 변수는 할당을 위해 명시적 선언 필요. (var num = 0;)
- 모든 변수는 스택메모리로서 할당 된 스코프를 벗어나면 제거. (참조카운팅 개념 없음, 필요시 복제)   
- 값타입은 리터럴 생성. (숫자, 논리, 문자열, 배열) 
- 값타입은 대입시 암시적으로 값 복사. (참조로 사용할 방법은 없음)
- 참조타입은 대입시 암시적 참조. (명시적 복제 함수 호출을 통해 값타입처럼 복제 가능)
- 구조체 안에 구조체를 계층적으로 정의 가능.   
- 구조체에 상속 기능 없음. (원본과 사본의 개념으로 확장 예정)
- 구조체에 생성자 함수는 존재하지만 초기화 타이밍을 위한 것이며, 복사생성자는 지원하지 않으므로 인자는 넣을 수 없음.
- 구조체는 퍼미션을 지원하지 않음. 모든 멤버는 public 접근 가능.
- 함수 안의 함수를 계층적으로 정의 가능. (하위 스코프이므로 상위 스코프 변수 접근 가능)   
- 함수는 무조건 반환이 존재. (명시적 반환이 없을 경우 null 반환)
- 함수에서 순서적 파라메터 외에 이름으로 지정 사용 가능. (인자가 일부 누락되어도 호출이 가능)
- 함수의 이름 오버로딩을 지원하지 않음.
- 스크립트 실행은 반드시 최상위 계층 함수 호출로 시작 필요.  
- 스크립트는 동적 로딩이 전제로 이미 로드된 스크립트 내에서 해석하므로 파일 단위의 include, using 등의 지시문 사용하지 않음.   
- 스크립트에서 런타임 등록된 함수 호출.
- 스크립트 추가 로딩을 통한 확장 가능. (구조체 선언 스코프를 동일하게 가져갈 경우 머지되고 멤버의 이름이 같을 경우 덮어 씀, 네임스페이스 처럼 사용 가능)

## 지원 예정
- 함수의 필수 인자에 대한 required 키워드
- try catch finally 키워드 지원
- call()
- clone()
- hash()
- enum
- dictionary
- 연산자 오버로딩

### script.txt
~~~text
struct Application
{
	var arr = [1, 2, 3, 4, 5]

	struct Application
	{
		function Application()
		{
		}
	}
}

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