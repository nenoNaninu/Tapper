import { MyEnum, Type2, Type3, Type4, Type5, Type6 } from './generated/json/Tapper.Tests.Server.Models';
import { randomUUID } from 'crypto';
import fetch from 'node-fetch';

export const fetchType2 = async () => {
    const obj: Type2 = {
        DateTime: new Date("2021-06-04"),
        Uri: "http://example.com/1",
    }

    const response = await fetch("http://localhost:5100/tapper/test1", {
        method: "POST",
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(obj)
    });

    const data = await response.json() as Type2;

    return { Uri: data.Uri, DateTime: typeof data.DateTime === 'string' ? new Date(data.DateTime) : data.DateTime };
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
        body: JSON.stringify(obj),
        headers: { 'Content-Type': 'application/json' },
    });

    return await response.json() as Type3;
}


export const fetchType4 = async () => {
    const obj: Type4 = {
        MyEnum : MyEnum.One,
        Value: 99
    };

    const response = await fetch("http://localhost:5100/tapper/test4", {
        method: "POST",
        body: JSON.stringify(obj),
        headers: { 'Content-Type': 'application/json' },
    });

    return await response.json() as Type3;
}

export const fetchType5 = async () => {
    const obj: Type5 = {
        Value: "R"
    };

    const response = await fetch("http://localhost:5100/tapper/test5", {
        method: "POST",
        body: JSON.stringify(obj),
        headers: { 'Content-Type': 'application/json' },
    });

    return await response.json() as Type5;
}

export const fetchType6 = async () => {
    const obj: Type6 = {
        Binary: Buffer.from([0, 7, 99]).toString("base64")
    }

    const response = await fetch("http://localhost:5100/tapper/test6", {
        method: "POST",
        body: JSON.stringify(obj),
        headers: { 'Content-Type': 'application/json' },
    });

    const ret = await response.json() as Type6;

    return ret;
}
