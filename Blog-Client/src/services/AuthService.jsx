import axios from 'axios';

const  getAuthToken = async (requestModel) => {
  try {
    const response = await axios.post('https://localhost:7299/api/auth', requestModel);
    return response.data;
  } catch(e) {
    console.log(e.message);
    return e.response.data;
  }
};
const  registerNewUser = async (requestModel) => {
  try {
    const response = await axios.post('https://localhost:7299/api/auth/signup', requestModel);
    return response.data;
  } catch(e) {
    console.log(e.message);
    return e.response.data;
  }
};


  export default {
    getAuthToken,
    registerNewUser
  };