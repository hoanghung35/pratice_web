import { UUID } from 'crypto';

export interface Currency {
    id: UUID;
    currentName: string;
    exchangeRate: number;
}
