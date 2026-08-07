import { useState, useEffect, use } from 'react';
import { Link } from "react-router-dom";

import type { User } from "../interfaces/User";
import App from '../App';

function myprofile() {
    const [user, setUser] = useState<User | null>(null);
    const [isOpen, setIsOpen] = useState(false);

    useEffect(() => {
        fetch("https://localhost:7094/api/User", {
            method: "GET",
            credentials: 'include',
        })
            .then(async r => {
                if (r.status === 401) {
                    setUser(null)
                    return null;
                }
                if (!r.ok) {
                    throw new Error(await r.text())
                }
                return r.json()
            })
            .then(data => {
                if (data) {
                    setUser(data);
                }
            })
            .catch(console.error)
    }, []);

    return (
        <div className="parent-container">
            <div className="main-container">
                <div className="topbar">
                    <div className="centered-container">
                        <Link to="/" className="website-logo">MyMail</Link>
                    </div>
                    {user === null ? (
                        <div className="auth-buttons">
                            <Link to="/signin" className="auth-button">Вход</Link>
                            <Link to="/signup" className="auth-button">Регистрация</Link>
                        </div>
                    ) : (
                        <div className="auth-buttons">
                            <Link to="/myprofile" className="auth-button">{user.name}</Link>
                        </div>
                    )}
                </div>
                <div className="myprofile-main-content">
                    <nav className="sidebar">
                        <Link to="/" className="leftbar-navigation-button"><img className="leftbar-navigation-button-style" src="/images/envelope.svg" alt="конверт"></img></Link>
                        <Link to="/sent" className="leftbar-navigation-button"><img className="leftbar-navigation-button-style" src="/images/plane.svg" alt="самолет"></img></Link>
                        <Link to="/starred" className="leftbar-navigation-button"><img className="leftbar-navigation-button-style" src="/images/star.svg" alt="звезда"></img></Link>
                        <Link to="/drafts" className="leftbar-navigation-button"><img className="leftbar-navigation-button-style" src="/images/draft.svg" alt="черновики"></img></Link>
                        <Link to="/spam" className="leftbar-navigation-button"><img className="leftbar-navigation-button-style" src="/images/spam.svg" alt="спам"></img></Link>
                        <Link to="/trash" className="leftbar-navigation-button"><img className="leftbar-navigation-button-style" src="/images/trash.svg" alt="корзина"></img></Link>
                    </nav>
                    <div className="myprofile-main-container">
                        <div className="myprofile-second-container">
                            <p className="myprofile-user-info">Имя: {user?.name}</p>
                            <p className="myprofile-user-info">Фамилия: {user?.surname}</p>
                            <p className="myprofile-user-info">Пол: {user?.isMan ? "мужской" : "женский"}</p>
                            <p className="myprofile-user-info">Почта: {user?.email}</p>
                            <p className="myprofile-user-info">Дата регистрации: 05.08.2026</p>
                        </div>
                        <div className="myprofile-buttons">
                            <button onClick={() => setIsOpen(true)} className="myprofile-button">Изменить данные</button>
                            <Link to="/auth/logout" className="myprofile-button">Выйти из аккаунта</Link>
                        </div>
                        {isOpen ? (
                            <>
                                {<RenderModalForm
                                    user={user}
                                    setIsOpen={setIsOpen}
                                />}
                            </>
                        ) : (
                            <></>
                        )}
                    </div>
                </div>
            </div>
        </div>
    );
}

type RenderModalFormProps = {
    user: User | null;
    setIsOpen: React.Dispatch<React.SetStateAction<boolean>>;
}

function RenderModalForm({ user, setIsOpen }: RenderModalFormProps) {
    const [sucsessed, setSucsessed] = useState(true);
    const [errorMessage, setErrorMessage] = useState("");

    const [name, setName] = useState(user?.name);
    const [surname, setSurname] = useState(user?.surname);
    const [isMan, setIsMan] = useState(user?.isMan);
    const [email, setEmail] = useState(user?.email);
        
    return (
        <>
            <div className="myprofile-modal-overlay">
                <div className="myprofile-modal-form">
                    <div className="myprofile-modal-form-forms-block">
                        <p className="myprofile-modal-form-heading">Изменение данных</p>
                        <input className="myprofile-modal-form-imput" value={name} onChange={(e) => setName(e.target.value)} placeholder="Имя"></input>
                        <input className="myprofile-modal-form-imput" value={surname} onChange={(e) => setSurname(e.target.value)} placeholder="Фамилия"></input>
                        <input checked={isMan} type="checkbox" onChange={(e) => setIsMan(e.target.checked)}></input> <label>Мужик?</label>
                        <input className="myprofile-modal-form-imput" value={email} onChange={(e) => setEmail(e.target.value)} placeholder="Почта"></input>
                    </div>
                    {sucsessed ? (
                        <p>{errorMessage}</p>
                    ) : (
                        <p>Ошибка</p>
                    )}
                    
                    <div className="myprofile-modal-form-buttons">
                        <button className="myprofile-modal-form-button" onClick={() => setIsOpen(false)}>Отмена</button>
                        <button className="myprofile-modal-form-button" onClick={() => SaveUserData()}>Сохранить</button>
                    </div>
                </div>
            </div>
        </>
    );

    async function SaveUserData() {
        console.log("responce " + isMan);
        const response = await fetch("https://localhost:7094/api/user", {
            credentials: 'include',
            method: "PATCH",
            headers: {
                'Content-Type': "application/json"
            },
            body: JSON.stringify({
                name,
                surname,
                isMan,
                email
            }),
        });

        if (!response.ok) {
            setSucsessed(false);
            setErrorMessage(await response.text());
        }
        else {
            setIsOpen(false);
            window.location.reload();
        }
    }
}

export default myprofile;