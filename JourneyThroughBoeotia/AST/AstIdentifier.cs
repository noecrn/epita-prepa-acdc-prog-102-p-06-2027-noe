using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using JourneyThroughBoeotia;

namespace JourneyThroughBoeotia;

public class AstIdentifier : Ast
{
    public string Name { get; }
    public static Dictionary<string, Ast> Variables;

    public AstIdentifier(string name)
    {
        Name = name;
    }

    public override double Evaluate()
    {
        if (!Variables.ContainsKey(Name))
        {
            throw new UnknownVariableException();
        }
        return Variables[Name].Evaluate();
    }

    public override string ToDot(int id)
    {
        return $"NODE_{id} [label=\"{Name}\"];\n";
    }

    public override double Solve(double target)
    {
        return 1;
    }
}