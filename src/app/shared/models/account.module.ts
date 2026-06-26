import { UUID } from 'crypto'

export interface Account {
    id: UUID;
    userCode: string;
    email: string;
    fullName: string;
    roleName: string;
}
