const express = require('express');
const cors = require('cors')
const app = express()
const http = require('http');
const server = http.createServer(app)
const { router } = require('./routes');

app.use(cors());
app.use(express.json({limit: '1mb'})); 
app.use(express.urlencoded({
  extended: true
}));
app.use("/packages", express.static('packages'));
app.use('/', router);

const port = 8080||process.env.Port;
server.listen(port, "0.0.0.0", ()=>{console.log(`listening to port ${port}`)});