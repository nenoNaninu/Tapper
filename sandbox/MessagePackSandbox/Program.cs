
using System.Text.Json;
using MessagePack;



var obj = new DateTest()
{
    Date = DateTime.Now,
    Value = (99, "revue starlight")
};

var bin = MessagePackSerializer.Serialize(obj);

var obj2 = MessagePackSerializer.Deserialize<DateTest>(bin);

Console.WriteLine(obj2);


//var chunkList = new List<Chunk>();

//chunkList.Add(new Chunk(0, 0));
//chunkList.Add(new Chunk(0, 1));
//chunkList.Add(new Chunk(2, 2));
//chunkList.Add(new Chunk(2, 3));
//chunkList.Add(new Chunk(3, 4));

//var g = chunkList.GroupBy(x => x.Type);

//var gj = MessagePackSerializer.Serialize(g);

//var gobj = MessagePackSerializer.Deserialize<IEnumerable<IGrouping<int, Chunk>>>(gj);

//foreach (var item in gobj!)
//{
//    Console.WriteLine(item);
//}

[MessagePackObject(true)]
public class Chunk
{
    public int Type { get; set; }
    public int Value { get; set; }

    public Chunk()
    {
    }

    public Chunk(int type, int value)
    {
        Type = type;
        Value = value;
    }
}

[MessagePackObject(true)]
public class DateTest
{
    public DateTime Date { get; set; }
    public (int, string?) Value { get; set; }
}
