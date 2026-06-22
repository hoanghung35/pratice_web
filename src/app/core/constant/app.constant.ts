import [UUID} from "node:crypto";

export const apiUrl = "http://10......../api";
export const pageSize = "";
export const urlRaise = "ng s --host _ip_host --port --open";
export interface UserInfor {
  userid: string,
  username: string,
  role: string,
  roleid: string,
  contact: string
}

export const timeShowAlter = 1200; //miliseconds

export cosnt pageOptions: number[] = [5, 10, 25, 30, 45, 60];

export const defaultKey: UUID = "d2dcc45a-a74f-472e-ac00-d99e422a1d39";
