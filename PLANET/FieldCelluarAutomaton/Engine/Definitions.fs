namespace ComplexMathLibrary

open System.Numerics

module FCA =
    [<AbstractClass>]
    type Grid =
        val array : Complex[,]
        new (w:int, h:int) = { array = Array2D.create w h Complex.Zero }
        new (arr:Complex[,]) = { array = arr }
        
        abstract member Array: Complex[,]
        abstract member Item : int*int -> Complex
    let ComplexTrue = Complex(1,1)
    let ComplexFalse = Complex.Zero
    type RuleParams = (Grid * Complex)
    type RuleType = (Grid * Complex) -> Complex
    let (Ruleset:Map<string, RuleType>) = Map []
    let _not (a:Complex) = if a = ComplexFalse then ComplexTrue else ComplexFalse
    let _and (a:Complex,b:Complex) = if a <> ComplexFalse && b <> ComplexFalse then ComplexTrue else ComplexFalse
    let _or (a:Complex,b:Complex) = if a <> ComplexFalse || b <> ComplexFalse then ComplexTrue else ComplexFalse
    let _xor (a:Complex,b:Complex) = if a <> b then ComplexTrue else ComplexFalse
    
    let neighborhoodOffsets = [
        Complex(-1, 0)  // Up
        Complex(1, 0)   // Down
        Complex(0, -1)  // Left
        Complex(0, 1)   // Right
        Complex(-1, -1) // Up-Left
        Complex(-1, 1)  // Up-Right
        Complex(1, -1)  // Down-Left
        Complex(1, 1)   // Down-Right
    ]
    let n (rule:RuleType) (field:Grid, position:Complex) =
        let neighborhood = neighborhoodOffsets |> List.map (fun offset -> rule (field,position+offset))
        neighborhood |> List.reduce (+)

    let c (rule:RuleType, offset:Complex) (field:Grid, position:Complex) =
        rule (field, position+offset)
    
    let p (offset:Complex) (field:Grid, position:Complex) =
        let pos = position + offset
        field[int pos.Real, int pos.Imaginary]

