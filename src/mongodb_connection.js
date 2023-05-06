import express from 'express'
import mongoose from 'mongoose'

const url = "mongodb+srv://sad9jai:04875398MON@csci3100.9m8rnfx.mongodb.net/pacman?retryWrites=true&w=majority"
const db = mongoose.connection;
mongoose.connect(url, {})
    .then(result => console.log("database connected"))
    .catch(err => console.log(err))

export default db