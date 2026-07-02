import { useEffect, useState } from 'react';
import { Routes, Route, Link } from "react-router-dom";

import type { User } from "../interfaces/User";
import type { Letter } from "../interfaces/Letter";

import HomePage from "../pages/homepage";

interface HomePageProps {
    user: User | null;
}

function Sent({ user }: HomePageProps) {
    return (
        <div className="parent-container">
            <div className="main-container">
                <div className="topbar">

                    <p className="website-logo">ТипоПочта</p>
                    <nav>
                        <Link to="/" className="links">Главная</Link>
                    </nav>
                    <p className="sent">Отправленные</p>
                </div>
                <div className="letters-block">
                    {user?.sentLetters?.map((l, i) => (
                        <div className="letter" key={i}>
                            <p className="letter-text">{l.text} {l.isSent} {i}</p>
                        </div>
                    ))}
                </div>
            </div>
        </div>
    );
}

export default Sent;