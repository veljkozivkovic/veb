import { Application } from "./Application.js";

const response = await fetch("https://localhost:7080/Ispit/SveBiblioteke");// KAD JE GET SAMO URL
const body = await response.json();
const app = new Application(body);

app.draw(document.body);