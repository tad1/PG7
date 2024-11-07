open FParsec

type Token = Hello | World

let a = pstring "hello" >>% Token.Hello
let b = pstring "world" >>% Token.World

let p = a <|> b
let result = run p "world"

printfn "%O" result

