namespace Rogz.Prelude


/// <summary>Operations on tuples.</summary>
[<Sealed; AbstractClass>]
type Tup =
    
    /// <summary>Return the first element of a tuple.</summary>
    static member inline _1: (^a * ^b) -> ^a
    /// <summary>Return the first element of a tuple.</summary>
    static member inline _1: (^a * ^b * ^c) -> ^a
    /// <summary>Return the first element of a tuple.</summary>
    static member inline _1: (^a * ^b * ^c * ^d) -> ^a
    /// <summary>Return the first element of a tuple.</summary>
    static member inline _1: struct (^a * ^b) -> ^a
    /// <summary>Return the first element of a tuple.</summary>
    static member inline _1: struct (^a * ^b * ^c) -> ^a
    /// <summary>Return the first element of a tuple.</summary>
    static member inline _1: struct (^a * ^b * ^c * ^d) -> ^a

    /// <summary>Return the second element of a tuple.</summary>
    static member inline _2: (^a * ^b) -> ^b
    /// <summary>Return the second element of a tuple.</summary>
    static member inline _2: (^a * ^b * ^c) -> ^b
    /// <summary>Return the second element of a tuple.</summary>
    static member inline _2: (^a * ^b * ^c * ^d) -> ^b
    /// <summary>Return the second element of a tuple.</summary>
    static member inline _2: struct (^a * ^b) -> ^b
    /// <summary>Return the second element of a tuple.</summary>
    static member inline _2: struct (^a * ^b * ^c) -> ^b
    /// <summary>Return the second element of a tuple.</summary>
    static member inline _2: struct (^a * ^b * ^c * ^d) -> ^b

    /// <summary>Return the third element of a tuple.</summary>
    static member inline _3: (^a * ^b * ^c )-> ^c
    /// <summary>Return the third element of a tuple.</summary>
    static member inline _3: (^a * ^b * ^c * ^d) -> ^c
    /// <summary>Return the third element of a tuple.</summary>
    static member inline _3: struct (^a * ^b * ^c) -> ^c
    /// <summary>Return the third element of a tuple.</summary>
    static member inline _3: struct (^a * ^b * ^c * ^d) -> ^c

    /// <summary>Return the fourth element of a tuple.</summary>
    static member inline _4: (^a * ^b * ^c * ^d) -> ^d
    /// <summary>Return the fourth element of a tuple.</summary>
    static member inline _4: struct (^a * ^b * ^c * ^d) -> ^d

    /// <summary>Transform a Tuple into a ValueTuple.</summary>
    static member inline _X: (^a * ^b) -> struct (^a * ^b)
    /// <summary>Transform a Tuple into a ValueTuple.</summary>
    static member inline _X: (^a * ^b * ^c) -> struct (^a * ^b * ^c)
    /// <summary>Transform a Tuple into a ValueTuple.</summary>
    static member inline _X: (^a * ^b * ^c * ^d) -> struct (^a * ^b * ^c * ^d)

    /// <summary>Transform a ValueTuple into a Tuple.</summary>
    static member inline _X: struct (^a * ^b) -> (^a * ^b)
    /// <summary>Transform a ValueTuple into a Tuple.</summary>
    static member inline _X: struct (^a * ^b * ^c) -> (^a * ^b * ^c)
    /// <summary>Transform a ValueTuple into a Tuple.</summary>
    static member inline _X: struct (^a * ^b * ^c * ^d) -> (^a * ^b * ^c * ^d)