import { useEffect, useState } from 'react';
import { Link, useParams } from "react-router-dom";

import type { User } from "../interfaces/User";
import type { Letter } from "../interfaces/Letter";


function letter() {
    const [letter, setLetter] = useState<Letter>();
    const [, setUser] = useState<User | null>(null);
    const { id } = useParams();

    useEffect(() => {
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

                // если письмо не от текущего пользователя,
                // здесь можно отправлять запрос на прочтение
                if (currentUser?.email !== letterData.adresseeEmail) {
                    // запрос на установку IsReaden
                }

                setLetter(letterData);

            } catch (error) {
                console.error(error);
            }
        }

        loadData();

    }, [id]);


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

                        <Link to="/" className="leftbar-navigation-button"><img className="leftbar-navigation-button-style" src="/images/envelope.svg" alt="конверт"></img></Link>
                        <Link to="/sent" className="leftbar-navigation-button"><img className="leftbar-navigation-button-style" src="/images/plane.svg" alt="самолет"></img></Link>
                        <Link to="/starred" className="leftbar-navigation-button"><img className="leftbar-navigation-button-style" src="/images/star.svg" alt="звезда"></img></Link>
                        <Link to="/drafts" className="leftbar-navigation-button"><img className="leftbar-navigation-button-style" src="/images/draft.svg" alt="черновики"></img></Link>
                        <Link to="/spam" className="leftbar-navigation-button"><img className="leftbar-navigation-button-style" src="/images/spam.svg" alt="спам"></img></Link>
                        <Link to="/trash" className="leftbar-navigation-button"><img className="leftbar-navigation-button-style" src="/images/trash.svg" alt="корзина"></img></Link>
                    </nav>
                    <div className="letters-block">
                        <div className="one-letter-topbar">
                            <Link to="/"><img className="arrow" src="/images/arrow.svg" alt="назад"></img></Link>
                        </div>
                        {letter === undefined ? (
                            <div className="one-letter-main-container">
                                <p>Данные загружаются</p>
                            </div>
                        ) : (
                            <div className="one-letter-main-container">

                                <div className="one-letter-header">
                                    <div className="one-letter-title-block">
                                        <p className="one-letter-title">{letter.title}</p>
                                        <p className="one-letter-datetime">
                                            {new Date(letter.sendTime).toLocaleDateString("ru-RU")} в
                                            {new Date(letter.sendTime).toLocaleTimeString("ru-RU")}
                                        </p>
                                    </div>
                                    <div className="one-letter-sender-block">
                                        <div className="one-letter-avatar">
                                            <p className="one-letter-avatar-letter">Е</p>
                                        </div>
                                        <div>
                                            <p className="one-letter-adressee-name">{letter.adresseeName}</p>
                                            <p className="one-letter-adressee-email">{letter.adresseeEmail}</p>
                                        </div>
                                    </div>
                                </div>

                                <p className="one-letter-text">{letter.text}</p>

                                <div className="one-letter-button-container">
                                    <div className="one-letter-activity-button">Ответить</div>
                                    <div className="one-letter-activity-button">Переслать</div>
                                </div>
                            </div>
                        )}
                    </div>
                </div>
            </div>
        </div>
    );
}

export default letter;