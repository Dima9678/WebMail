import { useEffect, useState } from 'react';
import { Link, useParams } from "react-router-dom";

import type { Draft } from "../../interfaces/Draft";
import type { User } from "../../interfaces/User";

function EditDraft() {
    const [recipientEmail, setRecipientEmail] = useState("");
    const [title, setTitle] = useState("");
    const [text, setText] = useState("");

    const [, setUser] = useState<User | null>(null);
    const [errorMessage, setErrorMessage] = useState("");
    const [sucsess, setSucsess] = useState(false);
    const { id } = useParams();

    const isNew = id === "new";

    useEffect(() => {
        if (!isNew && id) {

            async function loadData() {
                try {
                    const userResponse = await fetch("https://localhost:7094/api/User", {
                        credentials: "include"
                    });

                    let currentUser: User | null = null;

                    if (userResponse.status !== 401) {
                        if (!userResponse.ok) {
                            throw new Error(await userResponse.text());
                        }
                        currentUser = await userResponse.json();
                        setUser(currentUser);
                    }

                    const draftResponce = await fetch(
                        `https://localhost:7094/api/draft/getbyid/${id}`,
                        {
                            credentials: "include"
                        }
                    );

                    if (!draftResponce.ok) {
                        throw new Error(await draftResponce.text());
                    }

                    const draftData: Draft = await draftResponce.json();

                    setRecipientEmail(draftData.recipientEmail ?? "")
                    setTitle(draftData.title ?? "")
                    setText(draftData.text ?? "")

                } catch (error) {
                    console.error(error);
                }
            }
            loadData();
        }
        else {
            setRecipientEmail("")
            setTitle("")
            setText("")
        }
    }, [id]);


    const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        const submitter = (e.nativeEvent as SubmitEvent).submitter as HTMLButtonElement;

        if (submitter.value === "save") {
            const response = await fetch(`https://localhost:7094/api/draft/save/${id}`, {
                method: "POST",
                credentials: "include",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({
                    recipientEmail,
                    title,
                    text,
                })
            });

            if (response.ok) {
                setErrorMessage("Сохранено");
                setSucsess(true);
            }
            else {
                const message = await response.text();
                setErrorMessage(message);
            }

        } else if (submitter.value === "send") {
            const response = await fetch(`https://localhost:7094/api/draft/send/${id}`, {
                method: "POST",
                credentials: "include",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({
                    recipientEmail,
                    title,
                    text,
                })
            });

            if (response.ok) {
                setErrorMessage("Отправлено");
                setSucsess(true);
            }
            else {
                const message = await response.text();
                setErrorMessage(message);
            }
        } else if (submitter.value === "new") {
            const response = await fetch(`https://localhost:7094/api/draft/add`, {
                method: "POST",
                credentials: "include",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({
                    recipientEmail,
                    title,
                    text,
                })
            });

            if (response.ok) {
                setErrorMessage("Создано");
                setSucsess(true);
            }
            else {
                const message = await response.text();
                setErrorMessage(message);
            }
        }else if (submitter.value === "sendnew") {
            const response = await fetch(`https://localhost:7094/api/draft/sendnew`, {
                method: "POST",
                credentials: "include",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({
                    recipientEmail,
                    title,
                    text,
                })
            });

            if (response.ok) {
                setErrorMessage("Отправлено");
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
                            Изменить черновик
                        </div>
                        <form onSubmit={handleSubmit} className="write-letter-form">
                            <input
                                className="write-letter-recipient"
                                placeholder="Получатель"
                                value={recipientEmail}
                                onChange={(e) => setRecipientEmail(e.target.value)}
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

                                {isNew === true ? (
                                    <>
                                        <button type="submit" className="write-letter-submit-button" value="new">Создать</button>
                                        <button type="submit" className="write-letter-submit-button" value="sendnew">Отправить</button>
                                    </>
                                ) : (
                                    <>
                                        <button type="submit" className="write-letter-submit-button" value="save">Сохранить</button>
                                        <button type="submit" className="write-letter-submit-button" value="send">Отправить</button>
                                    </>
                                )}


                            </div>
                        </form>
                    </div>
                </div>
            </div>
        </div>
    );
}

export default EditDraft;