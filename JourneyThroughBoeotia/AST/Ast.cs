using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using JourneyThroughBoeotia;

namespace JourneyThroughBoeotia;

public abstract class Ast
{
    public abstract double Evaluate();
    
    public abstract string ToDot(int id);

    public string stringOperator(Token.TokenType type)
    {
        switch (type)
        {
            case Token.TokenType.TOKEN_BINARY_ADD:
            case Token.TokenType.TOKEN_UNARY_PLUS:
                return "+";
            case Token.TokenType.TOKEN_BINARY_DIV:
                return "/";
            case Token.TokenType.TOKEN_BINARY_MUL:
                return "*";
            case Token.TokenType.TOKEN_BINARY_POW:
                return "^";
            case Token.TokenType.TOKEN_BINARY_SUB:
            case Token.TokenType.TOKEN_UNARY_MINUS:
                return "-";
            default:
                throw new InvalidTokenException();
        }
    }

    public abstract double Solve(double target);
}