import type { User } from './User';


export interface Letter {
    id: string;
    title: string;
    text: string;
    sentTime: string;
    addresseeId: string;
    addressee: User;

    recipientId: string;
    recipient: User;
    isSent: boolean;
}
