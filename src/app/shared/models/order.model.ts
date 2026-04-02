import { UUID } from "crypto";

export interface Order {
    id: UUID;
    enName: string;
    vnName: string;
    maker: string;
    quantity: number;
    position: string;
    dateOrder: string;
}
