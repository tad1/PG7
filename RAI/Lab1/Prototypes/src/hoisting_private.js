"use strict";

function Pojazd(id, top_speed, speed){

    this.stop = function() {speed=0}
    this.getSpeed = function() {return speed}
    this.setSpeed = function(_speed){
        speed = parseFloat(_speed);
        if(speed > top_speed) speed = top_speed; 
    }
    this.status = function(){
        if(speed < -50) return "przestań to jest niebezpieczne"
        if(speed < 0) return "jazda do tyłu"
        if(speed === 0) return "[...]"
        if(speed === Infinity) return "please stop; you are breaking universe"
        if(speed > 299792458) return "stop! łamiesz prawo fizyki"
        if(speed > 140) return "stop! łamiesz prawo";
        if(speed > 0) return "brum brum";
    }
}

export {
    Pojazd
}