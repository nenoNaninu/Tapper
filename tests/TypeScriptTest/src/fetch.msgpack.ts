import { MyEnum, Type2, Type3, Type4 } from './generated/Tapper.Tests.Server.Models';
import { randomUUID } from 'crypto';
import { encode, decode, decodeAsync, decodeArrayStream } from "@msgpack/msgpack";
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

