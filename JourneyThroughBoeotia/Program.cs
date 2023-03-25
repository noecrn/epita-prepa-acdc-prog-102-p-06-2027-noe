using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using JourneyThroughBoeotia;
using Microsoft.Win32.SafeHandles;

namespace JourneyThroughBoeotia
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var expression = "_identifier42 * (cos (3 * 14)) ^ 2.5";
            Queue<Token> a = Lexer.Lex(expression);
            Stack<Token> b = Parser.ShuntingYard(a);
            PrintToken(b);
            Console.WriteLine("-----------------------------------------");
            Ast c = Parser.StackToAst(b);
            Console.WriteLine(c.ToDot(0));
            // Console.WriteLine(Lexer.whitespaceRemove(expression));
            // FileManagement.DumpASTAsDot("/home/noe.cornu/epita-prepa-acdc-prog-102-p-06-2027-noe.cornu/test.dot", c);
        }
        static void PrintToken(Stack<Token> tokens)
        {
            foreach (var token in tokens)
            {
                Console.WriteLine($"{token.Value} : {token.Type}");
            }
        }
    }
}