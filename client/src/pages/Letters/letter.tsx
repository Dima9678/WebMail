import { useEffect, useState } from 'react';
import { Link, useParams } from "react-router-dom";

import type { User } from "../interfaces/User";
import type { Letter } from "../interfaces/Letter";
import type { FullLetter } from '../../interfaces/FullLetter';


function letter() {
    const [letter, setLetter] = useState<FullLetter>();
    const [previousId, setPreviousId] = useState("");
    const [nextId, setNextId] = useState("");
    const [lettersTotal, setLettersTotal] = useState("");
    const [letterNumber, setLetterNumber] = useState(0);

    const [, setUser] = useState<User | null>(null);
    const { id } = useParams();

    useEffect(() => {
        async function loadData() {
            try {
                const userResponse = await fetch("https://localhost:7094/api/User", {
                    credentials: "include"
                });

                let currentUser: User | null = null;

                if (userResponse.status !== 401) {
                    if (!userResponse.ok) {
                        throw new Error(await userResponse.text());
                    }

                    currentUser = await userResponse.json();
                    setUser(currentUser);
                }

                const letterResponse = await fetch(
                    `https://localhost:7094/api/letter/${id}`,
                    {
                        credentials: "include"
                    }
                );

                if (!letterResponse.ok) {
                    throw new Error(await letterResponse.text());
                }

                const letterData: FullLetter = await letterResponse.json();

                setLetter(letterData);
                setPreviousId(letterData.previousLetterId);
                setNextId(letterData.nextLetterId);
                setLetterNumber(letterData.letterNumber)

                console.log("пред: ");

            } catch (error) {
                console.error(error);
            }
        }

        loadData();
        GetTotal();
    }, [id]);

    async function GetTotal() {
        const response = await fetch(`https://localhost:7094/api/letter/total`, {
            credentials: "include",
            method: "GET"
        });

        if (!response.ok)
            throw new Error(await response.text());

        const data = await response.json();
        setLettersTotal(data)
    }


    return (
        <div className="parent-container">
            <div className="main-container">
                <div className="topbar">
                    <div className="centered-container">
                        <Link to="/" className="website-logo">MyMail</Link>
                    </div>
                    <div className="auth-buttons">
                        <Link to="/myprofile" className="auth-button">Мой аккаунт</Link>
                    </div>
                </div>
                <div className="main-content">
                    <nav className="sidebar">

                        <Link to="/" className="leftbar-navigation-button"><img className="leftbar-navigation-button-style" src="/images/envelope.svg" alt="конверт"></img></Link>
                        <Link to="/sent" className="leftbar-navigation-button"><img className="leftbar-navigation-button-style" src="/images/plane.svg" alt="самолет"></img></Link>
                        <Link to="/starred" className="leftbar-navigation-button"><img className="leftbar-navigation-button-style" src="/images/star.svg" alt="звезда"></img></Link>
                        <Link to="/drafts" className="leftbar-navigation-button"><img className="leftbar-navigation-button-style" src="/images/draft.svg" alt="черновики"></img></Link>
                        <Link to="/spam" className="leftbar-navigation-button"><img className="leftbar-navigation-button-style" src="/images/spam.svg" alt="спам"></img></Link>
                        <Link to="/trash" className="leftbar-navigation-button"><img className="leftbar-navigation-button-style" src="/images/trash.svg" alt="корзина"></img></Link>
                    </nav>
                    <div className="letters-block">
                        <div className="one-letter-topbar">
                            <Link to="/"><img className="arrow" src="/images/arrow.svg" alt="назад"></img></Link>
                            <div className="one-letter-pages-navigation-container">
                                {nextId === null ? (
                                    <div className="pages-navigation-button-hidden"></div>
                                ) : (
                                    <Link to={`/letter/${nextId}`} className="pages-navigation-button">предыдущее</Link>
                                )}
                                <p className="pages-navigation-text">{letterNumber} из {lettersTotal}</p>
                                {previousId === null ? (
                                    <div className="pages-navigation-button-hidden"></div>
                                ) : (
                                    <Link to={`/letter/${previousId}`} className="pages-navigation-button">следующее</Link>
                                )}
                            </div>
                        </div>
                        {letter === undefined ? (
                            <div className="one-letter-main-container">
                                <p>Данные загружаются</p>
                            </div>
                        ) : (
                            <div className="one-letter-main-container">

                                <div className="one-letter-header">
                                    <div className="one-letter-title-block">
                                        <p className="one-letter-title">{letter.title}</p>
                                        <p className="one-letter-datetime">
                                            {new Date(letter.sendTime).toLocaleDateString("ru-RU")} в
                                            {new Date(letter.sendTime).toLocaleTimeString("ru-RU")}
                                        </p>
                                    </div>
                                    <div className="one-letter-sender-block">
                                        <div className="one-letter-avatar">
                                            <p className="one-letter-avatar-letter">Е</p>
                                        </div>
                                        <div>
                                            <p className="one-letter-adressee-name">{letter.adresseeName}</p>
                                            <p className="one-letter-adressee-email">{letter.adresseeEmail}</p>
                                        </div>
                                    </div>
                                </div>

                                <p className="one-letter-text">{letter.text}</p>

                                <div className="one-letter-button-container">
                                    <div className="one-letter-activity-button">Ответить</div>
                                    <div className="one-letter-activity-button">Переслать</div>
                                </div>
                            </div>
                        )}
                    </div>
                </div>
            </div>
        </div>
    );
}

export default letter;