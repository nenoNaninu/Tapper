using System;

namespace Tapper;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Interface)]
public class TranspilationSourceAttribute : Attribute
{
}
