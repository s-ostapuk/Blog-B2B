import React, { useContext, useState } from 'react';
import { AuthContext } from '../../context/AuthContext';
import { useMutation } from 'react-query';
import postService from '../../services/PostService';
import { useNavigate } from 'react-router-dom';
import './SinglePost.css';
const SinglePost = ({ post }) => {
  const [message, setMessage] = useState('');

  const { isAuthenticated } = useContext(AuthContext);
  const navigateTo = useNavigate();

  const handlePostDetailsClick = () => {
    var url = `/post/${post.id}/details`;
    navigateTo(url);
  };

  if (isAuthenticated) {
    let userId = localStorage.getItem('userId');

    const deletePostMutation = useMutation(postService.deletePost, {
      onSuccess: (data) => {

        console.log(data);
        if (data.isSuccess) {
          window.location.reload();
        }
        else {
          setMessage(data.errors[0])
        }
      },
      onError: (error) => {
        // Handle error scenario here
        console.log(error);
      },
    });


    const handlePostDeleteClick = () => {
      deletePostMutation.mutate(post.id)
    };

    const handlePostUpdateClick = () => {
      var url = `/post/update`;
      navigateTo(url,{state:{post:post}});
    };
    if (userId == post.userId) {
      return (
        <div className="post">
          <h2 className="post-title">{post.title}</h2>
          <p className="post-content">{post.content}</p>
          <button className="post-details-button" onClick={handlePostDetailsClick}>Details</button>
          <button className="post-delete-button" onClick={handlePostDeleteClick}>Delete</button>
          <button className="post-update-button" onClick={handlePostUpdateClick}>Update</button>
          <p>{message}</p>
        </div>
      );
  }
  }
  return (
    <div className="post">
      <h2 className="post-title">{post.title}</h2>
      <p className="post-content">{post.content}</p>
      <button className="post-details-button" onClick={handlePostDetailsClick}>Details</button>
    </div>
  );
};
export default SinglePost;
