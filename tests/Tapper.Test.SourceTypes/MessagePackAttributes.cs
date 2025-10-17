namespace MessagePack;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = true)]
public class MessagePackObjectAttribute : Attribute
{
    public bool KeyAsPropertyName { get; private set; }

    public MessagePackObjectAttribute(bool keyAsPropertyName = false)
    {
        this.KeyAsPropertyName = keyAsPropertyName;
    }
}

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
public class KeyAttribute : Attribute
{
    public int? IntKey { get; private set; }

    public string? StringKey { get; private set; }

    public KeyAttribute(int x)
    {
        this.IntKey = x;
    }

    public KeyAttribute(string x)
    {
        ArgumentNullException.ThrowIfNull(x);
        this.StringKey = x;
    }
}


[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
public class IgnoreMemberAttribute : Attribute
{
}
