import { expect } from 'chai';
import { Pojazd } from '../src/hoisting_private.js';

describe('canary-tests', function()
{
    it('should always pass the canary test', function()
    {
        expect(true).to.eql(true);
    });
});

describe('Pojazd', function()
{

    let pojazd;
    beforeEach(function(){
        pojazd = new Pojazd(1, 100, 40);
    });

    describe("#stop()", function(){
        it('should set speed to 0', function(){
            pojazd.stop();
            expect(pojazd.getSpeed()).equal(0);
        })
    });
    describe('#setSpeed()', function(){
        it('should cap to top_speed', function(){
            pojazd.setSpeed(150);
            expect(pojazd.getSpeed()).equal(100);
        });
        it('should set speed', function(){
            pojazd.setSpeed(50);
            expect(pojazd.getSpeed()).equal(50)
        });
        it('should use parameters value instead reference', function(){
            let newSpeed = 30;
            pojazd.setSpeed(newSpeed);
            newSpeed = 40;
            expect(pojazd.getSpeed()).equal(30);
        })

        // this feels over-extensive - we are testing edge cases, and JS here instead of logic
        it('should handle correct numerical string input', function(){
            pojazd.setSpeed("50");
            expect(pojazd.getSpeed()).equal(50)
        })
        it('should set text string to NaN', function(){
            pojazd.setSpeed("aaaaarhg!!")
            expect(pojazd.getSpeed()).is.NaN;
        })
    });

    describe('#status()', function(){
        it('should give funny message on various speeds', function(){
            
            pojazd.setSpeed(-100)
            expect(pojazd.status()).equals("przestań to jest niebezpieczne")
            
            pojazd.setSpeed(-10)
            expect(pojazd.status()).equals("jazda do tyłu")

            pojazd.setSpeed(0)
            expect(pojazd.status()).equals("[...]")

            pojazd.setSpeed(10)
            expect(pojazd.status()).equals("brum brum")
            
            pojazd = new Pojazd(1, 150, 150);
            expect(pojazd.status()).equals("stop! łamiesz prawo")
            
            pojazd = new Pojazd(1, 3.0e9, 3.0e9);
            expect(pojazd.status()).equals("stop! łamiesz prawo fizyki")
            
            pojazd = new Pojazd(1, Infinity, Infinity);
            expect(pojazd.status()).equals("please stop; you are breaking universe")
        })
    })
    
    it('should not change speed when setting field', function(){
        pojazd.speed = 0;
        expect(pojazd.getSpeed()).to.eql(40);
    });

    it('should have fields private', function(){
        expect(pojazd.speed).to.be.undefined;
        expect(pojazd.id).to.be.undefined;
        expect(pojazd.top_speed).to.be.undefined;
    });

});