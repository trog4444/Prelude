namespace Rogz.Prelude


[<AutoOpen>]
module Combinator =

    let inline apply inner (outer: ^a -> ^b -> ^c) a = outer a (inner a)

    let inline konst x (_: ^``_``) : ^a = x

    let inline fix f (x: ^a) : ^b = let rec go a = f go a in go x

    let inline fixc f (x: ^a) : ^b =
        let d = System.Collections.Generic.Dictionary<_,_>(HashIdentity.Structural)
        let rec go x =
            match d.TryGetValue(x) with
            | true, r  -> r
            | false, _ -> let r = f go x in d.[x] <- r; r
        go x

    let inline flip (f: ^a -> ^b -> ^c) b a = f a b

    let inline on (inner: ^a -> ^b) outer in1 in2 : ^c = outer (inner in1) (inner in2)

    let inline curry (f: ^a -> ^b -> ^c) (a, b) = f a b

    let inline curry3 (f: ^a -> ^b -> ^c -> ^d) (a, b, c) = f a b c

    let inline currys (f: ^a -> ^b -> ^c) (struct (a, b)) = f a b

    let inline currys3 (f: ^a -> ^b -> ^c -> ^d) (struct (a, b, c)) = f a b c

    let inline uncurry (f: ^a * ^b -> ^c) a b = f (a, b)

    let inline uncurry3 (f: ^a * ^b * ^c-> ^d) a b c = f (a, b, c)
    
    let inline uncurrys (f: struct (^a * ^b) -> ^c) a b = f (struct (a, b))

    let inline uncurrys3 (f: struct (^a * ^b * ^c) -> ^d) a b c = f (struct (a, b, c))