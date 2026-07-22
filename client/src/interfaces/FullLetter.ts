import type { Letter } from "./Letter";

export interface FullLetter extends Letter {
    previousLetterId: string;
    nextLetterId: string;
    letterNumber: number;
}