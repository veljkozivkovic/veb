import { Biblioteka } from "./Biblioteka.js"

export class Application {
    
    constructor(biblioteke) {
        this.biblioteke = biblioteke.map(x => new Biblioteka(x.id, x.naziv));
        console.log(this.biblioteke)
    }

    draw(container) {
        this.biblioteke.forEach(biblioteka => {
            const bibliotekaDiv = document.createElement("div");
            bibliotekaDiv.classList.add("biblioteka-div")
            container.appendChild(bibliotekaDiv);
            biblioteka.draw(bibliotekaDiv);
        });
    }
}