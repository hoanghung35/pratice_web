import { UUID } from "crypto";

export interface Request {
    id: UUID;
    picName: string;
    itemName: string;
    qty: number;
    kind: string;
    requestorName: string;
    requestorCode: string;
    reason: string;
    dateRequest: string;
    planRequest: string;
    status: string;
}
