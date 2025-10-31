import { Sala } from "./Sala.js";

export class Application{
    constructor(sale){
        this.saleNiz = sale.map(x => new Sala(x.id, x.imeFilma, x.vremeReprodukcije,x.brojSale,x.sifra,x.brojRedova,x.bazicnaCena, x.sedista, x.kapacitetSedista, x.trenutniBrojSedista))
        console.log(this.saleNiz[4].sedista);

        this.SelektovanoSedisteId = 1;

        this.levaStranaNiz = [
            {labela: "Red", klasa: "red"},
            {labela: "Broj sedista:", klasa: "broj-sedista"},
            {labela: "Cena Karte:", klasa: "cena-karte"},
            {labela: "Sifra:", klasa: "sifra"},
        ]
    }

    draw(container)
    {
        const kontejner = document.createElement('div');
        kontejner.classList.add('container');
        container.appendChild(kontejner);


        this.saleNiz.forEach(x =>
        {
            if(x.id == 5)
            {
                //console.log(x.sifra);

                const salaa = document.createElement('div');
                salaa.classList.add("sala");
                kontejner.appendChild(salaa);
                //naziv
                const nazivSale = document.createElement('div');
                nazivSale.classList.add('naziv-sale');
                console.log(x.imeFilma);

                const vremeDatum = x.vremeReprodukcije.split('T')[0];
                const vremeSati = x.vremeReprodukcije.split('T')[1].split(':')[0] + ':' +x.vremeReprodukcije.split('T')[1].split(':')[1];

                nazivSale.innerHTML = x.imeFilma + ': ' + vremeDatum + ' ' + vremeSati + ' - Sala ' + String(x.id); 

                salaa.appendChild(nazivSale);
                //salaizgled
                const salaIzgled = document.createElement('div');
                salaIzgled.classList.add('sala-izgled');
                salaa.appendChild(salaIzgled);

                const levaStrana = document.createElement('div');
                levaStrana.classList.add('leva-strana');
                salaIzgled.appendChild(levaStrana);

                const kupiKartu = document.createElement('div');
                kupiKartu.classList.add('kupi-kartu');
                levaStrana.appendChild(kupiKartu);
                


                this.levaStranaNiz.forEach(element =>{
                    const red = document.createElement('div');
                    red.classList.add('red');
                    levaStrana.appendChild(red);

                    const labela = document.createElement('label');
                    labela.innerHTML = element.labela;
                    red.appendChild(labela);

                    const inputt = document.createElement('input');
                    inputt.type = 'text';
                    inputt.classList.add(element.klasa + '-input');
                    red.appendChild(inputt)
                    inputt.innerHTML = '1';


                });

                const kupi = document.createElement('div');
                kupi.classList.add('kupi');
                levaStrana.appendChild(kupi);

                const kupiKartuButton = document.createElement('button');
                kupiKartuButton.classList.add('kupi-kartu-button')
                kupiKartuButton.innerHTML = " Kupi kartu";
                kupiKartuButton.addEventListener('click', (event) => this.kupiSelektovanuKartu(event, x))

                kupi.appendChild(kupiKartuButton);

               

                const desnaStrana = document.createElement('div');
                desnaStrana.classList.add('desna-strana');
                salaIzgled.appendChild(desnaStrana);


                this.iscrtajSedista(desnaStrana, x);
            }
        }
        );
    }


    iscrtajSedista(desnaStrana, x){
        //Sredi treba foreach red pa u svaki red div, ne valja u pomoc.html
                console.log("max sedista" + x.kapacitetSedista)// 12
                console.log("trenutno sedista" + x.trenutniBrojSedista) // 8
                console.log("ukupno redova " + x.brojRedova) // 4: 0, 1, 2, 3
                console.log("Broj reda" + x.sedista[4].brojReda);

                console.log(x.kapacitetSedista / x.brojRedova); // 3


                for(let i = 0; i < x.brojRedova; i++)
                {
                    const redSedista = document.createElement('div');
                    redSedista.classList.add('red-sedista');
                    desnaStrana.appendChild(redSedista);
                    //console.log(i);
                    for(let j = 0; j < x.kapacitetSedista / x.brojRedova; j++)
                    {
                        //console.log(i * (x.brojRedova - 1) + j );
                        if (i * (x.brojRedova - 1) + j < x.trenutniBrojSedista)
                        {
                            const sediste = document.createElement('button');
                            sediste.classList.add('sediste')
                            if(x.sedista[i * (x.brojRedova - 1) + j ].zauzeto)
                            {
                                sediste.classList.add('sediste-crveno')
                            }
                            else{
                                sediste.classList.add('sediste-zeleno')
                            }
                            
                            //sediste.innerHTML = i * x.brojRedova + j;
                            sediste.innerHTML = "Red " + String(i + 1) + ' Sediste: ' + (String(x.sedista[i * (x.brojRedova - 1) + j ].brojSedista + 1));
                            redSedista.appendChild(sediste);
                            //console.log(x.sifra);
                            sediste.addEventListener('click', (event) => this.prikaziSedisteInfo(event, x.sedista[i * (x.brojRedova - 1) + j], i +1,x.sedista[i * (x.brojRedova - 1) + j ].brojSedista + 1 , x.sifra))

                            //posle ce mi treba  x.sedista[i * (x.brojRedova - 1) + j ].id za postavljanje na levu stranu i da kupim kartutu
                        }
                        
                    }

                }
    }


    async kupiSelektovanuKartu(event,x){
        //console.log(event);
        console.log(this.SelektovanoSedisteId)

        const resp = await fetch(`https://localhost:7080/Ispit/KupiKartu/${this.SelektovanoSedisteId}`,{
            method: "PUT",
        });

        //const body = await resp.json();

        const sedista = await fetch(`https://localhost:7080/Ispit/VratiSedistaZaSalu/${x.id}`);
        const sedistaBody = await sedista.json();


        console.log(sedistaBody);
        x.sedista = sedistaBody;

        

        const desnaStrana = document.querySelector('.desna-strana');
        desnaStrana.replaceChildren();
        this.iscrtajSedista(desnaStrana, x);



    }
    //prosledi red broj sedista, cenu izvuci i sifru izvuci od sale
    async prikaziSedisteInfo(event, sediste, brojReda, brojSedista, sifraSale){
        //console.log(sediste.zauzeto);
        if(!sediste.zauzeto)
        {
            this.SelektovanoSedisteId = sediste.id;
        
            // red-input broj-sedista-input cena-karte-input sifra-input
            const redInput = document.querySelector('.red-input');
            redInput.value = brojReda;

            const brojSedistaa = document.querySelector('.broj-sedista-input');
            brojSedistaa.value = brojSedista;

            const cenaKarte = document.querySelector('.cena-karte-input');
            cenaKarte.value = sediste.cena;

            const sifraa = document.querySelector('.sifra-input');
            sifraa.value = sifraSale;
        }


    }
}