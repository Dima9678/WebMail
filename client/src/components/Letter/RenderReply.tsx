import type { Letter } from "../interfaces/User";

type RenderLetterProps = {
    currentLetter: Letter;
};
export default function RenderReply({
    currentLetter,
}: RenderLetterProps) {
    return (
        <div className="one-letter-reply-container">
            <div className="one-letter-header">
                <div className="one-letter-title-block">
                    <p className="one-letter-title">{currentLetter.title}</p>
                    <p className="one-letter-datetime">
                        {new Date(currentLetter.sendTime).toLocaleDateString("ru-RU")} в
                        {new Date(currentLetter.sendTime).toLocaleTimeString("ru-RU")}
                    </p>
                </div>
                <div className="one-letter-sender-block">
                    <div className="one-letter-avatar">
                        <p className="one-letter-avatar-letter">{currentLetter.adresseeName[0]}</p>
                    </div>
                    <div>
                        <p className="one-letter-adressee-name">{currentLetter.adresseeName} {currentLetter.adresseeSurname}</p>
                        <p className="one-letter-adressee-email">{currentLetter.adresseeEmail}</p>
                    </div>
                </div>
            </div>
            <p className="one-letter-text">{currentLetter.text}</p>
        </div>
    )
}