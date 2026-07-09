import type React from "react";
import { useState } from "react";
import { Link } from "react-router-dom";

function Signin() {
    
    const [password, setPassword] = useState("");
    const [email, setEmail] = useState("");
    const [resultMessage, setResultMessage] = useState("");
    const [authResult, setAuthResult] = useState(false);

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();


        const response = await fetch("https://localhost:7094/api/auth/login", {
            method: "POST",
            credentials: "include",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                email,
                password
            })
        });

        if (response.ok) {
            setResultMessage("Успешно");
            setPassword("");
            setEmail("");
            setAuthResult(true);
        }
        else {
            const message = await response.text();
            setResultMessage(message);
        }
    };
    return (
        <div className="sign-main-container">
            <div className="sign-main-box">
                <Link to="/" className="sign-topbar">MyMail</Link>
                <form onSubmit={handleSubmit} className="sign-form">
                    <p className="sign-page-name">Вход в аккаунт</p>
                    <input
                        className="sign-input-box"
                        placeholder="Адрес почты"
                        value={email}
                        onChange={(e) => setEmail(e.target.value)}
                    />
                    <input
                        className="sign-input-box"
                        placeholder="Пароль"
                        type="password"
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                    />
                    {authResult ? (
                        <div className="sign-error-container">
                            <p className="sign-error-message">{resultMessage}</p>
                            <Link to="/" className="links">На главную</Link>
                        </div>

                    ) : (
                        <div className="sign-error-container">
                            <p className="sign-error-message">{resultMessage}</p>
                        </div>
                    )}
                    <button type="submit" className="submit-login-button">Войти</button>
                </form>
            </div>
        </div>
    );
}

export default Signin;