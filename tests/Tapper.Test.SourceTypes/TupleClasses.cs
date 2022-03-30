using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tapper.Test.SourceTypes.Tuple;

[TranspilationSource]
public class SimpleTupleClass
{
    public (int, string) TapluField;
}

[TranspilationSource]
public class TupleClassIncludeNullable
{
    public (int?, Guid, DateTime) TapluField;
}

[TranspilationSource]
public class TupleClassNullableField
{
    public (int?, Guid, DateTime)? TapluField;
}

[TranspilationSource]
public class CustomType
{
    public int Field { get; set; }
    public int Property { get; set; }
}

[TranspilationSource]
public class TupleClassIncludeCustomType
{
    public (int?, CustomType, DateTime)? TapluField;
    public (int, CustomType?, DateTime?) TapluPropery { get; set; }
}
