import { getLat, getLng } from "../../modules/GeoTools.js"; 

//console.log(getLat("ST2522"));
//console.log(getLng("ST2522"));


var url = document.getElementById("getBirdData").value;


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
            console.log(data.birdRecords[0].longitude);
        })
});



