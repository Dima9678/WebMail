import type { User } from './User';

export interface Letter {
    id: string;
    title: string;
    text: string;

    adressee: User;
    adresseeId: string;

    recipient: User;
    recipientId: string;

    adresseeName: string;

    isReaden: boolean;
    starred: boolean;

    date: Date;
}