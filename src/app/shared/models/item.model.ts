import { UUID } from "crypto";

export interface Item {
    id: UUID;
    itemCode: string;   // not itemcode
    enName: string;
    vnName: string;
    deptName: string;
    maker: string;
    supplier: string;
    positionIn: string;
    quantity: number;
    deptId: string;
    unit: string;
    cost: number;
    image: string;
    currency: string;
}
