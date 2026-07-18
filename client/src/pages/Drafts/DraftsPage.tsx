import { useEffect, useState } from 'react';
import { Link } from "react-router-dom";

import type { Draft } from "../../interfaces/Draft";
import type { User } from "../../interfaces/User";

function Drafts() {

    const [maxOnPage, setMaxOnPage] = useState(20);
    const [LettersPage,] = useState(1);
    const [drafts, setDrafts] = useState<Draft[]>([]);

    const [user, setUser] = useState<User | null>(null);

    const [startIndex, setStartIndex] = useState(maxOnPage * LettersPage - maxOnPage + 1);
    const [endIndex, setEndIndex] = useState(maxOnPage * LettersPage);
    const [messagesTotal, setMessagesTotal] = useState(0);

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

            refreshDrafts();
        }
    }, []);

    useEffect(() => {
        if (user) {
            refreshDrafts();
        }
    }, [user]);

    async function refreshDrafts() {
        const response = await fetch('https://localhost:7094/api/draft/get',
            {
                credentials: "include",
            });

        if (!response.ok)
            throw new Error(await response.text());

        const data = await response.json();

        setDrafts(data);

        if (data.length < maxOnPage) {
            setEndIndex(data.length);
            setMaxOnPage(data.length);
            setMessagesTotal(data.length)

            if (data.length === 0) {
                setStartIndex(0);
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
                        <Link to="/draft/new" className="new-draft-button">Новый черновик</Link>
                        <div className="letters-topbar">
                            <button onClick={refreshDrafts} className="reload-button"><img src="/images/reload.svg" alt="reload"></img></button>
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
                                    <p className="please-sign">У вас нет черновиков</p>
                                ) : (
                                    drafts.map((draft, i) => (
                                        <Link to={`/draft/${draft.id}`} key={i} className="draft">
                                            <div className="draft-content">
                                                <p className="draft-theme">{draft.title}</p>
                                                <p className="draft-text"> - {draft.text}</p>
                                            </div>
                                            <p className="draft-date">{ new Date(draft.lastEditDate).toLocaleDateString("ru-RU") }</p>
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
export default Drafts;