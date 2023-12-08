using CleanArchitecture.WebUI.Controllers;
using NUnit.Framework;

namespace CleanArchitecture.Application.UnitTests.Controllers;

public class CodeExecutionTest
{
    [Test]
    public void ExecuteCodeTest()
    {
        CodeExecutionController p = new CodeExecutionController();
        string code = "public class Language { public int Sum(int a, int b){return a + b;}}";
        p.ExecuteCode(code);
        
    }
}