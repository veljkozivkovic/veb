import { Application } from "./Application.js";

const response = await fetch("https://localhost:7080/Ispit/VratiSvaKola");
const body = await response.json();
const app = new Application(body);

app.draw(document.body);