import { expect } from 'chai';
import { Pojazd } from '../src/hoisting_prototype.js';

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

    it('should not have own fields', function(){
        expect(pojazd).to.not.haveOwnProperty('speed');
        expect(pojazd).to.not.haveOwnProperty('id');
        expect(pojazd).to.not.haveOwnProperty('top_speed');
    });

    it('should not own methods', function(){
        expect(pojazd).to.not.haveOwnProperty('getSpeed')
        expect(pojazd).to.not.haveOwnProperty('setSpeed')
        expect(pojazd).to.not.haveOwnProperty('status')
        expect(pojazd).to.have.property('getSpeed')
        expect(pojazd).to.have.property('setSpeed')
        expect(pojazd).to.have.property('status')
    })

    it('should not own constructor', function(){
        expect(pojazd).to.not.haveOwnProperty('constructor');
        expect(pojazd).to.have.property('constructor');
    })
    
    it('should not have prototype', function(){
        expect(pojazd).to.not.have.property('prototype');
        expect(pojazd).to.not.have.property('_prototype');
    })

    it('should have __proto__', function(){
        expect(pojazd).to.have.property('__proto__');
    })

    it('should react to prototype change', function(){
        pojazd.constructor.prototype.testFunction = function(){return 'test'};
        expect(pojazd.testFunction()).equals('test')
    })

});

describe("Pojazd Constructor", function(){
    it("should own prototype", function(){
        expect(Pojazd).to.haveOwnProperty('prototype')
    })
});