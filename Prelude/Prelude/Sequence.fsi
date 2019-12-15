namespace Rogz.Prelude


/// <summary>Generic functions on sequences.</summary>
[<AutoOpen>]
module Sequence =

    /// <summary>Generate a sequence using the given `producer`, iterating over numbers ranging (up or down) from `start` to `stop`.
    /// This will always produce at least 1 result.</summary>
    val inline enumerate: producer: (int -> ^a) -> start: int -> stop: int -> ^a seq