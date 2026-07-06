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
        <div className="signup-main-container">
            <div className="signup-main-box">
                <form onSubmit={handleSubmit}>
                    <p className="login-form-name">Регистрация</p>
                    <input
                        className="login-input-box"
                        placeholder="Имя"
                        value={name}
                        onChange={(e) => setName(e.target.value)}
                    />
                    <label className="login-label">Создайте свой адрес электронной почты. Он должен заканчиваться на @mymail.com</label>
                    <input
                        className="login-input-box"
                        placeholder="Введите адрес"
                        value={email}
                        onChange={(e) => setEmail(e.target.value)}
                    />
                    <input
                        className="login-input-box"
                        placeholder="Пароль"
                        type="password"
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                    />
                    <input
                        className="login-input-box"
                        placeholder="Повторите пароль"
                        type="password"
                        value={repeatPassword}
                        onChange={(e) => setRepeatPassword(e.target.value)}
                    />
                    {authResult ? (
                        <>
                            <p className="error-message">{resultMessage}</p>
                            <nav>
                                <Link to="/signin" className="links">Вход</Link>
                            </nav>
                        </>
                    ) : (
                        <p className="error-message">{resultMessage}</p>
                    )}
                    <button type="submit">Зарегистрироваться</button>
                </form>
            </div>
        </div>
    );
}

export default Signup;