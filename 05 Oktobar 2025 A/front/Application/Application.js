import { Kuca } from "./Kuca.js";

export class Application{

    constructor(kuce)
    {
        this.kuce = kuce.map((x) => new Kuca(x.id, x.naziv, x.kategorije));

        this.nizRedova = [
            {labela: "Kategorija:", klasa: "kategorija-select"},
            {labela: "Film:", klasa: "film-select"},
            {labela: "Ocena:", klasa: "ocena-input"},
        ];
        
    }

    async vratiOcenjeneFilmoveZaKategoriju(idKategorije)
    {
        let filmovi = [];
        const najgori = await fetch(`https://localhost:7080/Ispit/NajgoreOcenjenFilm/${idKategorije}`);
        filmovi.push(await najgori.json());
        const srednje = await fetch(`https://localhost:7080/Ispit/SrednjeOcenjenFilm/${idKategorije}`);
        filmovi.push( await srednje.json());
        const najbolji = await fetch(`https://localhost:7080/Ispit/NajboljeOcenjenFilm/${idKategorije}`);
        filmovi.push(await najbolji.json());
        console.log(filmovi);
        return filmovi;
    }



    async vratiKategorijeZaKucu(idKuce)
    {
        const rezultat = await fetch(`https://localhost:7080/Ispit/VratiKategorijeZaKucu/${idKuce}`);
        var kategorije = await rezultat.json();
        return kategorije;
    }

    draw(container)
    {
        const kontejner = document.createElement('div');
        kontejner.classList.add('container');
        container.appendChild(kontejner);

        this.kuce.forEach(element => {
            const kuca = document.createElement('div');
            kuca.classList.add('kuca');
            kontejner.appendChild(kuca);

            const kucaNaziv = document.createElement('div');
            kucaNaziv.classList.add('kuca-naziv');
            kuca.appendChild(kucaNaziv);
            kucaNaziv.innerHTML = element.naziv;

            this.nizRedova.forEach(x => {
                const red = document.createElement('div');
                red.classList.add('red');
                kuca.appendChild(red);


                const labela = document.createElement('label');
                labela.innerHTML = x.labela;
                red.appendChild(labela);

                if(x.labela == "Ocena:"){
                    const inputt = document.createElement('input');
                    inputt.classList.add(`${x.klasa}-${element.id}`);
                    red.appendChild(inputt);
                }
                else{
                    const selekt = document.createElement('select');
                    selekt.classList.add(`${x.klasa}-${element.id}`);
                    red.appendChild(selekt);
                }
            });


            const snimiOcenu = document.createElement('button');
            kuca.appendChild(snimiOcenu);
            snimiOcenu.innerHTML = "Snimi Ocenu";
            snimiOcenu.addEventListener('click', (event) => this.snimiOcenu(event, element.id));


            const stubici = document.createElement('div');
            stubici.classList.add(`stubici-${element.id}`);
            stubici.classList.add('stubici');
            kuca.appendChild(stubici);


            // OVDE KAKO DA UBACIM KATEGORIJU IZ SELECT
            

            //UBACUJEM U SELEKT KATEGORIJE:
            const selekt = document.querySelector(`.kategorija-select-${element.id}`);
            

            selekt.addEventListener('change', (event) => this.KategorijaSelekt(event, element.id))
            this.vratiKategorijeZaKucu(element.id).then(kategorije => {
                //elekt.replaceChildren();
                kategorije.forEach(x => {
                    const opt = document.createElement('option');
                    opt.innerHTML = x.naziv;
                    opt.value = x.id;
                    selekt.appendChild(opt);
                });
                const filmoviSelekt = document.querySelector(`.film-select-${element.id}`);
                //treba vrednost iz selekt-a
                this.drawFilmovi(filmoviSelekt,selekt.value);
                this.drawStubici(stubici, selekt.value);
            });

            




        });
        



    }



    async KategorijaSelekt(event, idKuce){       
        const selektFilmovi = document.querySelector(`.film-select-${idKuce}`);

        await this.drawFilmovi(selektFilmovi, event.target.value);


    }

    async drawFilmovi(selekt, kategorijaId){
        const response = await fetch(`https://localhost:7080/Ispit/VratiFilmoveZaKategoriju/${kategorijaId}`);
        const body = await response.json();

        selekt.replaceChildren();

        console.log(body);

        body.forEach(x=>{
            const opt = document.createElement('option');
            opt.value = x.id;
            opt.innerHTML = x.naziv;
            selekt.appendChild(opt);
        });


    }



    async snimiOcenu(event, idKuce)
    {
        const selectKategorija = document.querySelector(`.kategorija-select-${idKuce}`);
        const selectFilm = document.querySelector(`.film-select-${idKuce}`);
        const ocenaInput = document.querySelector(`.ocena-input-${idKuce}`);

        

        console.log(selectFilm.value);
        const response = await fetch(`https://localhost:7080/Ispit/OceniFilm/${selectFilm.value}/${ocenaInput.value}`, {
             method: 'PUT',
         });

        const stubici = document.querySelector(`.stubici-${idKuce}`);
        this.drawStubici(stubici, selectKategorija.value);

    }

    drawStubici(container, kategorijaId){
                container.replaceChildren();
                this.vratiOcenjeneFilmoveZaKategoriju(kategorijaId).then(
                ocenjeni => {
                    ocenjeni.forEach(x => {
                        const stubic = document.createElement('div');
                        stubic.classList.add('stubic');
                        container.appendChild(stubic);

                        const stubicNaziv = document.createElement('div');
                        stubicNaziv.classList.add('stubic-naziv');
                        stubic.appendChild(stubicNaziv);
                        stubic.innerHTML = x.naziv;

                        const stubicNacrtan = document.createElement('div');
                        stubicNacrtan.classList.add('stubic-nacrtan');
                        stubic.appendChild(stubicNacrtan);
                        stubicNacrtan.style = `height : ${x.ocena * 10}px`

                        const stubicOcena = document.createElement('div');
                        stubicOcena.classList.add('stubic-ocena');
                        stubic.appendChild(stubicOcena);
                        stubicOcena.innerHTML = x.ocena;

                    });
                })

    }
}