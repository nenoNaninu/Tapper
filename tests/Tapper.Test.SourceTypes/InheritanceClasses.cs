using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tapper.Test.SourceTypes;

[TranspilationSource]
public class InheritanceClass0
{
    public int Value0 { get; set; }
}

[TranspilationSource]
public class InheritanceClass1 : InheritanceClass0
{
    public string InheritanceString1 { get; set; } = "";
}

[TranspilationSource]
public class InheritanceClass2 : InheritanceClass0
{
    public string? InheritanceString2 { get; set; } = null;
}
