import type { Letter } from './Letter';

export interface User {
    id: string;

    name: string;
    email: string;
    passwordHash: string;

    letters: Letter[];
    sentLetters: Letter[];
    acceptLetters: Letter[];
}
