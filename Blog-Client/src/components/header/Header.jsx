import React, { useContext } from 'react';
import { useNavigate  } from 'react-router-dom';
import { AuthContext } from '../../context/AuthContext'; 
import './Header.css';


const Header = () => {
  const { isAuthenticated, logoutAction } = useContext(AuthContext); // get the authentication state and functions
  const navigateTo = useNavigate();
  const OpenLoginPage = () => {
    navigateTo('/login'); 
  };

  return (
    <header className="header">
      <a href="/">
      <h1 className="title">Blog BTB</h1>
      </a>
      <nav className="nav">
        {isAuthenticated ? (
          <button className="link" onClick={logoutAction}>Logout</button>
        ) : (
          <button className="link" onClick={OpenLoginPage}>Login</button>
        )}
      </nav>
    </header>
  );
};

export default Header;