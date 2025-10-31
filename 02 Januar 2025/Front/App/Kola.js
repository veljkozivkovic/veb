
export class Kola{
    
    constructor(Kola){
        this.id = Kola.id;
        this.model = Kola.model;
        this.kilometraza = Kola.kilometraza;
        this.godiste = Kola.godiste;
        this.brojSedista = Kola.brojSedista;
        this.cenaPoDanu = Kola.cenaPoDanu;
        this.iznajmljen = Kola.daLiJeIznajmljen;
        
    }

    draw(container)
    {
        const kolaDiv = document.createElement("div");
        kolaDiv.classList("kola-div")
        container.appendChild(kolaDiv);
        
    }
}

