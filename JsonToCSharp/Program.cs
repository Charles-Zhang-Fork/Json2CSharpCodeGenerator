using Xamasoft.JsonClassGenerator.CodeWriterConfiguration;
using Xamasoft.JsonClassGenerator.CodeWriters;
using Xamasoft.JsonClassGenerator.Models;
using Xamasoft.JsonClassGenerator;
using System.IO;
using System;
using System.Linq;
using System.Collections.Generic;

namespace JsonToCSharp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2 && args.Length != 0 || args.First() == "--help") 
            {
                Console.WriteLine("JsonToCSharp <Input File> <Output File>");
                return;
            }

            if (args.Length == 2)
            {
                string inputPath = args[0];
                string outputPath = args[1];

                string jsonText = File.ReadAllText(inputPath);
                string cSharpSnippet = Generate(jsonText, out string errorMessage);
                File.WriteAllText(outputPath, cSharpSnippet);
                Console.WriteLine(errorMessage);
            }
            else if (args.Length == 0)
            {
                string line;
                List<string> lines = new List<string>();
                while (!string.IsNullOrEmpty(line = Console.ReadLine()))
                    lines.Add(line);

                string jsonText = string.Join(Environment.NewLine, lines);
                string cSharpSnippet = Generate(jsonText, out string errorMessage);

                if (!string.IsNullOrEmpty(errorMessage))
                {
                    Console.WriteLine("Error: ");
                    Console.WriteLine(errorMessage);
                    Console.WriteLine();
                }
                Console.WriteLine("Result:");
                Console.WriteLine(cSharpSnippet);
            }
        }

        private static string Generate(string input, out string errorMessage)
        {
            CSharpCodeWriterConfig csharpCodeWriterConfig = new CSharpCodeWriterConfig
            {
                OutputMembers = OutputMembers.AsProperties,
                UsePascalCase = true
            };
            CSharpCodeWriter csharpCodeWriter = new CSharpCodeWriter(csharpCodeWriterConfig);
            JsonClassGenerator jsonClassGenerator = new JsonClassGenerator
            {
                CodeWriter = csharpCodeWriter
            };

            return jsonClassGenerator
                    .GenerateClasses(input, out errorMessage)
                    .ToString();
        }
    }
}
