import { fetchType2, fetchType3, fetchType4, fetchType5 } from './fetch.msgpack'
import { MyEnum, Type2, Type3, Type4, Type5 } from './generated/msgpack/Tapper.Tests.Server.Models'

test('fetch1.msgpack', async () => {
    const res = await fetchType2();
    const gt: Type2 =
    {
        DateTime: new Date('2022-02-06T00:00:00Z'),
        Uri: "http://example.com/2",
    }
    expect(res).toEqual(gt);
});

test('fetch2.msgpack', async () => {
    const res = await fetchType3();
    const gt: Type3 =
    {
        CustomTypeList: [
            { Id: "3cc614b1-bbce-4677-afb2-9e8dc48c7677", Name: "maya", Value: 18 },
            { Id: "ee1868fa-7ff2-4699-932a-bc9f331aea11", Name: "kuro", Value: 11 },
        ]
    }
    expect(res).toEqual(gt);
});

test('fetch4.msgpack', async () => {
    const res = await fetchType4();
    const gt: Type4 =
    {
        MyEnum: MyEnum.Four,
        Value: 7
    }
    expect(res).toEqual(gt);
});

test('fetch5.msgpack', async () => {
    const res = await fetchType5();
    const gt: Type5 =
    {
        Value: 83
    }
    expect(res).toEqual(gt);
});
