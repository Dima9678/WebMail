import { useEffect, useState } from 'react';
import { Routes, Route, Link } from "react-router-dom";

import type { User } from "../interfaces/User";
import type { Letter } from "../interfaces/Letter";


function homepage() {
    return (
        <div className="parent-container">
            <div className="main-container">
                <div className="topbar">
                    <p className="website-logo">ТипоПочта</p>
                    <nav>
                        <Link to="/" className="links">Главная</Link>
                    </nav>
                </div>
            </div>
        </div>
    );
}

export default homepage;