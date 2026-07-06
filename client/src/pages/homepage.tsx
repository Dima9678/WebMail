import { useEffect, useState } from 'react';
import { Routes, Route, Link } from "react-router-dom";

import type { User } from "../interfaces/User";
import type { Letter } from "../interfaces/Letter";
import type { LetterDto } from "../interfaces/LetterDto";


interface HomePageProps {
    user: User | null;
}

function homepage({ user }: HomePageProps) {
    const [me, setMe] = useState(false);
    const [isAuth, setIsAuth] = useState(false);
    const [maxOnPage, setMaxOnPage] = useState(20);
    const [LettersPage, setLettersPage] = useState(1);

    const [letters, setLetters] = useState<LetterDto[] | null>([]);

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
    }, []);

    useEffect(() => {
        async function loadData() {
            const response = await fetch("https://localhost:7094/api/test", {
                credentials: "include"
            });

            if (response.ok) {
                const data = await response.json();
                setLetters(data);
            }
        }
        loadData();
    }, []);


    return (
        <div className="parent-container">
            <div className="main-container">
                <div className="topbar">
                    <div className="centered-container">
                        <Link to="/" className="website-logo">MyMail</Link>
                    </div>
                    <div className="auth-buttons">
                        <Link to="/signin" className="auth-button">Вход</Link>
                        <Link to="/signup" className="auth-button">Регистраци</Link>
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
                    <div className="letters-block">
                        <div className="letters-topbar">
                            <div className="search-string">
                                <img src="/images/loop.svg"></img>
                                <input className="search-input" placeholder="Поиск по почте"></input>
                            </div>
                            <div className="pagination">{maxOnPage * LettersPage - maxOnPage + 1}-{maxOnPage * LettersPage} из {maxOnPage}
                            </div>
                        </div>
                        <div className="letters">
                            {letters.map(letter =>
                                letter.isReaden ? (
                                    <div className="letter-read">
                                        <img src="/images/starred.svg" alt="star"></img>
                                        <p className="letter-sender-read">{letter.author}</p>
                                        <p className="letter-theme-read">{letter.title}</p>
                                        <p className="letter-text-read"> - {letter.text}</p>
                                        <p className="letter-date-read">{new Date(letter.date).toDateString()}</p>
                                    </div>
                                ) : (
                                    <div className="letter-unread">
                                        <img src="/images/unstarred.svg" alt="unst"></img>
                                        <p className="letter-sender-unread">{letter.author}</p>
                                        <p className="letter-theme-unread">{letter.title}</p>
                                        <p className="letter-text-unread"> - {letter.text}</p>
                                        <p className="letter-date-unread">{new Date(letter.date).toDateString()}</p>
                                    </div>
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