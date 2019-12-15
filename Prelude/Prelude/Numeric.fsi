namespace Rogz.Prelude


/// <summary>Operations and types relating to numeric values.</summary>
[<AutoOpen>]
module Numeric =

    /// <summary>Returns true when the supplied input is even.</summary>
    val inline isEven: n: ^num -> bool
        when ^num: (static member Zero: ^num)
        and  ^num: (static member One: ^num)
        and  ^num: (static member ( + ): ^num -> ^num -> ^num)
        and  ^num: (static member ( % ):  ^num -> ^num ->  ^num)
        and  ^num: equality

    /// <summary>Returns true when the supplied input is odd.</summary>
    val inline isOdd: n: ^num -> bool
        when ^num: (static member Zero: ^num)
        and  ^num: (static member One: ^num)
        and  ^num: (static member ( + ): ^num -> ^num -> ^num)
        and  ^num: (static member ( % ): ^num -> ^num -> ^num)
        and  ^num: equality

    /// <summary>Greatest common denominator of two values.</summary>
    val inline gcd: x: ^num -> y: ^num -> ^num
        when ^num: (static member Zero: ^num)
        and  ^num: (static member ( % ): ^num -> ^num -> ^num)
        and  ^num: equality

    /// <summary>Least common multiple of two values.</summary>
    val inline lcm: x: ^num -> y: ^num -> ^num
        when ^num: (static member Zero: ^num)
        and  ^num: (static member ( * ): ^num -> ^num -> ^num)
        and  ^num: (static member ( / ): ^num -> ^num -> ^num)
        and  ^num: (static member ( % ): ^num -> ^num -> ^num)
        and  ^num: (static member Abs: ^num -> ^num)
        and  ^num: equality


    ///// <summary>Representation of exact rational numbers.</summary>
    //[<Struct>]
    //type Rational<'N> = { Numer: 'N; Denom: 'N }
    //with
    //    /// <summary>Add two rational numbers.</summary>
    //    static member inline ( + ): r1: Rational< ^n> * r2: Rational< ^n> -> Rational< ^n>
    //        when ^n: (static member (+): ^n -> ^n -> ^n)
    //        and  ^n: (static member (*): ^n -> ^n -> ^n)

    //    /// <summary>Subtract two rational numbers.</summary>
    //    static member inline ( - ): r1: Rational< ^n> * r2: Rational< ^n> -> Rational< ^n>
    //        when ^n: (static member (-): ^n -> ^n -> ^n)
    //        and  ^n: (static member (*): ^n -> ^n -> ^n)

    //    /// <summary>Multiply two rational numbers.</summary>
    //    static member inline ( * ): r1: Rational< ^n> * r2: Rational< ^n> -> Rational< ^n>
    //        when ^n: (static member (*): ^n -> ^n -> ^n)

    //    /// <summary>Divide two rational numbers.</summary>
    //    static member inline ( / ): r1: Rational< ^n> * r2: Rational< ^n> -> Rational< ^n>
    //        when ^n: (static member (*): ^n -> ^n -> ^n)

    //    /// <summary>Raise a rational number to the given power.</summary>
    //    static member inline Pow: r1: Rational< ^n> * p: ^p -> Rational< ^n>
    //        when ^n: (static member Pow: ^n -> ^p -> ^n)

    //    /// <summary>Absolute value of a rational number.</summary>
    //    static member inline Abs: r: Rational< ^n> -> Rational< ^n>
    //        when ^n: (static member Abs: ^n -> ^n)

    //    /// <summary>Reduce a rational number to its lowest possible terms.</summary>
    //    static member inline Reduce: r: Rational< ^n> -> Rational< ^n>
    //        when ^n: (static member Zero: ^n)
    //        and  ^n: (static member (/): ^n -> ^n -> ^n)
    //        and  ^n: (static member (%): ^n -> ^n -> ^n)
    //        and  ^n: equality