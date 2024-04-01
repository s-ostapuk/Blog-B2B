import React, { useContext, useEffect, useState } from "react";
import { AuthContext } from '../../context/AuthContext';
import postService from '../../services/PostService';
import SinglePost from "../../components/post/SinglePost";
import PostComments from "../../components/postComments/PostComments";
import { useMutation } from 'react-query';
import { useParams } from 'react-router-dom';
import { useNavigate } from 'react-router-dom';
import * as yup from 'yup';

import './PostDetailsPage.css';

const schema = yup.object().shape({
    commentTextEdited: yup.string()
        .required('Comment text is required')
        .min(1, "Comment text must be at least 1 characters")
        .max(1000, "Comment text must be no more than 1000 characters")
});
const PostDetailsPage = () => {
    const navigateTo = useNavigate();
    const { postId } = useParams();
    const { isAuthenticated, loginAction } = useContext(AuthContext);
    const [blogPost, setBlogPost] = useState(null);
    const [loading, setLoading] = useState(true);
    const [commentTextEdited, setContent] = useState('');
    const [error, setError] = useState('');

    const mutationGetToken = useMutation(postService.createNewPostComment, {
        onSuccess: (data) => {
            console.log(data);
            if (data.isSuccess) {
                navigateTo(`/post/${postId}/details`);
                window.location.reload();
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
        schema.validate({ commentTextEdited })
            .then(() => {
                const requestModel = {
                    commentText: commentTextEdited
                };
                mutationGetToken.mutate({ postId, requestModel });
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
    useEffect(() => {
        const fetchBlogPost = async () => {
            setLoading(true);
            const data = await postService.getPostById(postId)
            setBlogPost(data);
            setLoading(false);
        };
        fetchBlogPost();
    }, []);
    if (loading) {
        return <p>Loading...</p>;
    }
    if (!loading && (!blogPost.data.post)) {
        return <p>No blog posts available.</p>;
    }
    if (isAuthenticated) {
        return (
            <div className="posts-container">
                <SinglePost key={blogPost.data.post.id} post={blogPost.data.post} />
                <p>Comments:</p>
                {blogPost.data.post.comments.map((comment) => (
                    <PostComments key={comment.id} comment={comment} />
                ))}
                <div className="comment-input">
                    <label>
                        <input
                            type="text"
                            value={commentTextEdited}
                            onChange={handleContentChange}
                            placeholder='text'
                        />
                    </label>
                    <button onClick={handleSubmitClick}>Add Comment</button>
                </div>
                <p>{error}</p>
            </div>
        );
    }
    else {
        return (
        <div className="posts-container">
        <SinglePost key={blogPost.data.post.id} post={blogPost.data.post} />
        <p>Comments:</p>
        {blogPost.data.post.comments.map((comment) => (
            <PostComments key={comment.id} comment={comment} />
        ))}
        </div>
        )
    }
}

export default PostDetailsPage;