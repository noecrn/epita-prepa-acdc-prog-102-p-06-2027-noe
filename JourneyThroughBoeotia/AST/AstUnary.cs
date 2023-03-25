using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using JourneyThroughBoeotia;

namespace JourneyThroughBoeotia;

public class AstUnary : Ast
{
    public Ast Argument { get; }
    public Token.TokenType Operation { get; }

    public AstUnary(Token.TokenType operation, Ast argument)
    {
        Argument = argument;
        Operation = operation;
    }

    public override double Evaluate()
    {
        var value = Argument.Evaluate();
        switch (Operation)
        {
            case Token.TokenType.TOKEN_UNARY_PLUS:
                return value;
            case Token.TokenType.TOKEN_UNARY_MINUS:
                return -value;
            default:
                throw new InvalidTokenException();
        }
    }

    public override string ToDot(int id)
    {
        return $"NODE_{id} [label=\"{stringOperator(Operation)}\"];\n" +
               $"NODE_{id} -> NODE_{2 * id + 1};\n" +
               $"NODE_{2 * id + 1} [label=\"{Argument.Evaluate()}\"];\n";
    }

    public override double Solve(double target)
    {
        return 1;
    }
}