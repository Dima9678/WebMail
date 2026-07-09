import { Route, Routes } from "react-router-dom";

import LogoutPage from "./pages/auth/logout";
import SignInPage from "./pages/auth/signin";
import SignUpPage from "./pages/auth/signup";
import MyProfilePage from "./pages/myprofile";

import HomePage from "./pages/homepage";
import NewLetter from "./pages/newletter";

import Letter from "./pages/letter";

import DraftPage from "./pages/drafts";
import SentPage from "./pages/sent";
import SpamPage from "./pages/spam";
import StarredMailsPage from "./pages/starred";
import TrashPage from "./pages/trash";

import './assets/css/App.css';
import './assets/css/Reset.css';


function App() {
    return (
        <>
            <Routes>
                <Route path="/" element={<HomePage />} />
                <Route path="/signin" element={<SignInPage />} />
                <Route path="/signup" element={<SignUpPage />} />
                <Route path="/auth/logout" element={<LogoutPage />} />
                <Route path="/myprofile" element={<MyProfilePage />} />

                <Route path="/newletter" element={<NewLetter />} />
                <Route path="/letter/:id" element={<Letter />} />

                <Route path="/sent" element={<SentPage />} />
                <Route path="/starred" element={<StarredMailsPage />} />
                <Route path="/drafts" element={<DraftPage />} />
                <Route path="/spam" element={<SpamPage />} />
                <Route path="/trash" element={<TrashPage />} />
            </Routes>
        </>

    );

}
export default App;