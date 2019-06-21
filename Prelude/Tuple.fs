namespace Prelude

///// Operations related to tuples.
//module Tuple =


//    /// Functor-map over an element of a 2-tuple.
//    let inline map (a, b) = a, f b

//    /// Replace an element in a tuple with another value.
//    let inline replace x (a, _) = a, x

//    /// Create a new tuple with a single value with unit as the first element.
//    let inline pack x = ((), x)

//    /// Apply functions inside tuples to values in other tuples.
//    let inline ap (a, f) (b, v) = (a + b + LanguagePrimitives.GenericZero, f v)

//    /// Create a new tuple, ignoring the results of the previous tuple.
//    let inline andthen b a = ap (fun _ -> b) a

//    /// Create a new tuple based on values in the previous tuple.
//    let inline bind f (a, b) = let a', c = f b in (a + a' + LanguagePrimitives.GenericZero, c)

//    /// Flatten a nested tuple.
//    let inline join x = bind id join

//    /// Compose two tuple-producing functions.
//    let inline mcompose g f = f >> bind g

//    /// Return a value held in a tuple.
//    let inline extract (_, x) = x

//    /// Generate a new tuple, keeping the first element and generating a value based on a function applied to the original tuple.
//    let inline extend f ((a, _) as p) = a, f p

//    /// Copy a tuple - keeping the first element in place and putting a value generated from the entire original tuple into the second position.
//    let inline duplicate ((a, _) as p) = (a, p)

//    /// Compose two tuple/comonad producing functions.
//    wcompose g f = extend f >> g

//    /// Swap elements of a pair.
//    let inline swap (a, b) = b, a

//    /// Map both elements of a pair.
//    let inline bimap f g (x, y) = f x, g y

//    /// Map a function over the first element of a pair.
//    let inline first f (x, y) = f x, y

//    /// Map a function over the second element of a pair.
//    let inline second f (x, y) = x, f y

//    /// Create a 2-tuple from a seed value and a generator function.
//    let inline unfold2 f s = let a = f s in a, f a

//    /// Create a 3-tuple from a seed value and two generator functions.
//    let inline unfold3 f s = let a = f s in let b = f a in a, b, f b

//    /// Create a 4-tuple from a seed value and three generator functions.
//    let inline unfold4 f s = let a = f s in let b = f a in let c = f b in a, b, c, f c

//    /// Combine two items into a pair.
//    let inline pair a b = a, b

//    /// Combine three items into a triple.
//    let inline triple a b c = a, b, c

//    /// Combine four items into a quad.
//    let inline quad a b c d = a, b, c, d

//    /// Turn a value into a pair using two functions.
//    let inline fork2 f g x = f x, g x

//    /// Turn a value into a triple using three functions.
//    let inline fork3 f g h x = f x, g x, h x

//    /// Turn a value into a quad using four functions.
//    let inline fork4 f g h i x = f x, g x, h x, i x

//    /// Apply a function over each element in a pair.
//    let inline over2 f (a, b) = f a, f b

//    /// Apply a function over each element in a triple.
//    let inline over3 f (a, b, c) = f a, f b, f c

//    /// Apply a function over each element in a quad.
//    let inline over4 f (a, b, c, d) = f a, f b, f c, f d

//    /// Create a new pair using separate functions on each item.
//    let inline alter2 f1 f2 (a, b) = f1 a, f2 b

//    /// Create a new 3-tuple using separate functions on each item.
//    let inline alter3 f1 f2 f3 (a, b, c) = f1 a, f2 b, f3 c

//    /// Create a new 4-tuple using separate functions on each item.
//    let inline alter4 f1 f2 f3 f4 (a, b, c, d) =
//        f1 a, f2 b, f3 c, f4 d

//    /// Convert an operation on two arguments into an operation on a pair.
//    let inline curry2 f (a, b) = f a b

//    /// Convert an operation on three arguments into an operation on a triple.
//    let inline curry3 f (a, b, c) = f a b c

//    /// Convert an operation on four arguments into an operation on a quad.
//    let inline curry4 f (a, b, c, d) = f a b c d
    
//    /// Convert an operation on two arguments into an operation on a pair.
//    let inline uncurry2 f a b = f (a, b)

//    /// Convert an operation on three arguments into an operation on a 3-tuple.
//    let inline uncurry3 f a b c = f (a, b, c)

//    /// Convert an operation on four arguments into an operation on a 4-tuple.
//    let inline uncurry4 f a b c d = f (a, b, c, d)

//    /// Combine two pairs into a single pair.
//    let inline combine2 f1 f2 (a1, b1) (a2, b2) =
//        f1 a1 a2, f2 b1 b2

//    /// Combine 3 triples into a single triple.
//    let inline combine3 f1 f2 f3 (a1, b1, c1) (a2, b2, c2) (a3, b3, c3) = 
//        f1 a1 a2 a3, f2 b1 b2 b3, f3 c1 c2 c3

//    /// Combine 4 quads into a single quad.
//    let inline combine4 f1 f2 f3 f4
//        (a1, b1, c1, d1) (a2, b2, c2, d2) (a3, b3, c3, d3) (a4, b4, c4, d4) =
//        f1 a1 a2 a3 a4, f2 b1 b2 b3 b4, f3 c1 c2 c3 c4, f4 d1 d2 d3 d4

//    /// Convert a struct pair to an object pair.
//    let inline ofStruct (struct (a, b)) = a, b

//    /// Convert a struct pair to an object pair.
//    let inline toStruct (a, b) = struct (a, b)
    

//    /// Module for struct tuples.
//    module Struct =

//        /// Functor-map over an element of a 2-tuple.
//        let inline map f (struct (a, b)) = struct (a, f b)

//        /// Replace an element in a tuple with another value.
//        let inline replace x (struct (a, _)) = struct (a, x)

//        /// Create a new tuple with a single value with unit as the first element.
//        let inline pack x = struct ((), x)

//        /// Apply functions inside tuples to values in other tuples.
//        let inline ap (struct (b, v)) (struct (a, f)) = struct (a + b + LanguagePrimitives.GenericZero, f v)

//        /// Create a new tuple, ignoring the results of the previous tuple.
//        let inline andthen b a = ap b (map (fun _ b -> b) a)

//        /// Create a new tuple based on values in the previous tuple.
//        let inline bind f (struct (a, b)) = let (struct (a', c)) = f b in struct (a + a' + LanguagePrimitives.GenericZero, c)

//        /// Flatten a nested tuple.
//        let inline join x = bind id x

//        /// Compose two tuple-producing functions.
//        let inline mcompose g f = f >> bind g

//        /// Return a value held in a tuple.
//        let inline extract (struct (_, x)) = x

//        /// Generate a new tuple, keeping the first element and generating a value based on a function applied to the original tuple.
//        let inline extend f (struct (a, _) as p) = struct (a, f p)

//        /// Copy a tuple - keeping the first element in place and putting a value generated from the entire original tuple into the second position.
//        let inline duplicate x = extend id x

//        /// Compose two tuple/comonad producing functions.
//        let inline wcompose g f = extend f >> g

//        /// Extract the first element of a 2 tuple.
//        let inline fst (struct (a, _)) = a

//        /// Extract the second element of a 2 tuple.
//        let inline snd (struct (_, b)) = b

//        /// Swap elements of a pair.
//        let inline swap (struct (a, b)) = struct (b, a)

//        /// Map both elements of a pair.
//        let inline bimap f g (struct (x, y)) = struct (f x, g y)

//        /// Map a function over the first element of a pair.
//        let inline first f (struct (x, y)) = struct (f x, y)

//        /// Map a function over the second element of a pair.
//        let inline second f (struct (x, y)) = struct (x, f y)

//        /// Create a 2-tuple from a seed value and a generator function.
//        let inline unfold2 f s = let a = f s in struct (a, f a)

//        /// Create a 3-tuple from a seed value and two generator functions.
//        let inline unfold3 f s = let a = f s in let b = f a in struct (a, b, f b)

//        /// Create a 4-tuple from a seed value and three generator functions.
//        let inline unfold4 f s = let a = f s in let b = f a in let c = f b in struct (a, b, c, f c)

//        /// Combine two items into a pair.
//        let inline pair a b = struct (a, b)

//        /// Combine three items into a triple.
//        let inline triple a b c = struct (a, b, c)

//        /// Combine four items into a quad.
//        let inline quad a b c d = struct (a, b, c, d)

//        /// Turn a value into a pair using two functions.
//        let inline fork2 f g x = struct (f x, g x)

//        /// Turn a value into a triple using three functions.
//        let inline fork3 f g h x = struct (f x, g x, h x)

//        /// Turn a value into a quad using four functions.
//        let inline fork4 f g h i x = struct (f x, g x, h x, i x)

//        /// Apply a function over each element in a pair.
//        let inline over2 f (struct (a, b)) = struct (f a, f b)

//        /// Apply a function over each element in a triple.
//        let inline over3 f (struct (a, b, c)) = struct (f a, f b, f c)

//        /// Apply a function over each element in a quad.
//        let inline over4 f (struct (a, b, c, d)) = struct (f a, f b, f c, f d)

//        /// Create a new pair using separate functions on each item.
//        let inline alter2 f1 f2 a b = struct (f1 a, f2 b)

//        /// Create a new 3-tuple using separate functions on each item.
//        let inline alter3 f1 f2 f3 (struct (a, b, c)) =
//            struct (f1 a, f2 b, f3 c)

//        /// Create a new 4-tuple using separate functions on each item.
//        let inline alter4 f1 f2 f3 f4 (struct (a, b, c, d)) =
//            struct (f1 a, f2 b, f3 c, f4 d)

//        /// Convert an operation on two arguments into an operation on a pair.
//        let inline curry2 f (struct (a, b)) = f a b

//        /// Convert an operation on three arguments into an operation on a triple.
//        let inline curry3 f (struct (a, b, c)) = f a b c

//        /// Convert an operation on four arguments into an operation on a quad.
//        let inline curry4 f (struct (a, b, c, d)) = f a b c d
    
//        /// Convert an operation on two arguments into an operation on a pair.
//        let inline uncurry2 f a b = f (struct (a, b))

//        /// Convert an operation on three arguments into an operation on a 3-tuple.
//        let inline uncurry3 f a b c = f (struct (a, b, c))

//        /// Convert an operation on four arguments into an operation on a 4-tuple.
//        let inline uncurry4 f a b c d = f (struct (a, b, c, d))

//        /// Combine two pairs into a single pair.
//        let inline combine2 f1 f2 (struct (a1, b1)) (struct (a2, b2)) =
//            struct (f1 a1 a2, f2 b1 b2)

//        /// Combine 3 triples into a single triple.
//        let inline combine3 f1 f2 f3
//            (struct (a1, b1, c1)) (struct (a2, b2, c2)) (struct (a3, b3, c3)) = 
//            struct (f1 a1 a2 a3, f2 b1 b2 b3, f3 c1 c2 c3)

//        /// Combine 4 quads into a single quad.
//        let inline combine4 f1 f2 f3 f4
//            (struct (a1, b1, c1, d1)) (struct (a2, b2, c2, d2))
//            (struct (a3, b3, c3, d3)) (struct (a4, b4, c4, d4)) =
//            struct (f1 a1 a2 a3 a4, f2 b1 b2 b3 b4, f3 c1 c2 c3 c4, f4 d1 d2 d3 d4)

//        /// Convert a pair to a tuple pair.
//        let inline ofPair (a, b) = struct (a, b)

//        /// Convert a struct pair to a tuple pair.
//        let inline toPair (struct (a, b)) = a, b