import { Parser } from 'xml2js';
const parser = new Parser(/* options */);

const localAddress = "http://127.0.0.1:3000/api/"
const userAPI = 'http://127.0.0.1:3000/api/user/';
const mapAPI = 'http://127.0.0.1:3000/api/map/';
const gameAPI = 'http://127.0.0.1:3000/api/game/';
const blogAPI = 'http://127.0.0.1:3000/api/blog/';

async function signUp(username, email, password) {
    try {
        const url = userAPI + "signup";
        const body = {
            username: username,
            email: email,
            password: password
        };
        const options = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(body)
        };
        const response = await fetch(url, options);
        return response.data;
    } catch (error) {
        console.error('Error fetching data:', error);
        throw new Error('Failed to fetch data from the API');
    }
}

async function login(email, password) {
    try {
        const url = userAPI + "login";
        const body = {
            email: email,
            password: password
        };
        const options = {
            method: 'POST', // HTTP method: GET, POST, PUT, DELETE, etc.
            headers: {  'Content-Type': 'application/json'}, // Set the request body type to JSON
            body: JSON.stringify(body) 
          };
        const response = await fetch(url, options); // Fetch the data from the API
        return response.data; // Return the data retrieved from the API
    } catch (error) {
        console.error('Error fetching data:', error);
        throw new Error('Failed to fetch data from the API'); // Throw an error if fetching fails
      }
    }

async function reset(username, oldPassword, newPassword) {
    try {
        const url = userAPI + "reset";
        const body = {
            username: username,
            oldPassword: oldPassword,
            newPassword: newPassword
        };
        const options = {
            method: 'POST', // HTTP method: GET, POST, PUT, DELETE, etc.
            headers: {  'Content-Type': 'application/json'}, // Set the request body type to JSON
            body: JSON.stringify(body) // Convert the body to JSON format
          };
        const response = await fetch(url, options); // Fetch the data from the API
        return response.data; // Return the data retrieved from the API
    } catch (error) {
        console.error('Error fetching data:', error);
        throw new Error('Failed to fetch data from the API'); // Throw an error if fetching fails
      }
    }

async function retrieveUser(username) {
    try {
        const url = userAPI + username;
        const options = {
            method: 'GET', // HTTP method: GET, POST, PUT, DELETE, etc.
            headers: {  'Content-Type': 'application/json'}, // Set the request body type to JSON
            };
        const response = await fetch(url, options); // Fetch the data from the API
        console.log("retrieve successfully");
        return response.data; // Return the data retrieved from the API
    } catch (error) {
        console.error('Error fetching data:', error);
        throw new Error('Failed to fetch data from the API'); // Throw an error if fetching fails
        }
    }

async function deleteUser(username) {
    try {
        const url = userAPI + username;
        const options = {
            method: 'DELETE', // HTTP method: GET, POST, PUT, DELETE, etc.
            headers: {  'Content-Type': 'application/json'}, // Set the request body type to JSON
            };
        const response = await fetch(url, options); // Fetch the data from the API
        return response.data; // Return the data retrieved from the API
    } catch (error) {
        console.error('Error fetching data:', error);
        throw new Error('Failed to fetch data from the API'); // Throw an error if fetching fails
        }
    }

async function addBlog(title, content, username) {
    try {
        const url = blogAPI + "add";
        const body = {
            title: title,
            content: content,
            username: username,
        };
        const options = {
            method: 'POST', // HTTP method: GET, POST, PUT, DELETE, etc.
            headers: {  'Content-Type': 'application/json'}, // Set the request body type to JSON
            body: JSON.stringify(body) // Convert the body to JSON format
            };
        const response = await fetch(url, options); // Fetch the data from the API
        return response.data; // Return the data retrieved from the API
    } catch (error) {
        console.error('Error fetching data:', error);
        throw new Error('Failed to fetch data from the API'); // Throw an error if fetching fails
        }
    }

async function getBlogByUser(creatorId) {
    try {
        const url = blogAPI + "get"; // Update the URL to the appropriate endpoint for getting blogs
        const options = {
            method: 'GET', // Use the GET HTTP method to retrieve data
            headers: {
                'Content-Type': 'application/json' // Set the request body type to JSON
            },
            params: { creatorId: creatorId } // Pass the creatorId as a query parameter
        };
        const response = await fetch(url, options); // Fetch the data from the API
        return response.data; // Return the data retrieved from the API
    } catch (error) {
        console.error('Error fetching data:', error);
        throw new Error('Failed to fetch data from the API'); // Throw an error if fetching fails
    }
}

async function deleteBlog(blogId) {
    try {
        const result = await fetch(blogAPI + blogId, { method: 'DELETE' }); // Update the URL to the appropriate endpoint for deleting a blog
        const data = await result.json(); // Extract the JSON response from the API
        if (data.isDeleted) {
            console.log('Blog deleted successfully');
        } else {
            console.error('Error deleting blog:', data.error_message);
        }
    } catch (error) {
        console.error('Error deleting blog:', error);
        throw new Error('Failed to delete blog from the API'); 
    }
}

async function likeBlog(blogId) {
    try {
        const result = await fetch(blogAPI + 'like', { 
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ blogId: blogId }) 
        });
        const data = await result.json();
        if (data.isLiked) {
            console.log('Blog liked successfully');
        } else {
            console.error('Error liking blog:', data.error_message);
        }
    } catch (error) {
        console.error('Error liking blog:', error);
        throw new Error('Failed to like blog from the API'); 
    }
}
    
async function saveMap(id, creatorID, name, mapSize, mapCells) {
    try {
      const url = mapAPI + "save";
      const body = {
        id: id,
        creatorID: creatorID,
        name: name,
        mapSize: mapSize,
        mapCells: mapCells
      };
      const options = {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(body)
      };
      const response = await fetch(url, options);
      return response.data;
    } catch (error) {
      console.error('Error fetching data:', error);
      throw new Error('Failed to fetch data from the API');
    }
  }

  async function getMapById(mapId) {
    try {
      const url = mapAPI + `get/${mapId}`;
      const options = {
        method: 'GET',
        headers: { 'Content-Type': 'application/json' },
      };
      const response = await fetch(url, options);
      const data = await response.json();
      return data;
    } catch (error) {
      console.error('Error fetching data:', error);
      throw new Error('Failed to fetch data from the API');
    }
  }

  async function getMapsByCreatorID(creatorID) {
    try {
        const url = mapAPI + `getMaps/${creatorID}`;
        const options = {
            method: 'GET',
            headers: { 'Content-Type': 'application/json' },
        };
        const response = await fetch(url, options);
        const data = await response.json();
        return data;
    } catch (error) {
        console.error('Error fetching data:', error);
        throw new Error('Failed to fetch data from the API');
    }
}

async function deleteMap(id) {
    try {
        const url = mapAPI + `delete/${id}`;
        const options = {
            method: 'DELETE',
            headers: { 'Content-Type': 'application/json' },
        };
        const response = await fetch(url, options);
        const data = await response.json();
        return data;
    } catch (error) {
        console.error('Error fetching data:', error);
        throw new Error('Failed to fetch data from the API');
    }
}
export default {signUp, login, reset, deleteUser, addBlog, retrieveUser, getBlogByUser, deleteBlog, likeBlog, saveMap, getMapById,
    getMapsByCreatorID, deleteMap};


