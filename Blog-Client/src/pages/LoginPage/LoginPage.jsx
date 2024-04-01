import React, { useContext, useState } from 'react';
import { AuthContext } from '../../context/AuthContext';
import authService from '../../services/AuthService';

import { useMutation } from 'react-query';
import { useNavigate } from 'react-router-dom';
import * as yup from 'yup';
import './LoginPage.css'; 



const schema = yup.object().shape({
    login: yup.string()
        .required('Username is required')
        .min(3, 'Username must be at least 3 characters')
        .max(10, 'Username must be no more than 10 characters'),
    password: yup.string()
        .required('Password is required')
        .min(6, "Password must be at least 6 characters")
});

const LoginPage = () => {
    

    const { isAuthenticated, loginAction} = useContext(AuthContext);
    const [login, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState('');
    const navigateTo = useNavigate();
    if(isAuthenticated){
        navigateTo('/');
    }
    const mutationGetToken = useMutation(authService.getAuthToken, {
        onSuccess: (data) => {

            console.log(data);
            if (data.isSuccess && data !== null && data.data !== null && data.data.accessToken !== null) {
                loginAction(data.data);
                navigateTo('/');
            }
            else {
                setError(data.errors[0]);
            }
        },
        onError: (error) => {
            // Handle error scenario here
            console.log(error);
        },
    });


    const handleLoginClick = () => {
        schema.validate({ login, password })
            .then(() => {
                const requestModel = {
                    Login: login,
                    Password: password
                };
                mutationGetToken.mutate(requestModel);
            })
            .catch((err) => {
                setError(err.errors[0]);
            });
    };
    const handleRegisterClick = () => {
        navigateTo('/register');
    };
    const handleLoginChange = (event) => {
        setUsername(event.target.value);
        schema.validate({ login, password })
            .then(() => {
                setError('');
            })
            .catch((err) => {
                setError(err.errors[0]);
            });
    };
    const handlePasswordChange = (event) => {
        setPassword(event.target.value);
        schema.validate({ login, password })
            .then(() => {
                setError('');
            })
            .catch((err) => {
                setError(err.errors[0]);
            });

    };
    return (
        
        <div className="login-page">
        
            <h2>Login</h2>
            <label>
                <input
                    type="text"
                    value={login}
                    onChange={handleLoginChange}
                    placeholder='Username'
                />
            </label>
            <label>
                <input
                    type="password"
                    value={password}
                    onChange={handlePasswordChange}
                    placeholder='Password'
                />
            </label>
            <p>{error}</p>
            <button onClick={handleLoginClick}>Login</button>
            <button onClick={handleRegisterClick}>
                Go to register page
            </button>
        </div>
    );
};

export default LoginPage;