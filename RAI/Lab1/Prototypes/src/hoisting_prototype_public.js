"use strict";

function Pojazd(id, top_speed, speed){
    this.id = id;
    this.top_speed = top_speed;
    this.speed = speed;
}

Pojazd.prototype.stop = function() {this.speed=0}
Pojazd.prototype.getSpeed = function() {return this.speed}
Pojazd.prototype.setSpeed = function(_speed){
    this.speed = parseFloat(_speed);
    if(this.speed > this.top_speed) this.speed = this.top_speed; 
}
Pojazd.prototype.status = function(){
    if(this.speed < -50) return "przestań to jest niebezpieczne"
    if(this.speed < 0) return "jazda do tyłu"
    if(this.speed === 0) return "[...]"
    if(this.speed === Infinity) return "please stop; you are breaking universe"
    if(this.speed > 299792458) return "stop! łamiesz prawo fizyki"
    if(this.speed > 140) return "stop! łamiesz prawo";
    if(this.speed > 0) return "brum brum";
}

export {
    Pojazd
}