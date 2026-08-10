import type { Letter } from "../interfaces/Letter";
import type { User } from "../interfaces/User";

type RenderLetterProps = {
    currentLetter: Letter;
    replyMode: boolean;
    forwardMode: boolean;
    replyText: string;
    forwardEmail: string;
    errorMessage: string;
    setReplyText: React.Dispatch<React.SetStateAction<string>>;
    setForwardEmail: React.Dispatch<React.SetStateAction<string>>;
    Reply: () => void;
    Forward: () => void;
    ChangeReplyMode: () => void;
    ChangeForwardMode: () => void;
};

export default function RenderLetter({
    currentLetter,
    replyMode,
    forwardMode,
    replyText,
    forwardEmail,
    errorMessage,
    setReplyText,
    setForwardEmail,
    Reply,
    Forward,
    ChangeReplyMode,
    ChangeForwardMode,
}: RenderLetterProps) {
    return (
        <div className="one-letter-main-container">
            {currentLetter.forwarded ? (
                <>
                    <p className="one-letter-forward-from">Переслано от: </p>
                    <div className="one-letter-sender-block">
                        <div className="one-letter-avatar">
                            <p className="one-letter-avatar-letter">{currentLetter?.originalAuthor.name[0]}</p>
                        </div>
                        <div className="one-letter-adressee-name-block">
                            <p className="one-letter-adressee-name">{currentLetter?.originalAuthor.name} {currentLetter?.originalAuthor.surname}</p>
                            <p className="one-letter-adressee-email">{currentLetter?.originalAuthor.email}</p>
                        </div>
                    </div>
                </>
            ) : (
                <></>
            )}
            <div className="one-letter-header">
                <div className="one-letter-title-block">
                    <p className="one-letter-title">{currentLetter.title}</p>
                    <div className="one-letter-datetime">
                        <p className="one-letter-datetime">
                            {new Date(currentLetter.sendTime).toLocaleDateString("ru-RU")}
                            {", "}
                            {new Date(currentLetter.sendTime).toLocaleDateString("ru-RU", {
                                weekday: "short"
                            })}
                            {", "}
                            {" в "}
                            {new Date(currentLetter.sendTime).toLocaleTimeString("ru-RU")}
                        </p>
                        <p>Кому:</p>
                        {currentLetter.recipients.map((user: User) => (

                            <p>{user.email}</p>

                        ))}
                    </div>
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

            {replyMode ? (
                <div className="main-reply-container">
                    <textarea
                        className="reply-textarea"
                        placeholder="Текст письма"
                        value={replyText}
                        onChange={(e) => setReplyText(e.target.value)}>
                    </textarea>
                    <div className="reply-form-footer">
                        <p className="reply-error-message">{errorMessage}</p>
                        <div className="reply-action-buttons">
                            <button onClick={Reply} className="reply-action-button">Ответить</button>
                            <button onClick={ChangeReplyMode} className="reply-action-button">Отменить</button>
                        </div>
                    </div>
                </div>
            ) : (
                <></>
            )}

            {forwardMode ? (
                <div className="main-forward-container">
                    <input
                        className="forward-input"
                        placeholder="Получатель"
                        value={forwardEmail}
                        onChange={(e) => setForwardEmail(e.target.value)}>
                    </input>
                    <div className="forward-form-footer">
                        <p className="forward-error-message">{errorMessage}</p>
                        <div className="forward-action-buttons">
                            <button onClick={Forward} className="forward-action-button">Переслать</button>
                            <button onClick={ChangeForwardMode} className="forward-action-button">Отменить</button>
                        </div>
                    </div>
                </div>
            ) : (
                <></>
            )}

            {!forwardMode && !replyMode ? (
                <div className="one-letter-button-container">
                    <button onClick={ChangeReplyMode} className="one-letter-activity-button">Ответить</button>
                    <button onClick={ChangeForwardMode} className="one-letter-activity-button">Переслать</button>
                </div>
            ) : (
                <></>
            )}

        </div>
    );
}