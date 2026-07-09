import { useEffect, useState } from 'react';
import { Link } from "react-router-dom";

import type { Letter } from "../interfaces/Letter";
import type { User } from "../interfaces/User";



function homepage() {
    const [maxOnPage, setMaxOnPage] = useState(20);
    const [LettersPage,] = useState(1);
    const [acceptLetters, setAcceptLetters] = useState<Letter[]>([]);

    const [user, setUser] = useState<User | null>(null);

    const [startIndex, setStartIndex] = useState(maxOnPage * LettersPage - maxOnPage + 1);
    const [endIndex, setEndIndex] = useState(maxOnPage * LettersPage);
    const [messagesTotal, setMessagesTotal] = useState(0);



    for (let entry of acceptLetters) {
        console.log(entry.sendTime);
    }

    useEffect(() => {
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

        if (user != null) {

        refreshLetters();
        }
    }, []);

    useEffect(() => {
        if (user) {
            refreshLetters();
        }
    }, [user]);

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
            setMessagesTotal(data.length)

            if (data.length === 0) {
                setStartIndex(0);
            }
        }
    }
    function changeStarred(i: number) {
        fetch(`https://localhost:7094/api/letter/changestarred/${acceptLetters[i].id}`, {
            credentials: "include",
            method: "PUT"
        })
            .then(async r => {
                if (!r.ok) {
                    throw new Error(await r.text());
                }

                setAcceptLetters(prev => {
                    const copy = [...prev];

                    copy[i] = {
                        ...copy[i],
                        starred: !copy[i].starred
                    };

                    return copy;
                });
            })
            .catch(console.error);
    }

        return (
            <div className="parent-container">
                <div className="main-container">
                    <div className="topbar">
                        <div className="centered-container">
                            <Link to="/" className="website-logo">MyMail</Link>
                        </div>
                        {user === null ? (
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
                            <Link to="/" className="leftbar-navigation-button"><img className="leftbar-navigation-button-style" src="/images/envelope.svg" alt="конверт"></img></Link>
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
                                {user == null ? (
                                <div className="pagination"></div>
                                ) : (
                                        <div className="pagination">{startIndex}-{endIndex} из {maxOnPage}</div>
                                    )}
                            </div>

                            <div className="letters">
                                {user === null ? (
                                    <p className="please-sign">Войдите в свой аккаунт или зарегиструйтесь</p>
                                ) : (

                                    messagesTotal === 0 ? (
                                        <p className="please-sign">Входящих сообщений нет</p>
                                    ) : (


                                        acceptLetters.map((letter, i) =>
                                            letter.isReaden ? (
                                                <Link to={`/letter/${letter.id}`} key={i} className="letter-read">
                                                    {letter.starred ? (
                                                        <button onClick={(e) => {
                                                            e.preventDefault();
                                                            changeStarred(i);
                                                        }}><img src="/images/starred.svg" alt="star" /></button>
                                                    ) : (
                                                        <button onClick={(e) => {
                                                            e.preventDefault();
                                                            changeStarred(i);
                                                        }}><img src="/images/unstarred.svg" alt="star" /></button>
                                                    )}
                                                    <p className="letter-sender-read">{letter.adresseeName}</p>
                                                    <div className="letter-content">
                                                        <p className="letter-theme-read">{letter.title}</p>
                                                        <p className="letter-text-read"> - {letter.text}</p>
                                                    </div>
                                                    <p className="letter-date-read">
                                                        {new Date(letter.sendTime).toLocaleDateString("ru-RU")}
                                                    </p>
                                                </Link>
                                            ) : (
                                                <Link to={`/letter/${letter.id}`} key={i} className="letter-unread">
                                                    {letter.starred ? (
                                                        <button onClick={(e) => {
                                                            e.preventDefault();
                                                            changeStarred(i);
                                                        }}><img src="/images/starred.svg" alt="star" /></button>
                                                    ) : (
                                                        <button onClick={(e) => {
                                                            e.preventDefault();
                                                            changeStarred(i);
                                                        }}><img src="/images/unstarred.svg" alt="star" /></button>
                                                    )}
                                                    <p className="letter-sender-unread">{letter.adresseeName}</p>
                                                    <div className="letter-content">
                                                        <p className="letter-theme-unread">{letter.title}</p>
                                                        <p className="letter-text-unread"> - {letter.text}</p>
                                                    </div>
                                                    <p className="letter-date-unread">
                                                        {new Date(letter.sendTime).toLocaleDateString("ru-RU")}
                                                    </p>
                                                </Link>
                                            )
                                        )
                                    ))}
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        );
    }
export default homepage;