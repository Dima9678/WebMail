import type { Letter } from './Letter';
import type { LetterState } from './LetterState';
import type { Draft } from './Draft';

export interface User {
    id: string;

    name: string;
    email: string;

    sentLetters: Letter[];
    acceptLetters: Letter[];

    letterStates: LetterState[];

    drafts: Draft[];
}
