import { Application } from "./Application/Application.js";


const response = await fetch('https://localhost:7080/Ispit/VratiSveKuce');
const body = await response.json();

console.log(body);

var app = new Application(body);
await app.vratiOcenjeneFilmoveZaKategoriju(1);
app.draw(document.body);