using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using JourneyThroughBoeotia;

namespace JourneyThroughBoeotia;

public class AstNumber : Ast
{
    public double Value { get; }

    public AstNumber(double value)
    {
        Value = value;
    }

    public override double Evaluate()
    {
        return Value;
    }

    public override string ToDot(int id)
    {
        return $"NODE_{id} [label=\"{Value}\"];\n";
    }

    public override double Solve(double target)
    {
        return 1;
    }
}