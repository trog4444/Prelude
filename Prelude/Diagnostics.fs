namespace Prelude


/// Functions for diagnostic purposes.
module Diagnostics =



    let xs = seq { 1I..100000000I }
    
    let inline f x = fun y -> x + y

    let inline g x y = x + y

    let inline h x =
        let inline g y = x + y
        g

    let inline i x =
        let inline g () = fun y -> x + y
        g ()

    let t = System.Diagnostics.Stopwatch.StartNew()

    let a = Seq.fold f 0I xs

    let ta = t.ElapsedMilliseconds

    System.GC.Collect()
    System.GC.GetTotalMemory true |> ignore
    System.GC.Collect()

    t.Restart()

    let b = Seq.fold g 0I xs

    let tb = t.ElapsedMilliseconds

    System.GC.Collect()
    System.GC.GetTotalMemory true |> ignore
    System.GC.Collect()

    t.Restart()

    let c = Seq.fold h 0I xs

    let tc = t.ElapsedMilliseconds

    System.GC.Collect()
    System.GC.GetTotalMemory true |> ignore
    System.GC.Collect()

    t.Restart()

    let d = Seq.fold (+) 0I xs

    let td = t.ElapsedMilliseconds

    System.GC.Collect()
    System.GC.GetTotalMemory true |> ignore
    System.GC.Collect()

    t.Restart()

    let e = Seq.sum xs

    let te = t.ElapsedMilliseconds

    System.GC.Collect()
    System.GC.GetTotalMemory true |> ignore
    System.GC.Collect()

    t.Restart()

    let f' = Seq.fold i 0I xs

    let tf = t.ElapsedMilliseconds

    //val a : System.Numerics.BigInteger = 500000000500000000
    //val ta : int64 = 987518L
    //val b : System.Numerics.BigInteger = 500000000500000000
    //val tb : int64 = 1078938L
    //val c : System.Numerics.BigInteger = 500000000500000000
    //val tc : int64 = 952289L


    /// Synonymn for System.Diagnostics.Stopwatch.
    type Timer = System.Diagnostics.Stopwatch

    type [<Measure>] millisecond =
        static member inline Tag x = x * 1<millisecond>
        static member inline Tag x = x * 1L<millisecond>
        static member inline Tag x = x * 1.<millisecond>
        static member inline Tag x = x * 1M<millisecond>
        static member inline Drop x : int = x / 1<millisecond>
        static member inline Drop x : int64 = x / 1L<millisecond>
        static member inline Drop x : float = x / 1.<millisecond>
        static member inline Drop x : decimal = x / 1M<millisecond>

    type [<Measure>] Megabyte =
        static member inline Tag x = x * 1<Megabyte>
        static member inline Tag x = x * 1L<Megabyte>
        static member inline Tag x = x * 1.<Megabyte>
        static member inline Tag x = x * 1M<Megabyte>
        static member inline Drop x : int = x / 1<Megabyte>
        static member inline Drop x : int64 = x / 1L<Megabyte>
        static member inline Drop x : float = x / 1.<Megabyte>
        static member inline Drop x : decimal = x / 1M<Megabyte>

    /// Run a function and return the result along with the Megabytes the function generated.
    let inline withMemory finalize operation input =         
        let m1 = System.GC.GetTotalMemory finalize
        let r = operation input
        let m2 = System.GC.GetTotalMemory finalize
        r, Megabyte.Tag ((m2 - m1) / 1048576L) (*Byte to Megabytesyte conversion*)        

    /// Return the result of a function paired with the time it takes to run.
    let inline runTimed f =
        let t = Timer.StartNew()
        fun x ->
            let r = f x
            t.Stop()
            let p = r, millisecond.Tag t.ElapsedMilliseconds
            t.Reset()
            p

    /// Run an operation with a timer and perform an action with the elapsed time before returning the result.
    let inline time f action =
        let t = Timer.StartNew()
        fun input ->
            let r = f input
            t.Stop()
            action (millisecond.Tag t.ElapsedMilliseconds)
            t.Reset()
            r

    /// Run an operation with a timer and perform an action with the elapsed time, 
    /// the input, and result before returning the result.
    let inline time' f action  =
        let t = Timer.StartNew()
        fun input ->
            let r = f input
            t.Stop()
            action input r (millisecond.Tag t.ElapsedMilliseconds)
            t.Reset()
            r
    
    /// Turn a sequence of operations on the same input into a sequence that runs a shared timer
    /// and performs an action based on the elapsed time once the operation is finished.
    let inline syncTime operations =
        let t = Timer()
        Seq.map (fun (f, action) input ->
            if not t.IsRunning then t.Start()
            let result = f input
            action (millisecond.Tag t.ElapsedMilliseconds)
            result) operations

    /// Turn a sequence of operations on the same input into a sequence that runs a shared timer
    /// and performs an action based on the elapsed time, input, and output once the operation is finished.
    let inline syncTime' operations =
        let t1 = Timer()
        let t2 = Timer()
        Seq.map (fun (f, action) input ->
            if not t1.IsRunning then t1.Start()
            t2.Start()
            let result = f input
            action input result (millisecond.Tag t1.ElapsedMilliseconds) (millisecond.Tag t2.ElapsedMilliseconds)
            t2.Reset()
            result) operations

    /// Turn a sequence of operations on the same input into a sequence that runs a shared timer
    /// and performs an action based on the elapsed time, input, and output once the operation is finished.
    let inline syncTime'' operations =
        let t1 = Timer()
        let t2 = Timer()
        let t3 = Timer()
        Seq.map (fun (f, action) input ->
            if not t1.IsRunning then t1.Start()
            t2.Start()
            t3.Start()
            let result = f input
            t2.Stop()
            t3.Reset()
            action input result (millisecond.Tag t1.ElapsedMilliseconds) (millisecond.Tag t3.ElapsedMilliseconds) (millisecond.Tag t3.ElapsedMilliseconds)
            result) operations

    /// Run a function, perform an action based on the output, and then return the output.
    let inline enact action operation input =
        let r = operation input
        action r
        r

    /// Run a function, perform an action based on the input and output, and then return the output.
    let inline enact' action operation input =
        let r = operation input
        action input r
        r

    /// Pause the console on a command prompt.
    let inline pauseConsole () =
        ignore (System.Console.ReadKey true)

    /// Pause the console on a command prompt and end with an exit code.
    let inline pauseConsoleN exitcode =
        ignore (System.Console.ReadKey true)
        exitcode

    /// During the application of a function, activate the garbage collector to manually reset the memory.
    let inline wreset f x =
        System.GC.Collect()
        let mem0 = (System.GC.GetTotalMemory true)
        System.GC.Collect()
        let r = f x
        System.GC.Collect()
        let mem1 = System.GC.GetTotalMemory true
        System.GC.Collect()
        (mem1 - mem0), r

    module Seqs =

        open System
        open System.Collections
        open System.Collections.Generic
        open System.Diagnostics

        type private TimedEnumerator<'t>(source: IEnumerator< ^t>, blockSize, f) =
            let swUpstream = Stopwatch()
            let swDownstream = Stopwatch()
            let swTotal = Stopwatch()
            let mutable count = 0
            member inline __.Reset() = source.Reset()
            member inline __.Dispose() = source.Dispose()
            member inline __.Current = source.Current
            //member inline __.Current = (source :> IEnumerator).Current
            member inline __.MoveNext() =
                // downstream consumer has finished processing the previous item
                swDownstream.Stop()

                // start the total throughput timer if this is the first item
                if not swTotal.IsRunning then swTotal.Start()

                // measure upstream MoveNext() call
                swUpstream.Start()
                let result = source.MoveNext()
                swUpstream.Stop()

                if result then count <- count + 1

                // blockSize reached or upstream is complete - invoke callback and reset timers
                if (result && count = blockSize) || (not result && count > 0) then
                    swTotal.Stop()
                    let upDownTotal = swUpstream.Elapsed + swDownstream.Elapsed
                    f count swTotal.Elapsed (swUpstream.Elapsed.TotalMilliseconds / upDownTotal.TotalMilliseconds)
                    count <- 0
                    swUpstream.Reset()
                    swDownstream.Reset()
                    swTotal.Restart()

                // clock starts for downstream consumer
                swDownstream.Start()
                result
            interface IEnumerator<'t> with
                member me.Reset() = me.Reset()
                member me.Dispose() = me.Dispose()
                member me.Current = source.Current
                member me.Current = (source :> IEnumerator).Current
                member me.MoveNext() = me.MoveNext()

        let inline private mkTimed blockSize (getEnum: int -> TimedEnumerator< ^a >) : ^a seq =
            let block =
                match blockSize with
                | Some bs when bs <= 0 -> -1
                | Some bs -> bs
                | None -> -1
            { new IEnumerable<'a> with
                  member __.GetEnumerator() = upcast (getEnum block)
              interface IEnumerable with
                  member __.GetEnumerator() = upcast (getEnum block) }

        let inline timed blockSize f (source: ^t seq) =
            use enum = source.GetEnumerator()
            mkTimed blockSize (fun block -> new TimedEnumerator< ^t >(enum, block, f))

        let private stat unitString getUnit blockSize elapsedTotal upstreamRatio =
            let totalScaled = getUnit (elapsedTotal : TimeSpan)
            let itemsPerTime = (float blockSize) / totalScaled
            let timePerItem = totalScaled / (float blockSize)
            let upDownPercentStr =
                if Double.IsNaN(upstreamRatio) then ""
                else sprintf " (%2.0f%% upstream | %2.0f%% downstream)" (100. * upstreamRatio) (100. * (1. - upstreamRatio))

            Console.WriteLine(
                sprintf "%d items %10.2f{0} %10.2f items/{0} %10.2f {0}/item%s"
                    blockSize totalScaled itemsPerTime timePerItem upDownPercentStr,
                (unitString : string))

        type stats =
            static member inline seconds  blockSize elapsedTotal upstreamRatio = stat "s"  (fun ts -> ts.TotalSeconds) blockSize elapsedTotal upstreamRatio
            static member inline mseconds blockSize elapsedTotal upstreamRatio = stat "ms" (fun ts -> ts.TotalMilliseconds) blockSize elapsedTotal upstreamRatio      

        let b = 
            seq { 1..1000000 }
            |> Seq.map ((+)1)
            |> timed None stats.mseconds
            |> Seq.filter ((<)100000)
            |> timed None stats.mseconds
            |> Seq.length

        (*

            module Seqs =

        open System
        open System.Collections
        open System.Collections.Generic
        open System.Diagnostics

        let inline timedEnumerator (source: IEnumerator<'t>) blockSize f =
            seq {
            
                let swUpstream = Stopwatch()
                let swDownstream = Stopwatch()
                let swTotal = Stopwatch.StartNew()
                let mutable count = 0
                // downstream consumer has finished processing the previous item
                ////swDownstream.Stop()
                
                let next () = 
                    // measure upstream MoveNext() call
                    swUpstream.Start()
                    let result = source.MoveNext()
                    swUpstream.Stop()
                    result

                while next () do
                    ////if result then
                    count <- count + 1

                    // blockSize reached or upstream is complete - invoke callback and reset timers
                    ////if (result && count = blockSize) || (not result && count > 0) then
                    if (count = blockSize) || (blockSize < 0) then                    
                        swTotal.Stop()
                        let upDownTotal = swUpstream.Elapsed + swDownstream.Elapsed
                        f count swTotal.Elapsed (swUpstream.Elapsed.TotalMilliseconds / upDownTotal.TotalMilliseconds)
                        count <- 0
                        swUpstream.Reset()
                        swDownstream.Reset()
                        swTotal.Restart()

                    // clock starts for downstream consumer
                    swDownstream.Start()
                    yield source.Current }

        let inline private mkTimed f blockSize (source: ^a seq) =
            let block =
                match blockSize with
                | Some bs when bs <= 0 -> -1
                | Some bs -> bs
                | None -> -1
            use enum = source.GetEnumerator()
            timedEnumerator enum block f
            //{ new IEnumerable<'a> with
            //      member __.GetEnumerator() = upcast (getEnum block)
            //  interface IEnumerable with
            //      member __.GetEnumerator() = upcast (getEnum block) }

        //let inline timed blockSize f (source: ^t seq) =
        //    use enum = source.GetEnumerator()
        //    mkTimed f blockSize (fun block -> (timedEnumerator enum block f).GetEnumerator())

        let private stats unitString getUnit blockSize elapsedTotal upstreamRatio =
            let totalScaled = getUnit (elapsedTotal : TimeSpan)
            let itemsPerTime = (float blockSize) / totalScaled
            let timePerItem = totalScaled / (float blockSize)
            let upDownPercentStr =
                if Double.IsNaN(upstreamRatio) then ""
                else sprintf " (%2.0f%% upstream | %2.0f%% downstream)" (100. * upstreamRatio) (100. * (1. - upstreamRatio))

            Console.WriteLine(
                sprintf "%d items %10.2f{0} %10.2f items/{0} %10.2f {0}/item%s"
                    blockSize totalScaled itemsPerTime timePerItem upDownPercentStr,
                (unitString : string))

        let s blockSize elapsedTotal upstreamRatio = stats "s"  (fun ts -> ts.TotalSeconds) blockSize elapsedTotal upstreamRatio
        let ms blockSize elapsedTotal upstreamRatio = stats "ms" (fun ts -> ts.TotalMilliseconds) blockSize elapsedTotal upstreamRatio      

        let b = 
            seq { 1..1000000 }
            |> Seq.map ((+)1)
            |> mkTimed ms None
            |> Seq.filter (fun _ -> true)
            |> mkTimed ms None
            |> Seq.toList

            *)