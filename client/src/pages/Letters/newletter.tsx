import { useEffect, useState } from 'react';
import { Link, useParams } from "react-router-dom";

import type { User } from "../interfaces/User";
import type { Letter } from "../interfaces/Letter";
import letter from './letter';


function newletter() {

    const [, setUser] = useState<User | null>(null);

    const { id } = useParams();
    const [parentLetter, setParentLetter] = useState<Letter>()

    const [recipient, setRecipient] = useState("");
    const [title, setTitle] = useState("");
    const [text, setText] = useState("");

    const [errorMessage, setErrorMessage] = useState("");
    const [sucsess, setSucsess] = useState(false);


    useEffect(() => {
        //Если пришел id письма, то мы пишем ответ на него
        const isReply = async () => {
            if (id != undefined) {
                const letterResponse = await fetch(
                    `https://localhost:7094/api/letter/${id}`,
                    {
                        credentials: "include"
                    }
                );
                if (!letterResponse.ok) {
                    throw new Error(await letterResponse.text());
                }
                const letterData: Letter = await letterResponse.json();
                setParentLetter(letterData);
                setRecipient(letterData.adresseeEmail);
            }
        }

        isReply();

        fetch("https://localhost:7094/api/User", {
            credentials: "include"
        })
            .then(async r => {
                if (r.status === 401) {
                    setUser(null);
                    return null;
                }
                if (!r.ok) {
                    throw new Error(await r.text());
                }
                return r.json();
            })
            .then(data => {
                if (data) {
                    setUser(data);
                }
            })
            .catch(console.error);
    }, []);

    const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault()

        const submitter = (e.nativeEvent as SubmitEvent).submitter as HTMLButtonElement;

        if (submitter.value === "letter") {
            let response;
            
            if (id === undefined) {
                response = await fetch("https://localhost:7094/api/letter/write", {
                    method: "POST",
                    credentials: "include",
                    headers: {
                        "Content-Type": "application/json"
                    },
                    body: JSON.stringify({
                        recipient,
                        title,
                        text,
                    })
                });
            }
            else {
                const replyId = id;
                response = await fetch(`https://localhost:7094/api/letter/write/reply/${id}`, {
                    method: "POST",
                    credentials: "include",
                    headers: {
                        "Content-Type": "application/json"
                    },
                    body: JSON.stringify({
                        recipient,
                        title,
                        text,
                        replyId,
                    })
                });
            }

            if (response.ok) {
                setRecipient("");
                setTitle("");
                setText("");
                setErrorMessage("Отправлено");
                setSucsess(true);
            }
            else {
                const message = await response.text();
                setErrorMessage(message);
            }

        } else if (submitter.value === "draft") {
            console.log("draft");
            const response = await fetch("https://localhost:7094/api/draft/add", {
                method: "POST",
                credentials: "include",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({
                    recipient,
                    title,
                    text,
                })
            });

            if (response.ok) {
                setRecipient("");
                setTitle("");
                setText("");
                setErrorMessage("Черновик сохранен");
                setSucsess(true);
            }
            else {
                const message = await response.text();
                setErrorMessage(message);
            }
        }
    }

    return (
        <div className="parent-container">
            <div className="main-container">
                <div className="topbar">
                    <div className="centered-container">
                        <Link to="/" className="website-logo">MyMail</Link>
                    </div>
                    <div className="auth-buttons">
                        <Link to="/myprofile" className="auth-button">Мой аккаунт</Link>
                    </div>
                </div>
                <div className="main-content">
                    <nav className="sidebar">
                        <Link to="/allmails" className="leftbar-navigation-button"><img className="leftbar-navigation-button-style" src="/images/envelope.svg" alt="конверт"></img></Link>
                        <Link to="/sent" className="leftbar-navigation-button"><img className="leftbar-navigation-button-style" src="/images/plane.svg" alt="самолет"></img></Link>
                        <Link to="/starred" className="leftbar-navigation-button"><img className="leftbar-navigation-button-style" src="/images/star.svg" alt="звезда"></img></Link>
                        <Link to="/drafts" className="leftbar-navigation-button"><img className="leftbar-navigation-button-style" src="/images/draft.svg" alt="черновики"></img></Link>
                        <Link to="/spam" className="leftbar-navigation-button"><img className="leftbar-navigation-button-style" src="/images/spam.svg" alt="спам"></img></Link>
                        <Link to="/trash" className="leftbar-navigation-button"><img className="leftbar-navigation-button-style" src="/images/trash.svg" alt="корзина"></img></Link>
                    </nav>

                    <div className="write-letter">
                        <div className="write-letter-topbar">
                            Написать письмо
                        </div>
                        {parentLetter != undefined ? (
                            <div>
                                <p>В ответ на: </p>
                                <p>Тема: {parentLetter.title}</p>
                                <p>Адресат: {parentLetter.adresseeEmail}</p>
                            </div>
                        ) : (
                            <></>
                        )}

                        <form onSubmit={handleSubmit} className="write-letter-form">
                            <input
                                className="write-letter-recipient"
                                placeholder="Получатель"
                                value={recipient}
                                onChange={(e) => setRecipient(e.target.value)}
                            >
                            </input>
                            <input
                                className="write-letter-title"
                                placeholder="Тема письма"
                                value={title}
                                onChange={(e) => setTitle(e.target.value)}
                            >
                            </input>
                            <textarea
                                className="write-letter-text"
                                placeholder="Текст письма"
                                value={text}
                                onChange={(e) => setText(e.target.value)}
                            >
                            </textarea>
                            <div className="submit-button-centrer">
                                {sucsess ? (
                                    <p className="write-letter-sucsess-message">{errorMessage}</p>
                                ) : (
                                    <p className="write-letter-error-message">{errorMessage}</p>
                                )}
                                <div className="write-letter-activity-buttons">
                                    <button type="submit" className="write-letter-submit-button" value="letter">Отправить</button>
                                    <button type="submit" className="write-letter-submit-button" value="draft">В черновики</button>
                                </div>
                            </div>
                        </form>
                    </div>
                </div>
            </div>
        </div>
    );
}

export default newletter;