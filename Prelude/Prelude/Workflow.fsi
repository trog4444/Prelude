namespace Rogz.Prelude


/// <summary>Computation expressions for standard types.</summary>
module Workflow =
   
   /// <summary>Computation expression builder for function types.</summary>
    type FnBuilder =
        new: unit -> FnBuilder
        member Return: 'a -> ('x -> ^a)
        member ReturnFrom: m: ('a -> 'b) -> (^a -> ^b)
        member ReturnFrom: m: System.Func<'a, 'b> -> System.Func< ^a, ^b>
        member inline Bind: m: (^a -> ^b) * f: (^b -> ^a -> ^c) -> (^a -> ^c)
        member inline Bind: m: (^a -> ^b) * f: System.Func< ^b, ^a, ^c> -> (^a -> ^c)
        member Zero: unit -> ('x -> unit)


    /// <summary>Computation expression builder for function types.</summary>
    val fn: FnBuilder


    /// <summary>Computation expression builder for the Option and ValueOption types.</summary>
    type OptionBuilder =
        new: unit -> OptionBuilder
        member Return: x: 'a -> ^a voption
        member ReturnFrom: m: 'a option  -> ^a option
        member ReturnFrom: m: 'a voption -> ^a voption
        member inline Bind: m: ^a option  * f: (^a -> ^b option)  -> ^b option
        member inline Bind: m: ^a voption * f: (^a -> ^b option)  -> ^b option
        member inline Bind: m: ^a option  * f: (^a -> ^b voption) -> ^b voption
        member inline Bind: m: ^a voption * f: (^a -> ^b voption) -> ^b voption
        member Zero: unit -> unit voption
        member Using: d: 'd * f: ('d -> 'a voption) -> 'a voption when 'd :> System.IDisposable
        member Using: d: 'd * f: ('d -> 'a option)  -> 'a option  when 'd :> System.IDisposable


    /// <summary>Computation expression builder for the Option and ValueOption types.</summary>
    val option: OptionBuilder


    /// <summary>Computation expression builder for the Result type.</summary>
    type ResultBuilder =
        new: unit -> ResultBuilder
        member Return: x: 'a -> Result< ^a, 'b>
        member ReturnFrom: m: Result<'a, 'b> -> Result< ^a, ^b>
        member inline Bind: m: Result< ^a, ^e> * f: (^a -> Result< ^b, ^e>) -> Result< ^b, ^e>
        member Zero: unit -> Result<unit, 'a>
        member Using: d: 'd * f: ('d -> Result<'a, 'b>)  -> Result<'a, 'b> when 'd :> System.IDisposable


    /// <summary>Computation expression builder for the Result type.</summary>
    val result: ResultBuilder


    /// <summary>Computation expression builder for the Choice(of2) type.</summary>
    type ChoiceBuilder =
        new: unit -> ChoiceBuilder
        member Return: x: 'a -> Choice<'a, 'b>
        member ReturnFrom: m: Choice<'a, 'b> -> Choice< ^a, ^b>
        member inline Bind: m: Choice< ^a, ^e> * f: (^a -> Choice< ^b, ^e>) -> Choice< ^b, ^e>
        member Zero: unit -> Choice<unit, 'a>
        member Using: d: 'd * f: ('d -> Choice<'a, 'b>)  -> Choice<'a, 'b> when 'd :> System.IDisposable


    /// <summary>Computation expression builder for the Choice(of2) type.</summary>
    val choice: ChoiceBuilder