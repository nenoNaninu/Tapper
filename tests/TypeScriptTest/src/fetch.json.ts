import {
  MyEnum,
  Type2,
  Type3,
  Type4,
  Type5,
  Type6,
  Type7,
} from "./generated/json/Tapper.Tests.Server.Models";
import { randomUUID } from "crypto";
import fetch from "node-fetch";

export const fetchType2 = async () => {
  const obj: Type2 = {
    dateTime: new Date("2021-06-04"),
    uri: "http://example.com/1",
  };

  const response = await fetch("http://localhost:5100/tapper/test1", {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(obj),
  });

  const data = (await response.json()) as Type2;

  data.dateTime =
    typeof data.dateTime === "string" ? new Date(data.dateTime) : data.dateTime;

  return data;
};

export const fetchType3 = async () => {
  const obj: Type3 = {
    customTypeList: [
      { id: randomUUID(), name: "nana", value: 15 },
      { id: randomUUID(), name: "junna", value: 25 },
    ],
  };

  const response = await fetch("http://localhost:5100/tapper/test2", {
    method: "POST",
    body: JSON.stringify(obj),
    headers: { "Content-Type": "application/json" },
  });

  return (await response.json()) as Type3;
};

export const fetchType4 = async () => {
  const obj: Type4 = {
    myEnum: MyEnum.One,
    value: 99,
  };

  const response = await fetch("http://localhost:5100/tapper/test4", {
    method: "POST",
    body: JSON.stringify(obj),
    headers: { "Content-Type": "application/json" },
  });

  return (await response.json()) as Type3;
};

export const fetchType5 = async () => {
  const obj: Type5 = {
    value: "R",
  };

  const response = await fetch("http://localhost:5100/tapper/test5", {
    method: "POST",
    body: JSON.stringify(obj),
    headers: { "Content-Type": "application/json" },
  });

  return (await response.json()) as Type5;
};

export const fetchType6 = async () => {
  const obj: Type6 = {
    binary: Buffer.from([0, 7, 99]).toString("base64"),
  };

  const response = await fetch("http://localhost:5100/tapper/test6", {
    method: "POST",
    body: JSON.stringify(obj),
    headers: { "Content-Type": "application/json" },
  });

  const ret = (await response.json()) as Type6;

  return ret;
};

export const fetchType7 = async () => {
  const obj: Type7 = {
    referencesType: {
      inheritanceClass0: {
        value0: 1337,
      },
    },
  };

  const response = await fetch("http://localhost:5100/tapper/test7", {
    method: "POST",
    body: JSON.stringify(obj),
    headers: { "Content-Type": "application/json" },
  });

  const ret = (await response.json()) as Type7;

  return ret;
};
