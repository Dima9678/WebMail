import type { User } from './User';
import type { LetterState } from './LetterState';

export interface Letter {
    id: string;
    title: string;
    text: string;

    adressee: User;
    adresseeId: string;

    recipient: User;
    recipientId: string;

    adresseeName: string;
    adresseeEmail: string;

    recipientName: string;
    recipientEmail: string;

    sendTime: Date;
    letterStates: LetterState[];
}