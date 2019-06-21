namespace Prelude

/// General use Infix and Prefix operators.
module Operators =



(* ==================================================================================================== *)


    (* Combinators *)

    /// Make a function infix; i.e. have its first argument come before the function.
    ///
    /// Ex: ~~ 10 (/) 5 = 2
    let inline (~~) a = fun f b -> f a b

    /// Function composition where the second function has flipped arguments.
    let inline (<~<) f g = fun a b -> f (g b a)

    /// Function composition where the first function has flipped arguments.
    let inline (>~>) f g = fun a b -> g (f b a)

    /// Right-associative version of function application.
    let inline (^*) f x = f x

    /// Right-associative version of function composition.
    let inline (^.) x f = f x

60 - 3 * 16 = 60 - 48 = 12
16, 16, 12

16 s
15 s
3 s
 

(* ==================================================================================================== *)


    (* Tuples *)

    /// Apply a function to four values, the values being a 4-tuple on the right.
    let inline (||||>) (a, b, c, d) f = f a b c d

    /// Apply a function to four values, the values being a 4-tuple on the left.
    let inline (<||||) f (a, b, c, d) =  f a b c d 

    /// Convert a function on a pair into a function on two arguments.
    let inline (!!>) f = fun a b -> f (a, b)

    /// Convert a function on a 3-tuple into a function on three arguments.
    let inline (!!!>) f = fun a b c -> f (a, b, c)

    /// Convert a function on a 4-tuple into a function on four arguments.
    let inline (!!!!>) f = fun a b c d -> f (a, b, c, d)


    (* Struct Tuples *)


    /// Convert a function on a pair into a function on two arguments.
    let inline (!|>) f = fun a b -> f (struct (a, b))

    /// Convert a function on a 3-tuple into a function on three arguments.
    let inline (!||>) f = fun a b c -> f (struct (a, b, c))

    /// Convert a function on a 4-tuple into a function on four arguments.
    let inline (!|||>) f = fun a b c d -> f (struct (a, b, c, d))

    /// Convert a function of two arguments into a function on a pair.
    let inline (<|!) f = fun (struct (a, b)) -> f a b

    /// Convert a function of three arguments into a function on a 3-tuple.
    let inline (<||!) f = fun (struct (a, b, c)) -> f a b c

    /// Convert a function of four arguments into a function on a 4-tuple.
    let inline (<|||!) f = fun (struct (a, b, c, d)) -> f a b c d