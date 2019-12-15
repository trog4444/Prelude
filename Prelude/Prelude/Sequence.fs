namespace Rogz.Prelude


[<AutoOpen>]
module Sequence =

    let inline enumerate (producer: int -> ^a) start stop = seq {
        if start = stop then yield producer start
        elif start > stop then for i = start downto stop do yield producer i
        else for i = start to stop do yield producer i }