using System;

namespace Tapper;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum)]
public class TranspilationSourceAttribute : Attribute
{
}
