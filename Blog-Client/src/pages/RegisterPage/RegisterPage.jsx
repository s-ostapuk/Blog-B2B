import React, { useContext, useState } from 'react';
import { AuthContext } from '../../context/AuthContext';
import authService from '../../services/AuthService';

import { useMutation } from 'react-query';
import { useNavigate } from 'react-router-dom';
import * as yup from 'yup';
import './RegisterPage.css';



const schema = yup.object().shape({
    login: yup.string()
        .required('Username is required')
        .min(3, 'Username must be at least 3 characters')
        .max(10, 'Username must be no more than 10 characters'),
    password: yup.string()
        .required('Password is required')
        .min(6, "Password must be at least 6 characters"),
    email: yup.string()
        .required('Email is required')
        .email('Invalid email adress')
});

const RegisterPage = () => {


    const { isAuthenticated} = useContext(AuthContext);
    const [login, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [email, setEmail] = useState('');
    const [error, setError] = useState('');
    const navigateTo = useNavigate();
    if (isAuthenticated) {
        navigateTo('/');
    }

    const mutationRegisterNewUser = useMutation(authService.registerNewUser, {
        onSuccess: (data) => {
            console.log(data);
            if (data.isSuccess) {
                navigateTo('/Login');
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


    const handleRegisterClick = () => {
        schema.validate({ login, password,email })
            .then(() => {
                const requestModel = {
                    Login: login,
                    Password: password,
                    Email:email
                };
                mutationRegisterNewUser.mutate(requestModel);
            })
            .catch((err) => {
                setError(err.errors[0]);
            });
    };

    const handleLoginClick = () => {
        navigateTo('/register');
    };
    const handleLoginChange = (event) => {
        setUsername(event.target.value);
        schema.validate({ login, password,email })
            .then(() => {
                setError('');
            })
            .catch((err) => {
                setError(err.errors[0]);
            });
    };
    const handlePasswordChange = (event) => {
        setPassword(event.target.value);
        schema.validate({ login, password,email })
            .then(() => {
                setError('');
            })
            .catch((err) => {
                setError(err.errors[0]);
            });

    };
    const handlePasswordEmail = (event) => {
        setEmail(event.target.value);
        schema.validate({ login, password,email })
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
                    type="text"
                    value={email}
                    onChange={handlePasswordEmail}
                    placeholder='email'
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
            <button onClick={handleRegisterClick}>Register</button>
            <button onClick={handleLoginClick}>
                Go to Login page
            </button>
        </div>
    );
};

export default RegisterPage;