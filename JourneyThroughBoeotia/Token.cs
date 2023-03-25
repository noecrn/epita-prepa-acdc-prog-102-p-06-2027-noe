using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using JourneyThroughBoeotia;

namespace JourneyThroughBoeotia;

public class Token
{
    public TokenType Type { get; set; }
    public string Value { get; set; }

    public enum TokenType
    {
        TOKEN_NUMBER,
        TOKEN_BINARY_ADD,
        TOKEN_BINARY_SUB,
        TOKEN_BINARY_MUL,
        TOKEN_BINARY_DIV,
        TOKEN_BINARY_POW,
        TOKEN_UNARY_PLUS,
        TOKEN_UNARY_MINUS,
        TOKEN_LEFT_PARENTHESIS,
        TOKEN_RIGHT_PARENTHESIS,
        TOKEN_FUNCTION,
        TOKEN_IDENTIFIER
    }

    public Token()
    {
        
    }

    public Token(TokenType tokenType, string value)
    {
        Type = tokenType;
        Value = value;
    }
}