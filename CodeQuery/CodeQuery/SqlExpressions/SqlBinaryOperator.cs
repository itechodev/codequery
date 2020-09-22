namespace CodeQuery.SqlExpressions
{
    public enum SqlBinaryOperator
    {
        // Arithmetic
        Plus,
        Divide,
        Multiply,
        Subtract,
        Concat,
        Modulo,
        Exponentiation,
        SquareRoot,
        CubeRoot,
        Factorial,
        Absolute,
        OnesComplement,
        Power,
        
        // Bitwise
        BitwiseAnd,
        BitwiseOr,
        BitwiseXor,
        BitwiseNot,
        BitwiseShiftLeft,
        BitwiseShiftRight,
        
        // Boolean
        And,
        Or,
        Not,
        Xor,
        GreaterThan,
        GreaterThanOrEqual,
        LessThan,
        LessThanOrEqual,
        
        Coalesce,

        
    }
}