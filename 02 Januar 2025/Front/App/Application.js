import {Kola} from "./Kola.js"

export class Application{
    
    constructor(kola){
        this.kola = kola.map(x=> new Kola(x));
        console.log(this.kola);
        this.nizKola = [
            {labela: "Model", property: "model"},
            {labela: "Kilometraza", property: "kilometraza"},
            {labela: "Godiste", property: "godiste"},
            {labela: "Broj Sedista", property:"brojSedista"},
            {labela: "Cena Po Danu", property: "cenaPoDanu"},
            {labela: "Iznajmljen", property: "daLiJeIznajmljen"}
        ]

        this.nizLevoGore = [
            {labela: "Ime i prezime", polje: "text", name: "imeIPrezime"},
            {labela: "JMBG", polje: "text", name: "jmbg"},
            {labela: "Broj vozacke dozvole", polje: "text", name: "brojVozacke"},
            {labela: "Broj Dana", polje: "number", name: "brojDana"}
        ]
        
        this.nizLevoDole = [
            {labela: "Predjena Kilometraza", polje: "text", name: "kilometraza"},
            {labela: "Broj sedista", polje: "text", name: "sedista"},
            {labela: "Cena", polje: "number", name: "cena"},
        ]
        // ne zaboravi model select

    }

    draw(container)
    {

        const celiDiv = document.createElement('div');
        celiDiv.classList.add('celi-div');
        container.appendChild(celiDiv);

        const leviDiv = document.createElement("div");
        leviDiv.classList.add('levi-div');
        celiDiv.appendChild(leviDiv);

        const leviGoreDiv = document.createElement("div");
        leviGoreDiv.classList.add('levi-gore-div');
        leviDiv.appendChild(leviGoreDiv);

        const leviDoleDiv = document.createElement("div");
        leviDoleDiv.classList.add('levi-dole-div');
        leviDiv.appendChild(leviDoleDiv);


        this.napuniLeviGoreDiv(leviGoreDiv);
        this.napuniLeviDoleDiv(leviDoleDiv);

        const desniDiv = document.createElement("div");
        desniDiv.classList.add('desni-div-ceo');
        celiDiv.appendChild(desniDiv);
        this.popuniDesniDiv(desniDiv);

        

    }

    popuniDesniDiv(container)
    {
        this.kola.forEach(element =>{

            const div = document.createElement('div');
            
            container.appendChild(div);

            this.nizKola.forEach(x =>{
                const vrsta = document.createElement("div");
                vrsta.classList.add('desni-div-vrsta');
                div.appendChild(vrsta);

                const labela = document.createElement('label');
                labela.classList.add('desni-div-vrsta-labela');
                labela.innerHTML = x.labela;
                vrsta.appendChild(labela);

                const property = document.createElement('label');
                property.classList.add('desni-div-vrsta-property');
                
                
                if(x.property == 'daLiJeIznajmljen')
                {
                    
                    if(element.iznajmljen)
                    {
                        property.innerHTML = "true";
                        div.classList.add('desni-div-crveni')
                    }
                    else{
                        property.innerHTML = "false";
                        
                        div.classList.add('desni-div-zeleni')
                    }
                }
                else{
                    property.innerHTML = element[x.property];
                }
                vrsta.appendChild(property);


            });

            const iznajmi = document.createElement('button');
            iznajmi.classList.add(`iznajmi-button-${element.id}`);
            iznajmi.classList.add('iznajmi-button');
            div.appendChild(iznajmi);
            iznajmi.innerHTML = "IZNAJMI"
            iznajmi.addEventListener('click', (event) => this.iznajmiAuto(event, element.id));

        })
    }

    async iznajmiAuto(event, idAuta){

        const brojDana = document.querySelector('.brojDana')
        if(brojDana.value != '')
        {
            //console.log(brojDana.value);
            const response = await fetch(`https://localhost:7080/Ispit/DodajIznajmljivanje/${idAuta}/1`,
                {
                    method: "POST",
                    body: JSON.stringify(
                        {
                            "brojDana": brojDana.value
                        }
                    ),
                    headers:    {
                                "Content-Type": "application/json",
                            },
                }

            )
            const responsee = await fetch("https://localhost:7080/Ispit/VratiSvaKola");
            const body = await responsee.json();
            this.kola = body.map(x=> new Kola(x));
            const desniDiv = document.querySelector('.desni-div-ceo');
            console.log(desniDiv);
            desniDiv.replaceChildren();
            console.log(desniDiv);
            this.popuniDesniDiv(desniDiv);
        }
        
    }

    napuniLeviGoreDiv(container)
    {
        this.nizLevoGore.forEach(element => {
            const diva = document.createElement('div');
            diva.classList.add('polje-div');
            container.appendChild(diva);

            const labela = document.createElement("label");
            labela.classList.add('levi-gore-label')
            labela.innerHTML = element.labela;
            diva.appendChild(labela);

            const polje = document.createElement('input');
            polje.type = element.polje;
            polje.classList.add(`${element.name}`)
            diva.appendChild(polje);

        })
    }

    napuniLeviDoleDiv(container){
        
        this.nizLevoDole.forEach(element => {
            const div = document.createElement("div");
            div.classList.add("polje-div");
            container.appendChild(div);

            const labela = document.createElement("label");
            labela.classList.add("levi-dole-label");
            labela.innerHTML = element.labela;
            div.appendChild(labela);


            const polje = document.createElement('input');
            polje.type = element.polje;
            polje.classList.add(`${element.name}`);
            div.appendChild(polje);


        })

        const div = document.createElement("div");
        div.classList.add("polje-div");
        container.appendChild(div);

        const label = document.createElement('label');
        label.classList.add('levi-dole-label')
        label.innerHTML = "Model"
        div.appendChild(label);
        

        const polje = document.createElement('select');

        fetch("https://localhost:7080/Ispit/VratiSveModele")
        .then(res => res.json())
        .then(modeli => {
            polje.replaceChildren();
            modeli.forEach(element => {
                const opt = document.createElement("option");
                opt.value = element.id;
                opt.innerHTML = element.naziv;
                polje.appendChild(opt);
            });
        });

        div.appendChild(polje);
        
        
        const dugme = document.createElement('button');
        dugme.classList.add("button-pretrazi");
        container.appendChild(dugme);
        dugme.innerHTML = "PRETRAZI";
        dugme.addEventListener('click', (event) => this.popuniDesniDivPretrazenih(event));

    }

    async popuniDesniDivPretrazenih(event){
        //console.log(event)
        const predjenaKilometraza = document.querySelector('.kilometraza');
        const brojSedista = document.querySelector('.sedista');
        const cena = document.querySelector('.cena');
        const model = document.querySelector('select');

        console.log(predjenaKilometraza.value)
        console.log(brojSedista.value)
        console.log(cena.value)
        console.log(model.value )

        const resp = await fetch(`https://localhost:7080/Ispit/PretraziKola?PredjenaKilometraza=${predjenaKilometraza.value}&BrojSedista=${brojSedista.value}&Cena=${cena.value}&ModelID=${model.value}`);
        const body = await resp.json();
        this.kola = body.map(x=> new Kola(x));

        const desniDiv = document.querySelector('.desni-div-ceo');
        console.log(desniDiv);
        desniDiv.replaceChildren();
        console.log(desniDiv);
        this.popuniDesniDiv(desniDiv);
        


    }


}


            // {labela: "Predjena Kilometraza", polje: "text", name: "kilometraza"},
            // {labela: "Broj sedista", polje: "text", name: "sedista"},
            // {labela: "Cena", polje: "number", name: "cena"},