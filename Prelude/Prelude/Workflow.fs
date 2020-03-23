namespace Rogz.Prelude


module Workflow =

    type FnBuilder() =
        member _.Return(x) : ('x -> 'a) = fun _ -> x
        member _.ReturnFrom(m) : ('a -> 'b) = m
        member _.ReturnFrom(m) : System.Func<'a, 'b> = m
        member inline _.Bind(m, f: (^b -> ^a -> ^c)) = fun a -> f (m a) a
        member inline _.Bind(m, f: System.Func< ^b, ^a, ^c>) = fun a -> f.Invoke(m a, a)
        member _.Zero() : ('x -> unit) = fun _ -> ()


    let fn = FnBuilder()


    type OptionBuilder() =
        member _.Return(x) : 'a voption = ValueSome x
        member _.ReturnFrom(m) : 'a option  = m
        member _.ReturnFrom(m) : 'a voption = m
        member inline _.Bind(m: ^a option, f: (^a -> ^b option)) : ^b option =
            match m with None -> None | Some a -> f a
        member inline _.Bind(m: ^a voption, f: (^a -> ^b option)) : ^b option =
            match m with ValueNone -> None | ValueSome a -> f a
        member inline _.Bind(m: ^a option, f: (^a -> ^b voption)) : ^b voption =
            match m with None -> ValueNone | Some a -> f a
        member inline _.Bind(m: ^a voption, f: (^a -> ^b voption)) : ^b voption =
            match m with ValueNone -> ValueNone | ValueSome a -> f a
        member _.Zero() = ValueSome ()
        member _.Using(d: 'd, f) : 'a voption when 'd :> System.IDisposable = using d f
        member _.Using(d: 'd, f) : 'a option  when 'd :> System.IDisposable = using d f


    let option = OptionBuilder()


    type ResultBuilder() =
        member _.Return(x) : Result<'a, 'b> = Ok x
        member _.ReturnFrom(m) : Result<'a, 'b> = m
        member inline _.Bind(m, f: (^a -> Result< ^b, ^e>)) =
            match m with Error e -> Error e | Ok a -> f a
        member _.Zero() : Result<unit, 'a> = Ok ()
        member _.Using(d: 'd, f) : Result<'a, 'e>  when 'd :> System.IDisposable = using d f


    let result = ResultBuilder()


    type ChoiceBuilder() =
        member _.Return(x) : Choice<'a, 'b> = Choice1Of2 x
        member _.ReturnFrom(m) : Choice<'a, 'b> = m
        member inline _.Bind(m, f: (^a -> Choice< ^b, ^e>)) =
            match m with Choice2Of2 e -> Choice2Of2 e | Choice1Of2 a -> f a
        member _.Zero() : Choice<unit, 'a> = Choice1Of2 ()
        member _.Using(d: 'd, f) : Choice<'a, 'b> when 'd :> System.IDisposable = using d f


    let choice = ChoiceBuilder()