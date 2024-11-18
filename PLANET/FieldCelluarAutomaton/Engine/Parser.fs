// based on https://www.youtube.com/watch?v=34C_7halqGw
module ComplexMathLibrary.Parser

    open System
    open System.Numerics
    open FParsec

    type BinaryExprKind =
        | Add
        | Subtract
        | Multiply
        | Divide
        | And
        | Or
        | Xor
        | Equals
        | NotEquals
        | GreaterThan
        | GreaterThanOrEquals
        | LesserThan
        | LesserThanOrEquals
    
    type RuleIdent = string
    let rng = System.Random()

    
    type Expr =
        // | Self of Complex
        | True of unit
        | False of unit
        | ComplexLiteral of Complex
        | Neighbour of RuleIdent
        | If of (Expr * Expr * Expr)
        | Get of Expr
        | Magnitude of Expr
        | Unit of Expr
        | Check of (RuleIdent * Expr)
        | Binary of (Expr * Expr * BinaryExprKind)
        | Random of unit
        | CurrentPosition of unit

    let ruleIdent : Parser<RuleIdent, unit> = skipChar 'R' >>. many1Chars (letter <|> digit <|> pchar '_') |>> (fun s -> $"R{s}")
    let sign = (charReturn '+' 1.0 <|> charReturn '-' -1.0)
    
    let real = pfloat |>> fun v -> Complex(v,0)
    let imaginary =
        (opt sign) .>>.? (pfloat .>>? skipChar 'i') |>> (fun (s,i) -> Complex(0, (defaultArg s 1.0) * i))

    let complexIm =
        pfloat .>>.? sign .>>.? (pfloat .>>? skipChar 'i') |>> (fun ((r,s),i) -> Complex(r,s*i))
    let complex: Parser<Complex, unit> = (complexIm <|> imaginary <|> real) .>> spaces
    

    let comma = skipChar ',' .>> spaces
    let prandom = skipChar 'r' .>> spaces |>> Random
    let pcurrentPos = skipChar 'x' .>> spaces |>> CurrentPosition
    
    let parenthesesExpr p =
        between (pstring "(") (pstring ")") p
    
    let complexIteral =
        complex |>> ComplexLiteral
    
    (*
    let ws1 = skipMany1 (skipChar ' ')
    let leftBracket : Parser<_,unit> = skipChar '('
    let rightBracket : Parser<_,unit> = skipChar ')'*)
    
    // let intOrFloatLiteral =
    //     numberLiteral (NumberLiteralOptions.DefaultFloat ||| NumberLiteralOptions.DefaultInteger) "number"
    //     |>> fun n ->
    //         if n.IsInteger then Expr.IntLiteral (int n.String)
    //         else Expr.FloatLiteral(float n.String)
    
    // let imaginaryLiteral = skipChar 'i' >>. intOrFloatLiteral
    // let complexLiteral = intOrFloatLiteral .>> skipChar '+' .>> ImaginaryLiteral
    // let ruleIdentifier = skipChar 'R' .>>. many1Chars (letter <|> digit) |>> Expr.Identifier
    //
    // let func = skileftBracket >>. manyCharsTill anyChar rightBracket |>> Expr.Identifier
    //
    let opp = OperatorPrecedenceParser<Expr, _, _>()
    let expr = opp.ExpressionParser
    
    
    let pfunc =
        between (pstring "p(") (pstring ")") expr .>> spaces |>> Get
        
    let ifFunc =
        between (pstring "?(") (pstring ")") (sepBy1 expr comma) |>> function
        | [a;b;c] -> If (a,b,c)
        | _ -> failwith "Expected exactly three elements"
    
    let nfunc =
        between (pstring "n(") (pstring ")") ruleIdent .>> spaces |>> Neighbour
        
    let cfunc =
        between (pstring "c(") (pstring ")") ( ruleIdent .>> comma .>>. expr) .>> spaces |>> Check
    
    let mfunc =
        between (pstring "m(") (pstring ")") expr .>> spaces |>> Magnitude
    
    let ufunc =
        between (pstring "u(") (pstring ")") expr .>> spaces |>> Unit
    
    let trueFunc =
        skipString "true" .>> spaces |>> True        
    
    let falseFunc =
        skipString "false" .>> spaces |>> False    
    
    let term = parenthesesExpr expr .>> spaces

    opp.TermParser <- choice [
        term
        complexIteral
        prandom
        pcurrentPos
        pfunc
        nfunc
        cfunc
        falseFunc
        trueFunc
        ifFunc
        mfunc
        ufunc
    ]
    
    opp.AddOperator <| InfixOperator("*", spaces, 1, Associativity.Left, fun x y -> Expr.Binary (x, y, BinaryExprKind.Multiply))
    opp.AddOperator <| InfixOperator("/", spaces, 2, Associativity.Left, fun x y -> Expr.Binary (x, y, BinaryExprKind.Divide))
    opp.AddOperator <| InfixOperator("-", spaces, 3, Associativity.Left, fun x y -> Expr.Binary (x, y, BinaryExprKind.Subtract))
    opp.AddOperator <| InfixOperator("+", spaces, 4, Associativity.Left, fun x y -> Expr.Binary (x, y, BinaryExprKind.Add))
    opp.AddOperator <| InfixOperator("and", spaces, 5, Associativity.Left, fun x y -> Expr.Binary (x, y, BinaryExprKind.And))
    opp.AddOperator <| InfixOperator("or", spaces, 6, Associativity.Left, fun x y -> Expr.Binary (x, y, BinaryExprKind.Or))
    opp.AddOperator <| InfixOperator("xor", spaces, 6, Associativity.Left, fun x y -> Expr.Binary (x, y, BinaryExprKind.Xor))
    opp.AddOperator <| InfixOperator("=", spaces, 7, Associativity.None, fun x y -> Expr.Binary (x, y, BinaryExprKind.Equals))
    opp.AddOperator <| InfixOperator("!=", spaces, 8, Associativity.None, fun x y -> Expr.Binary (x, y, BinaryExprKind.NotEquals))
    opp.AddOperator <| InfixOperator(">", spaces, 9, Associativity.None, fun x y -> Expr.Binary (x, y, BinaryExprKind.GreaterThan))
    opp.AddOperator <| InfixOperator(">=", spaces, 10, Associativity.None, fun x y -> Expr.Binary (x, y, BinaryExprKind.GreaterThanOrEquals))
    opp.AddOperator <| InfixOperator("<", spaces, 11, Associativity.None, fun x y -> Expr.Binary (x, y, BinaryExprKind.LesserThan))
    opp.AddOperator <| InfixOperator("<=", spaces, 12, Associativity.None, fun x y -> Expr.Binary (x, y, BinaryExprKind.LesserThanOrEquals))

    //
    let ruleFull = ruleIdent .>> skipChar ':' .>> spaces .>>. expr .>> spaces .>> eof
    
    let parseExpr input =
        match run expr input with
        | Success(res, _, _) -> Result.Ok res
        | Failure(err, _, _) -> Result.Error err
    let parse input =
        match run ruleFull input with
        | Success(res, _, _) -> Result.Ok res
        | Failure(err, _, _) -> Result.Error err
    
    let execute (ruleset:Map<string,FCA.RuleType>) tokens =
        let rec evaluate expr:(FCA.RuleType)=
            match expr with
            | ComplexLiteral i ->
                fun _ -> i
            | Random _ ->
                fun _ ->
                    let theta = rng.NextDouble() * 2.0 * Math.PI
                    Complex(Math.Cos(theta), Math.Sin(theta))
            | CurrentPosition _ ->
                fun (_,pos) -> pos
            | Neighbour s ->
                match ruleset.TryFind s with
                | None -> failwith "invalid rule"
                | Some value -> FCA.n value
            | Get exp ->
                let evaluated = (evaluate exp)
                fun v -> FCA.p (evaluated v) v 
            | Check (rule, exp) ->
                match ruleset.TryFind rule with
                | None -> failwith "invalid rule"
                | Some rule ->
                    let evaluated = (evaluate exp)
                    fun v -> FCA.c (rule,(evaluated v)) v
            | Binary (left, right, kind) ->
                let leftEvaluated = (evaluate left)
                let rightEvaluated = (evaluate right)
                
                match kind with
                | Add -> (fun v -> (leftEvaluated v) + (rightEvaluated v))
                | Subtract -> (fun v -> (leftEvaluated v) - (rightEvaluated v))
                | Multiply -> (fun v -> (leftEvaluated v) * (rightEvaluated v))
                | Divide -> (fun v -> (leftEvaluated v) / (rightEvaluated v))
                | And -> (fun v -> FCA._and(leftEvaluated v, rightEvaluated v))
                | Or -> (fun v -> FCA._or(leftEvaluated v, rightEvaluated v))
                | Xor -> (fun v -> FCA._xor(leftEvaluated v, rightEvaluated v))
                | Equals -> (fun v -> if leftEvaluated v = rightEvaluated v then FCA.ComplexTrue else FCA.ComplexFalse)
                | NotEquals -> (fun v -> if not(leftEvaluated v = rightEvaluated v) then FCA.ComplexTrue else FCA.ComplexFalse)
                | GreaterThan -> (fun v -> if (leftEvaluated v).Real > (rightEvaluated v).Real then FCA.ComplexTrue else FCA.ComplexFalse)
                | GreaterThanOrEquals -> (fun v -> if (leftEvaluated v).Real >= (rightEvaluated v).Real then FCA.ComplexTrue else FCA.ComplexFalse)
                | LesserThan -> (fun v -> if (leftEvaluated v).Real < (rightEvaluated v).Real then FCA.ComplexTrue else FCA.ComplexFalse)
                | LesserThanOrEquals -> (fun v -> if (leftEvaluated v).Real <= (rightEvaluated v).Real then FCA.ComplexTrue else FCA.ComplexFalse)
            | True unit ->
                fun _ -> FCA.ComplexTrue
            | False unit ->
                fun _ -> FCA.ComplexFalse
            | If (condition, trueExpr, falseExpr) ->
                let conditionEvaluated = (evaluate condition)
                let trueExprEvaluated = (evaluate trueExpr)
                let falseExprEvaluated = (evaluate falseExpr)
                fun v ->
                    if conditionEvaluated v = FCA.ComplexFalse then
                        falseExprEvaluated v
                        else trueExprEvaluated v
            | Magnitude expr ->
                let evaluated = evaluate expr
                fun v ->
                    Complex((evaluated v).Magnitude, 0)
            | Unit expr ->
                let evaluated = evaluate expr
                fun v ->
                    let res = evaluated v
                    if(res = Complex.Zero) then Complex.Zero
                    else res / res.Magnitude
                    
        
        evaluate tokens
    //     
    // let execute query =
    //     let evaluate expr element =
    //         match expr with
    //         | IntLiteral i -> i
    
    //