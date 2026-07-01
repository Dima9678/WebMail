import { useEffect, useState } from 'react';
import { Routes, Route, Link } from "react-router-dom";

import AcceptPage from "./pages/accept";
import SentPage from "./pages/sent";

import './assets/css/Reset.css'
import './assets/css/App.css'

interface User {
    id: string;

    name: string;
    email: string;
    passwordHash: string;

    letters: Letter[];
    sentLetters: Letter[];
    acceptLetters: Letter[];
}

interface Letter {
    id: string;
    title: string;
    text: string;

    addresseeId: string;
    addressee: User;

    recipientId: string;
    recipient: User;
    isSent: boolean;
}

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
        <div className="parent-container">
            <div className="main-container">
                <div className="topbar">
                    
                    <p className="website-logo">ТипоПочта</p>
                    <nav>
                        <Link to="/sent">Отправленные</Link>
                        <Link to="/accept">Полученные</Link>
                    </nav>
                    <Routes>
                        <Route path="/sent" element={<SentPage />} />
                        <Route path="/accept" element={<AcceptPage />} />
                    </Routes>
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
export default App;