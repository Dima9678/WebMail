import { useEffect, useState } from 'react';
import { Link } from "react-router-dom";

import type { Letter } from "../interfaces/Letter";
import type { User } from "../interfaces/User";



function homepage() {
    const [user, setUser] = useState<User | null>(null);
    const [acceptLetters, setAcceptLetters] = useState<Letter[]>([]);

    const [maxOnPage] = useState(40);

    const [minLettersPage] = useState(0);
    const [lettersPage, setlettersPage] = useState(0);

    const [total, setTotal] = useState(0);

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
    }, []);

    useEffect(() => {
        if (user) {
            TotalLettersGet();
            refreshLetters(0, maxOnPage - 1);
        }
    }, [user]);

    async function refreshLetters(startIndex: number, endIndex: number) {
        const response = await fetch(`https://localhost:7094/api/letter/getuserletters/${startIndex}/${endIndex}`,
            {
                credentials: "include",
                method: "GET",
            });

        if (!response.ok)
            throw new Error(await response.text());

        const data = await response.json();

        setAcceptLetters(data);
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

                setAcceptLetters(prev =>
                    prev.map((letter, index) =>
                        index === i
                            ? {
                                ...letter,
                                letterStates: letter.letterStates.map(state =>
                                    state.userId === user?.id
                                        ? { ...state, starred: !state.starred }
                                        : state
                                )
                            }
                            : letter
                    )
                );
            })
            .catch(console.error);
    }

    function ReadStatusLetter({ letter, i, user }) {
        const state = letter.letterStates.find(s => s.userId === user.id);

        return (
            state?.isRead ? (
                <Link to={`/letter/${letter.id}`} key={i} className="letter-read">
                    <StarredStatusLetter
                        state={state}
                        i={i}
                        changeStarred={changeStarred}
                    />
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
                    <StarredStatusLetter
                        state={state}
                        i={i}
                        changeStarred={changeStarred}
                    />
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
        );
    }

    async function TotalLettersGet() {
        const response = await fetch(`https://localhost:7094/api/letter/total`, {
            credentials: "include",
            method: "GET"
        })

        if (!response.ok)
            throw new Error(await response.text());

        const data = await response.json();
        setTotal(data);

    }

    function StarredStatusLetter({ state, i, changeStarred }) {
        return (
            <>
                {
                    state?.starred ? (
                        <button onClick={(e) => {
                            e.preventDefault();
                            changeStarred(i);
                        }}>
                            <img src="/images/starred.svg" alt="star" /></button>
                    ) : (
                        <button onClick={(e) => {
                            e.preventDefault();
                            changeStarred(i);
                        }}>
                            <img src="/images/unstarred.svg" alt="star" /></button>
                    )
                }
            </>
        );
    }

    const ClickHandler = (event: React.MouseEvent<HTMLButtonElement>) => {
        const value = event.currentTarget.value;

        const newPage = value === "prev"
            ? lettersPage - 1
            : lettersPage + 1;

        const newStart = newPage * maxOnPage;
        const newEnd = Math.min(newStart + maxOnPage - 1, total - 1);

        setlettersPage(newPage);

        refreshLetters(newStart, newEnd);
    };

    const maxLettersPage = Math.ceil(total / maxOnPage);
    var startIndex = lettersPage * maxOnPage;
    
    const endIndex = Math.min(startIndex + maxOnPage - 1, total - 1);

    console.log(total)
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

                        <Link to="/" className="leftbar-navigation-button"><img className="leftbar-navigation-button-style" src="/images/envelope.svg" alt="конверт"></img></Link>
                        <Link to="/sent" className="leftbar-navigation-button"><img className="leftbar-navigation-button-style" src="/images/plane.svg" alt="самолет"></img></Link>
                        <Link to="/starred" className="leftbar-navigation-button"><img className="leftbar-navigation-button-style" src="/images/star.svg" alt="звезда"></img></Link>
                        <Link to="/drafts" className="leftbar-navigation-button"><img className="leftbar-navigation-button-style" src="/images/draft.svg" alt="черновики"></img></Link>
                        <Link to="/spam" className="leftbar-navigation-button"><img className="leftbar-navigation-button-style" src="/images/spam.svg" alt="спам"></img></Link>
                        <Link to="/trash" className="leftbar-navigation-button"><img className="leftbar-navigation-button-style" src="/images/trash.svg" alt="корзина"></img></Link>
                    </nav>
                    <div className="letters-block">
                        <Link to="/newletter" className="new-letter-button">Новое письмо</Link>
                        <div className="letters-topbar">
                            <button onClick={refreshLetters} className="reload-button"><img src="/images/reload.svg" alt="reload"></img></button>
                            <div className="search-string">
                                <img src="/images/loop.svg"></img>
                                <input className="search-input" placeholder="Поиск по почте"></input>
                                <button className="search-details-button"><img src="/images/searchDetails.svg"></img></button>
                            </div>
                            {user === null || total === 0 ? (
                                <div className="pagination"></div>
                            ) : (
                                <div className="pagination">
                                    {lettersPage > minLettersPage ? (
                                        <button value="prev" onClick={ClickHandler} className="pagination-button">назад</button>
                                    ) : (
                                        <></>
                                    )}
                                    <p>
                                        {startIndex + 1}-{endIndex + 1} из {total}
                                    </p>
                                    {lettersPage < maxLettersPage - 1 ? (
                                        <button value="next" onClick={ClickHandler} className="pagination-button">вперед</button>
                                    ) : (
                                        <></>
                                    )}

                                </div>
                            )}
                        </div>

                        <div className="letters">
                            {user === null ? (
                                <p className="please-sign">Войдите в свой аккаунт или зарегиструйтесь</p>
                            ) : (

                                total === 0 ? (
                                    <p className="please-sign">Входящих сообщений нет</p>
                                ) : (

                                    acceptLetters.map((letter, i) => (
                                        <ReadStatusLetter
                                            key={letter.id}
                                            letter={letter}
                                            user={user}
                                            i={i}
                                        />
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