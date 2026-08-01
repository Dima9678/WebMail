import { useEffect, useState } from 'react';
import { Link, useParams } from "react-router-dom";

import type { User } from "../interfaces/User";
import type { Letter } from "../interfaces/Letter";
import type { FullLetter } from '../../interfaces/FullLetter';

import RenderLetter from "../../components/Letter/RenderLetter";
import RenderReply from "../../components/Letter/RenderReply";


function letter() {
    const [user, setUser] = useState<User | null>(null);

    const { id } = useParams();
    const [letter, setLetter] = useState<FullLetter>();

    const [starred, setStarred] = useState(false);
    const [isRead, setIsRead] = useState(false);

    const [previousId, setPreviousId] = useState("");
    const [nextId, setNextId] = useState("");
    const [lettersTotal, setLettersTotal] = useState("");
    const [letterNumber, setLetterNumber] = useState(0);

    const [replyMode, setReplyMode] = useState(false);
    const [replyText, setReplyText] = useState("");

    const [forwardMode, setForwardMode] = useState(false);
    const [forwardEmail, setForwardEmail] = useState("");

    const [errorMessage, setErrorMessage] = useState("");
    const [sucsess, setSucsess] = useState(false);

    useEffect(() => {
        async function loadData() {
            try {
                await LoadUser();

            } catch (error) {
                console.error(error);
            }
        }

        loadData();
    }, [id]);

    async function LoadUser() {
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
            LoadLetter(currentUser.id)
        }
    }
    async function LoadLetter(currentUserId: string) {
        const letterResponse = await fetch(`https://localhost:7094/api/letter/${id}`,
            {
                credentials: "include",
                method:"GET"
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

        const state = letterData.letterStates.find(s => s.userId === currentUserId);

        setStarred(state.starred)
        setIsRead(state.isRead)

        if (letter?.adresseeId === user?.id) {
            GetTotal("sent");
        }
        GetTotal("accept");
    }
    async function GetTotal(type: string) {
        if (type === "sent") {
            const response = await fetch(`https://localhost:7094/api/letter/inbox/count`, {
                credentials: "include",
                method: "GET"
            });

            if (!response.ok)
                throw new Error(await response.text());

            const data = await response.json();
            setLettersTotal(data)
        }
        else {
            const response = await fetch(`https://localhost:7094/api/letter/sent/count`, {
                credentials: "include",
                method: "GET"
            });

            if (!response.ok)
                throw new Error(await response.text());

            const data = await response.json();
            setLettersTotal(data)
        }
    }

    function changeStarred() {
        fetch(`https://localhost:7094/api/letter/${letter?.id}/toggle-starred`, {
            credentials: "include",
            method: "PUT"
        })
            .then(async r => {
                if (!r.ok) {
                    throw new Error(await r.text());
                }
                setStarred(!starred);
            })
            .catch(console.error);
    }
    function changeReadState() {
        fetch(`https://localhost:7094/api/letter/${letter?.id}/toggle-read`, {
            credentials: "include",
            method: "PUT"
        })
            .then(async r => {
                if (!r.ok) {
                    throw new Error(await r.text());
                }
                setIsRead(!isRead);
            })
            .catch(console.error);
    }
    function ChangeReplyMode() {
        setReplyMode(!replyMode);
    }
    function ChangeForwardMode() {
        setForwardMode(!forwardMode);
    }

    async function Reply() {
        const replyId = id;
        const response = await fetch(`https://localhost:7094/api/letter/reply/${id}`, {
            method: "POST",
            credentials: "include",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                replyText
            })
        });

        if (response.ok) {
            setReplyText("");
            setReplyMode(false);
            setSucsess(true);

            LoadLetter(user.id);
        }
        else {
            setSucsess(false);
            const message = await response.text();
            setErrorMessage(message);
        }
    }
    async function Forward() {
        const letterId = letter?.parentLetter ? letter.parentLetterId : id;
        const response = await fetch("https://localhost:7094/api/letter/forward", {
            credentials: 'include',
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                forwardEmail,
                letterId,
            })
        })
        if (response.ok) {
            setForwardEmail("");
            setForwardMode(false);
            setSucsess(true);
            setErrorMessage("Успешно");
        }
        else {
            setSucsess(false);
            setErrorMessage("");
            const message = await response.text();
            setErrorMessage(message);
        }
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

                            <div className="one-letter-topbar-buttons">
                                {starred ? (
                                    <button onClick={changeStarred}><img src="/images/letterPage/starred.svg" className="one-letter-topbar-button" alt="star" /></button>
                                ) : (
                                    <button onClick={changeStarred}><img src="/images/letterPage/unstarred.svg" className="one-letter-topbar-button" alt="star" /></button>
                                )}

                                {isRead ? (
                                    <button onClick={changeReadState}><img src="/images/letterPage/mark_unread.svg" className="one-letter-topbar-button" alt="unread" /></button>
                                ) : (
                                    <button onClick={changeReadState}><img src="/images/letterPage/mark_read.svg" className="one-letter-topbar-button" alt="read" /></button>
                                )}

                                <button><img src="/images/letterPage/spam.svg" className="one-letter-topbar-button" alt="spam" /></button>
                                <button><img src="/images/letterPage/trash.svg" className="one-letter-topbar-button" alt="trash" /></button>
                            </div>
                            <div className="one-letter-pages-navigation-container">
                                {nextId === null ? (
                                    <div className="pages-navigation-button-hidden"></div>
                                ) : (
                                    <Link to={`/letter/${nextId}`} className="pages-navigation-button">Предыдущее</Link>
                                )}
                                <p className="pages-navigation-text">{letterNumber} из {lettersTotal}</p>
                                {previousId === null ? (
                                    <div className="pages-navigation-button-hidden"></div>
                                ) : (
                                    <Link to={`/letter/${previousId}`} className="pages-navigation-button">Следующее</Link>
                                )}
                            </div>
                        </div>

                        {letter ? (
                            <>
                                {letter?.parentLetter ? (
                                    <>
                                        <RenderLetter
                                            currentLetter={letter.parentLetter}
                                            replyMode={replyMode}
                                            forwardMode={forwardMode}
                                            replyText={replyText}
                                            forwardEmail={forwardEmail}
                                            errorMessage={errorMessage}
                                            setReplyText={setReplyText}
                                            setForwardEmail={setForwardEmail}
                                            Reply={Reply}
                                            Forward={Forward}
                                            ChangeReplyMode={ChangeReplyMode}
                                            ChangeForwardMode={ChangeForwardMode}
                                        />
                                        <RenderReply currentLetter={letter} />
                                    </>

                                ) : (
                                    <>
                                        <RenderLetter
                                            currentLetter={letter}
                                            replyMode={replyMode}
                                            forwardMode={forwardMode}
                                            replyText={replyText}
                                            forwardEmail={forwardEmail}
                                            errorMessage={errorMessage}
                                            setReplyText={setReplyText}
                                            setForwardEmail={setForwardEmail}
                                            Reply={Reply}
                                            Forward={Forward}
                                            ChangeReplyMode={ChangeReplyMode}
                                            ChangeForwardMode={ChangeForwardMode}
                                        />

                                        {letter?.childrenLetters?.map((childLetter) =>
                                        (
                                            <RenderReply currentLetter={childLetter} key={childLetter.id} />
                                        ))}
                                    </>
                                )}
                            </>
                        ) : (
                            <div className="one-letter-main-container">
                                <p>Данные загружаются</p>
                            </div>
                        )}
                    </div>
                </div>
            </div>
        </div>
    );
}

export default letter;