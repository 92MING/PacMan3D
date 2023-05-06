import express from 'express';
import { Router } from 'express';
const UserRouter = new Router();
import db from "../mongodb_connection.js"
import nodemailer from 'nodemailer';

UserRouter.post('/signup', async (req, res) => {
    const username = req.body.username;
    const password = req.body.password;
    const email = req.body.email;
    const userExisted = await db.collection("users").findOne({ email: email });
    if (userExisted != null) {
        res.send({ isCreated: false, error_message: "User already exists"});
    } else {
        const result = await db.collection("users").insertOne({ username: username, password: password, email: email });
        if (!result) res.send({ isCreated: false, error_message: "Failed to sign up"});
        else res.send({ isCreated: true });
    }
});

UserRouter.post('/login', async (req, res) => {
    const email = req.body.email
    const password = req.body.password;
    const result = await db.collection("users").findOne({email: email, password: password})
    if (!result) res.send({ isLogin: false })
    else res.send({isLogin: true , username: result.username})
})


UserRouter.get('/:username', async (req, res) => {
    const username = req.params.username
    const result = await db.collection("users").findOne({"username" : username})
    if (!result) res.send(null)
    else res.send(result)
})

UserRouter.delete("/:username", async (req, res) => {
    const username = req.params.username
    const result = await db.collection("users").deleteOne({ "username": username })
    if (!result) res.send({isDeleted: false, error_message: "Failed to delete user"})
    else res.send({isDeleted: true})
})

UserRouter.post('/reset', async (req, res) => {
    const oldPassword = req.body.oldPassword;
    const newPassword = req.body.newPassword;
    const email = req.body.email;
    // Check if user exists in the database
    const user = await db.collection("users").findOne({ email: email });
    if (user == null) {
        res.send({ isReset: false, error_message: "User not found" });
    } else {
        // Verify old password against database record
        if (oldPassword === user.password) {
            // Update password in the database
            const result = await db.collection("users").updateOne({ email:email }, { $set: { password: newPassword } });
            if (!result) {
                res.send({ isReset: false, error_message: "Failed to reset password" });
            } else {
                res.send({ isReset: true });
            }
        } else {
            res.send({ isReset: false, error_message: "Old password does not match" });
        }
    }
});


UserRouter.post('/send-email', async (req, res) => {
    const email= req.body.email;
    // Check if user exists in the database
    const user = await db.collection("users").findOne({ email: email });
    const password = user.password;
    if (!user) {
          res.send({ isSent: false, message: "User not found" });
        } 
    else {
           // Send the password to the user's email address
            const transporter = nodemailer.createTransport({
                service: 'gmail',
                auth: {
                    user: 'csci3100pacman@gmail.com',
                    pass: 'dvwgfmycxoonnedf'
          }
        });
        const mailOptions = {
          from: 'csci3100pacman@gmail.com',
          to: email,
          subject: 'Password recovery',
          text: `Hi ${user.username}, your password is: ${password}`
        };
        await transporter.sendMail(mailOptions);
        res.send({ isSent: true ,message: "Password sent" });
      }
  });


export default UserRouter;
