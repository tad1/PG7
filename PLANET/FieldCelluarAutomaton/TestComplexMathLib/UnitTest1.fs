module TestComplexMathLib

open System.Numerics
open ComplexMathLibrary
open NUnit.Framework
open FParsec
open ComplexMathLibrary.Parser

[<SetUp>]
let Setup () =
    ()

[<Test>]
let Test1 () =
    let field = Array2D.create 5 5 Complex.Zero
    let result = Public.add_rule "R5: p(-1-1i)+p(1+1i)"
    match result with
    | Result.Error errorValue ->
        Assert.Fail(errorValue)
    | Result.Ok() ->
        let rule = Public.ruleset.Item("R5")
        Public.apply rule
    Assert.Pass()

[<TestCase("2", 2.0, 0.0)>]
[<TestCase("3 + 4", 3.0, 4.0)>]
[<TestCase("-5 - 6i", -5.0, -6.0)>]
[<TestCase("-5-6i", -5.0, -6.0)>]
[<TestCase("-5+6i", -5.0, 6.0)>]
[<TestCase("-5+-6i", -5.0, -6.0)>]
[<TestCase("7i", 0.0, 7.0)>]
[<TestCase("-8i", 0.0, -8.0)>]
let TestComplex (input: string, expectedReal: float, expectedImaginary: float) =
    let expected:Expr = ComplexLiteral (Complex(expectedReal, expectedImaginary))
    
    // Parse the input expression and compare it to the expected Complex result
    let result = parseExpr input

    match result with
    | Result.Ok(c) ->
        printfn "%A" result
        Assert.AreEqual(expected, c, "Parsed complex number does not match expected value.")
    | _ ->
        Assert.Fail($"Parsing failed for input '{input}' when it should have succeeded.")