namespace Prelude

/// Module containing functions dealing with strings.
module Strings =


// ====================================================================================================


    /// Split a string based on different options.
    module Split =

        /// Synonymn for System.StringSplitOptions.
        type SplitOptions = System.StringSplitOptions

        /// Split a string by a given char.
        let inline byChar (delimiter: char) (text: string) =
            text.Split([|delimiter|], SplitOptions.None)

        /// Split a string by the given chars.
        let inline byChars (delimiters: char []) (text: string) =
            text.Split(delimiters, SplitOptions.None)

        /// Split a string by a given char with the given split options.
        let inline byCharW (delimiter: char) (options: SplitOptions) (text: string) =
            text.Split([|delimiter|], options)

        /// Split a string by the given chars with the given split options.
        let inline byCharsW (delimiters: char []) (options: SplitOptions) (text: string) =
            text.Split(delimiters, options)

        /// Lazily split a string by a char. Faster when only the first few elements are needed but slower if all elements are used.
        let inline byCharL (delimiter: char) (text: string) =
            let delimiter = string delimiter
            match delimiter, text with
            | (_, (null | "")) -> Seq.empty
            | ((null | ""), _) -> Seq.singleton text
            | _ ->
              let tLen = text.Length
              let dLen = delimiter.Length
              let rec loop pos = 
                seq {
                  if pos < tLen then
                    let index = match text.IndexOf(delimiter, pos) with -1 -> tLen | i -> i
                    yield text.Substring(pos, index - pos)
                    yield! loop (index + dLen) }
              Seq.cache (loop 0)

        /// Lazily split a string by a char using the given split options. Faster when only the first few elements are needed but slower if all elements are used.
        let inline byCharNoEmptyL (delimiter: char) (text: string) =
            let delimiter = string delimiter            
            match delimiter, text with
            | (_, (null | "")) -> Seq.empty
            | ((null | ""), _) -> Seq.singleton text
            | _ ->
              let tLen  = text.Length
              let dLen  = delimiter.Length
              let rec loop pos = seq {
                  if pos < tLen then
                    let i = match text.IndexOf(delimiter, pos) with -1 -> tLen | i -> i
                    let sub = text.Substring(pos, i - pos)
                    yield sub
                    yield! loop (i + dLen) }
              Seq.cache (Seq.filter ((<>) "") (loop 0))

        /// Split a string by a string.
        let inline byStr (delimiter: string) (text: string) =
            text.Split([|delimiter|], SplitOptions.None)

        /// Split a string by the given strings.
        let inline byStrs (delimiters: string []) (text: string) =
            text.Split(delimiters, SplitOptions.None)

        /// Split a string by a string with the given split options.
        let inline byStrW (delimiter: string) (options: SplitOptions) (text: string) =
            text.Split([|delimiter|], options)

        /// Split a string by the given strings with the given split options.
        let inline byStrsW (delimiters: string []) (options: SplitOptions) (text: string) =
            text.Split(delimiters, options)

        /// Lazily split a string by a string. Faster when only the first few elements are needed but slower if all elements are used.
        let inline byStrL (delimiter: string) (text: string) =
            match delimiter, text with
            | (_, (null | "")) -> Seq.empty
            | ((null | ""), _) -> Seq.singleton text
            | _ ->
              let tLen = text.Length
              let dLen = delimiter.Length
              let rec loop pos = seq {
                  if pos < tLen then
                    let index = match text.IndexOf(delimiter, pos) with -1 -> tLen | i -> i
                    yield text.Substring(pos, index - pos)
                    yield! loop (index + dLen) }
              loop 0

        /// Lazily split a string by a string using the given split options. Faster when only the first few elements are needed but slower if all elements are used.
        let inline byStrNoEmptyL (delimiter: string) (text: string) =            
            match delimiter, text with
            | (_, (null | "")) -> Seq.empty
            | ((null | ""), _) -> Seq.singleton text
            | _ ->
              let tLen  = text.Length
              let dLen  = delimiter.Length
              let rec loop pos = 
                seq {
                  if pos < tLen then
                    let i = match text.IndexOf(delimiter, pos) with -1 -> tLen | i -> i
                    let sub = text.Substring(pos, i - pos)
                    yield sub
                    yield! loop (i + dLen) }
              Seq.filter ((<>) "") (loop 0)
    

// ====================================================================================================


    /// Compare a string based on different options.
    module Compare =

        /// Synonym for System.StringComparison.
        type CompareOptions = System.StringComparison

        /// Test if two strings are equal. Case insensitive.
        let inline equals str1 str2 =
            0 = System.String.Compare(str1, str2, true)

        /// Test if two strings are equal. Case sensitive.
        let inline equalsByCase str1 str2 =
            0 = System.String.Compare(str1, str2, false)

        /// Test if two strings are equal using the given comparison options.
        let inline equalsWith (options: CompareOptions) str1 str2 =
            0 = System.String.Compare(str1, str2, options)

        /// Returns the string that is greater. Case sensitive.
        let inline proceeds str1 str2 =
            match str1 with
            | (null | "") -> str2
            | _ ->
                match System.String.Compare(str1, str2, false) with
                | i when i < 0 -> str1
                | _            -> str2
              
        /// Returns the string that is lesser. Case sensitive.
        let inline follows str1 str2 =
            match str1 with
            | (null | "") -> str2
            | _ ->
                match System.String.Compare(str1, str2, false) with
                | i when i > 0 -> str1
                | _            -> str2
 
        /// Returns the string that is greater than the other using default F# comparison operator.
        let inline greater (str1: string) (str2: string) =
            if str1 > str2 then str1 else str2

        /// Returns the string that is lesser than the other using default F# comparison operator.
        let inline lesser (str1: string) (str2: string) =
            if str1 < str2 then str1 else str2

        /// Slightly optimized string sorting function.
        let inline quickSort (input: string[]) : string [] =
            let inline cmp (depth: int) (piv: string) (str: string) =
                match str.Length, piv.Length with
                | s, t when s = depth && t = depth -> 0                
                | s, _ when s = depth -> -1
                | _, t when t = depth -> 1
                | _ -> match str.[depth], piv.[depth] with
                        | s, t when s < t -> -1
                        | s, t when s = t -> 0
                        | _ -> 1
            let rec go (xs: string []) depth =
                if Array.isEmpty xs then xs else            
                    let inline cmpr f (ys: string []) = Array.partition (cmp depth (ys.[ys.Length / 2]) >> f) ys
                    //in reality the following filters are done in place in two passes
                    //let less = filter(fun s -> cmp(depth, s, pivot) < 0) input //  s < pivot
                    //let eq = filter(fun s -> cmp(depth, s, pivot) = 0) input   //  s = pivot
                    //let gt = filter(fun s -> cmp(depth, s, pivot) > 0) input   //  s > pivot
                
                    let lt, eq, gt = match cmpr ((>) 0) xs with
                                     | ls, gtoet -> match cmpr ((=) 0) gtoet with
                                                    | eq, gt -> ls, eq, gt
                    let sortedLT = go lt depth
                    let sortedEQ = if eq.Length > 0 && depth < eq.[0].Length then go eq (depth + 1) else eq
                    let sortedGT = go gt depth
                    //in reality the algorithm updates the same array and					
                    //recursively passes from and to indices to define subarrays
                    Array.concat [|sortedLT; sortedEQ; sortedGT|]
            match input with
            | null -> Array.empty
            | _    -> go (Array.filter ((<>) null) input) 0        

        /// Fast string sorting when strings have only a few characters or there are many duplicate entries.
        let inline sortSmall (xs: string seq) : string seq =
            match xs with
            | null -> Seq.empty
            | _ -> (Seq.groupBy id
                        >> Seq.sortBy fst
                        >> Seq.collect (fun (k, vs) -> match k with null -> Seq.empty | _ -> vs))
                     xs

        tests:
        //with this string setup the built in is faster
        let r = System.Random()
        let xs = Array.map string (Array.init 1000000 (fun _ -> r.Next(0, 10000000)))
        let t = System.Diagnostics.Stopwatch.StartNew()
        let a = sortSmall xs |> Seq.toArray
        let ta = t.ElapsedMilliseconds       
        t.Restart()
        let b = quickSort xs
        let tb = t.ElapsedMilliseconds
        t.Restart()
        let c = Array.sort (Array.filter ((<>) null) xs)
        let tc = t.ElapsedMilliseconds
        let lena, lenb, lenc = a.Length, b.Length, c.Length
        let ab, ac, bc, sorter_, quicksort, natsort = (a = b), (a = c), (b = c), ta, tb, tc
        //let same, timeSerial, timePar, timeNat = (a = b && a = c), ta, tb, tc
        //let matches = a=b, a=c, b=c

    /// Convert a sequence of values into text, each value on a new line.
    let inline toLines (toString: ^a -> string) (items: ^a seq) =
        match items with
        | null -> ""
        | _ -> let sb = System.Text.StringBuilder()
               let inline append x = ignore (sb.AppendLine(toString x))
               match items with
               | :? array<'a> as xs -> Array.iter append xs
               | :? list<'a>  as xs -> List.iter  append xs
               | xs                 -> Seq.iter   append xs
               sb.ToString()

    /// Convert a sequence of values into text.
    let inline toText (toString: ^a -> string) (items: ^a seq) =
        match items with
        | null -> ""
        | _ -> let sb = System.Text.StringBuilder()
               let inline append x = ignore (sb.Append(toString x))
               match items with
               | :? array<'a> as xs -> Array.iter append xs
               | :? list<'a>  as xs -> List.iter  append xs
               | xs                 -> Seq.iter   append xs
               sb.ToString()

    /// Delimit a sequence with a delimiter.
    let inline delimit (delimiter: string) (toString: ^a -> string) (items: ^a seq) =
        match Seq.cache items with
        | null -> ""
        | xs when Seq.isEmpty xs -> ""
        | xs -> let sb = System.Text.StringBuilder()
                let inline append x = ignore (sb.Append(delimiter + toString x))
                ignore (sb.Append(toString (Seq.head xs)))
                Seq.iter append (Seq.tail xs)
                sb.ToString()

    /// Returns true if the source string contains the substring.
    let inline contains (substring: string) (source: string) = source.Contains substring

    /// Counts instances of a character in a string.
    let inline countChar char (text: string) =
        Seq.sumBy (fun c -> if c = char then 1 else 0) text


// ====================================================================================================


    /// Replaces each instance of a target string with a replacement string in a source string.
    module Replace =
        
        /// Replaces each instance of a target string with a replacement string.
        let inline withStr (target: string) (replacement: string) (source: string) =
            source.Replace(target, replacement) 
        
        /// Replaces each instance of a target character with a replacement character.
        let inline charWChar (target: char) (replacement: char) (source: string) =
            source.Replace(target, replacement)

        /// Replaces each instance of a target character with a replacement string.
        let inline charWStr (target: char) (replacement: string) (source: string) =
            (System.Text.StringBuilder() |>
             Seq.fold
                (fun sb c -> if c = target
                             then sb.Append(replacement)
                             else sb.Append(c))
            <| source).ToString()

    /// Replace escape strings with their literal string representation.
    let inline unescapeChars (str: string) =
        let rec go (sb: System.Text.StringBuilder) = function
        | i when i < str.Length ->
            go
                (match str.[i] with
                | '\'' -> sb.Append "\\\'" // allow to enter a ' in a character literal.
                | '\"' -> sb.Append "\\\"" // allow to enter a " in a string literal.
                | '\\' -> sb.Append "\\"   // allow to enter a \ character in a character or string literal.
                | '\a' -> sb.Append "\\a"  // alarm (usually the HW beep).
                | '\b' -> sb.Append "\\b"  // back-space.
                | '\f' -> sb.Append "\\f"  // form-feed (next page).
                | '\n' -> sb.Append "\\n"  // line-feed (next line).
                | '\r' -> sb.Append "\\r"  // carriage-return (move to the beginning of the line).                      
                | '\t' -> sb.Append "\\t"  // horizontal tab.                      
                | '\v' -> sb.Append "\\v"  // vertical-tab.
                | c    -> sb.Append c)     // normal char.
                (i + 1)
        | _ -> sb
        (go (System.Text.StringBuilder(str.Length)) 0).ToString()

    /// Replace literal escape strings with their escaped representation.
    let inline escapeChars (str: string) =
        let len = str.Length
        let rec go (sb: System.Text.StringBuilder) = function
        | i when i < len ->
            if i + 1 < len
            then match str.[i..i + 1] with
                 | "\\'"  -> go (sb.Append '\'') (i + 2)    // allow to enter a ' in a character literal.
                 | "\\\"" -> go (sb.Append '\"') (i + 2)    // allow to enter a " in a string literal.
                 | "\\\\" -> go (sb.Append '\\') (i + 2)    // allow to enter a \ character in a character or string literal.
                 | "\\a"  -> go (sb.Append '\a') (i + 2)    // alarm (usually the HW beep).
                 | "\\b"  -> go (sb.Append '\b') (i + 2)    // back-space.
                 | "\\f"  -> go (sb.Append '\f') (i + 2)    // form-feed (next page).
                 | "\\n"  -> go (sb.Append '\n') (i + 2)    // line-feed (next line).
                 | "\\r"  -> go (sb.Append '\r') (i + 2)    // carriage-return (move to the beginning of the line).                      
                 | "\\t"  -> go (sb.Append '\t') (i + 2)    // horizontal tab.                      
                 | "\\v"  -> go (sb.Append '\v') (i + 2)    // vertical-tab.
                 | _      -> go (sb.Append str.[i]) (i + 1) // normal char.
            else sb.Append str.[i]                          // normal char.
        | _ -> sb
        (go (System.Text.StringBuilder(len)) 0).ToString()


    /// Returns true if the main text begins with the substring.
    let inline startsWith (start: string) (text: string) =
        if start.Length > text.Length
        then false
        else text.[0..start.Length-1] = start

    /// Returns true if the main text ends with the substring.
    let inline endsWith (``end``: string) (text: string) =
        let elen, tlen = ``end``.Length, text.Length
        if elen > tlen
        then false
        else text.[tlen - elen..] = ``end``


// ====================================================================================================


    /// Computation expression for StringBuilder type.
    module StrBuilder =

        (* Original StringBuilder uses System.Text, so we must use SB as new type and Text.StringBuilder *)

        open System

        /// Wrapper type for StringBuilder type used for lightweight computation expressions.
        [<Struct>]
        type StrBuilder
            = private SB of (Text.StringBuilder -> unit) with
                override me.ToString() =
                    let sb = Text.StringBuilder()
                    match me with SB f -> f sb
                    sb.ToString()

        /// Short for StringBuilders's .ToString method.
        let inline eval (SB f) =
            let sb = Text.StringBuilder()
            f sb
            sb.ToString()

        /// Type used to determine which type of Append to use in a yield expression.
        [<Struct>]
        type AppendType = Append | AppendLine

        /// StringBuilder computation expression type.
        type SBBuilder () =

            member inline private __.run (SB f) = f
            member inline private __.append (txt: string) t = SB(fun sb -> ignore (match t with Append -> sb.Append txt | AppendLine -> sb.AppendLine txt))
            member inline __.Yield (txt: string) = SB(fun sb -> ignore (sb.Append txt))
            member inline me.Yield ((txt, t)) = me.append txt t
            member inline me.Yield ((a, t: AppendType)) = let txt = sprintf "%A" a in me.append txt t
            member inline me.Yield a = let txt = sprintf "%A" a in me.append txt Append
            member inline __.YieldFrom m = m : StrBuilder
            member inline me.Delay f = SB(fun sb -> me.run (f()) sb)
            member inline __.Zero () = SB(fun _ -> ())
            member inline me.For (xs, f) = SB (fun sb -> for x in xs do me.run (f x) sb)
            member inline me.While (p, f) = SB (fun sb -> while p () do me.run f sb)            
            member inline __.Combine((SB f), (SB g)) = SB(fun sb -> f sb; g sb)
            
            [<CustomOperation("append", MaintainsVariableSpace=true, AllowIntoPattern=true)>]
            member inline me.Append (SB f, txt) = SB(fun sb -> f sb; ignore (me.append txt Append))
            
            [<CustomOperation("appendline", MaintainsVariableSpace=true, AllowIntoPattern=true)>]
            member inline me.AppendLine (SB f, txt) = SB(fun sb -> f sb; ignore (me.append txt AppendLine))

        /// Builds a StringBuilder computation expression.
        let sb = SBBuilder ()