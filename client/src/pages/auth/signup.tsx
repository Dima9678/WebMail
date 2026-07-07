import type React from "react";
import { useEffect, useState } from 'react';
import { Routes, Route, Link } from "react-router-dom";



function Signup() {
    const [name, setName] = useState("");
    const [login, setLogin] = useState("");
    const [password, setPassword] = useState("");
    const [repeatPassword, setRepeatPassword] = useState("");
    const [email, setEmail] = useState("");

    const [resultMessage, setResultMessage] = useState("");
    const [authResult, setAuthResult] = useState(false);

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();


        const response = await fetch("https://localhost:7094/api/auth/register", {
            method: "POST",
            credentials: "include",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                name,
                email,
                password,
                repeatPassword,
            })
        });

        if (response.ok) {
            setAuthResult(true);
            setResultMessage("Успешно, теперь вы можете войти в свой аккаунт");
            setName("");
            setLogin("");
            setPassword("");
            setRepeatPassword("");
            setEmail("");
        }
        else {
            const message = await response.text();
            setAuthResult(false);
            setResultMessage(message);
        }
    };
    return (
        <div className="sign-main-container">
            <div className="sign-main-box">
                <div className="sign-topbar">MyMail</div>
                <form onSubmit={handleSubmit} className="sign-form">
                    <p className="sign-page-name">Регистрация</p>
                    <input
                        className="sign-input-box"
                        placeholder="Имя"
                        value={name}
                        onChange={(e) => setName(e.target.value)}
                    />
                    <div className="email-create-main">
                        <input
                            className="email-create-input-box"
                            placeholder="Создайте почту"
                            value={email}
                            onChange={(e) => setEmail(e.target.value)}
                        />
                        <p className="email-create-template">@mymail.com</p>
                    </div>
                    
                    <input
                        className="sign-input-box"
                        placeholder="Пароль"
                        type="password"
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                    />
                    <input
                        className="sign-input-box"
                        placeholder="Повторите пароль"
                        type="password"
                        value={repeatPassword}
                        onChange={(e) => setRepeatPassword(e.target.value)}
                    />
                    <p className="have-a-account-message">Уже есть аккаунт? <Link to="/signin" className="links">Вход</Link></p>
                    {authResult ? (
                        <>
                            <p className="sign-error-message">{resultMessage}</p>
                            <nav>
                                <Link to="/signin" className="links">Вход</Link>
                            </nav>
                        </>
                    ) : (
                        <p className="sign-error-message">{resultMessage}</p>
                    )}
                    <button type="submit" className="submit-login-button">Создать аккаунт</button>
                </form>
            </div>
        </div>
    );
}

export default Signup;