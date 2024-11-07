module ComplexMathLibrary.Public
open System
open System.Collections.Generic
open System.Numerics
open Microsoft.FSharp.Collections
open Parser

let mutable ruleset:Map<string,FCA.RuleType> = Map []

type RuleInfo = {
    RuleId : string
    RuleStr: string
}
let mutable rulesetInfo: Map<string, RuleInfo> = Map []

type RuleHandlerDelegate =
    Action<Map<string,RuleInfo>>
    
let ruleHandlers : ResizeArray<RuleHandlerDelegate> = ResizeArray<RuleHandlerDelegate>()

type CheckedGrid =
    inherit FCA.Grid
    new (w,h) = {inherit FCA.Grid(w,h)}
    new (arr) = {inherit FCA.Grid(arr)}
    override this.Item(x,y) =
        if x>=this.array.GetLowerBound(0)&&y>=this.array.GetLowerBound(1)&&x<=this.array.GetUpperBound(0)&&y<=this.array.GetUpperBound(0) then
            this.array[x,y]
        else
            Complex.Zero

    override this.Array = this.array;

type GridUpdateHandler =
    Action

let gridUpdateHandlers : ResizeArray<GridUpdateHandler> = ResizeArray<GridUpdateHandler>()

let mutable grid:FCA.Grid = CheckedGrid(10,10)
let add_rule (input:string) : Result<_,string> =
    let result = parse input
    
    match result with
    | Error errorValue -> Error(errorValue)
    | Ok (identifier, tokens) ->
        try
            let name:string = identifier
            let rule = Parser.execute ruleset tokens
            ruleset <- ruleset.Add(name,rule)
            rulesetInfo <- rulesetInfo.Add(name, {RuleId = name; RuleStr = input })
            Ok()
        with
        | Failure(msg) -> Error(msg)
        
let create_grid width height =
    grid <- CheckedGrid(width, height)
    
let apply (rule:FCA.RuleType) =
    let arr = grid.array |> Array2D.mapi (fun x y z -> rule (grid, Complex(x,y)))
    grid <- CheckedGrid(arr)
    
