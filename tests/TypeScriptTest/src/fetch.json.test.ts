import { fetchType2, fetchType3, fetchType4, fetchType5, fetchType6, fetchType7 } from './fetch.json'
import { MyEnum, Type2, Type3, Type4, Type5, Type6, Type7 } from './generated/json/Tapper.Tests.Server.Models'

test('fetch1.json', async () => {
    const res = await fetchType2();
    const gt: Type2 =
    {
        dateTime: new Date('2022-02-06T00:00:00Z'),
        uri: "http://example.com/2",
    }
    expect(res).toEqual(gt);
});

test('fetch2.json', async () => {
    const res = await fetchType3();
    const gt: Type3 =
    {
        customTypeList: [
            { id: "3cc614b1-bbce-4677-afb2-9e8dc48c7677", name: "maya", value: 18 },
            { id: "ee1868fa-7ff2-4699-932a-bc9f331aea11", name: "kuro", value: 11 },
        ]
    }
    expect(res).toEqual(gt);
});

test('fetch4.json', async () => {
    const res = await fetchType4();
    const gt: Type4 =
    {
        myEnum: MyEnum.Four,
        value: 7
    }
    expect(res).toEqual(gt);
});


test('fetch5.json', async () => {
    const res = await fetchType5();
    const gt: Type5 =
    {
        value: "S"
    }
    expect(res).toEqual(gt);
});

test('fetch6.json', async () => {
    const res = await fetchType6();

    const gt: Type6 =
    {
        binary: Buffer.from([99, 7, 0]).toString("base64")
    }

    expect(res).toEqual(gt);
});

test('fetch7.json', async () => {
    const res = await fetchType7();

    const gt: Type7 = 
    {
        referencedType: 
        {
            customType2:
            {
                value: 1337,
                value2: 12.02,
                dateTime: new Date('2022-02-06T00:00:00.000Z'),
                dateTime2: new Date('2022-09-04T00:00:00.000Z')
            }
        }
    }

    expect(res).toEqual(gt);
});
