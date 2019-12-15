namespace Rogz.Prelude


[<AutoOpen>]
module Numeric =

    let inline isEven (n: ^num) =
        let one = LanguagePrimitives.GenericOne< ^num>
        n % ((one + one): ^num) = LanguagePrimitives.GenericZero< ^num>

    let inline isOdd (n: ^num) =
        let one = LanguagePrimitives.GenericOne< ^num>
        n % ((one + one): ^num) <> LanguagePrimitives.GenericZero< ^num>

    let inline gcd (x: ^num) (y: ^num) : ^num =
        let z = LanguagePrimitives.GenericZero< ^num>
        let rec go x y = if y = z then x else go y (x % y)
        go x y

    let inline lcm (x: ^num) (y: ^num) : ^num = abs ((x * y): ^num) / (gcd x y)


    //[<Struct>]
    //type Rational<'N> = { Numer: 'N; Denom: 'N }
    //with
    //    static member inline ( + ) (r1: Rational< ^n>, r2: Rational< ^n>) : Rational< ^n> =
    //        { Numer = r1.Numer * r2.Denom + r2.Numer * r1.Denom
    //        ; Denom = r1.Denom * r2.Denom }

    //    static member inline ( - ) (r1: Rational< ^n>, r2: Rational< ^n>) : Rational< ^n> =
    //        { Numer = r1.Numer * r2.Denom - r2.Numer * r1.Denom
    //        ; Denom = r1.Denom * r2.Denom }

    //    static member inline ( * ) (r1: Rational< ^n>, r2: Rational< ^n>) : Rational< ^n> =
    //        { Numer = r1.Numer * r2.Numer
    //        ; Denom = r1.Denom * r2.Denom }

    //    static member inline ( / ) (r1: Rational< ^n>, r2: Rational< ^n>) : Rational< ^n> =
    //        { Numer = r1.Numer * r2.Denom
    //        ; Denom = r1.Denom * r2.Numer }

    //    static member inline Pow (r1: Rational< ^n>, p: ^p) : Rational< ^n> =
    //        { Numer = r1.Numer ** p
    //        ; Denom = r1.Denom ** p }

    //    static member inline Abs(r: Rational< ^n>) : Rational< ^n> =
    //        { Numer = abs r.Numer
    //        ; Denom = abs r.Denom }

    //    static member inline Reduce(r: Rational< ^n>) : Rational< ^n> =
    //        let d = gcd r.Numer r.Denom
    //        { Numer = r.Numer / d
    //        ; Denom = r.Denom / d }