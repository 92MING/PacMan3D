import express from 'express';
import { Router } from 'express';
const MapRouter = new Router();
import db from "../mongodb_connection.js"

MapRouter.post("/saveMap", async (req, res) => {
    const { id, creatorID, name, mapSize, mapCells } = req.body;
    const result = await db.collection("maps").insertOne({
      id: id,
      creatorID: creatorID,
      name: name,
      mapSize: mapSize,
      mapCells: mapCells,
      createdAt: new Date()
    });
    if (result) {
      res.send({ isSaved: true, message: "Map saved successfully" });
    } else {
      res.send({ isSaved: false, error_message: "Failed to save map" });
    }
  });

  MapRouter.get('/getMap/:id', async (req, res) => {
    const mapId = req.params.id;
    const result = await db.collection('maps').findOne({ id: id });
    if (result) {
      res.send({ isGot: true, data: result });
    } else {
      res.send({ isGot: false, error_message: `Map with ID ${mapId} not found` });
    }
  });

  MapRouter.get("/getMap/:creatorId", async (req, res) => {
    const creatorId = req.params.creatorId;
    const result = await db.collection("maps").find({ creatorId: creatorId }).toArray();
    if (result) {
        res.send({ isGot: true, data: result });
      } else {
        res.send({ isGot: false, error_message: `Map with user ID ${creatorId} not found` });
      }
  });

  MapRouter.delete("/:id", async (req, res) => {
    const id = req.params.id;
    const result = await db.collection("maps").deleteOne({ id: id });
    if (result.deletedCount === 1) {
      res.send({ isDeleted: true, message: "Map deleted successfully" });
    } else {
      res.send({ isDeleted: false, error_message: "Failed to delete map" });
    }
  });
export default MapRouter;
