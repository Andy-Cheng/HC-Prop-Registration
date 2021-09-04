const path = require('path');
const router = require('express').Router();
const multer  = require('multer');
const { DeviceModel } = require("./DB/db");

// Read HC devices
router.get("/devices", async (req, res, next)=>{
    await DeviceModel.find({}).exec((err, devices)=>{
        if(err)
        {
            console.log(`Read devices error: ${err}`);
        }
        res.status(200).json(devices);
    });
});


// Write HC devices
router.post("/device", async(req, res, next)=>{
    const deviceData = req.body.data;
    // console.log(req.body.data);

    const newDevice = new DeviceModel(JSON.parse(req.body.data));

    await newDevice.save((err) => {
        if (err) {
            console.log(`New device saved  error: ${err}`);
            res.status(200).json("save error");
        }
        else {
            console.log('New device saved successfully');
            res.status(200).json("Device Saved Successfully");
        }
    });


});


// Upload file
const storage = multer.diskStorage({
    destination: function (req, file, callback) {
        callback(null, path.join(__dirname, "/packages"));
    },
    filename: function (req, file, callback) {
        callback(null, file.originalname);
    }
});

const upload = multer({ storage: storage });
router.post("/package", upload.single("package"), (req, res, next)=>{
    res.status(200).json("Add package successfully.");
});

module.exports = {
    router
}; 