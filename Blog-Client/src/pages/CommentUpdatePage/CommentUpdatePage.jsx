import React, { useContext, useState } from 'react';
import { AuthContext } from '../../context/AuthContext';
import postService from '../../services/PostService';

import { useMutation } from 'react-query';
import { useNavigate } from 'react-router-dom';
import { useParams } from 'react-router-dom';
import * as yup from 'yup';
import './CommentUpdatePage.css';



const schema = yup.object().shape({
    commentTextEdited: yup.string()
        .required('Comment text is required')
        .min(1, "Comment text must be at least 1 characters")
        .max(1000, "Comment text must be no more than 1000 characters")
});

const CommentUpdatePage = () => {
    const navigateTo = useNavigate();

    const { postId } = useParams();
    const { commentId } = useParams();
    const { commentText } = useParams();
    
    const [commentTextEdited, setContent] = useState(commentText);
    const [error, setError] = useState('');

    const mutationGetToken = useMutation(postService.updatePostComment, {
        onSuccess: (data) => {
            console.log(data);
            if (data.isSuccess) {
                navigateTo(`/post/${postId}/details`);
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
        schema.validate({commentTextEdited })
            .then(() => {
                const requestModel = {
                    commentText: commentTextEdited
                };
                mutationGetToken.mutate({postId, commentId, requestModel});
            })
            .catch((err) => {
                setError(err.errors[0]);
            });
    };

    const handleContentChange = (event) => {
        setContent(event.target.value);
        schema.validate({ commentTextEdited })
            .then(() => {
                setError('');
            })
            .catch((err) => {
                setError(err.errors[0]);
            });

    };
    return (

        <div className="post-page">
            <h2>Comment Edit</h2>
            <label>
                <input
                    type="text"
                    value={commentTextEdited}
                    onChange={handleContentChange}
                    placeholder='text'
                />
            </label>
            <p>{error}</p>
            <button onClick={handleSubmitClick}>Submit</button>
        </div>
    );
}

export default CommentUpdatePage;