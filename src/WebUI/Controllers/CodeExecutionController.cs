using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace CleanArchitecture.WebUI.Controllers;

public class CodeExecutionController : ApiControllerBase
{
    [HttpPost]
    public async Task<IActionResult> ExecuteCode([FromBody] string code)
    {
        try
        {
            var defaultNamespaces = new[]
            {
                "using System",
                "using System.Collections.Generic",
                // Add more as needed
            };
            
            /*var staticAssemblies = defaultNamespaces
                .Select(ns => Assembly.Load(ns))
                .Where(assembly => !assembly.IsDynamic)
                .ToList();*/
            var dfNsp = string.Join(';', defaultNamespaces);
            code = dfNsp + ";" +  code;

            var methodCall = $"new {GetClassName(code)}().Sum(1,1)";
            code += methodCall;
            var result = await CSharpScript.EvaluateAsync(code);
            return Ok(new { Result = result.ToString() });
        }
        catch (CompilationErrorException compilationError)
        {
            return BadRequest(new { Errors = compilationError.Diagnostics });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }



    private string GetClassName(string code)
    {
        // Define a regular expression pattern to match the class content
        string pattern = @"class\s+([\w\d_]+)\s*{";

        // Match the pattern in the input code
        Match match = Regex.Match(code, pattern);

        if (match.Success)
        {
            // Extract the content between '{' and '}'
            string classContent = match.Groups[1].Value.Trim();
            return classContent;
        }
        else
        {
            // Pattern not found
            return "Class content not found.";
        }

        var className = GetStringAfterWord(code, "class");
        return className;
    }
    
    string GetStringAfterWord(string input, string searchWord)
    {
        int index = input.IndexOf(searchWord);

        if (index != -1)
        {
            // Add the length of the search word to get the position after the word
            int startIndex = index + searchWord.Length;

            // Use Substring to get the part of the string after the word
            string result = input.Substring(startIndex);

            return result;
        }

        // Return null if the word is not found
        return null;
    }
  //  "class Program{ static int FindMax(int num1, int num2) {int result;if (num1 > num2)result = num1;else result = num2;return result;}static void Main(string[] args) {int a = 100;int b = 200;int ret = FindMax(a, b); Console.Write(ret);}}"
}