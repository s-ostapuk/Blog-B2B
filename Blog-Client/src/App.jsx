import React, { useContext, useState } from 'react';
import { AuthContext } from './context/AuthContext';
import { AuthProvider } from './authProvider/AuthProvider';

import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import { QueryClient, QueryClientProvider } from 'react-query';


import Header from './components/header/Header';
import HomePage from './pages/HomePage/HomePage';
import NotFoundPage from './pages/NotFoundPage/NotFoundPage';
import LoginPage from './pages/LoginPage/LoginPage';
import RegisterPage from './pages/RegisterPage/RegisterPage';
import PostUpdatePage from './pages/PostUpdatePage/PostUpdatePage';
import PostDetailsPage from './pages/PostDetailsPage/PostDetailsPage';
import PostCreatePage from './pages/PostCreatePage/PostCreatePage';
import CommentUpdatePage from './pages/CommentUpdatePage/CommentUpdatePage';


const queryClient = new QueryClient();
function App() {
  return (
    <AuthProvider>
      <QueryClientProvider client={queryClient}>
        <Router>
          <Header />
          <Routes>
            <Route exact path="/" element={<HomePage />} />
            <Route exact path="/login" element={<LoginPage />} />
            <Route exact path="/register" element={<RegisterPage />} />
            <Route exact path="/post/update" element={<PostUpdatePage />} />
            <Route exact path="/post/create" element={<PostCreatePage />} />
            <Route exact path="/post/:postId/details" element={<PostDetailsPage />} />
            <Route exact path="/post/:postId/comment/:commentId/update/:commentText" element={<CommentUpdatePage />} />
            <Route exact path="*" element={<NotFoundPage />} />
          </Routes>
        </Router>
      </QueryClientProvider>
    </AuthProvider>
  );
}

export default App;