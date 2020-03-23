namespace Rogz.Prelude


/// <summary>Module containing function combinators.</summary>
module Combinator =

    /// <summary>Provides for sequential application of functions which share an initial argument.
    ///
    /// Applies an inner function to an input value, then applies an outer function to the original input and the result of the inner function.</summary>
    val inline apply: inner: (^a -> ^b) -> outer: (^a -> ^b -> ^c) -> (^a -> ^c)

    /// <summary>Convert a value into a single argument function that ignores its argument.</summary>
    val inline konst: ^a -> ^``_`` -> ^a

    /// <summary>Makes recursive functions out of non-recursive functions by providing a continuation function which can be applied to carry out computations.
    ///
    /// Note that the continuation must be in tail-call position to guarantee constant stack space.</summary>
    val inline fix: f: ((^a -> ^b) -> ^a -> ^b) -> x: ^a -> ^b

    /// <summary>Makes recursive functions out of non-recursive functions by providing a continuation function which can be applied to carry out computations.
    ///
    /// This can be used in cases where some (sub)computations are repeated to drastically improve performance over `fix`.
    ///
    /// Note that the continuation must be in tail-call position to guarantee constant stack space.</summary>
    val inline fixc: f: ((^a -> ^b) -> ^a -> ^b) -> x: ^a -> ^b when ^a: equality

    /// <summary>Flip the first two arguments of a given function.</summary>
    val inline flip: f: (^a -> ^b -> ^c) -> ^b -> ^a -> ^c

    /// <summary>Apply the same inner function to two inputs and combine their results with the outer function.</summary>
    val inline on: inner: (^a -> ^b) -> outer: (^b -> ^b -> ^c) -> input1: ^a -> input2: ^a -> ^c

    /// <summary>Apply a curried function to a tuple.</summary>
    val inline curry: f: (^a -> ^b -> ^c) -> ^a * ^b -> ^c
    
    /// <summary>Apply a curried function to a tuple.</summary>
    val inline curry3: f: (^a -> ^b -> ^c -> ^d) -> ^a * ^b * ^c -> ^d

    /// <summary>Apply a curried function to a struct-tuple.</summary>
    val inline currys: f: (^a -> ^b -> ^c) -> struct (^a * ^b) -> ^c
    
    /// <summary>Apply a curried function to a struct-tuple.</summary>
    val inline currys3: f: (^a -> ^b -> ^c -> ^d) -> struct (^a * ^b * ^c) -> ^d

    /// <summary>Convert a function on a tuple to a curried function.</summary>
    val inline uncurry: f: (^a * ^b -> ^c) -> ^a -> ^b -> ^c
       
    /// <summary>Convert a function on a tuple to a curried function.</summary>
    val inline uncurry3: f: (^a * ^b * ^c -> ^d) -> ^a -> ^b -> ^c -> ^d

    /// <summary>Convert a function on a struct-tuple to a curried function.</summary>
    val inline uncurrys: f: (struct (^a * ^b) -> ^c) -> ^a -> ^b -> ^c

    /// <summary>Convert a function on a struct-tuple to a curried function.</summary>
    val inline uncurrys3: f: (struct (^a * ^b * ^c) -> ^d) -> ^a -> ^b -> ^c -> ^d