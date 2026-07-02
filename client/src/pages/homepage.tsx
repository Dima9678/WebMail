import { useEffect, useState } from 'react';
import { Routes, Route, Link } from "react-router-dom";

import type { User } from "../interfaces/User";
import type { Letter } from "../interfaces/Letter";

interface HomePageProps {
    user: User | null;
}

function homepage({ user }: HomePageProps) {
    return (
        <div className="parent-container">
            <div className="main-container">
                <div className="topbar">

                    <p className="website-logo">ТипоПочта</p>
                    <nav>
                        <Link to="/sent" className="links">Отправленные</Link>
                        <Link to="/accept" className="links">Полученные</Link>
                        <Link to="/newletter" className="links">Написать</Link>
                        <Link to="/signin" className="links">Вход</Link>
                        <Link to="/singup" className="links">Регистрация</Link>
                    </nav>
                </div>
                <div className="letters-block">
                    {user?.letters?.map((l, i) => (
                        <div className="letter" key={i}>
                            <p className="letter-text">{l.text} {i}</p>
                        </div>
                    ))}
                </div>
            </div>
        </div>
    );
}

export default homepage;