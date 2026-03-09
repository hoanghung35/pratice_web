import { UUID } from "crypto";

export interface Item {
    id: UUID;
    itemCode: string;   // not itemcode
    enName: string;
    vnName: string;
    quantity: number;
    deptId: UUID;       // not deptid
    areaId: UUID;       // not areaid
    unit: string;
    cost: number;
    currency: string;
    maker: string;
    supplier: string;
    image: string;
    positionIn: string; // not positionin
}
