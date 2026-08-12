import { useEffect, useState } from 'react';
import { Link } from "react-router-dom";

import type { Draft } from "../../interfaces/Draft";
import type { User } from "../../interfaces/User";

function Drafts() {
    const [user, setUser] = useState<User | null>(null);
    const [drafts, setDrafts] = useState<Draft[]>([]);

    const [maxOnPage] = useState(20);

    const [minDraftPage] = useState(0);
    const [draftPage, setDraftPage] = useState(0);

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
            TotalDraftsGet();
            refreshDrafts(0, maxOnPage - 1);
        }
    }, [user]);

    async function refreshDrafts(startIndex: number, endIndex: number) {
        const response = await fetch(`https://localhost:7094/api/draft/${startIndex}/${endIndex}`,
            {
                credentials: "include",
                method: "GET"
            });

        if (!response.ok)
            throw new Error(await response.text());

        const data = await response.json();
        TotalDraftsGet();
        setDrafts(data);
    }

    async function TotalDraftsGet() {
        console.log("Перед запросом. Максимум на странице: " + maxOnPage)
        const response = await fetch(`https://localhost:7094/api/draft/count`, {
            credentials: "include",
            method: "GET"
        })

        if (!response.ok)
            throw new Error(await response.text());

        const data = await response.json();
        setTotal(data);
    }

    const ClickHandler = (event: React.MouseEvent<HTMLButtonElement>) => {
        const value = event.currentTarget.value;

        const newPage = value === "prev"
            ? draftPage - 1
            : draftPage + 1;

        const newStart = newPage * maxOnPage;
        const newEnd = Math.min(newStart + maxOnPage - 1, total - 1);

        setDraftPage(newPage);

        refreshDrafts(newStart, newEnd);
    };

    const maxDraftPage = Math.ceil(total / maxOnPage);
    var startIndex = draftPage * maxOnPage;

    const endIndex = Math.min(startIndex + maxOnPage - 1, total - 1);

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
                            <Link to="/myprofile" className="auth-button">{user.name}</Link>
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
                        <Link to="/draft/new" className="new-draft-button">Новый черновик</Link>
                        <div className="letters-topbar">
                            <button onClick={() => refreshDrafts(0,maxOnPage-1)} className="reload-button"><img src="/images/reload.svg" alt="reload"></img></button>
                            <div className="search-string">
                                <img src="/images/loop.svg"></img>
                                <input className="search-input" placeholder="Поиск по почте"></input>
                            </div>
                            {user === null || total === 0 ? (
                                <div className="pagination"></div>
                            ) : (
                                <div className="pagination">
                                    {draftPage > minDraftPage ? (
                                        <button value="prev" onClick={ClickHandler} className="pagination-button">назад</button>
                                    ) : (
                                        <div className="pagination-button-hidden"></div>
                                    )}
                                    <p>
                                        {startIndex + 1}-{endIndex + 1} из {total}
                                    </p>
                                    {draftPage < maxDraftPage - 1 ? (
                                        <button value="next" onClick={ClickHandler} className="pagination-button">вперед</button>
                                    ) : (
                                        <div className="pagination-button-hidden"></div>
                                    )}
                                </div>
                            )}
                        </div>

                        <div className="letters">
                            {user === null ? (
                                <p className="please-sign">Войдите в свой аккаунт или зарегиструйтесь</p>
                            ) : (

                                total === 0 ? (
                                    <p className="please-sign">У вас нет черновиков</p>
                                ) : (
                                    drafts.map((draft, i) => (
                                        <Link to={`/draft/${draft.id}`} key={i} className="draft">
                                            <div className="draft-content">
                                                <p className="draft-theme">{draft.title}</p>
                                                <p className="draft-text"> - {draft.text}</p>
                                            </div>
                                            <p className="draft-date">{new Date(draft.lastEditDate).toLocaleDateString("ru-RU")}</p>
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