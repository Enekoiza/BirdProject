import { getLat, getLng } from "../../modules/GeoTools.js";




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


var fetchInput = document.getElementById("metalRing");

console.log(fetchInput.value);

window.addEventListener("DOMContentLoaded", function () {
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
