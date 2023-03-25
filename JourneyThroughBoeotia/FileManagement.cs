using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using JourneyThroughBoeotia;

namespace JourneyThroughBoeotia;

public class FileManagement
{
    public static void DumpASTAsDot(string filepath, Ast ast)
    {
        string text = "digraph {\n" + ast.ToDot(0) + "}\n";

        File.WriteAllTextAsync(filepath, text);
    }

    public static void ExtractVariables(string filepath)
    {
        string[] lines = File.ReadAllLines(filepath);

        lines = lines.Where(line => !line.Trim().StartsWith("#") && !string.IsNullOrWhiteSpace(line)).ToArray();

        foreach (string line in lines)
        {
            int equalsIndex = line.IndexOf("=");

            if (equalsIndex == -1)
            {
                throw new InvalidDataException();
            }

            string variableName = line.Substring(0, equalsIndex).Trim();
            string variableExpression = line.Substring(equalsIndex + 1).Trim();

            Queue<Token> variableQueue = Lexer.Lex(variableExpression);
            Stack<Token> variableStack = Parser.ShuntingYard(variableQueue);
            Ast variableAst = Parser.StackToAst(variableStack);

            if (AstIdentifier.Variables.ContainsKey(variableName))
            {
                AstIdentifier.Variables[variableName] = variableAst;
            }
            else
            {
                AstIdentifier.Variables.Add(variableName, variableAst);
            }
        }
    }
    
    public static double ComputeExpression(string filepath)
    {
        string[] lines = File.ReadAllLines(filepath);

        string lastLine = lines.Last().Trim();

        Queue<Token> expressionQueue = Lexer.Lex(lastLine);
        Stack<Token> expressionStack = Parser.ShuntingYard(expressionQueue);
        Ast expressionAst = Parser.StackToAst(expressionStack);

        return expressionAst.Evaluate();
    }
}