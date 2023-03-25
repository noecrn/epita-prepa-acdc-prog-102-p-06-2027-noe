using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using JourneyThroughBoeotia;

namespace JourneyThroughBoeotia;

public class AstBinary : Ast
{
    public Token.TokenType Operation { get; }
    public Ast Left { get; }
    public Ast Right { get; }

    public AstBinary(Token.TokenType operation, Ast left, Ast right)
    {
        Operation = operation;
        Left = left;
        Right = right;
    }

    public override double Evaluate()
    {
        if (Left.Evaluate() > 0.001 || Right.Evaluate() > 0.001)
        {
            switch (Operation)
            {
                case Token.TokenType.TOKEN_BINARY_ADD:
                    return Right.Evaluate() + Left.Evaluate();
                case Token.TokenType.TOKEN_BINARY_DIV:
                    return Right.Evaluate() / Left.Evaluate();
                case Token.TokenType.TOKEN_BINARY_MUL:
                    return Right.Evaluate() * Left.Evaluate();
                case Token.TokenType.TOKEN_BINARY_POW:
                    return Math.Pow(Right.Evaluate(), Left.Evaluate());
                case Token.TokenType.TOKEN_BINARY_SUB:
                    return Right.Evaluate() - Left.Evaluate();
            }
        }
        throw new InvalidTokenException();
    }

    public override string ToDot(int id)
    {
        return $"NODE_{id} [label=\"{stringOperator(Operation)}\"];\n" +
               $"NODE_{id} -> {{NODE_{2 * id + 1} NODE_{2 * id + 2}}};\n" +
               Left.ToDot(id*2+1) + Right.ToDot(id*2+2);
    }

    public override double Solve(double target)
    {
        return 1;
    }
}