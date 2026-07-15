import type { Letter } from './Letter';
import type { User } from './User';

export interface LetterState
{
    id: string;
    letterId: string;
    letter: Letter;
    userId: string;
    user: User;
    starred: boolean;
    isDeleted: boolean;
    isRead: boolean;
}