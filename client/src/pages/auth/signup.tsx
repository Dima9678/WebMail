import type React from "react";
import { useEffect, useState } from 'react';
import { Link } from "react-router-dom";



function Signup() {
    const [name, setName] = useState("");
    const [surname, setSurame] = useState("");
    const [, setLogin] = useState("");
    const [password, setPassword] = useState("");
    const [isMan, setIsMan] = useState(true);
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
                surname,
                email,
                password,
                repeatPassword,
                isMan
            })
        });

        if (response.ok) {
            setAuthResult(true);
            setResultMessage("Успешно");
            setName("");
            setSurame("");
            setLogin("");
            setPassword("");
            setIsMan(true);
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
                <Link to="/" className="sign-topbar">MyMail</Link>
                <form onSubmit={handleSubmit} className="sign-form">
                    <p className="sign-page-name">Регистрация</p>
                    <input
                        className="sign-input-box"
                        placeholder="Имя"
                        value={name}
                        onChange={(e) => setName(e.target.value)}
                    />
                    <input
                        className="sign-input-box"
                        placeholder="Фамилия"
                        value={surname}
                        onChange={(e) => setSurame(e.target.value)}
                    />

                    <div className="gender-selector">
                        <div
                            className={isMan ? "gender-selector-left-on" : "gender-selector-left-off"}
                            onClick={(e) => setIsMan(true)}
                        >
                        М
                        </div>
                        <div
                            className={!isMan ? "gender-selector-right-on" : "gender-selector-right-off"}
                            onClick={(e) => setIsMan(false)}
                        >
                        Ж
                        </div>
                    </div>

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
                        <div className="sign-error-container">
                            <p className="sign-error-message">{resultMessage}</p>
                            <Link to="/" className="links">На главную</Link>
                        </div>

                    ) : (
                        <div className="sign-error-container">
                                <p className="sign-error-message">{resultMessage}</p>
                        </div>
                    )}
                    <button type="submit" className="submit-login-button">Создать аккаунт</button>
                </form>
            </div>
        </div>
    );
}

export default Signup;