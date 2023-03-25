using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using JourneyThroughBoeotia;

namespace JourneyThroughBoeotia;

public static class Parser
{
    public static Stack<Token> ShuntingYard(Queue<Token> tokens)
    {
        Stack<Token> outputStack = new Stack<Token>();
        Stack<Token> operatorStack = new Stack<Token>();
        while (tokens.Count > 0)
        {
            var token = tokens.Dequeue();
            switch (token.Type)
            {
                case Token.TokenType.TOKEN_NUMBER:
                case Token.TokenType.TOKEN_IDENTIFIER:
                    outputStack.Push(token);
                    break;
                case Token.TokenType.TOKEN_FUNCTION:
                    operatorStack.Push(token);
                    break;
                case Token.TokenType.TOKEN_BINARY_ADD:
                case Token.TokenType.TOKEN_BINARY_SUB:
                case Token.TokenType.TOKEN_BINARY_MUL:
                case Token.TokenType.TOKEN_BINARY_DIV:
                case Token.TokenType.TOKEN_BINARY_POW:
                    while (operatorStack.Count > 0 && operatorStack.Peek().Type != Token.TokenType.TOKEN_LEFT_PARENTHESIS && (GetOperatorProperties(token.Type).precedence < GetOperatorProperties(operatorStack.Peek().Type).precedence || GetOperatorProperties(token.Type).precedence == GetOperatorProperties(operatorStack.Peek().Type).precedence) && GetOperatorProperties(token.Type).associativity == "left")
                    {
                        outputStack.Push(operatorStack.Pop());
                    }
                    operatorStack.Push(token);
                    break;
                case Token.TokenType.TOKEN_UNARY_PLUS:
                case Token.TokenType.TOKEN_UNARY_MINUS:
                    while (operatorStack.Count > 0 &&
                           operatorStack.Peek().Type != Token.TokenType.TOKEN_LEFT_PARENTHESIS &&
                           ((GetOperatorProperties(token.Type).precedence < GetOperatorProperties(operatorStack.Peek().Type).precedence) ||
                            (GetOperatorProperties(token.Type).precedence == GetOperatorProperties(operatorStack.Peek().Type).precedence)) && GetOperatorProperties(token.Type).associativity == "right")
                    {
                        outputStack.Push(operatorStack.Pop());
                    }
                    operatorStack.Push(token);
                    break;
                case Token.TokenType.TOKEN_LEFT_PARENTHESIS:
                    operatorStack.Push(token);
                    break;
                case Token.TokenType.TOKEN_RIGHT_PARENTHESIS:
                    while (operatorStack.Count > 0 && operatorStack.Peek().Type != Token.TokenType.TOKEN_LEFT_PARENTHESIS)
                    {
                        outputStack.Push(operatorStack.Pop());
                    }
                    if (operatorStack.Count == 0)
                    {
                        throw new MismatchedParenthesisException();
                    }
                    operatorStack.Pop();
                    if (operatorStack.Count > 0 && operatorStack.Peek().Type == Token.TokenType.TOKEN_FUNCTION)
                    {
                        outputStack.Push(operatorStack.Pop());
                    }
                    break;
            }
        }
        while (operatorStack.Count > 0)
        {
            if (operatorStack.Peek().Type == Token.TokenType.TOKEN_LEFT_PARENTHESIS || operatorStack.Peek().Type == Token.TokenType.TOKEN_RIGHT_PARENTHESIS)
            {
                throw new MismatchedParenthesisException();
            }
            outputStack.Push(operatorStack.Pop());
        }
        return outputStack;
    }

    private static (int precedence, string associativity) GetOperatorProperties(Token.TokenType tokenOperator)
    {
        switch (tokenOperator)
        {
            case Token.TokenType.TOKEN_BINARY_POW:
                return (4, "right");
            case Token.TokenType.TOKEN_BINARY_MUL:
            case Token.TokenType.TOKEN_BINARY_DIV:
                return (3, "left");
            case Token.TokenType.TOKEN_BINARY_ADD:
            case Token.TokenType.TOKEN_BINARY_SUB:
                return (2, "left");
            case Token.TokenType.TOKEN_UNARY_PLUS:
            case Token.TokenType.TOKEN_UNARY_MINUS:
            case Token.TokenType.TOKEN_LEFT_PARENTHESIS:
            case Token.TokenType.TOKEN_RIGHT_PARENTHESIS:
                return (0, "right");
            default:
                return (0, "right");
        }
    }
    
    private static Ast StackToAstRecurse(Stack<Token> rpn)
    {
        Token obj = rpn.Pop();
        
        if (obj.Type == Token.TokenType.TOKEN_IDENTIFIER)
        {
            return new AstIdentifier(obj.Value);
        }
        
        if (obj.Type == Token.TokenType.TOKEN_NUMBER)
        {
            return new AstNumber(double.Parse(obj.Value, new CultureInfo("en-Us")));
        }
        
        if (obj.Type == Token.TokenType.TOKEN_FUNCTION)
        {
            switch (obj.Value)
            {
                case "cos":
                    return new AstFunction("cos", Math.Cos, Math.Acos, StackToAst(rpn));
                case "sin":
                    return new AstFunction("sin", Math.Sin, Math.Asin, StackToAst(rpn));
                case "tan":
                    return new AstFunction("tan", Math.Tan, Math.Atan, StackToAst(rpn));
                case "ln":
                    return new AstFunction("ln", Math.Log, Math.Exp, StackToAst(rpn));
                case "exp":
                    return new AstFunction("exp", Math.Exp, Math.Log, StackToAst(rpn));
                default:
                    throw new InvalidCastException();
            }
        }

        if (obj.Type == Token.TokenType.TOKEN_UNARY_PLUS ||
            obj.Type == Token.TokenType.TOKEN_UNARY_MINUS)
        {
            return new AstUnary(obj.Type, StackToAstRecurse(rpn));
        }

        if (obj.Type == Token.TokenType.TOKEN_BINARY_ADD ||
            obj.Type == Token.TokenType.TOKEN_BINARY_DIV ||
            obj.Type == Token.TokenType.TOKEN_BINARY_MUL ||
            obj.Type == Token.TokenType.TOKEN_BINARY_POW ||
            obj.Type == Token.TokenType.TOKEN_BINARY_SUB)
        {
            Ast right = StackToAstRecurse(rpn);
            Ast left = StackToAstRecurse(rpn);
            return new AstBinary(obj.Type, left, right);
        }
        return null;
    }
    public static Ast StackToAst(Stack<Token> rpn)
    {
        if (rpn.Count == 0)
        {
            throw new InvalidSyntaxException();
        }

        Ast ast = StackToAstRecurse(rpn);

        if (rpn.Count != 0)
            throw new InvalidSyntaxException();
        
        return ast;
    }
}