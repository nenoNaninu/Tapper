using System.Globalization;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;


var o = new C1()
{
    Array = new Dictionary<int, string>()
    {
        { 0, "ssss" },
        { 1, "bbbbb" }
    }
};

var j = JsonSerializer.Serialize(o);

Console.WriteLine(j);

var o2 = JsonSerializer.Deserialize<C1>(j);

Console.WriteLine(o2);

public class C1
{
    public IReadOnlyDictionary<int, string>? Array { get; set; }
}


//var s = @"{""DateTime"":""2021-06-04T00:00:00.000Z "",""Uri "":""http://example.com/1""}";

//var t = typeof(int);

//Console.WriteLine(t.FullName);
//var d = new Type2(DateTime.Parse("2022-02-06"), new Uri("http://example.com/2"), (null, 99, "orchestra"));
//var js = JsonSerializer.Serialize(d);
//Console.WriteLine(js);

//var d = DateTime.Now.ToUniversalTime();

//var j = JsonSerializer.Serialize(d);
//Console.WriteLine(j);

//var date = new DateTime(2021, 6, 4, 0, 0, 0, DateTimeKind.Utc);
//Console.WriteLine(date.ToString("o"));

//var span = TimeSpan.FromHours(-9);

//var j = JsonSerializer.Serialize(span);
//Console.WriteLine(j);

//string js = @"{
//    ""DateTime"" : ""2022-03-20T07:16:11.285Z"",
//    ""Span"" : -540,
//}";

//var o2 = JsonSerializer.Deserialize<DatetimeWrap>(js);

//Console.WriteLine(o2.ToString());


//var c = new Chunk(0, 2);

//Console.WriteLine(s);

//var c2 = JsonSerializer.Deserialize<Chunk>(s);


//Console.WriteLine(c2);




//var stack = new Stack<int>();

//stack.Push(0);
//stack.Push(1);
//stack.Push(2);
//stack.Push(3);
//stack.Push(5);
//stack.Push(7);

//var json = JsonSerializer.Serialize(stack);

//Console.WriteLine(json);

//var obj = JsonSerializer.Deserialize<Stack<int>>(json);

//foreach (var item in obj!)
//{
//    Console.WriteLine(item);
//}


//var chunkList = new List<Chunk>();

//chunkList.Add(new Chunk(0, 0));
//chunkList.Add(new Chunk(0, 1));
//chunkList.Add(new Chunk(2, 2));
//chunkList.Add(new Chunk(2, 3));
//chunkList.Add(new Chunk(3, 4));

//var g = chunkList.GroupBy(x => x.Type);


//var gj = JsonSerializer.Serialize(g, new JsonSerializerOptions { Encoder = JavaScriptEncoder.Create(UnicodeRanges.All), WriteIndented = true });
////var r = g[0];

//Console.WriteLine(gj);

//var gobj = JsonSerializer.Deserialize<ILookup<int, Chunk>>(gj);

//foreach (var item in gobj!)
//{
//    Console.WriteLine(item);
//}


public class Chunk
{
    public int Type { get; }
    public int Value { get; }


    //[JsonConstructor]
    public Chunk(int type, int value)
    {
        Type = type;
        Value = value;
    }
}

public class DatetimeWrap
{
    public DateTime DateTime { get; set; }
    public TimeSpan Span { get; set; }
}

public class Type2
{
    public DateTime DateTime { get; }
    public Uri? Uri { get; }

    // tuple containing nullable
    public (int?, int, string) TupleValue { get; }

    public Type2(DateTime dateTime, Uri? uri, (int?, int, string) tupleValue)
    {
        DateTime = dateTime;
        Uri = uri;
        TupleValue = tupleValue;
    }
}
