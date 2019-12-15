namespace Rogz.Prelude


/// <summary>Computation expressions for standard types.</summary>
[<AutoOpen>]
module Workflow =
    
    /// <summary>Computation expression builder for the Option and ValueOption types.</summary>
    type OptionBuilder =
        new: unit -> OptionBuilder
        member ReturnFrom: m: 'a option  -> ^a option
        member ReturnFrom: m: 'a voption -> ^a voption
        member inline Bind: m: ^a option  * f: (^a -> ^b option)  -> ^b option
        member inline Bind: m: ^a voption * f: (^a -> ^b option)  -> ^b option
        member inline Bind: m: ^a option  * f: (^a -> ^b voption) -> ^b voption
        member inline Bind: m: ^a voption * f: (^a -> ^b voption) -> ^b voption


    /// <summary>Computation expression builder for the Option and ValueOption types.</summary>
    val option: OptionBuilder


    /// <summary>Computation expression builder for the Result type.</summary>
    type ResultBuilder =
        new: unit -> ResultBuilder
        member Return: x: 'a -> Result< ^a, 'b>
        member ReturnFrom: m: Result<'a, 'b> -> Result< ^a, ^b>
        member inline Bind: m: Result< ^a, ^e> * f: (^a -> Result< ^b, ^e>) -> Result< ^b, ^e>
        member Zero: unit -> Result<unit, 'a>


    /// <summary>Computation expression builder for the Result type.</summary>
    val result: ResultBuilder


    /// <summary>Computation expression builder for the Choice(of2) type.</summary>
    type ChoiceBuilder =
        new: unit -> ChoiceBuilder
        member Return: x: 'a -> Choice<'a, 'b>
        member ReturnFrom: m: Choice<'a, 'b> -> Choice< ^a, ^b>
        member inline Bind: m: Choice< ^a, ^e> * f: (^a -> Choice< ^b, ^e>) -> Choice< ^b, ^e>
        member Zero: unit -> Choice<unit, 'a>


    /// <summary>Computation expression builder for the Choice(of2) type.</summary>
    val choice: ChoiceBuilder