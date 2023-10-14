internal static class SyntaxFacts{
    public static  int GetBinaryOperatorPrecedence(this SyntaxKind kind ) // extension method just to be cute :)
    {
        switch(kind){
            
            case SyntaxKind.StarToken:
            case SyntaxKind.SlashToken:
            case SyntaxKind.ExponentialToken:
                return 2;

            case SyntaxKind.PlusToken:
            case SyntaxKind.MinusToken:
            case SyntaxKind.EqualsToken:
            case SyntaxKind.CommaToken:
                return 1;

            default:
                return 0;
        }
    }
    public static  int GetUnaryOperatorPrecedence(this SyntaxKind kind ) // extension method just to be cute :)
    {
        switch(kind){
            case SyntaxKind.PlusToken:
            case SyntaxKind.MinusToken:
                return 3;

            default:
                return 0;
        }
    }
}