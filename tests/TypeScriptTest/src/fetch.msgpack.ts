import { MyEnum, Type2, Type3, Type4, Type5, Type6, Type7 } from './generated/msgpack/Tapper.Tests.Server.Models';
import { randomUUID } from 'crypto';
import { encode, decode } from "@msgpack/msgpack";
import fetch from 'node-fetch';

export const fetchType2 = async () => {
    const obj: Type2 = {
        DateTime: new Date("2021-06-04"),
        Uri: "http://example.com/1",
    }

    const response = await fetch("http://localhost:5100/tapper/test1", {
        method: "POST",
        headers: {
            'Content-Type': 'application/x-msgpack',
            'Accept': 'application/x-msgpack'
        },
        body: encode(obj)
    });

    const buf = await response.buffer()
    const ret = decode<Type2>(buf);

    return ret;
}


export const fetchType3 = async () => {
    const obj: Type3 = {
        CustomTypeList: [
            { Id: randomUUID(), Name: "nana", Value: 15 },
            { Id: randomUUID(), Name: "junna", Value: 25 }
        ]
    }

    const response = await fetch("http://localhost:5100/tapper/test2", {
        method: "POST",
        body: encode(obj),
        headers: {
            'Content-Type': 'application/x-msgpack',
            'Accept': 'application/x-msgpack'
        },
    });

    const buf = await response.buffer()
    const ret = decode<Type3>(buf);

    return ret;
}

export const fetchType4 = async () => {
    const obj: Type4 = {
        MyEnum: MyEnum.One,
        Value: 99
    }

    const response = await fetch("http://localhost:5100/tapper/test4", {
        method: "POST",
        body: encode(obj),
        headers: {
            'Content-Type': 'application/x-msgpack',
            'Accept': 'application/x-msgpack'
        },
    });

    const buf = await response.buffer()
    const ret = decode<Type4>(buf);

    return ret;
}

export const fetchType5 = async () => {
    const obj: Type5 = {
        Value: 82
    }

    const response = await fetch("http://localhost:5100/tapper/test5", {
        method: "POST",
        body: encode(obj),
        headers: {
            'Content-Type': 'application/x-msgpack',
            'Accept': 'application/x-msgpack'
        },
    });

    const buf = await response.buffer()
    const ret = decode<Type5>(buf);

    return ret;
}

export const fetchType6 = async () => {
    const obj: Type6 = {
        Binary: new Uint8Array([0, 7, 99])
    }

    const response = await fetch("http://localhost:5100/tapper/test6", {
        method: "POST",
        body: encode(obj),
        headers: {
            'Content-Type': 'application/x-msgpack',
            'Accept': 'application/x-msgpack'
        },
    });

    const buf = await response.buffer()
    const ret = decode<Type6>(buf) as Type6;

    return ret;
}

export const fetchType7 = async () => {
    const obj: Type7 = {
        ReferencedType: {
            CustomType2: {
                Value: 1337,
                Value2: 12.02,
                DateTime: new Date('2021-06-04T00:00:00.000Z'),
                DateTime2: new Date('2021-10-06T00:00:00.000Z')
            }
        }
    }

    const response = await fetch("http://localhost:5100/tapper/test7", {
        method: "POST",
        body: encode(obj),
        headers: {
            'Content-Type': 'application/x-msgpack',
            'Accept': 'application/x-msgpack'
        },
    });

    const buf = await response.buffer()
    const ret = decode<Type7>(buf) as Type7;

    ret.ReferencedType.CustomType2!.Value2 = parseFloat(ret.ReferencedType.CustomType2?.Value2.toFixed(2)!);
    return ret;
}
