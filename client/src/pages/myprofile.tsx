import { Routes, Route, Link, useParams } from "react-router-dom";

function myprofile() {
  return (
      <Link to="/auth/logout">Выйти из аккаунта</Link>
  );
}

export default myprofile;