import express from 'express'
import mongoose from 'mongoose'

import UserRouter from './api/user.js'
import MapRouter from './api/map.js'
import GameRouter from './api/game.js'
import BlogRouter from './api/blog.js'
import helper from "./functions.js"
import schema from './schema.js'

import db from "./mongodb_connection.js"
import bodyParser from 'body-parser'
import cors from 'cors'

// const url = "mongodb://localhost:27017/pacman"
// const port = 3000;
const app = express();
// const db = mongoose.connection;
// mongoose.connect(url, {})
//     .then(result => console.log("database connected"))
//     .catch(err => console.log(err))

app.use(bodyParser.json({limit: '50mb'}))
app.use(bodyParser.urlencoded({extended: false}))
app.use(cors({orogin: '*'}))

app.use('/api/user', UserRouter)
app.use('/api/map', MapRouter)
app.use('/api/game', GameRouter)
app.use('/api/blog', BlogRouter)

app.get('/', (req,res) => {
    res.send("<h1>hello from node js app</h1>")
})

app.listen(3000,'127.0.0.1')

// await helper.signUp("a1","a1@gmail.com", "password").then(res => console.log(res))
// await helper.reset("a1","password", "password1").then(res => console.log(res))
// await helper.retrieveUser("user1").then(res => console.log(res))
// await helper.deleteUser("a1").then(res => console.log(res))

// await helper.addBlog("CSCI3100.2","pacman3D project.2", "Grp1").then(res => console.log(res))
// await helper.getBlogByUser("Grp1").then(res => console.log(res))
// await helper.deleteBlog("643402e26581fc9abecc0c25").then(res => console.log(res))
// await helper.likeBlog("643408ade1651e47415a1757").then(res => console.log(res))






