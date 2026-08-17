import type { Letter } from './Letter';
import type { LetterState } from './LetterState';
import type { Draft } from './Draft';

export interface User {
    id: string;

    name: string;
    surname: string;
    email: string;

    isMan: boolean;

    sentLetters: Letter[];
    acceptLetters: Letter[];

    letterStates: LetterState[];

    drafts: Draft[];

    spamEmails: string[];
}
