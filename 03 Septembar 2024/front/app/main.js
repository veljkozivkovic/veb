import { Aplication } from "./Aplication.js";

const response = await fetch("https://localhost:7080/Ispit/VratiRacuneZaStan/1")
const body = await response.json();


const stanovi = await fetch("https://localhost:7080/Ispit/VratiSveStanove");
const stanoviBody = await stanovi.json();

const app = new Aplication(body, stanoviBody);

app.draw(document.body);