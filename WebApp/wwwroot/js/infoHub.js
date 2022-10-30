var activeRoomId = '';

var infoConnection = new signalR.HubConnectionBuilder()
    .withUrl('/infoHub')
    .build();

infoConnection.on('GetInfoCount', loadInfoCount);

infoConnection.onclose(function () {
    handleDisconnected(startInfoConnection);
});


function startInfoConnection() {
    infoConnection
        .start()
        .catch(function (err) {
            console.error(err);
        });
}

function handleDisconnected(retryFunc) {
    console.log('Reconnecting in 5 seconds...');
    setTimeout(retryFunc, 5000);
}

function ready() {
    startInfoConnection();
}

var projectCountEl = document.getElementById('projectCount');
var filesCountEl = document.getElementById('filesCount');
var ocredCountEl = document.getElementById('ocredCount');
var remainCountEl = document.getElementById('remainCount');


function loadInfoCount(projectCount, filesCount, ocredCount, remainCount) {    
   
    projectCountEl.innerHTML = projectCount
    filesCountEl.innerHTML = filesCount
    ocredCountEl.innerHTML = ocredCount
    remainCountEl.innerHTML = remainCount
}



document.addEventListener('DOMContentLoaded', ready);