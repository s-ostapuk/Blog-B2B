import React, { useEffect, useState } from "react";
import { AuthContext } from '../context/AuthContext';

export const AuthProvider = ({ children }) => {
    const [isAuthenticated, setIsAuthenticated] = useState(false);

    const loginAction = (data) => {
        localStorage.setItem('accessToken', data.accessToken);
        localStorage.setItem('userId', data.userInfo.id);
        localStorage.setItem('userLogin', data.userInfo.login);
        setIsAuthenticated(true);
    };

    const logoutAction = () => {
        setIsAuthenticated(false);
        localStorage.removeItem("accessToken");
        localStorage.removeItem("userId");
        localStorage.removeItem("userLogin");
        window.location.reload();
    };

    useEffect(() => {
        // Get the authentication status from local storage
        const savedAuthStatus = localStorage.getItem("accessToken");
        if (savedAuthStatus) {
            setIsAuthenticated(true);
        }
      }, []);
    return (
        <AuthContext.Provider value={{ isAuthenticated, loginAction, logoutAction }}>
            {children}
        </AuthContext.Provider>
    );
};