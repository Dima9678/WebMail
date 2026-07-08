import { useEffect, useState } from 'react';
import { Routes, Route, Link, useParams } from "react-router-dom";


import type { User } from "../interfaces/User";
import type { Letter } from "../interfaces/Letter";

function letter() {
    const [letter, setLetter] = useState<Letter>();
    const { id } = useParams();

    useEffect(() => {
        fetch(`https://localhost:7094/api/letter/${id}`, {
            credentials: "include"
        })
            .then(async r => {
                if (!r.ok) throw new Error(await r.text());
                return r.json();
            })
            .then(data => setLetter(data))

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
                        <Link to="/newletter" className="new-letter-button"><img className="leftbar-navigation-button-style" src="/images/pencil.svg" alt="написать"></img></Link>
                        <Link to="/allmails" className="leftbar-navigation-button"><img className="leftbar-navigation-button-style" src="/images/envelope.svg" alt="конверт"></img></Link>
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
                                <h1 className="one-letter-title">{letter.title}</h1>
                                <div className="one-letter-info">
                                    <p className="one-letter-adressee-name">{letter.adresseeName}</p>
                                    <p className="one-letter-adressee-email">{letter.adresseeEmail}</p>
                                    <div className="one-letter-data-block">
                                        <p className="one-letter-sending-date">{new Date(letter.sendTime).toLocaleDateString("ru-RU")}</p>
                                        <p className="one-letter-sending-time">{new Date(letter.sendTime).toLocaleTimeString("ru-RU")}</p>
                                    </div>
                                </div>
                                <p className="one-letter-sending-text">{letter.text}</p>
                            </div>
                        )}
                    </div>
                </div>
            </div>
        </div>
    );
}

export default letter;