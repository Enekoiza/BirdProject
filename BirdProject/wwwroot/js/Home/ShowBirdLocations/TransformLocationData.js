import { getLat, getLng } from "../../modules/GeoTools.js"; 

//console.log(getLat("ST2522"));
//console.log(getLng("ST2522"));


var url = document.getElementById("getBirdData").value;

console.log(url);

async function postData(url = '', data = {}) {
    const response = await fetch(url, {
        method: 'post',
        headers: {
            'Content-Type': 'application/json',
            'data': JSON.stringify(data)
        }
    });
    return response.json();
}


var fetchButton = document.getElementById("fecthButton");
var fetchInput = document.getElementById("fetchBirdInput");

fetchButton.addEventListener("click", function () {
    postData(url, { ringCode: fetchInput.value })
        .then(data => {

            

            for (let i = 0; i < data.birdData.length; i++) {
                if (data.birdData[i].gridRef != null) {
                    data.birdData[i].latitude = getLat(data.birdData[i].gridRef);
                    data.birdData[i].longitude = getLng(data.birdData[i].gridRef);
                }


            } 
            console.log(data.birdData);
            console.log(data);

            var url1 = "/results/generateSearchResult";


            var asd = url1 + "?test=" + JSON.stringify(data);

            console.log(JSON.stringify(data));

            console.log(asd);

            window.location = asd;

        })
});
