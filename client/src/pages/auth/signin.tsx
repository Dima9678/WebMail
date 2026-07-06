import type React from "react";
import { useState } from "react";

function Signin() {
    
    const [password, setPassword] = useState("");
    const [email, setEmail] = useState("");
    const [resultMessage, setResultMessage] = useState("");

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
        }
        else {
            const message = await response.text();
            setResultMessage(message);
        }
    };
    return (
        <div className="signup-main-container">
            <div className="signup-main-box">
                <form onSubmit={handleSubmit}>
                    <p className="login-form-name">Вход в аккаунт</p>
                    <input
                        className="login-input-box"
                        placeholder="Адрес почты"
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
                    <p className="error-message">{resultMessage}</p>
                    <button type="submit">Зарегистрироваться</button>
                </form>
            </div>
        </div>
    );
}

export default Signin;