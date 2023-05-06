import express from 'express';
import { Router } from 'express';
import { ObjectId } from 'mongodb';
const BlogRouter = new Router();
import db from "../mongodb_connection.js"

BlogRouter.post("/add", async (req, res) => {
    const { blog_title, blog_content, user} = req.body;
    const result = await db.collection("blogs").insertOne({ title: blog_title, content: blog_content, creatorId: user,  numberOfLikes: 0, createdAt: new Date() });
    if (result) {
        res.send({ isAdded: true, message: "Blog added successfully" });
    } else {
        res.send({ isAdded: false, error_message: "Failed to add blog" });
    }
});

// GET /api/blog - get all blogs
BlogRouter.get("/", async (req, res) => {
    const blogs = await db.collection("blogs").find().toArray();
    res.status(200).json(blogs);
  });

BlogRouter.get("/:creatorId", async (req, res) => {
    const creatorId = req.params.creatorId;
    const result = await db.collection("blogs").find({"creatorId": creatorId}).toArray()
    if (!result) res.send(null)
    else res.send(result)
});

BlogRouter.delete("/:blogId", async (req, res) => {
    const blogId = req.params.blogId;
    const result = await db.collection("blogs").deleteOne({ _id: new ObjectId(blogId) });
    if (result.deletedCount === 0) {
        res.send({ isDeleted: false, error_message: "Blog not found" });
    } else {
        res.send({ isDeleted: true, message: "Blog deleted successfully" });
    }
});

BlogRouter.post("/like", async (req, res) => {
    const { blogId } = req.body;
    const blog = await db.collection("blogs").findOne({ _id: new ObjectId(blogId) });
    if (!blog) {
        res.send({ isLiked: false, error_message: "Blog not found" });
        return;
    }
    const result = await db.collection("blogs").updateOne({ _id: new ObjectId(blogId) }, { $inc: { numberOfLikes: 1 } });
    if (!result) {
        res.send({ isLiked: false, error_message: "Failed to like the blog" });
    } else {
        res.send({ isLiked: true });
    }
});



export default BlogRouter;
