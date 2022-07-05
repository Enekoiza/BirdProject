// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var customIcon = L.icon({
    iconUrl: "https://unpkg.com/leaflet@1.8.0/dist/images/marker-icon.png",
    iconAnchor: [19, 46],//changed marker icon position
    popupAnchor: [0, -36]
});

//Create a map and set the view to London

var map = L.map('map').setView([51.505, -0.09], 13);


//Create the map layer that will show the map

L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {}).addTo(map);

longitudeObj = document.getElementById("longitude");
latitudeObj = document.getElementById("latitude");


var marker;
var longitude;
var latitude;


//On click remove the existing marker and add one in the clicking position
map.on('click', function (e) {

    if (marker) {
        map.removeLayer(marker);
    }


    marker = new L.marker(e.latlng).addTo(map);
    console.log(marker);
    latitudeObj.value = e.latlng.lat;
    longitudeObj.value = e.latlng.lng;
});

