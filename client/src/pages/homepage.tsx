import { useEffect, useState } from 'react';
import { Routes, Route, Link } from "react-router-dom";

import type { User } from "../interfaces/User";
import type { Letter } from "../interfaces/Letter";


interface HomePageProps {
    user: User | null;
}

interface AuthUserData {
    name: string;
    email: string;
    id: string;
}

function homepage({ user }: HomePageProps) {
    const [me, setMe] = useState<AuthUserData | null>();
    const [isAuth, setIsAuth] = useState(false);
    const [maxOnPage, setMaxOnPage] = useState(20);
    const [LettersPage, setLettersPage] = useState(1);
    const [acceptLetters, setAcceptLetters] = useState<Letter[]>([]);

    const [startIndex, setStartIndex] = useState(maxOnPage * LettersPage - maxOnPage + 1);
    const [endIndex, setEndIndex] = useState(maxOnPage * LettersPage);



    for (let entry of acceptLetters) {
        console.log(entry.sendTime);
    }

    useEffect(() => {
        async function loadMe() {

            const response = await fetch("https://localhost:7094/api/auth/me", {
                credentials: "include"
            });

            if (response.ok) {
                const data = await response.json();
                setMe(data);
                setIsAuth(true);
            } else {
                setIsAuth(false);
            }
        }

        loadMe();
        refreshLetters();
    }, []);

    async function refreshLetters() {
        const response = await fetch('https://localhost:7094/api/letter/getuserletters',
            {
                credentials: "include",
            });

        if (!response.ok)
            throw new Error(await response.text());

        const data = await response.json();

        setAcceptLetters(data);

        if (data.length < maxOnPage) {
            setEndIndex(data.length);
            setMaxOnPage(data.length);
        }
    }
    return (
        <div className="parent-container">
            <div className="main-container">
                <div className="topbar">
                    <div className="centered-container">
                        <Link to="/" className="website-logo">MyMail</Link>
                    </div>
                    {isAuth === false ? (
                        <div className="auth-buttons">

                            <Link to="/signin" className="auth-button">Вход</Link>
                            <Link to="/signup" className="auth-button">Регистрация</Link>
                        </div>
                    ) : (
                        <div className="auth-buttons">
                            <Link to="/myprofile" className="auth-button">Мой аккаунт</Link>
                        </div>
                    )}

                </div>
                <div className="main-content">
                    <nav className="sidebar">
                        <Link to="/newletter" className="new-letter-button"><img className="leftbar-navigation-button-style" src="/images/pencil.svg" alt="написать"></img></Link>
                        <Link to="/allmails" className="leftbar-navigation-button"><img className="leftbar-navigation-button-style" src="/images/envelope.svg" alt="конверт"></img></Link>
                        <Link to="/sent" className="leftbar-navigation-button"><img className="leftbar-navigation-button-style" src="/images/plane.svg" alt="самолет"></img></Link>
                        <Link to="/starred" className="leftbar-navigation-button"><img className="leftbar-navigation-button-style" src="/images/star.svg" alt="звезда"></img></Link>
                        <Link to="/drafts" className="leftbar-navigation-button"><img className="leftbar-navigation-button-style" src="/images/draft.svg" alt="черновики"></img></Link>
                        <Link to="/spam" className="leftbar-navigation-button"><img className="leftbar-navigation-button-style" src="/images/spam.svg" alt="спам"></img></Link>
                        <Link to="/trash" className="leftbar-navigation-button"><img className="leftbar-navigation-button-style" src="/images/trash.svg" alt="корзина"></img></Link>
                    </nav>
                    <div className="letters-block">

                        <div className="letters-topbar">
                            <button onClick={refreshLetters} className="reload-button"><img src="/images/reload.svg" alt="reload"></img></button>
                            <div className="search-string">
                                <img src="/images/loop.svg"></img>
                                <input className="search-input" placeholder="Поиск по почте"></input>
                            </div>
                            <div className="pagination">{startIndex}-{endIndex} из {maxOnPage}
                            </div>
                        </div>

                        <div className="letters">
                            {user === null ? (
                                <p className="please-sign">Войдите в свой аккаунт или зарегиструйтесь</p>
                            ) : (
                                acceptLetters.map((letter, i) =>
                                    letter.isReaden ? (
                                        <div key={i} className="letter-read">
                                            <img src="/images/starred.svg" alt="star" />
                                            <p className="letter-sender-read">{letter.adresseeName}</p>
                                            <p className="letter-theme-read">{letter.title}</p>
                                            <p className="letter-text-read"> - {letter.text}</p>
                                            <p className="letter-date-read">
                                                {new Date(letter.sendTime).toString()}
                                            </p>
                                        </div>
                                    ) : (
                                        <div key={i} className="letter-unread">
                                            <img src="/images/unstarred.svg" alt="unst" />
                                            <p className="letter-sender-unread">{letter.adresseeName}</p>
                                            <p className="letter-theme-unread">{letter.title}</p>
                                            <p className="letter-text-unread"> - {letter.text}</p>
                                            <p className="letter-date-unread">
                                                    {new Date(letter.sendTime).toLocaleDateString("ru-RU")}
                                            </p>
                                        </div>
                                    )
                                )
                            )}
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}

export default homepage;