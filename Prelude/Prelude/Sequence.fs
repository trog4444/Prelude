namespace Rogz.Prelude


module Sequence =

    let inline fold folder (state: ^State) (source: ^T seq) =
        let mutable s = state
        match source with
        | null -> ()
        | :? array< ^T> as xs ->
            for i = 0 to xs.Length - 1 do s <- folder s xs.[i]
        | :? ResizeArray< ^T> as xs ->
            for i = 0 to xs.Count - 1 do s <- folder s xs.[i]
        | :? list< ^T> as xs -> for x in xs do s <- folder s x
        | _ -> for x in source do s <- folder s x
        s

    let inline enumerate (producer: int -> ^a) start stop = seq {
        if start = stop then yield producer start
        elif start > stop then for i = start downto stop do yield producer i
        else for i = start to stop do yield producer i }

    let inline iterate producer (seed: ^s) : ^a seq = seq {
        let mutable s = seed
        while true do
            let struct (a, z) = producer s
            yield a
            s <- z }

    let intersperse elem (source: 'a seq) = seq {
        use e = source.GetEnumerator()
        if e.MoveNext() then
            yield e.Current
            while e.MoveNext() do
                yield elem
                yield e.Current }

    let inline ( ^& ) (x: ^a) xs = x::xs