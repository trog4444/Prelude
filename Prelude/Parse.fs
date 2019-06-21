namespace Prelude

/// Parsers that attempt to parse a value from a string (or other type) into it's actual representation.
/// Each method returns Some value if the parse is successful, else returns None.
module Parse =

    /// Try to parse an sbyte.
    let inline toSByte sbyte =
        match System.SByte.TryParse sbyte with
        | true, b  -> Some b
        | false, _ -> None

    /// Try to parse a byte.
    let inline toByte byte =
        match System.Byte.TryParse byte with
        | true, b  -> Some b
        | false, _ -> None

    /// Try to parse an int.
    let inline toInt int =
        match System.Int32.TryParse int with
        | true, n  -> Some n
        | false, _ -> None

    /// Try to parse a uint.
    let inline toUInt uint =
        match System.UInt32.TryParse uint with
        | true, n  -> Some n
        | false, _ -> None

    /// Try to parse an int64.
    let inline toInt64 int64 =
        match System.Int64.TryParse int64 with
        | true, n  -> Some n
        | false, _ -> None

    /// Try to parse a uint64.
    let inline toUInt64 uint64 =
        match System.UInt64.TryParse uint64 with
        | true, n  -> Some n
        | false, _ -> None

    /// Try to parse a float32.
    let inline toFloat float =
        match System.Double.TryParse float with
        | true, f  -> Some f
        | false, _ -> None

    /// Try to parse a decimal.
    let inline toDecimal decimal =
        match System.Decimal.TryParse decimal with
        | true, d  -> Some d
        | false, _ -> None
       
    /// Try to parse a date.
    let inline toDate date =
        match System.DateTime.TryParse date with
        | true, d  -> Some d
        | false, _ -> None

    /// Try to parse a bool.
    let inline toBool bool =
        match System.Boolean.TryParse bool with
        | true, b  -> Some b
        | false, _ -> None

    /// Try to parse a char.
    let inline toChar char =
        match System.Char.TryParse char with
        | true, c  -> Some c
        | false, _ -> None


    /// Parsers for enum types.
    type Enum =
        
        /// Try to parse an enum from a string. Must provide enum type in <> brackets.
        static member inline tryParse< ^Enum> (enum: char) =
            let typedef = typedefof< ^Enum>
            try if System.Enum.IsDefined(typedef, enum)
                then Some (System.Enum.ToObject(typedef, enum) :?> ^Enum)
                else None
            with _ -> None                

        /// Try to parse an enum from a string. Must provide enum type in <> brackets.
        static member inline tryParse< ^Enum> (enum: string) =
            let typedef = typedefof< ^Enum>
            try if System.Enum.IsDefined(typedef, enum)
                then Some (System.Enum.ToObject(typedef, enum) :?> ^Enum)
                else None
            with _ -> None 

        /// Try to parse an enum from a byte. Must provide enum type in <> brackets.
        static member inline tryParse< ^Enum> (enum: byte) =
            let typedef = typedefof< ^Enum>
            try if System.Enum.IsDefined(typedef, enum)
                then Some (System.Enum.ToObject(typedef, enum) :?> ^Enum)
                else None
            with _ -> None 

        /// Try to parse an enum from an sbyte. Must provide enum type in <> brackets.
        static member inline tryParse< ^Enum> (enum: sbyte) =
            let typedef = typedefof< ^Enum>
            try if System.Enum.IsDefined(typedef, enum)
                then Some (System.Enum.ToObject(typedef, enum) :?> ^Enum)
                else None
            with _ -> None 

        /// Try to parse an enum from a UInt16. Must provide enum type in <> brackets.
        static member inline tryParse< ^Enum> (enum: System.UInt16) =
            let typedef = typedefof< ^Enum>
            try if System.Enum.IsDefined(typedef, enum)
                then Some (System.Enum.ToObject(typedef, enum) :?> ^Enum)
                else None
            with _ -> None 

        /// Try to parse an enum from an Int16. Must provide enum type in <> brackets.
        static member inline tryParse< ^Enum> (enum: System.Int16) =
            let typedef = typedefof< ^Enum>
            try if System.Enum.IsDefined(typedef, enum)
                then Some (System.Enum.ToObject(typedef, enum) :?> ^Enum)
                else None
            with _ -> None 

        /// Try to parse an enum from a UInt32. Must provide enum type in <> brackets.
        static member inline tryParse< ^Enum> (enum: System.UInt32) =
            let typedef = typedefof< ^Enum>
            try if System.Enum.IsDefined(typedef, enum)
                then Some (System.Enum.ToObject(typedef, enum) :?> ^Enum)
                else None
            with _ -> None 

        /// Try to parse an enum from an Int32. Must provide enum type in <> brackets.
        static member inline tryParse< ^Enum> (enum: int) =
            let typedef = typedefof< ^Enum>
            try if System.Enum.IsDefined(typedef, enum)
                then Some (System.Enum.ToObject(typedef, enum) :?> ^Enum)
                else None
            with _ -> None 

        /// Try to parse an enum from a UInt64. Must provide enum type in <> brackets.
        static member inline tryParse<'Enum> (enum: System.UInt64) =
            let typedef = typedefof<'Enum>
            try if System.Enum.IsDefined(typedef, enum)
                then Some (System.Enum.ToObject(typedef, enum) :?> 'Enum)
                else None
            with _ -> None 

        /// Try to parse an enum from an Int64. Must provide enum type in <> brackets.
        static member inline tryParse< ^Enum> (enum: System.Int64) =
            let typedef = typedefof< ^Enum>
            try if System.Enum.IsDefined(typedef, enum)
                then Some (System.Enum.ToObject(typedef, enum) :?> ^Enum)
                else None
            with _ -> None 

        /// Try to parse an enum from an Int64. Must provide enum type in <> brackets.
        static member inline tryParse< ^Enum> (enum: obj) =
            try
                if System.Enum.IsDefined(typedefof< ^Enum>, enum)
                then Some (enum :?> 'Enum)
                else None
            with _ -> None