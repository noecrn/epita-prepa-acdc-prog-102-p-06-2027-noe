using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using JourneyThroughBoeotia;

namespace JourneyThroughBoeotia;

public class InvalidTokenException : Exception {}
public class MismatchedParenthesisException : Exception {}
public class InvalidSyntaxException : Exception {}
public class UnsolvableException : Exception {}
public class UnknownVariableException : Exception {}