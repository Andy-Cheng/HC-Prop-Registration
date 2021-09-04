/**
 *@file Defines schemas of collections in MongoDB.
 */
const mongoose = require("mongoose");
require('./connect');

const Parameter = new mongoose.Schema({
    name: String,
    type: Number,
    description: String
});


const DeviceFunction = new mongoose.Schema({
    functionName: String,
    functionDescription: String,
    parameters: [Parameter]
});

const Device = new mongoose.Schema({
    name: String,
    description: String,
    commuMethod: Number,
    trackingMethod: Number,
    functions: [DeviceFunction],
    messages: [DeviceFunction]
}, { timestamps: true });


// initiate models
const DeviceModel = mongoose.model('Device', Device);


module.exports = {
    DeviceModel,
};