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

## 문법
상세 문법은 작성 예정입니다.
### script.txt
~~~text
// 주석.
function doSomething() {
	var num = 1 + 2 + 3;
	var message = "Hello Scripting: " + num;
	print(message);
}
~~~
### CS
~~~cs
using Crockhead.Scripting;
using System.IO;
...
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