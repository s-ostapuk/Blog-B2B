import React, { useContext, useEffect, useState } from "react";
import { AuthContext } from '../../context/AuthContext';
import postService from '../../services/PostService';
import SinglePost from "../../components/post/SinglePost";
import './HomePage.css';



const HomePage = () => {
    const [blogPosts, setBlogPosts] = useState(null);
    const [loading, setLoading] = useState(true);
    const { isAuthenticated } = useContext(AuthContext);
    useEffect(() => {
        const fetchBlogPosts = async () => {
            setLoading(true);
            const data = await postService.getAllPosts()
            setBlogPosts(data);
            setLoading(false);
        };

        fetchBlogPosts();
    }, []);

    if (loading) {
        return <p>Loading...</p>;
    }

    if (!loading && (!blogPosts || blogPosts.length === 0)) {
        return (<div>
             <p>No blog posts available.</p>;
             <a href="post/create">Create post</a>
        </div>)
            
    }
    if (isAuthenticated) {
        return (
            <div className="posts-container">
                {blogPosts.map((post) => (
                    <SinglePost key={post.id} post={post} />
                ))}
            <a href="post/create">Create post</a>
            </div>
        );
    }
    
    return (
        <div className="posts-container">
            {blogPosts.map((post) => (
                <SinglePost key={post.id} post={post} />
            ))}
        </div>
    );
};
export default HomePage;