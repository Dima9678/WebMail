import type { User } from './User';
import type { State } from './State';
import type { FullLetter } from './FullLetter';

export interface Letter {
    id: string;
    title: string;
    text: string;

    adressee: User;
    adresseeId: string;

    recipients: User[];

    adresseeName: string;
    adresseeSurname: string;
    adresseeEmail: string;

    recipientName: string;
    recipientEmail: string;

    forwarded: boolean;
    originalAuthor: User;

    sendTime: Date;
    state: State;

    childrenLetters: FullLetter[];
    parentLetter: FullLetter;
    parentLetterId: string;
}