const express = require('express');

const apiUrl = process.env.API_URL || 'http://localhost:41187/api/';
console.log(`api url is ${apiUrl}`);

const clientSideDist = process.env.CLIENT_APP_DIST || './nebbodoro.spa';
const app = express();

app.use(express.static(clientSideDist));
console.log(`serving ${clientSideDist}`);

app.get('/api/environments', (req, res) => {
    res.json({
        apiUrl : apiUrl
    });
});

app.get('*', (req, res) => {
  res.sendFile(`index.html`, { root: clientSideDist });
});

const port = process.env.SERVER_PORT || '80';
app.listen(port, () => console.log(`Express app running on localhost:${port}`));