namespace Prelude


/// Common functions.
module Common =

    /// Functions based on statically called methods.
    module Method =

        /// Convert an item to its string representation.
        let inline show item = sprintf "%A" item

        /// Return the length of any type with a .Length property.
        let inline len (item: ^a) = (^a : (member Length : ^b) item)
    
        /// Return the count of any type with a .Count() method.
        let inline count (item: ^a) = (^a : (member Count : unit -> ^b) item)

        /// Retrieve the value for a type with a .Value property.
        let inline value (item: ^a) = (^a : (member Value : ^b) item)

        /// Force an action on an item.
        let inline force a = (^a: (member Force: unit -> ^b) a)

        /// Dispose of an item.
        let inline dispose (a: #System.IDisposable) = a.Dispose()

        /// Reset an item and then return it.
        let inline reset r = (^a: (member Reset: unit -> unit) r) ; r


    /// Optimization functions such as functions that cache (memoize) their result(s).
    module Optimize =

        /// Memoize a function.
        let inline memo (f: ^a -> ^b) =
            let c = System.Collections.Concurrent.ConcurrentDictionary< ^a, ^b>(HashIdentity.Structural)
            let r = ref Unchecked.defaultof< ^b>
            fun w -> if c.TryGetValue(w, r) then !r else c.GetOrAdd(key = w, value = f w)

        /// Memoize a binary function.
        let inline memo2 f =
            let c = System.Collections.Concurrent.ConcurrentDictionary<struct( ^a * ^b), ^c>(HashIdentity.Structural)
            let r = ref Unchecked.defaultof< ^c>
            fun w x -> let p = struct (w, x)
                       if c.TryGetValue(p, r) then !r else c.GetOrAdd(key = p, value = f w x)

        /// Memoize a ternary function.
        let inline memo3 f =
            let c = System.Collections.Concurrent.ConcurrentDictionary<struct( ^a * ^b * ^c), ^d>(HashIdentity.Structural)
            let r = ref Unchecked.defaultof< ^d>
            fun w x y -> let p = struct (w, x, y)
                         if c.TryGetValue(p, r) then !r else c.GetOrAdd(key = p, value = f w x y)

        /// Memoize a quaternary function.
        let inline memo4 f =
            let c = System.Collections.Concurrent.ConcurrentDictionary<struct( ^a * ^b * ^c * ^d), ^e>(HashIdentity.Structural)
            let r = ref Unchecked.defaultof< ^e>
            fun w x y z -> let p = struct (w, x, y, z)
                           if c.TryGetValue(p, r) then !r else c.GetOrAdd(key = p, value = f w x y z)


    /// Functions whose values depend on computational circumstances (e.g. choice, errors, etc).
    module Predicated =

        /// Function version of an if/else statement.
        let inline ifElse predicate then_ else_ x =
            match predicate x with
            | true  -> then_ x
            | false -> else_ x

        /// Function version of a complete try/finally block.
        let inline tryF f finalizer input =        
            try f input finally finalizer input

        /// Function version of a complete try/with block.
        let inline tryW f handler input =        
            try Ok (f input) with e -> Error (handler e input)

        /// Function version of a complete try/with/finally block.
        let inline tryWF f handler finalizer input =        
            try try Ok (f input) with e -> Error (handler e input)
            finally finalizer input


    /// Conversion functions between 'FSharpFuncs' and 'System.Funcs'.
    module Interop =
        
        /// Convert an FSharpFunc to a generic .Net Func.
        let inline toFunc1 f = System.Func<_, _>f

        /// Convert an FSharpFunc to a generic .Net Func.
        let inline toFunc2 f = System.Func<_, _, _>f

        /// Convert an FSharpFunc to a generic .Net Func.
        let inline toFunc3 f = System.Func<_, _, _, _>f

        /// Convert an FSharpFunc to a generic .Net Func.
        let inline toFunc4 f = System.Func<_, _, _, _, _>f

        /// Convert an FSharpFunc to a generic .Net Func.
        let inline toFunc5 f = System.Func<_, _, _, _, _, _>f

        /// Convert a .Net Func to a generic FSharpFunc.
        let inline ofFunc1 (f: System.Func<_, _>) a = f.Invoke(a)

        /// Convert a .Net Func to a generic FSharpFunc.
        let inline ofFunc2 (f: System.Func<_, _, _>) a b = f.Invoke(a, b)

        /// Convert a .Net Func to a generic FSharpFunc.
        let inline ofFunc3 (f: System.Func<_, _, _, _>) a b c = f.Invoke(a, b, c)

        /// Convert a .Net Func to a generic FSharpFunc.
        let inline ofFunc4 (f: System.Func<_, _, _, _, _>) a b c d = f.Invoke(a, b, c, d)