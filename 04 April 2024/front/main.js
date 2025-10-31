import { Application } from "./app/Application.js";



const sale = await fetch("https://localhost:7080/Ispit/VratiSveSale")
const bodySale = await sale.json()

//console.log(bodySale);
var app = new Application(bodySale)

app.draw(document.body);