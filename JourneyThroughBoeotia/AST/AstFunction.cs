using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using JourneyThroughBoeotia;

namespace JourneyThroughBoeotia;

public class AstFunction : Ast
{
    public string Name { get; }
    public Func<double, double> Function { get; }
    public Func<double, double> Inverse { get; }
    public Ast Argument { get; }

    public AstFunction(string name, Func<double, double> function, Func<double, double> inverse, Ast argument)
    {
        Name = name;
        Function = function;
        Inverse = inverse;
        Argument = argument;
    }

    public override double Evaluate()
    {
        return Function(Argument.Evaluate());
    }

    public override string ToDot(int id)
    {
        return $"NODE_{id} [label=\"{Name}\"];\n" +
               $"NODE_{id} -> NODE_{2 * id + 1};\n" +
                Argument.ToDot(2 * id + 1);
    }

    public override double Solve(double target)
    {
        return 1;
    }
}