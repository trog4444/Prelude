namespace Prelude


/// Module containing function combinators.
module Combinator =

    /// Lambda calculus combinators.
    module Lambda =

        /// S combinator == 'ap' function for the function monad
        ///
        /// Applies an inner function to an input value,
        /// then applies an outer function to the original input and the result of the inner function.
        let inline S outer inner a = outer a (inner a)

        /// K combinator == 'const' function
        ///
        /// convert a value into a single argument function that ignores its argument.
        let inline K x (_: ^``_``) = x

        /// Y combinator - used to make recursive functions out of (generally) non-recursive functions.
        ///
        /// Ex: factorial n == Y (fun k (n, r) -> if n <= 0 then r else k (n - 1, r * n)) (5, 1) => 120
        let inline Y (f: (^a -> ^b) -> ^a -> ^b) (x: ^a) : ^b =
            let rec go a = f go a in go x


    /// Generic function combinators.
    module Function =

        /// Flip the first two arguments of a given function.
        let inline flip f b a = f a b

        /// Compose a binary function with another function.
        let inline compose2 fOf2 f1 a b = f1 (fOf2 a b)

        /// Compose a ternary function with another function.
        let inline compose3 fOf3 f1 a b c = f1 (fOf3 a b c)

        /// Apply the same result producing function to two inputs and combine their results
        /// with a third final function.
        let inline on inner outer a1 a2 = outer (inner a1) (inner a2)

        /// Apply the two different result producing functions to one input and combine their results
        /// with a third final function.
        let inline over inner1 inner2 outer a = outer (inner1 a) (inner2 a)

        /// Combine a binary function with a unary function and a value to produce a unary function.
        let inline scan fOf2 f1 s = fOf2 s << f1

        /// Modifies higher order functions to take different arguments;
        /// e.g. for Seq functions the input sequences are replaced by sequences of functions and a constant input that's applied in each iteration.
        let inline swing f c a = f ((|>) a) c

        /// Make a function infix; i.e. have its first argument come before the function.
        let inline infix a f b = f a b

        /// Repeatedly apply a function to a value until the process terminates.
        let inline loop f seed =
            let rec go a = match f a with Some s -> go s | None -> a in go seed

        /// Run two functions simultaneously -- where both functions share a (locally) mutable variable.
        /// Returns the final result when either function returns a 'None' value.
        let inline sync f g seedF seedG (seedZ: ^z) =
            let z = ref seedZ
            let rec go a b =
                match a with
                | None   -> !z
                | Some a ->
                    match b with
                    | None   -> !z
                    | Some b -> go (f z a) (g z b)                    
            match f z seedF with
            | None -> !z
            | Some _ as a -> go a (g z seedG)

        /// Add a callback to any function.
        let inline wcallback callback f a =
            let r = f a in callback a r ; r