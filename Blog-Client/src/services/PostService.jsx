import axios from 'axios';

const getAllPosts = async () => {
  try {
  
    const response = await axios.get('http://localhost:5000/api/posts');
    return response.data.data.blogPosts;
  } catch (e) {
    console.log(e.message);
    return e.response;
  }
};

const getPostById = async (postId) => {
  try {
    const response = await axios.get(`http://localhost:5000/api/posts/${postId}`);
    return response.data;
  } catch (e) {
    console.log(e.message);
    return e.response;
  }
};

const createNewPost = async (requestModel) => {
  try {
    const config = {
      headers: {
        Authorization: `Bearer ${localStorage.getItem("accessToken")}`,
      },
    };
    const response = await axios.post(`http://localhost:5000/api/posts/create`, requestModel.requestModel, config);
    return response.data;
  } catch (e) {

    console.log(e.message);
    return e.response;
  }
};
const updatePost = async (data) => {
  try {
    const config = {
      headers: {
        Authorization: `Bearer ${localStorage.getItem("accessToken")}`,
      },
    };
  
    const response = await axios.put(`http://localhost:5000/api/posts/${data.postId}`, data.requestModel ,config);
    return response.data;
  } catch (e) {

    console.log(e.message);
    return e.response;
  }
};
const deletePost = async (postId) => {
  try {
    const config = {
      headers: {
        Authorization: `Bearer ${localStorage.getItem("accessToken")}`,
      },
    };
  
    const response = await axios.delete(`http://localhost:5000/api/posts/${postId}`,config);
    return response.data;
  } catch (e) {
    console.log(e.message);
    return e.response;
  }
};

const getCommentsByPostId = async (postId) => {
  try {
    const response = await axios.get(`http://localhost:5000/api/posts/${postId}/comments`);
    return response.data;
  } catch (e) {
    console.log(e.message);
    return e.response;
  }
};
const createNewPostComment = async (data) => {
  try {
    const config = {
      headers: {
        Authorization: `Bearer ${localStorage.getItem("accessToken")}`,
      },
    };
    
    const response = await axios.post(`http://localhost:5000/api/posts/${data.postId}/comments/create`, data.requestModel ,config);
    return response.data;
  } catch (e) {

    console.log(e.message);
    return e.response;
  }
};
const updatePostComment = async (data) => {
  try {
    const config = {
      headers: {
        Authorization: `Bearer ${localStorage.getItem("accessToken")}`,
      },
    };
    const response = await axios.put(`http://localhost:5000/api/posts/${data.postId}/comments/${data.commentId}`, data.requestModel, config);
    return response.data;
  } catch (e) {
   
    console.log(e.message);
    return e.response;
  }
};
const DeletePostComment = async (comment) => {
  try {
    const config = {
      headers: {
        Authorization: `Bearer ${localStorage.getItem("accessToken")}`,
      },
    };
    const response = await axios.delete(`http://localhost:5000/api/posts/${comment.comment.postId}/comments/${comment.comment.id}`,config);
    return response.data;
  } catch (e) {
   
    console.log(e.message);
    return e.response;
  }
};


export default {
  getAllPosts,
  getPostById,
  createNewPost,
  updatePost,
  deletePost,
  getCommentsByPostId,
  createNewPostComment,
  updatePostComment,
  DeletePostComment
};