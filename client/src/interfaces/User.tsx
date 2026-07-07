import type { Letter } from './Letter';

export interface User {
    id: string;

    name: string;
    email: string;

    sentLetters: Letter[];
    acceptLetters: Letter[];
}
