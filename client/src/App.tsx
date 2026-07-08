import { useEffect, useState } from 'react';
import { Routes, Route, Link } from "react-router-dom";

import SignInPage from "./pages/auth/signin";
import SignUpPage from "./pages/auth/signup";
import LogoutPage from "./pages/auth/logout";
import MyProfilePage from "./pages/myprofile";

import HomePage from "./pages/homepage";
import NewLetter from "./pages/newletter";

import Letter from "./pages/letter";

import AllMailsPage from "./pages/allmails";
import SentPage from "./pages/sent";
import StarredMailsPage from "./pages/starred";
import DraftPage from "./pages/drafts";
import SpamPage from "./pages/spam";
import TrashPage from "./pages/trash";

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
                <Route path="/auth/logout" element={<LogoutPage />} />
                <Route path="/myprofile" element={<MyProfilePage />} />

                <Route path="/newletter" element={<NewLetter />} />
                <Route path="/letter/:id" element={<Letter />} />

                <Route path="/allmails" element={<AllMailsPage />} />
                <Route path="/sent" element={<SentPage />} />
                <Route path="/starred" element={<StarredMailsPage />} />
                <Route path="/drafts" element={<DraftPage />} />
                <Route path="/spam" element={<SpamPage />} />
                <Route path="/trash" element={<TrashPage />} />
            </Routes>
        </>

    );

}
export default App;