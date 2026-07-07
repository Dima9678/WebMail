import { useEffect, useState } from 'react';
import { Link } from "react-router-dom";

import type { User } from "../interfaces/User";


interface HomePageProps {
    user: User | null;
}

interface AuthUserData {
    name: string;
    email: string;
    id: string;
}

function newletter({ user }: HomePageProps) {
    const [me, setMe] = useState<AuthUserData | null>();

    const [recipient, setRecipient] = useState("");
    const [title, setTitle] = useState("");
    const [text, setText] = useState("");
    const [errorMessage, setErrorMessage] = useState("");
    const [sucsess, setSucsess] = useState(false);

    useEffect(() => {
        async function loadMe() {

            const response = await fetch("https://localhost:7094/api/auth/me", {
                credentials: "include"
            });

            if (response.ok) {
                const data = await response.json();
                setMe(data);
            }
        }

        loadMe();
    }, []);

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        const response = await fetch("https://localhost:7094/api/letter/write", {
            
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
            setErrorMessage("Успешно");
            setSucsess(true);
        }
        else {
            const message = await response.text();
            setErrorMessage(message);
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
                        <Link to="/allmails" className="leftbar-navigation-button"><img src="/images/envelope.svg" alt="конверт"></img></Link>
                        <Link to="/sent" className="leftbar-navigation-button"><img src="/images/plane.svg" alt="самолет"></img></Link>
                        <Link to="/starred" className="leftbar-navigation-button"><img src="/images/star.svg" alt="звезда"></img></Link>
                        <Link to="/drafts" className="leftbar-navigation-button"><img src="/images/draft.svg" alt="черновики"></img></Link>
                        <Link to="/spam" className="leftbar-navigation-button"><img src="/images/spam.svg" alt="спам"></img></Link>
                        <Link to="/trash" className="leftbar-navigation-button"><img src="/images/trash.svg" alt="корзина"></img></Link>
                    </nav>

                    <div className="write-letter">
                        <div className="write-letter-topbar">
                            Написать письмо
                        </div>
                        <form onSubmit={handleSubmit} className="write-letter-form">
                            <input
                                className="write-letter-recipient"
                                placeholder="Адресат"
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
                                    <p className="write-letter-sucsess-message">Сообщение отправлено</p>
                                ) : (
                                        <p className="write-letter-error-message">{errorMessage}</p>
                                    )}
                                <button type="submit" className="write-letter-submit-button">Отправить</button>
                            </div>
                        </form>
                    </div>
                </div>
            </div>
        </div>
    );
}

export default newletter;