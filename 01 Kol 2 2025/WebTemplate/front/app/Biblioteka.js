
export class Biblioteka {

    constructor(id, naziv) {
        this.id = id;
        this.naziv = naziv;
        this.niz = [
            {labela:"Naslov", polje:"text"},
            {labela:"Autor", polje:"text"},
            {labela:"Godina", polje:"number"},
            {labela:"Izdavac", polje:"text"},
            {labela:"Broj", polje:"text"},
        ]
    }

    draw(container) {
        const naslovDiv = document.createElement("div");
        naslovDiv.innerHTML = this.naziv;
        naslovDiv.classList.add("naslov-div");
        container.appendChild(naslovDiv);

        const formaDiv = document.createElement("div");
        formaDiv.classList.add("forma-div")
        container.appendChild(formaDiv);

        this.drawKnjigaDiv(formaDiv);
        this.drawIzdavanjeVracanjeDiv(formaDiv);
        this.drawNajcitanijaKnjigaDiv(formaDiv);
    }

    drawKnjigaDiv(container) {
        const knjigaDiv = document.createElement("div");
        knjigaDiv.classList.add("knjiga-div");
        container.appendChild(knjigaDiv);
        this.drawKnjigaNaslovDiv(knjigaDiv);
        this.drawKnjigaElementiDiv(knjigaDiv);
    }

    drawIzdavanjeVracanjeDiv(container) {
        const izdavanjeVraacnjeDiv = document.createElement("div");
        izdavanjeVraacnjeDiv.classList.add("izdavanje-vracanje-div");
        container.appendChild(izdavanjeVraacnjeDiv);
        this.drawIzdavanjeVracanjeNaslovDiv(izdavanjeVraacnjeDiv);

        this.drawIzdavanjeVracanjeElementiDiv(izdavanjeVraacnjeDiv);

    }

    drawNajcitanijaKnjigaDiv(container) {
        const najcitanijaKnjigaDiv = document.createElement("div");
        najcitanijaKnjigaDiv.classList.add("najcitanija-knjiga-div");
        container.appendChild(najcitanijaKnjigaDiv);
        this.drawNajcitanijeKnjigaNaslovDiv(najcitanijaKnjigaDiv);

        const najcitanijaKnjigaTextDiv = document.createElement("div");
        najcitanijaKnjigaTextDiv.classList.add("najcitanija-knjiga-text-div");
        najcitanijaKnjigaDiv.appendChild(najcitanijaKnjigaTextDiv);


        let url = `https://localhost:7080/Ispit/NajcitanijaKnjiga/${this.id}`
        var resp = fetch(url).then(p => p.json().then(x => najcitanijaKnjigaTextDiv.innerHTML = x.naziv)); // KAD JE GET SAMO URL, radim then jer ne smem napravim ovo sve async, 
        //                                                       uradim then na fetch jer je async, pa opet then na json jer i on je async
    }

    drawKnjigaNaslovDiv(container){
        const NaslovKnjigeDiv = document.createElement("div");
        NaslovKnjigeDiv.classList.add("naslov-knjige-div");
        container.appendChild(NaslovKnjigeDiv);
        NaslovKnjigeDiv.innerHTML = "Knjiga"; 
    }

    drawKnjigaElementiDiv(container){
        const KnjigaElementiDiv = document.createElement("div");
        KnjigaElementiDiv.classList.add("knjiga-elementi-div");
        container.appendChild(KnjigaElementiDiv);
        this.niz.forEach(
            element => {
                const labelaelementa = document.createElement("label");
                labelaelementa.innerHTML = element.labela;
                KnjigaElementiDiv.appendChild(labelaelementa);
                const polje = document.createElement("input");
                polje.classList.add(`${element.labela}-${this.id}`) // OVAKO DA SVAKI ELEMENT MOZES JEDINSTVENO DA IDENTIFIKUJES KAD IZVLACIS VREDNOSTI ZA DODAVANJE
                polje.type = element.polje;
                KnjigaElementiDiv.appendChild(polje);
            }
        )

        const Dodaj = document.createElement("input");
        Dodaj.type = "submit";
        Dodaj.classList.add("dodaj-button");
        KnjigaElementiDiv.appendChild(Dodaj);
        Dodaj.onclick = () => {this.OnSubmitClick(this.id)}; // i mora da ga zajebes sa lambdu jer ovde bi trebalo bez parametri funkciju ubacis

    }

    // posto je asinhrono moracu da isforsiram id biblioteke da ubacim jer ne moze da mu pristupi jer je asinhrono wtsh
    async OnSubmitClick(id){
        let str = `.Broj-${id}`
        let url = `https://localhost:7080/Ispit/DodajKnjigu/${id}/${ document.querySelector(str).value}`
        const resp = await fetch(url,
            {
                method : "POST",
                body : JSON.stringify(
                    {
                        
                            "naziv": document.querySelector(`.Naslov-${id}`).value,
                            "autor": document.querySelector(`.Autor-${id}`).value,
                            "godinaIzdavanja": document.querySelector(`.Godina-${id}`).value,
                            "nazivIzdavaca": document.querySelector(`.Izdavac-${id}`).value

                    }
                ),
                headers:    {
                                "Content-Type": "application/json",
                            },
            }
        )
        console.log(resp);
        if (resp.ok) {
            this.niz.forEach(
                element => {
                document.querySelector(`.${element.labela}-${this.id}`).value = ""
            }
        )
        alert("Radi");
        } else {
            alert(await resp.text());
        }
        
    }
    


    drawIzdavanjeVracanjeNaslovDiv(container){
        const IzdavanjeVracanjeNaslovDiv = document.createElement("div");
        IzdavanjeVracanjeNaslovDiv.classList.add("izdavanje-vracanje-naslov-div");
        container.appendChild(IzdavanjeVracanjeNaslovDiv);
        IzdavanjeVracanjeNaslovDiv.innerHTML = "Izdavanje/Vracanje"
    }

    drawIzdavanjeVracanjeElementiDiv(container) {
        const izdavanjeVracanjeElementiDiv = document.createElement("div");
        izdavanjeVracanjeElementiDiv.classList.add("izdavanje-vracanje-elementi-div");
        container.appendChild(izdavanjeVracanjeElementiDiv);
        
        const searchInput = document.createElement("input");
        searchInput.type = "text";
        searchInput.classList.add(`search-${this.id}`);
        izdavanjeVracanjeElementiDiv.appendChild(searchInput);

        const pretraziBtn = document.createElement("button");
        pretraziBtn.innerHTML = "Pretrazi";
        pretraziBtn.onclick = () => {this.OnPretraziClick(this.id)}
        izdavanjeVracanjeElementiDiv.appendChild(pretraziBtn)

        const iznajmiVratiDugme = document.createElement("input");
        iznajmiVratiDugme.type = "submit";
        iznajmiVratiDugme.classList.add("iznajmi-vrati-dugme")
        izdavanjeVracanjeElementiDiv.appendChild(iznajmiVratiDugme);
        iznajmiVratiDugme.onclick = () => {this.OnIznajmiVratiClick(this.id)};

        const comboBox = document.createElement("select");
        comboBox.classList.add("combobox", `combobox-${this.id}`);
        izdavanjeVracanjeElementiDiv.appendChild(comboBox);
        comboBox.onchange = function () {
            console.log(this.value.split("/")[1]);
            if (this.value.split("/")[1] == "true") {
                iznajmiVratiDugme.value = "vrati";
            } else {
                iznajmiVratiDugme.value = "iznajmi";
            }
        }
    }

    async OnIznajmiVratiClick(id) {
        const val = document.querySelector(`.combobox-${id}`).value;
        console.log("VREDNOSTII")
        console.log(val);
        const idKnjige = val.split("/")[0];
        
        const url = `https://localhost:7080/Ispit/IzdajVrati/${id}/${idKnjige}`
        const resp = await fetch(url,
            {
                method: "PUT",
                headers:{
                    "Content-Type": "application/json",
                }
            }
        );

        if(resp.ok)
        {
            console.log(document.querySelector(".iznajmi-vrati-dugme").value)
            if(document.querySelector(".iznajmi-vrati-dugme").value == "vrati")
            {
                document.querySelector(".iznajmi-vrati-dugme").value = "iznajmi";
            }else{
                document.querySelector(".iznajmi-vrati-dugme").value = "Vrati"
            }
        }
    }

    async OnPretraziClick(id) {
        const pretraga = document.querySelector(`.search-${id}`).value;
        const resp = await fetch(`https://localhost:7080/Ispit/NadjiKnjigu/${id}?pretraga=${pretraga}`);
        const body = await resp.json();

        document.querySelector(`.combobox-${id}`).replaceChildren();  // OVO DODAJEMO DA BI ISPRAZNILI KOMBOBOX. AKO OVO NEMA SVAKI PUT KAD SE KLIKNE DUGME CE SE DODAJU STVARI U KOMBOBOX I POSTOJACE DUPLIKATI
        body.forEach(x => {
            const option = document.createElement("option");
            option.innerHTML = x.naziv;
            option.value = `${x.id}/${x.izdata}`;
            document.querySelector(`.combobox-${id}`).appendChild(option);
        })
    }

    drawNajcitanijeKnjigaNaslovDiv(container){
        const NajcitanijeKnjigaNaslovDiv = document.createElement("div");
        NajcitanijeKnjigaNaslovDiv.classList.add("najcitanije-knjiga-naslov-div");
        container.appendChild(NajcitanijeKnjigaNaslovDiv);
        NajcitanijeKnjigaNaslovDiv.innerHTML = "Najcitanija knjiga";
    }



}