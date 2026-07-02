import { useEffect, useState } from 'react';
import { Routes, Route, Link } from "react-router-dom";

import AcceptPage from "./pages/accept";
import SentPage from "./pages/sent";
import SignInPage from "./pages/signin";
import SignUpPage from "./pages/signup";
import HomePage from "./pages/homepage";

import './assets/css/Reset.css'
import './assets/css/App.css'

import type { User } from './interfaces/User';
import type { Letter } from './interfaces/Letter';

function App() {
    const [user, setUser] = useState<User | null>(null);

    //этот код выполняется один раз при запуске, и отвечает за обслуживание
    //mode динамический, если он меняется, пройдет устловная конструкция
    useEffect(() => {
        fetch("https://localhost:7094/api/User")
            .then(r => r.json())
            .then(data => setUser(data))
            .catch(console.error);
    }, []);

    return (
        <>
            <Routes>
                <Route path="/" element={<HomePage user={user} />} />
                <Route path="/sent" element={<SentPage user={user} />} />
                <Route path="/accept" element={<AcceptPage user={user} />} />
                <Route path="/singin" element={<SignInPage />} />
                <Route path="/singup" element={<SignUpPage />} />
            </Routes>
        </>

    );

}
export default App;