var counter = document.getElementById("counter").value;


var customIcon1 = L.icon({
    iconUrl: "https://raw.githubusercontent.com/pointhi/leaflet-color-markers/master/img/marker-icon-red.png",
    iconAnchor: [19, 46],//changed marker icon position
    popupAnchor: [0, -36]
});

//Create a map and set the view to London

var map = L.map('map');


//Create the map layer that will show the map

L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {}).addTo(map);


var longitude, latitude;

var boundsArray = new Array();

var latLngArray = new Array()



for (let x = 0; x < counter; x++) {

    longitude = document.getElementById("longitude" + x.toString()).value;

    latitude = document.getElementById("latitude" + x.toString()).value;

    date = document.getElementById("date" + x.toString()).value;

    var latlng = L.latLng(latitude, longitude);

    latLngArray = [latitude, longitude];

    boundsArray.push(latLngArray);


    if (x == (counter - 1)) {
        L.marker(latlng, { icon: customIcon1 }).addTo(map).bindPopup("First Capture");

        break;
    }
    else {
        L.marker(latlng).addTo(map).bindPopup("Retrapped");
    }

}

var bounds = new L.LatLngBounds(boundsArray);

map.fitBounds(bounds);

map.setZoom(map.getBoundsZoom(bounds) - 1);



//Before unload ask the user if they want to leave the page
window.onbeforeunload = function () {
    return "Do you want to see another ring data or stay in the page?";
}