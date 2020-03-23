namespace Rogz.Prelude


[<Sealed; AbstractClass>]
type Tup =

    static member inline _1 ((x: ^a, _: ^b)) = x
    static member inline _1 ((x: ^a, _: ^b, _: ^c)) = x
    static member inline _1 ((x: ^a, _: ^b, _: ^c, _: ^d)) = x

    static member inline _1 (struct (x: ^a, _: ^b)) = x
    static member inline _1 (struct (x: ^a, _: ^b, _: ^c)) = x
    static member inline _1 (struct (x: ^a, _: ^b, _: ^c, _: ^d)) = x

    static member inline _2 ((_: ^a, x: ^b)) = x
    static member inline _2 ((_: ^a, x: ^b, _: ^c)) = x
    static member inline _2 ((_: ^a, x: ^b, _: ^c, _: ^d)) = x

    static member inline _2 (struct (_: ^a, x: ^b)) = x
    static member inline _2 (struct (_: ^a, x: ^b, _: ^c)) = x
    static member inline _2 (struct (_: ^a, x: ^b, _: ^c, _: ^d)) = x

    static member inline _3 ((_: ^a, _: ^b, x: ^c)) = x
    static member inline _3 ((_: ^a, _: ^b, x: ^c, _: ^d)) = x

    static member inline _3 (struct (_: ^a, _: ^b, x: ^c)) = x
    static member inline _3 (struct (_: ^a, _: ^b, x: ^c, _: ^d)) = x

    static member inline _4 ((_: ^a, _: ^b, _: ^c, x: ^d)) = x

    static member inline _4 (struct (_: ^a, _: ^b, _: ^c, x: ^d)) = x

    static member inline _X ((a: ^a, b: ^b)) = struct (a, b)
    static member inline _X ((a: ^a, b: ^b, c: ^c)) = struct (a, b, c)
    static member inline _X ((a: ^a, b: ^b, c: ^c, d: ^d)) = struct (a, b, c, d)

    static member inline _X (struct (a: ^a, b: ^b)) = (a, b)
    static member inline _X (struct (a: ^a, b: ^b, c: ^c)) = (a, b, c)
    static member inline _X (struct (a: ^a, b: ^b, c: ^c, d: ^d)) = (a, b, c, d)