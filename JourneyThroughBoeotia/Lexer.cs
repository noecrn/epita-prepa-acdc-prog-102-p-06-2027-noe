using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using JourneyThroughBoeotia;

namespace JourneyThroughBoeotia;

public static class Lexer
{
    public static Queue<Token> Lex(string expression)
    {
        Queue<Token> tokens = new Queue<Token>();
        string replaceExpression = whitespaceRemove(expression);
        int i = 0;
        int n = replaceExpression.Length;
        while (i < n)
        {
            char c = replaceExpression[i];
            if (Char.IsWhiteSpace(c))
            {
                i++;
            }
            else if (Char.IsDigit(c))
            {
                int nbpoint = 0;
                string sb = "";
                while (i < n && (Char.IsDigit(replaceExpression[i]) || replaceExpression[i] == '.'))
                {
                    if (replaceExpression[i] == '.')
                    {
                        nbpoint++;
                    }

                    sb += replaceExpression[i];
                    i++;
                }

                if (nbpoint > 1)
                {
                    throw new InvalidTokenException();
                }

                tokens.Enqueue(new Token(Token.TokenType.TOKEN_NUMBER, sb));
            }
            else if (c == '(')
            {
                tokens.Enqueue(new Token(Token.TokenType.TOKEN_LEFT_PARENTHESIS, "("));
                i++;
            }
            else if (c == ')')
            {
                tokens.Enqueue(new Token(Token.TokenType.TOKEN_RIGHT_PARENTHESIS, ")"));
                i++;
            }
            else if (c == '+' || c == '-' || c == '*' || c == '/' || c == '^')
            {
                if ((c == '+' || c == '-') && (i == 0 && (Char.IsDigit(replaceExpression[i + 1]) || replaceExpression[i + 1] == '+' || replaceExpression[i + 1] == '-' || replaceExpression[i + 1] == '*' || replaceExpression[i + 1] == '/' || replaceExpression[i + 1] == '^') || i != 0 && (replaceExpression[i - 1] == '(' || !Char.IsDigit(replaceExpression[i - 1])) && Char.IsDigit(replaceExpression[i + 1])))
                {
                    switch (c)
                    {
                        case '+':
                            tokens.Enqueue(new Token(Token.TokenType.TOKEN_UNARY_PLUS, "+"));
                            i++;
                            break;
                        case '-':
                            tokens.Enqueue(new Token(Token.TokenType.TOKEN_UNARY_MINUS, "-"));
                            i++;
                            break;
                        default:
                            throw new InvalidTokenException();
                    }
                }
                else if (i != 0 && (Char.IsDigit(replaceExpression[i - 1]) || replaceExpression[i - 1] == ')'))
                {
                    switch (c)
                    {
                        case '+':
                            tokens.Enqueue(new Token(Token.TokenType.TOKEN_BINARY_ADD, "+"));
                            i++;
                            break;
                        case '-':
                            tokens.Enqueue(new Token(Token.TokenType.TOKEN_BINARY_SUB, "-"));
                            i++;
                            break;
                        case '/':
                            tokens.Enqueue(new Token(Token.TokenType.TOKEN_BINARY_DIV, "/"));
                            i++;
                            break;
                        case '*':
                            tokens.Enqueue(new Token(Token.TokenType.TOKEN_BINARY_MUL, "*"));
                            i++;
                            break;
                        case '^':
                            tokens.Enqueue(new Token(Token.TokenType.TOKEN_BINARY_POW, "^"));
                            i++;
                            break;
                        default:
                            throw new InvalidTokenException();
                    }
                }
                else
                {
                    throw new InvalidTokenException();
                }
            }
            else if (Char.IsLetter(c) || c == '_')
            {
                string temp = "";
                while (i < n && (Char.IsLetter(replaceExpression[i]) || replaceExpression[i] == '_' || Char.IsDigit(replaceExpression[i])))
                {
                    temp += replaceExpression[i];
                    i++;
                }
                switch (temp)
                {
                    case "cos":
                        tokens.Enqueue(new Token(Token.TokenType.TOKEN_FUNCTION, temp));
                        temp = "";
                        break;
                    case "sin":
                        tokens.Enqueue(new Token(Token.TokenType.TOKEN_FUNCTION, temp));
                        temp = "";
                        break;
                    case "tan":
                        tokens.Enqueue(new Token(Token.TokenType.TOKEN_FUNCTION, temp));
                        temp = "";
                        break;
                    case "ln":
                        tokens.Enqueue(new Token(Token.TokenType.TOKEN_FUNCTION, temp));
                        temp = "";
                        break;
                    case "exp":
                        tokens.Enqueue(new Token(Token.TokenType.TOKEN_FUNCTION, temp));
                        temp = "";
                        break;
                    default:
                        tokens.Enqueue(new Token(Token.TokenType.TOKEN_IDENTIFIER, temp));
                        temp = "";
                        break;
                }
            }
            else
            {
                throw new InvalidTokenException();
            }
        }

        return tokens;
    }

    public static string whitespaceRemove(string _expression)
    {
        int len = _expression.Length;
        string result = _expression[0].ToString();
        
        for (int i = 1; i < len - 1; i++)
        {
            char charMinus = _expression[i - 1];
            char c = _expression[i];
            char charAdd = _expression[i + 1];
            if (Char.IsWhiteSpace(c) && ((Char.IsDigit(charMinus) || Char.IsDigit(charAdd)) && charAdd != '+' && charAdd != '-' && charAdd != '*' && charAdd != '/' && charAdd != '^' && charMinus != '+' && charMinus != '-' && charMinus != '*' && charMinus != '/' && charMinus != '^') || 
                !Char.IsWhiteSpace(c))
            {
                result += c;
            }
        }

        result += _expression[len - 1];
        return result;
    }
}