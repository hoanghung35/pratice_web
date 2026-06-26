import { UUID } from 'crypto';

export interface Approve {
    id: UUID;
    picName: string;
    dept: string;
    itemName: string;
    qty: number;
    kind: string;
    requestorCode: string;
    requestorName: string;
    purpose: string;
    dateRequest: string;
    planRequest: string;
    status: string;
}
