"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/progressHub").build();

//connection.on("ReceiveMessage", function (user, message) {
//    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
//    var encodedMsg = user + " says " + msg;
//    var li = document.createElement("li");
//    li.textContent = encodedMsg;
//    document.getElementById("messagesList").appendChild(li);
//});

connection.start().catch(function (err) {
    return console.error(err.toString());
});
setTimeout(function () {
    connection.invoke("ThrowException").catch(err => console.error(err.toString()));
}, 1500);



// Reference the auto-generated proxy for the hub.
//var progress = $.connection.progressHub;
//console.log(progress);

// Create a function that the hub can call back to display messages.
connection.client.addProgress = function (message, percentage) {
    //at this point server side had send message and percentage back to the client
    //and then we handle progress bar any way we want it

    //Using a function in Helper.js file we show modal and display text and percentage
    ProgressBarModal("show", message + " " + percentage);

    //We're filling blue progress indicator by setting the width property to the variable
    //that was returned from the server
    $('#ProgressMessage').width(percentage);

    //closing modal when the progress gets to 100%
    if (percentage === "100%") {
        ProgressBarModal();
    }
};

//MENTOR REQUEST UPDATE
connection.client.requestUpdate = function (message) {
    MentorRequestUpdateModal(message);
};

//Before doing anything with our hub we must start it
$.connection.hub.start().done(function () {

    //getting the connection ID in case you want to display progress to the specific user
    //that started the process in the first place.
    var connectionId = $.connection.hub.id;
    console.log(new { Message: "Connection Id:", connectionId });
});