using System.Buffers;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Tapper.Tests.Server.Models;

namespace Tapper.Tests.Server.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class TapperController : ControllerBase
{
    private readonly ILogger<TapperController> _logger;

    public TapperController(ILogger<TapperController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public int Test0()
    {
        return 0;
    }

    [HttpPost]
    public Type2? Test1(Type2 type2)
    {
        var result1 = type2.DateTime.Year == 2021 && type2.DateTime.Month == 6 && type2.DateTime.Day == 4;
        var result2 = type2.Uri == new Uri("http://example.com/1");

        if (result1 && result2)
        {
            _logger.Log(LogLevel.Information, "Test1 ok");
            return new Type2(new DateTime(2022, 02, 06, 0, 0, 0, DateTimeKind.Utc), new Uri("http://example.com/2"));
        }

        _logger.Log(LogLevel.Information, "Test1: {obj}", JsonSerializer.Serialize(type2));

        return null;
    }

    [HttpPost]
    public Type3? Test2(Type3 type3)
    {
        if (type3.CustomTypeList.Count != 2)
        {
            _logger.Log(LogLevel.Error, "Test2: {obj}", JsonSerializer.Serialize(type3));
        }

        if (type3.CustomTypeList[0].Name is "nana" && type3.CustomTypeList[0].Value is 15
            && type3.CustomTypeList[1].Name is "junna" && type3.CustomTypeList[1].Value is 25)
        {
            var o1 = new Type1(Guid.Parse("3cc614b1-bbce-4677-afb2-9e8dc48c7677"), "maya", 18);
            var o2 = new Type1(Guid.Parse("ee1868fa-7ff2-4699-932a-bc9f331aea11"), "kuro", 11);

            var ret = new Type3(new() { o1, o2 });

            _logger.Log(LogLevel.Information, "Test2 ok");
            _logger.Log(LogLevel.Information, "Test2 input: {obj}", JsonSerializer.Serialize(type3));

            return ret;
        }

        _logger.Log(LogLevel.Error, "Test2: {obj}", JsonSerializer.Serialize(type3));

        return null;
    }

    [HttpPost]
    public Type4? Test4(Type4 type4)
    {
        if (type4.MyEnum is MyEnum.One && type4.Value is 99)
        {
            return new Type4(MyEnum.Four, 7);
        }

        return null;
    }

    [HttpPost]
    public Type5? Test5(Type5 type5)
    {
        if (type5.Value == 'R')
        {
            return new Type5('S');
        }

        return null;
    }

    [HttpPost]
    public Type6? Test6(Type6 type6)
    {
        if (type6.Binary.Length == 3
            && type6.Binary[0] == 0
            && type6.Binary[1] == 7
            && type6.Binary[2] == 99)
        {
            return new Type6(new byte[] { 99, 7, 0 });
        }

        return null;
    }

    [HttpPost]
    public Type7? Test7(Type7 type7)
    {
        var ans = DateTimeOffset.Parse("2022-09-17T16:29:55.2260000+09:00");

        if (type7.DateTimeOffset == ans)
        {
            return new Type7(DateTimeOffset.Parse("2022-09-17T16:51:00.4600000+09:00"));
        }

        return null;
    }
}
