import { useEffect } from "react";
import { useNavigate } from "react-router-dom";

function LogoutPage() {
    const navigate = useNavigate();

    useEffect(() => {
        async function doLogout() {
            await fetch("https://localhost:7094/api/auth/logout", {
                method: "POST",
                credentials: "include"
            });

            navigate("/");
        }

        doLogout();
    }, []);

    return <p>Выход...</p>;
}

export default LogoutPage;