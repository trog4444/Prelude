namespace Prelude


/// Functions extending sequences.
module Seqs =


(* ====================================================================================================== *)


    /// If the given sequence is null, replace it with an empty sequence.
    let inline noNil xs = match xs with null -> Seq.empty | _ -> xs


(* ====================================================================================================== *)


    /// Builds two separate sequences as pairs.
    ///
    /// Note: The source sequence is eagerly evaluated.
    let inline build2 f g (xs: ^a seq) =
        match xs with
        | null -> Seq.empty, Seq.empty
        | xs when Seq.isEmpty xs -> Seq.empty, Seq.empty
        | :? list< ^a> as xs ->
            let rec go kf kg = function
            | x::xs -> go (fun xs -> kf (f x::xs)) (fun xs -> kg (g x::xs)) xs
            | []    -> seq (kf []), seq (kg [])
            go id id xs
        | :? array< ^a> as xs ->
            let len = xs.Length - 1
            let rec go kf kg = function
            | n when n <= len -> go (fun ys -> kf (f xs.[n]::ys)) (fun ys -> kg (g xs.[n]::ys)) (n + 1)
            | _               -> seq (kf []), seq (kg [])
            go id id 0
        | _ ->
            use enum = xs.GetEnumerator()
            let rec go kf kg = function
            | false -> kf Seq.empty, kg Seq.empty
            | true  -> let x = enum.Current
                       go (fun xs -> kf (seq { yield f x; yield! xs })) (fun xs -> kg (seq { yield g x; yield! xs })) (enum.MoveNext())
            go id id (enum.MoveNext())


(* ====================================================================================================== *)


    /// Repeat an existing sequence infinitely.
    let inline cycle source =
        match source with
        | null -> Seq.empty
        | _    -> seq { while true do yield! source }

    /// Repeats a value by the given number of times.
    let inline repeat count item =
        seq { for i = 1I to count do yield item }


(* ====================================================================================================== *)


    /// Builds a new collection from a collection producing function, using an index (starting from 0).
    let inline collecti projection xs =
        seq { let mutable i = 0
              for x in xs do              
              yield! noNil (projection i x)
              i <- i + 1 }

    /// Project a sequence producing function across two sequences.
    let inline collect2 projection xs ys =
        seq { for (x, y) in Seq.allPairs (noNil xs) (noNil ys) do
              yield! noNil (projection x y) }

    /// Safe exists2 - does not throw an exception when the two input sequences are of unequal lengths - instead it returns false.
    let inline exists2 p xs ys =
        Seq.exists ((<||) p) (Seq.zip (noNil xs) (noNil ys))


(* ====================================================================================================== *)


    /// Unzip a seq of pairs.
    let inline unzip source =
        let s = Seq.cache (noNil source)
        Seq.map fst s, Seq.map snd s

    /// Map a function that produces a pair across a sequence and unzip the results into a pair of sequences.
    let inline mapUnzip mapping source =
        let s = Seq.cache (Seq.map mapping (noNil source))
        Seq.map fst s, Seq.map snd s

    /// Map a function that produces a sequence of pairs across a sequence and unzip the results into a sequence of pairs of sequences.
    let inline collectUnzip projection source =
        unzip (Seq.collect (projection >> noNil) (noNil source))

    /// Unzips a sequence of pairs then maps over each one individually.
    let inline unzipMap on1 on2 source =
        let a, b = unzip source
        Seq.map on1 a, Seq.map on2 b

    /// Unzips a sequence of pairs then projects over each one individually.
    let inline unzipCollect on1 on2 source =
        let a, b = unzip source
        Seq.map on1 a, Seq.map on2 b


(* ====================================================================================================== *)


    /// Apply a filter and mapping in a single pass over a sequence.
    let inline mapIf predicate mapping (source: ^a seq) =
        match source with
        | null -> Seq.empty
        | :? array<'a> as xs ->
            seq { for i = 0 to xs.Length - 1 do
                    let x = xs.[i]
                    if predicate x then yield mapping x }
        | :? list<'a> as xs ->
            let rec go xs = seq { 
                match xs with
                | x::xs -> if predicate x then yield mapping x
                           yield! go xs
                | [] -> () }
            go xs
        | _ -> seq { for x in source do if predicate x then yield mapping x }

    /// Apply a filter and projection in a single pass over a sequence.
    let inline collectIf predicate projection (source: ^a seq) =
        match source with
        | null -> Seq.empty
        | xs when Seq.isEmpty xs -> Seq.empty
        | :? array<'a> as xs -> seq {
            for i = 0 to xs.Length - 1 do
                let x = xs.[i]
                if predicate x then match noNil (projection x) with xs when Seq.isEmpty xs -> () | xs -> yield! xs }
        | :? list<'a> as xs ->
            let rec go xs = seq {
                match xs with
                | x::xs -> if predicate x
                           then match noNil (projection x) with xs when Seq.isEmpty xs -> () | xs -> yield! xs
                                yield! go xs
                | [] -> () }
            go xs
        | _ -> seq { for x in source do if predicate x then match noNil (projection x) with xs when Seq.isEmpty xs -> () | xs -> yield! xs }


(* ====================================================================================================== *)


    /// Right-associative fold that is lazy in its second argument.
    let inline foldr' f s0 (xs: _ seq) =
        match xs with
        | null -> s0 ()
        | s when Seq.isEmpty s -> s0 ()
        | :? array<_> as xs ->
            let rec go acc = function
            | -1 | 0 -> acc ()
            | n -> f xs.[n] (fun () -> go acc (n - 1))
            go s0 (xs.Length - 1)
        | :? list<_> as xs ->
            let rec go acc = function
            | [] -> acc ()
            | x::xs -> f x (fun () -> go acc xs)
            go s0 xs
        | _ ->
            use e = xs.GetEnumerator ()
            let rec go acc = function
                | false -> acc ()
                | true  -> f e.Current (fun () -> go acc (e.MoveNext()))
            go s0 (e.MoveNext())

    /// Left-associative fold that is lazy in its first argument.
    let inline foldl' f s0 (xs: _ seq) =
        match xs with
        | null -> s0 ()
        | s when Seq.isEmpty s -> s0 ()
        | :? array<_> as xs ->
            let len = xs.Length - 1
            let mutable flg = true
            let f' s x = let z = f (fun () -> flg <- true; s ()) x in fun () -> z
            let rec go acc = function
            | n when n >= len -> acc
            | n -> if flg
                   then flg <- false; go (f' acc xs.[n]) (n + 1)
                   else acc
            go s0 0 ()
        | :? list<_> as xs ->
            let mutable flg = true
            let f' s x = let z = f (fun () -> flg <- true; s ()) x in fun () -> z
            let rec go acc = function
            | [] -> acc
            | x::xs -> if flg
                       then flg <- false; go (f' acc x) xs
                       else acc
            go s0 xs ()
        | _ ->
            use e = xs.GetEnumerator ()
            let mutable flg = true
            let f' s x = let z = f (fun () -> flg <- true; s ()) x in fun () -> z
            let rec go acc = function
            | false -> acc
            | true  -> match e.MoveNext () with
                       | false -> acc
                       | true  -> flg <- false
                                  go (f' acc e.Current) flg
            go s0 true ()

    /// An infinite sequence unfold.
    let inline forever generator (seed: ^s) =
        seq { let mutable sd = seed
              while true do
                match generator sd with
                | x, s -> sd <- s; yield x }

    /// Indexed left fold.
    let inline foldli folder seed (source: ^a seq) =        
        let s = struct (seed, 0)
        let inline f (struct (s, i)) x = struct (folder s i x, i + 1)
        match (match noNil source with
               | null -> s
               | xs when Seq.isEmpty xs -> s
               | :? list<'a>  as xs -> List.fold  f s xs
               | :? array<'a> as xs -> Array.fold f s xs
               | xs                 -> Seq.fold   f s xs)
         with struct (s, _) -> s

    /// Indexed right fold.
    let inline foldri folder seed (source: ^a seq) =
        let s = struct (seed, 0)
        let inline f x (struct (s, i)) = struct (folder x i s, i + 1)
        match (match source with
               | null -> s
               | m when Seq.isEmpty m -> s
               | :? list<'a>  as xs -> List.foldBack  f xs s
               | :? array<'a> as xs -> Array.foldBack f xs s
               | xs                 -> Seq.foldBack   f xs s)
         with struct (s, _) -> s

    /// Fold with a continuation. Can be used for early termination, reversing sequences, etc.
    let inline foldk folder seed (source: ^a seq) =
        match source with
        | null -> seed
        | xs when Seq.isEmpty xs -> seed
        | :? array<'a> as xs ->
            let len = xs.Length - 1
            let rec go a = function
            | i when i > len -> a
            | i -> folder xs.[i] a (fun z -> go z (i + 1))
            go seed 0
        | :? list<'a> as xs ->
            let rec go a = function
            | []    -> a
            | x::xs -> folder x a (fun z -> go z xs)
            go seed xs
        | xs ->
            use enum = xs.GetEnumerator()
            let rec go a = function
            | false -> a
            | true  -> folder enum.Current a (fun z -> go z (enum.MoveNext()))
            go seed (enum.MoveNext())    

    /// Fold over a sequence with possible early termination.
    let inline foldWhile folder seed (source: ^a seq) =      
        let mutable st = seed
        match source with
        | null -> st
        | m when Seq.isEmpty m -> st
        | m -> Seq.iter ignore (Seq.takeWhile (fun x -> match folder x st with
                                                        | None   -> false
                                                        | Some s -> st <- s; true) m); st


(* ====================================================================================================== *)


    /// Counts the number of elements in a sequence that match a given predicate.
    let inline countFor predicate source  =
        Seq.sumBy (fun x -> if predicate x then 1I else 0I) (noNil source)

    /// Counts the number of unique elements in a sequence produced by a given function.
    let inline countDistinct mapping (source: #seq< ^a>) =
        match noNil source with
        | xs when Seq.isEmpty xs -> 0
        | xs -> let d1 = System.Collections.Generic.Dictionary< ^a, unit>()
                let d2 = System.Collections.Generic.Dictionary< ^b, unit>()
                let mutable c = 0
                for x in xs do
                    match d1.TryGetValue x with
                    | true,  _ -> ()
                    | false, _ -> d1.[x] <- ()
                                  let r = mapping x
                                  match d2.TryGetValue r with
                                  | true,  _ -> ()
                                  | false, _ -> c <- c + 1; d2.[r] <- ()
                c


(* ====================================================================================================== *)


    /// Run multiple functions on a single input.
    let inline mapf mappings input =
        Seq.map ((|>) input) (noNil mappings)

    /// Map with a running accumulator.
    let inline mapWith mapping seed source =
        seq { let mutable s = seed
              for x in noNil source do
                match mapping s x with a, r -> s <- a; yield r }

    /// Collect with a running accumulator.
    let inline collectWith projection seed source =
        seq { let mutable s = seed
              for x in noNil source do
                match projection s x with a, rs -> s <- a; yield! noNil rs }


(* ====================================================================================================== *)


    /// Safe version of Seq.item function.
    let inline tryItem index source =
        try Some (Seq.item index source)
        with _ -> None


(* ====================================================================================================== *)


    /// Maps a function across a sequence, caching its results along the way to prevent recalculating
    /// previously used inputs.
    let inline memoMap (mapping: ^a -> ^b) source =        
        seq { let cache = System.Collections.Concurrent.ConcurrentDictionary<'a, ^b>()
              for x in noNil source -> cache.GetOrAdd(x, mapping) }

    /// Maps a function across a sequence, caching its results along the way to prevent recalculating
    /// previously used inputs.
    let inline memoCollect (projection: ^a -> #seq< ^b>) source =        
        seq { let cache = System.Collections.Concurrent.ConcurrentDictionary< ^a, #seq< ^b>>()
              for x in noNil source do yield! cache.GetOrAdd(x, projection) }

    /// Filters a sequence, caching its results along the way to prevent recalculating previously used inputs.
    let inline memoFilter predicate (xs: ^a seq) =        
        seq { let pass = System.Collections.Concurrent.ConcurrentDictionary< ^a, unit>()
              let fail = System.Collections.Concurrent.ConcurrentDictionary< ^a, unit>()
              for x in noNil xs do
              match pass.TryGetValue x with
                    | true,  _ -> yield x
                    | false, _ -> match fail.TryGetValue x with
                                  | true,  _ -> ()
                                  | false, _ -> if predicate x
                                                then pass.[x] <- (); yield x
                                                else fail.[x] <- () }


(* ====================================================================================================== *)


    /// Applied to a predicate p and a sequence xs, returns a tuple where
    /// first element is longest prefix (possibly empty) of xs of elements that
    /// satisfy p and the second element is the remainder of the sequence.
    let inline span p xs =
        let mutable n = 0
        let ys = Seq.toList (noNil xs)
        let trues  = List.takeWhile (fun x -> if p x then n <- n + 1; true else false) ys
        let falses = List.skip n ys
        trues, falses       

    /// Applied to a predicate p and a sequence xs, returns a tuple where
    /// first element is longest prefix (possibly empty) of xs of elements that
    /// satisfy p mapped by function f1 and the second element is the remainder
    /// of the sequence mapped by function f2.
    let inline spanMap p f1 f2 xs =
        let mutable n = 0
        let inline f x = if p x then n <- n + 1; true else false
        let ys = Seq.cache (noNil xs)
        let trues  = Seq.toList (Seq.map f1 (Seq.takeWhile f ys))
        let falses = Seq.toList (Seq.map f2 (Seq.skip n ys))
        trues, falses

    /// Applied to a predicate p and a sequence xs, returns a tuple where
    /// first element is longest prefix (possibly empty) of xs of elements that
    /// satisfy p mapped by function f1 and the second element is the remainder
    /// of the sequence mapped by function f2.
    let inline spanCollect p f1 f2 xs =
        let mutable n = 0
        let inline f x = if p x then n <- n + 1; true else false
        let ys = Seq.cache (noNil xs)
        let trues  = Seq.toList (Seq.collect f1 (Seq.takeWhile f ys))
        let falses = Seq.toList (Seq.collect f2 (Seq.skip n ys))
        trues, falses


(* ====================================================================================================== *)


    /// Map a function across a sequence and remove duplicates.
    let inline distinctMap mapping source =
        Seq.distinct (Seq.map mapping (noNil source))

    /// Project a function across a sequence and remove duplicates.
    let inline distinctCollect projection source =
        Seq.distinct (Seq.collect (projection >> noNil) (noNil source))


(* ======================================================================== *)


    /// Maps a function across a sequence and returns the largest result.
    let inline maxOf f min source =
        Seq.foldBack (f >> max) (noNil source) min