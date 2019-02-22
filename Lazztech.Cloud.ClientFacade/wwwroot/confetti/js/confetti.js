﻿
async function confetti() {
    for (let i = 0; i < 125; i++) {
        create(i);
    }

    setTimeout(stop, 4300);
}

function stop() {
    for (let i = 0; i < 125; i++) {
        $('#cnft' + i).remove();
    }
}

function sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
}

function create(i) {
    var width = Math.random() * 12;
    var height = width * 0.4;
    var colourIdx = Math.ceil(Math.random() * 3);
    var colour = "red";
    switch (colourIdx) {
        case 1:
            colour = "yellow";
            break;
        case 2:
            colour = "blue";
            break;
        default:
            colour = "red";
    }
    $('<div id="cnft' + i + '"' + 'class="confetti-' + i + ' ' + colour + '"></div>').css({
        "width": width + "px",
        "height": height + "px",
        "top": -Math.random() * 20 + "%",
        "left": Math.random() * 100 + "%",
        "opacity": Math.random() + 0.5,
        "transform": "rotate(" + Math.random() * 360 + "deg)"
    }).appendTo('.wrapper');

    drop(i);
}

function drop(x) {
    $('.confetti-' + x).animate({
        top: "100%",
        left: "+=" + Math.random() * 15 + "%"
    }, Math.random() * 3000 + 3000, function () {
        reset(x);
    });
}

function reset(x) {
    $('.confetti-' + x).animate({
        "top": -Math.random() * 20 + "%",
        "left": "-=" + Math.random() * 15 + "%"
    }, 0, function () {
        drop(x);
    });
}