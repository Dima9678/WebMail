import type { User } from './User';
import type { LetterState } from './LetterState';
import type { FullLetter } from './FullLetter';

export interface Letter {
    id: string;
    title: string;
    text: string;

    adressee: User;
    adresseeId: string;

    recipient: User;
    recipientId: string;

    adresseeName: string;
    adresseeSurname: string;
    adresseeEmail: string;

    recipientName: string;
    recipientEmail: string;

    forwarded: boolean;
    originalAuthor: User;

    sendTime: Date;
    letterStates: LetterState[];

    childrenLetters: FullLetter[];
    parentLetter: FullLetter;
    parentLetterId: string;
}