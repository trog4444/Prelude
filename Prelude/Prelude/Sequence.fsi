namespace Rogz.Prelude


/// <summary>Generic functions on sequences.</summary>
module Sequence =

    /// <summary>Serves the same function as `Seq.fold` but can be more efficient in some cases.
    /// A `null` source is treated as an empty sequence rather than throwing an exception.</summary>
    val inline fold: folder: (^State -> ^T -> ^State) -> state: ^State -> source: ^T seq -> ^State

    /// <summary>Generate a sequence using the given `producer`, iterating over numbers ranging (up or down) from `start` to `stop`.
    /// This will always produce at least 1 result.</summary>
    val inline enumerate: producer: (int -> ^a) -> start: int -> stop: int -> ^a seq
    
    /// <summary>Returns an infinite, lazy sequence of repeated applications of the producer to some state value.</summary>
    val inline iterate: producer: (^s -> struct (^a * ^s)) -> seed: ^s -> ^a seq

    /// <summary>Takes an element and a sequence and `intersperses` that element between the elements of the sequence.</summary>
    /// <exception cref="System.ArgumentNullException">Thrown when the input sequence is null.</exception>
    val intersperse: elem: 'a -> source: ^a seq -> ^a seq

    /// <summary>(Right associative) Adds an element to the front of a list.</summary>
    val inline ( ^& ): x: ^a -> xs: ^a list -> ^a list