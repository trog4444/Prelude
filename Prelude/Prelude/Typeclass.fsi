namespace Rogz.Prelude


/// <summary>Representations of basic `interfaces` using explicit 'dictionary-passing' style,
/// allowing multiple, custom interface implementations for various types.</summary>
module Typeclass =

// Eq

    /// <summary>Encapsulates types which can be tested for equality.</summary>
    [<NoComparison; NoEquality>]
    type Eq<'T> =
        new: test: (^T -> ^T -> bool) -> Eq< ^T>
        new: test_: System.Func< ^T, ^T, bool> -> Eq< ^T>
        /// <summary></summary>
        member Eq: a: ^T * b: ^T -> bool

    /// <summary></summary>
    val inline eq: tester: #Eq< ^a> -> x: ^a -> y: ^a -> bool

    /// <summary></summary>
    val inline notEq: tester: #Eq< ^a> -> x: ^a -> y: ^a -> bool


// Ord

    /// <summary></summary>
    [<Struct>]
    type Ordering = LT | EQ | GT with
        /// <summary></summary>
        member Value: int
    
    /// <summary>Encapsulates types which can be compared for order.</summary>
    [<NoComparison; NoEquality>]
    type Ord<'T> =
        new: ord: (^T -> ^T -> Ordering) * eq: (^T -> ^T -> bool) -> Ord< ^T>
        inherit Eq<'T>
        new: ord_: (^T -> ^T -> Ordering) -> Ord< ^T>
        new: ord_: System.Func< ^T, ^T, Ordering> -> Ord< ^T>
        new: ord_: System.Func< ^T, ^T, Ordering> * eq_: System.Func< ^T, ^T, bool> -> Ord< ^T>
        /// <summary></summary>
        member Ord: a: ^T * b: ^T -> Ordering
        /// <summary></summary>
        member Compare: a: ^T * b: ^T -> int

    /// <summary></summary>
    val inline ord: ordering: Ord< ^T> -> x: ^T -> y: ^T -> Ordering

    /// <summary></summary>
    val inline minimum: ordering: Ord< ^T> -> x: ^T -> y: ^T -> ^T

    /// <summary></summary>
    val inline maximum: ordering: Ord< ^T> -> x: ^T -> y: ^T -> ^T


// Show

    /// <summary>Encapsulates types which can be represented as strings.</summary>
    [<NoComparison; NoEquality>]
    type Show<'T> =
        new: show: (^T -> string) -> Show< ^T>
        new: show_: System.Func< ^T, string> -> Show< ^T>
        /// <summary></summary>
        member Show: x: ^T -> string

    /// <summary></summary>
    val inline show: shower: Show< ^T> -> x: ^T -> string


// Semigroup

    /// <summary>Encapsulates types which can be 'appended' together.</summary>
    [<NoComparison; NoEquality>]
    type Semigroup<'T> =
        new: append: (^T -> ^T -> ^T) -> Semigroup< ^T>
        new: append_: System.Func< ^T, ^T, ^T> -> Semigroup< ^T>
        /// <summary></summary>
        member Append: x: ^T * y: ^T -> ^T

    /// <summary></summary>
    val inline append: sg: #Semigroup< ^T> -> x: ^T -> y: ^T -> ^T


// Monoid

    /// <summary>Encapsulates types which can be 'appended' together and has an 'identity' element.</summary>
    [<NoComparison; NoEquality>]
    type Monoid<'T> =
        new: append: (^T -> ^T -> ^T) * empty: ^T -> Monoid< ^T>
        inherit Semigroup<'T>
        new: append_: System.Func< ^T, ^T, ^T> * empty_: ^T -> Monoid< ^T>
        /// <summary></summary>
        member Empty: ^T

    /// <summary></summary>
    val inline toMonoid: sg: Semigroup< ^T> -> empty: ^T -> Monoid< ^T>

    /// <summary></summary>
    val empty: m: Monoid<'T> -> ^T


// Enum

    /// <summary>Encapsulates types which can be enumerated over.</summary>
    [<NoComparison; NoEquality>]
    type Index<'T> =
        new: toEnum: (int -> ^T) * fromEnum: (^T -> int) -> Index< ^T>
        new: toEnum_: System.Func<int, ^T> * fromEnum_: System.Func< ^T, int> -> Index< ^T>
        /// <summary></summary>
        member ToEnum: i: int -> ^T
        /// <summary></summary>
        member FromEnum: x: ^T -> int


    /// <summary></summary>
    val succ: ix: Index<'T> -> elem: ^T -> ^T

    /// <summary></summary>
    val pred: ix: Index<'T> -> elem: ^T -> ^T

    /// <summary></summary>
    val inline indexTo: ix: Index< ^T> -> start: int -> stop: int -> ^T seq

    /// <summary></summary>
    val inline indexBy: ix: Index< ^T> -> start: int -> step: int -> stop: int -> ^T seq