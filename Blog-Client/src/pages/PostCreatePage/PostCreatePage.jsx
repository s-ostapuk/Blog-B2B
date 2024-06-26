import React, { useContext, useState } from 'react';
import { AuthContext } from '../../context/AuthContext';
import postService from '../../services/PostService';

import { useMutation } from 'react-query';
import { useNavigate } from 'react-router-dom';
import { useLocation } from 'react-router-dom';
import * as yup from 'yup';
import './PostCreatePage.css';



const schema = yup.object().shape({
    title: yup.string()
        .required('Title is required')
        .min(3, 'Title must be at least 3 characters')
        .max(100, 'Title must be no more than 100 characters'),
    content: yup.string()
        .required('content is required')
        .min(1, "content must be at least 1 characters")
        .max(1000, "content must be no more than 1000 characters")
});

const PostCreatePage = () => {
    const { isAuthenticated, loginAction } = useContext(AuthContext);
    const navigateTo = useNavigate();
    const location = useLocation();
        const [title, setTitle] = useState('');
        const [content, setContent] = useState('');
        const [error, setError] = useState('');
        const mutationGetToken = useMutation(postService.createNewPost, {
            onSuccess: (data) => {
                console.log(data);
                if (data.isSuccess) {
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
    
        const handleSubmitClick = () => {
            schema.validate({ title, content })
                .then(() => {
                    const requestModel = {
                        Title: title,
                        Content: content
                    };
                    mutationGetToken.mutate({requestModel});
                })
                .catch((err) => {
                    setError(err.errors[0]);
                });
        };
    
        const handleTitleChange = (event) => {
            setTitle(event.target.value);
            schema.validate({ title, content })
                .then(() => {
                    setError('');
                })
                .catch((err) => {
                    setError(err.errors[0]);
                });
        };
        const handleContentChange = (event) => {
            setContent(event.target.value);
            schema.validate({ title, content })
                .then(() => {
                    setError('');
                })
                .catch((err) => {
                    setError(err.errors[0]);
                });
    
        };
        return (
    
            <div className="post-page">
                <h2>Post Edit</h2>
                    <input
                        type="text"
                        value={title}
                        onChange={handleTitleChange}
                        placeholder='Title'
                    />
                    <input
                        type="text"
                        value={content}
                        onChange={handleContentChange}
                        placeholder='Content'
                    />
                <p>{error}</p>
                <button onClick={handleSubmitClick}>Submit</button>
            </div>
        );
   
};

export default PostCreatePage;