import React, { useContext, useState } from 'react';
import { AuthContext } from '../../context/AuthContext';
import { useMutation } from 'react-query';
import postService from '../../services/PostService';
import { useNavigate } from 'react-router-dom';
import './PostComments.css';
const PostComments = ({ comment}) => {
  const [message, setMessage] = useState('');
  const { isAuthenticated } = useContext(AuthContext);
  const navigateTo = useNavigate();

  if (isAuthenticated) {
    let userLogin = localStorage.getItem('userLogin');

    const deleteCommentMutation = useMutation(postService.DeletePostComment, {
      onSuccess: (data) => {
        console.log(data);
        if (data.isSuccess) {
          navigateTo(`/post/${comment.postId}/details`);
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
    const handleCommentDeleteClick = () => {
      deleteCommentMutation.mutate({comment})
    };

    const handleCommentUpdateClick = () => {
      var url = `/post/${comment.postId}/comment/${comment.id}/update/${comment.commentText}`;
      navigateTo(url);
    };
    if (userLogin == comment.authorLogin) {
      return (
        <div className="comment">
          <h1 className="comment-title">{comment.authorLogin}</h1>
          <p className="comment-content">{comment.commentText}</p>
          <p>{comment.createdAt}</p>
          <button className="comment-delete-button" onClick={handleCommentDeleteClick}>Delete</button>
          <button className="comment-update-button" onClick={handleCommentUpdateClick}>Update</button>
          <p>{message}</p>
        </div>
      );
  }
  }
  return (
    <div className="comment">
      <h1 className="comment-title">{comment.authorLogin}</h1>
      <p className="comment-content">{comment.commentText}</p>
    </div>
  );
};
export default PostComments;
