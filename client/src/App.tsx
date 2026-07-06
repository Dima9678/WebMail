import { useEffect, useState } from 'react';
import { Routes, Route, Link } from "react-router-dom";

import SignInPage from "./pages/auth/signin";
import SignUpPage from "./pages/auth/signup";
import Logout from "./pages/auth/logout";

import HomePage from "./pages/homepage";

import AllMailsPage from "./pages/AllMails";
import SentPage from "./pages/Sent";
import StarredMailsPage from "./pages/Starred";
import DraftPage from "./pages/Drafts";
import SpamPage from "./pages/Spam";
import TrashPage from "./pages/Trash";

import './assets/css/Reset.css'
import './assets/css/App.css'

import type { User } from './interfaces/User';

function App() {
    const [user, setUser] = useState<User | null>(null);

    //этот код выполняется один раз при запуске, и отвечает за обслуживание
    //mode динамический, если он меняется, пройдет устловная конструкция
    useEffect(() => {
        fetch("https://localhost:7094/api/User", {
            credentials: "include"
        })
            .then(async r => {
                if (!r.ok) throw new Error(await r.text());
                return r.json();
            })
            .then(data => setUser(data))
            .catch(console.error);
    }, []);

    return (
        <>
            <Routes>
                <Route path="/" element={<HomePage user={user}  />} />
                <Route path="/signin" element={<SignInPage />} />
                <Route path="/signup" element={<SignUpPage />} />
                <Route path="/logout" element={<Logout />} />
            </Routes>
        </>

    );

}
export default App;