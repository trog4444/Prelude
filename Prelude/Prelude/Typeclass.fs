namespace Rogz.Prelude


module Typeclass =

// Eq

    [<NoComparison; NoEquality>]
    type Eq<'T>(test: ^T -> ^T -> bool) =
        new(test_: System.Func< ^T, ^T, bool>) = Eq< ^T>(test = fun a b -> test_.Invoke(a, b))
        member _.Eq(a: ^T, b: ^T) = test a b

    let inline eq (tester: #Eq< ^a>) x y = tester.Eq(x, y)

    let inline notEq (tester: #Eq< ^a>) x y = not (tester.Eq(x, y))


// Ord

    [<Struct>]
    type Ordering = LT | EQ | GT with
        member s.Value = match s with LT -> -1 | EQ -> 0 | GT -> 1

    [<NoComparison; NoEquality>]
    type Ord<'T>(ord: ^T -> ^T -> Ordering, eq: ^T -> ^T -> bool) =
        inherit Eq<'T>(test = eq)
        new(ord_: ^T -> ^T -> Ordering) =
            Ord< ^T>(ord = ord_, eq = fun a b -> (ord_ a b) = EQ)
        new(ord_: System.Func< ^T, ^T, Ordering>) =
            Ord< ^T>(ord = (fun a b -> ord_.Invoke(a, b)),
                     eq  = fun a b -> (ord_.Invoke(a, b)) = EQ)
        new(ord_: System.Func< ^T, ^T, Ordering>, eq_: System.Func< ^T, ^T, bool>) =
            Ord< ^T>(ord = (fun a b -> ord_.Invoke(a, b)),
                     eq  = fun a b -> eq_.Invoke(a, b))
        member _.Ord(a, b) = ord a b
        member _.Compare(a, b) = (ord a b).Value

    let inline ord (ordering: Ord< ^T>) x y = ordering.Ord(x, y)

    let inline minimum (ordering: Ord< ^T>) x y =
        match ordering.Ord(x, y) with LT | EQ -> x | GT -> y

    let inline maximum (ordering: Ord< ^T>) x y =
        match ordering.Ord(x, y) with LT | EQ -> y | GT -> x


// Show

    [<NoComparison; NoEquality>]
    type Show<'T>(show: ^T -> string) =
        new(show_: System.Func< ^T, string>) = Show< ^T>(show = show_.Invoke)
        member _.Show(x: ^T) = show x

    let inline show (shower: Show< ^T>) x = shower.Show x


//// Semigroupoid

//    [<NoComparison; NoEquality>]
//    type Semigroupiod<'T, 'U, 'V> = { Compose: ('U -> 'V) -> ('T -> 'U) -> ('T -> 'V) } with
//        static member inline New(compose_: System.Func<System.Func< ^U, ^V>, System.Func< ^T, ^U>, System.Func< ^T, ^V>>) =
//            { Compose = fun f g t -> compose_.Invoke(System.Func<_,_>f, System.Func<_,_>g).Invoke(t) }
//        member inline s.ComposeF(f_: System.Func< ^U, ^V>, g_: System.Func< ^T, ^U>) =
//            s.Compose (f_.Invoke) (g_.Invoke)

//    let inline compose (sg: Semigroupiod< ^a, ^b, ^c>) f g = sg.Compose f g


//// Category
    
//    [<NoComparison; NoEquality>]
//    type Category<'T>


// Semigroup

    [<NoComparison; NoEquality>]
    type Semigroup<'T>(append: ^T -> ^T -> ^T) =
        new(append_: System.Func< ^T, ^T, ^T>) = Semigroup< ^T>(append = fun a b -> append_.Invoke(a, b))
        member _.Append(x, y) = append x y

    let inline append (sg: #Semigroup< ^T>) x y = sg.Append(x, y)


// Monoid

    [<NoComparison; NoEquality>]
    type Monoid<'T>(append: ^T -> ^T -> ^T, empty: ^T) =
        inherit Semigroup<'T>(append = append)
        new(append_: System.Func< ^T, ^T, ^T>, empty_: ^T) =
            Monoid< ^T>(append = (fun a b -> append_.Invoke(a, b)), empty = empty_)
        member _.Empty = empty

    let inline toMonoid (sg: Semigroup< ^T>) empty =
        Monoid(append = (fun a b -> sg.Append(a, b)), empty = empty)

    let empty (m: Monoid<'T>) = m.Empty


// Enum

    do failwith "Should Index implement Succ and Pred?"
    'default' could use
        succ = toEnum . (+ 1) . fromEnum
        pred = toEnum . (- 1) . fromEnum
    also, add ALL XML docs for this file

    ----
    [<NoComparison; NoEquality>]
    type Index<'T>(toEnum: int -> ^T, fromEnum: ^T -> int) =
        new(toEnum_: System.Func<int, ^T>, fromEnum_: System.Func< ^T, int>) =
            Index< ^T>(toEnum = toEnum_.Invoke, fromEnum = fromEnum_.Invoke)
        member _.ToEnum(i) = toEnum i
        member _.FromEnum(x) = fromEnum x


    let succ (ix: Index<'T>) elem = ix.ToEnum(ix.FromEnum(elem) + 1)

    let pred (ix: Index<'T>) elem = ix.ToEnum(ix.FromEnum(elem) - 1)

    let inline indexTo (ix: Index< ^T>) start stop =
        seq { for i = start to stop do yield ix.ToEnum i }

    let inline indexBy (ix: Index< ^T>) start step stop =
        seq { let mutable i = start
              while i <= stop do
                yield ix.ToEnum i
                i <- i + step }

    let inline range (ix: Index< ^T>) start =
        seq { let mutable i = ix.FromEnum start
              yield start
              while true do
                let x = ix.ToEnum(i + 1)
                yield x
                i <- ix.FromEnum x }

    let ix = Index(toEnum = (fun a -> string <| a + 1), fromEnum = int)
    let xs = range ix "7" |> Seq.take 10 |> Seq.toList