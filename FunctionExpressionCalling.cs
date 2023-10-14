sealed class FunctionExpressionCalling : ExpressionSyntax{


    public FunctionExpressionCalling(string text, SyntaxToken left,ExpressionSyntax expression, SyntaxToken right, string parameters)
    {
        Text = text ; 
        Left = left; 
        Expression = expression;
        Right = right;
        Parameters = parameters;
    }
    public string Text{get;}
    public override SyntaxKind Kind => SyntaxKind.FunctionExpressionCallingToken;
    public SyntaxToken Left {get;}
    public ExpressionSyntax Expression{get;}
    public SyntaxToken Right{get;}
    public string Parameters {get;}
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return Left;
        yield return Expression;
        yield return Right;
    }
}