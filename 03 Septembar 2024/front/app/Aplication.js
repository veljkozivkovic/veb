import { Racun } from "./Racun.js"
import { Stan } from "./Stan.js";

export class Aplication{
    constructor(racuni,stanovi)
    {
        this.racuni = racuni.map(x => new Racun(x.id, x.mesec, x.voda, x.struja, x.usluge, x.placen));
        this.stanovi = stanovi.map(x => new Stan(x.id, x.imeVlasnika, x.brojClanova));
        this.pocetniStan = stanovi[0]

        //console.log(this.racuni)
        //console.log(this.stanovi[0]);
        //console.log(this.stanovi);


        this.nizDoleLevo = [
            {labela: "Broj stana:", klasa: "broj-stana-div", pocetniProperty: String(this.pocetniStan.id)},
            {labela: "Ime vlasnika:", klasa: "ime-vlasnika-div", pocetniProperty: this.pocetniStan.imeVlasnika},
            {labela: "Povrsina(m2)", klasa: "povrsina-div",pocetniProperty: "36"},
            {labela: "Broj clanova:", klasa: "broj-clanova-div", pocetniProperty: this.pocetniStan.brojClanova},
        ]

        this.nizZaPrikazStana = [
            {labela: "Mesec:", property: "mesec"},
            {labela: "Voda:", property: "voda"},
            {labela: "Struja:", property: "struja"},
            {labela: "Komunalne Usluge:", property: "usluge"},
            {labela: "Placen:", property: "placen"},
        ]
    }

    draw(container){

        const kontejner = document.createElement('div');
        kontejner.classList.add('container');
        container.appendChild(kontejner);

        const levaStrana = document.createElement('div');
        levaStrana.classList.add('leva-strana');
        kontejner.appendChild(levaStrana);


        const goreLeva = document.createElement('div');
        goreLeva.classList.add('gore-leva-strana');
        levaStrana.appendChild(goreLeva);

        const glPrviRed = document.createElement('div');
        glPrviRed.classList.add('gl-prvi-red');
        goreLeva.appendChild(glPrviRed);

        const birajStanLabel = document.createElement('label');
        birajStanLabel.classList.add('biraj-stan-label');
        birajStanLabel.innerHTML = "Biraj stan";
        glPrviRed.appendChild(birajStanLabel);

        const birajStanSelect = document.createElement('select');
        birajStanSelect.classList.add('select-stan')
        //console.log("STANOVIIIIIII")
        //console.log(this.stanovi)

        this.stanovi.forEach(element => {
            const opcija = document.createElement('option');
            opcija.value = element.id;
            opcija.innerHTML = element.id;
            birajStanSelect.appendChild(opcija);
        });

        glPrviRed.appendChild(birajStanSelect);

        const glDrugiRed = document.createElement('div');
        glDrugiRed.classList.add('gl-drugi-red');
        goreLeva.appendChild(glDrugiRed);

        const prikazInformacija = document.createElement('button');
        prikazInformacija.classList.add('prikaz-informacija');
        glDrugiRed.appendChild(prikazInformacija);
        prikazInformacija.innerHTML = "Prikaz informacija"; 

        prikazInformacija.addEventListener('click', (event) => this.prikazInformacijaKlik(event, birajStanSelect.value ));


        const doleLevaStrana = document.createElement('div');
        doleLevaStrana.classList.add('dole-leva-strana');
        levaStrana.appendChild(doleLevaStrana);

        
        this.nizDoleLevo.forEach(x => {
            const dlRed = document.createElement('div');
            dlRed.classList.add('dl-red');
            doleLevaStrana.appendChild(dlRed);

            const labela = document.createElement('label');
            labela.innerHTML = x.labela;
            dlRed.appendChild(labela);

            const divv = document.createElement('div');
            divv.classList.add(x.klasa);
            dlRed.appendChild(divv);
            divv.innerHTML = x.pocetniProperty;

            //console.log(x.pocetniProperty);
        });
        const dlRedZadnji = document.createElement('div');
        dlRedZadnji.classList.add('dl-red');
        doleLevaStrana.appendChild(dlRedZadnji);

        const izracunajUkupno = document.createElement('button');
        izracunajUkupno.classList.add('izracunaj-ukupno-zad-button');
        izracunajUkupno.innerHTML = "Izracunaj ukupno zaduzenje";
        dlRedZadnji.appendChild(izracunajUkupno);
        izracunajUkupno.addEventListener('click', this.vratiUkupnoZaduzenje)


        const desnaStrana = document.createElement('div');
        desnaStrana.classList.add('desna-strana');
        kontejner.appendChild(desnaStrana);

        this.nacrtajRacuni(desnaStrana);







    }


    async vratiUkupnoZaduzenje(event){
        //console.log(event);
        const selectStan = document.querySelector('.select-stan');
        //console.log(selectStan.value);
        const resp = await fetch(`https://localhost:7080/Ispit/UkupnoZaduzenjeZaStan/${selectStan.value}`)
        const body = await resp.json();

        //console.log(body);

        alert(`Ukupno zaduzenje je ${body}`);
    }


    nacrtajRacuni(desnaStrana){
        desnaStrana.replaceChildren();
        this.racuni.forEach(x => {
            const racunDiv =  document.createElement('div');
            racunDiv.classList.add('racun-div');
            desnaStrana.appendChild(racunDiv);

            this.nizZaPrikazStana.forEach(element =>{
                const redDiv =  document.createElement('div');
                redDiv.classList.add('stan-red-div');
                racunDiv.appendChild(redDiv);

                const labela =  document.createElement('label');
                labela.innerHTML = element.labela;
                redDiv.appendChild(labela);

                const divv =  document.createElement('div');
                divv.innerHTML = x[element.property];
                redDiv.appendChild(divv);

                if(element.property == "placen")
                {

                    if(x[element.property])
                    {
                        racunDiv.classList.add('racun-div-zeleni')
                        divv.innerHTML = "Da";
                    }
                    else{
                        racunDiv.classList.add('racun-div-crveni')
                        divv.innerHTML = "Ne";
                    }
                }

            });


        });
    }

    async prikazInformacijaKlik(event, stanId){
        console.log(event);
        console.log(stanId);

        const stanResp = await fetch(`https://localhost:7080/Ispit/VratiStan/${stanId}`);
        const stan = await stanResp.json();
        //broj-stana-div ime-vlasnika-div broj-clanova-div
        console.log(stan);
        const brojStana = document.querySelector('.broj-stana-div');
        brojStana.innerHTML = stan[0].id;

        const imeVlasnika = document.querySelector('.ime-vlasnika-div');
        imeVlasnika.innerHTML = stan[0].imeVlasnika;

        const brojClanova = document.querySelector('.broj-clanova-div')
        brojClanova.innerHTML = stan[0].brojClanova;

        console.log(stan[0].racuni)
        this.racuni = stan[0].racuni.map(x => new Racun(x.id, x.mesec, x.voda, x.struja, x.usluge, x.placen));
        const desnaStrana = document.querySelector('.desna-strana')
        this.nacrtajRacuni(desnaStrana);

    }
}