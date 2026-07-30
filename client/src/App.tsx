import { Route, Routes } from "react-router-dom";

import LogoutPage from "./pages/auth/logout";
import SignInPage from "./pages/auth/signin";
import SignUpPage from "./pages/auth/signup";
import MyProfilePage from "./pages/myprofile";

import HomePage from "./pages/homepage";
import NewLetter from "./pages/Letters/newletter";

import Letter from "./pages/Letters/letter";

import DraftPage from "./pages/Drafts/DraftsPage";
import Draft from "./pages/Drafts/EditDraft";

import SentPage from "./pages/sent";
import SpamPage from "./pages/Spam";
import StarredMailsPage from "./pages/Starred";
import TrashPage from "./pages/Trash";

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

                {/*вопросик обозначает что параметр необязательный*/}
                <Route path="/draft/:id?" element={<Draft />} />

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