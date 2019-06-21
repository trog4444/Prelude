namespace Prelude


/// Basic math operations.
module Math =

    
// ====================================================================================================


    /// Generic functions for ref cells.
    module RefCells =

        /// General increment function for any type of numeric ref cells.
        let inline gincr (x: ^a ref) = x := !x + LanguagePrimitives.GenericOne

        /// General increment function for any type of numeric ref cells.
        let inline gdecr (x: ^a ref) = x := !x - LanguagePrimitives.GenericOne


// ====================================================================================================


    /// General math functions.
    module General =

        /// Monus operator for numbers.
        let inline monus larger smaller =
            if larger < smaller then LanguagePrimitives.GenericZero
            else larger - smaller

        /// Arrange two items as a pair with the min on the left and the max on the right.
        let inline minmax a b = if min a b = a then a, b else b, a

        /// Returns the number of times a number can be divided by another number, plus the remainder.
        let inline divRem numerator denominator =
            match denominator with
            | 0 -> None
            | _ -> Some (System.Math.DivRem(numerator, denominator))

        /// Determining if a number is divisible by another number.
        let inline isDivisibleBy by n =
            n % by = LanguagePrimitives.GenericZero

        /// Greatest common denominator.
        let inline gcd x y =
            let rec go x = function
            | 0 -> x
            | y -> go y (x % y)
            go x y

        /// Least common multiple.
        let inline lcm x y = abs (x * y) / (gcd x y)

        /// Test for evenness.
        let inline isEven n = 
            let one  = LanguagePrimitives.GenericOne
            let zero = LanguagePrimitives.GenericZero
            n % (one + one) = zero

        /// Test for oddness.
        let inline isOdd n = not (isEven n)        

        /// Active pattern for checking the even-ness or odd-ness of a generic number.
        let (|Even|Odd|) n = if isEven n then Even else Odd

        /// Find the factorial of any arbitrary numeric type.
        let inline factorial n = 
            let one = LanguagePrimitives.GenericOne
            let zero = LanguagePrimitives.GenericZero
            if n < zero then None
            else let rec go a = function
                 | n when n <= one -> a
                 | n               -> go (Checked.(*) a n) (n - one)
                 try Some (go one n)
                 with _ -> None

        /// Find the product of any arbitrary numeric type (Checks for overflows and returns None if overflow occurs).
        let inline product (nums: ^a seq) =
            let zero = LanguagePrimitives.GenericZero
            let mutable p = LanguagePrimitives.GenericOne
            let update x = p <- Checked.(*) p x
            try 
                match nums with
                | :? array<'a> as xs ->
                    let len = xs.Length
                    let rec go = function                
                    | i when i < len ->
                        let x = xs.[i]
                        if x = zero
                        then p <- zero
                        else update x
                             go (i + 1)
                    | _ -> ()
                    go 0
                | :? list<'a> as xs ->
                    let rec go = function
                    | x::xs ->
                        if x = zero
                        then p <- zero
                        else update x
                             go xs
                    | [] -> ()
                    go xs
                | _ ->
                    use enum = nums.GetEnumerator()
                    while p <> zero && enum.MoveNext() do
                        update enum.Current
                Some p
            with _ -> None


// ====================================================================================================


    /// Common calculus functions.
    module Calculus =


        /// Estimate the derivative of a function with the given tolerance 'eps).
        /// Note: can fail if the tolerance is too small.
        let inline derivative eps f x =
            let zero = LanguagePrimitives.GenericZero        
            let eps = abs eps
            if eps = zero then None else
                let one  = LanguagePrimitives.GenericOne
                let two  = one + one
                let rec go dx prev =
                    let r = (f (x + dx) - f x) / dx
                    match r - prev with
                    | a when a <= eps -> Some r
                    | _ -> go (dx / two) r
                go eps zero



        can replace unfold >> sum >> (*) dx with a hylo for better performance ?? needs testing



        //    let inline hylo generator folder seedA seedC =
        //        let rec go z = function
        //        | Some (x, s) -> go (folder x z) (generator s)
        //        | None -> z
        //        go seedC (generator seedA)

        /// Estimate the derivative of a function with the given tolerance 'eps).
        ///
        /// Note: As tolerance gets smaller, calculation time increases exponentially.
        let inline integral eps f (start, ``end``) =                        
            let zero = LanguagePrimitives.GenericZero
            match abs eps with
            | e when e = zero -> None
            | e ->        
              match if max start ``end`` = start then start - ``end`` else ``end`` - start with
              | del when del = zero -> None
              | del ->
                   let one = LanguagePrimitives.GenericOne
                   let two = one + one
                   let sections = Array.sum (Array.replicate 500 two)
                   let rec go sections prev =
                       let dx = del / sections
                       let r = Seq.unfold (fun (s, d) ->
                        match s > sections with
                        | true -> None
                        | false -> let d' = d + dx
                                   Some (f d', (s + one, d'))) (zero, zero)
                               |> (Seq.sum >> (*) dx)
                       match abs (r - prev) with
                       | a when a <= e -> r
                       | _ -> go (sections * two) r
                   Some (go sections zero)


        //let f f' eps = integral eps f' 

        let eps = 0.00000001
        let f x = 0.5 * x
        let range = 0., 1.
        let t = System.Diagnostics.Stopwatch.StartNew()
        let a = integral eps f range
        let ta = t.ElapsedMilliseconds
        let resultA = sprintf "a: %A in %i" a ta
        //70 => ratio 7.43
        //520 => ratio 15.58
        //8100

// ====================================================================================================


    /// Commons statistics functions.
    module Statistics =


        /// Compute the variance of a sequence.
        let inline variance (samples: ^a seq) = 
            match samples with
            | null -> None
            | xs when Seq.isEmpty xs -> None
            | :? array< ^a> as xs ->
                let struct (sum, len) = Array.fold (fun struct (s, l) x -> struct (s + x, l + LanguagePrimitives.GenericOne)) (struct (LanguagePrimitives.GenericZero, LanguagePrimitives.GenericZero)) xs
                match len with
                | z when z = LanguagePrimitives.GenericZero -> None
                | _ -> let mean = sum / len in Some (Array.sumBy ((-) mean >> pown >> (|>) 2) xs / len)
            | :? List< ^a> as xs ->
                let struct (sum, len) = List.fold (fun struct (s, l) x -> struct (s + x, l + LanguagePrimitives.GenericOne)) (struct (LanguagePrimitives.GenericZero, LanguagePrimitives.GenericZero)) xs
                match len with
                | z when z = LanguagePrimitives.GenericZero -> None
                | _ -> let mean = sum / len in Some (List.sumBy ((-) mean >> pown >> (|>) 2) xs / len)
            | xs -> let ys = Seq.cache xs
                    let struct (sum, len) = Seq.fold (fun struct (s, l) x -> struct (s + x, l + LanguagePrimitives.GenericOne)) (struct (LanguagePrimitives.GenericZero, LanguagePrimitives.GenericZero)) ys
                    match len with
                    | z when z = LanguagePrimitives.GenericZero -> None
                    | _ -> let mean = sum / len in Some (Seq.sumBy ((-) mean >> pown >> (|>) 2) ys / len)

        /// Compute the standard deviation of a sequence.
        let inline stdev samples = Option.map sqrt (variance samples)


// ====================================================================================================


    /// Generators for (possibly) infinite random sequences.
    module Random =

    ?? make seed optional ??
        /// Generate an infinite sequence of random integers.
        let inline intGen seed =            
           seq { let r = match seed with
                         | Some s -> new System.Random(s)
                         | None -> new System.Random() in while true do yield r.Next() }

        /// Generate an infinite sequence of random integers in a specified range.
        let inline intsIn seed (min, max)  =                        
            let min, max = if min < max then min, max else max, min
            seq { let r = new System.Random(seed) in while true do yield r.Next(min, max) }

        /// Generate an infinite sequence of random floats.
        let inline floatGen seed =
            seq { let r = new System.Random(seed) in while true do yield r.NextDouble() }