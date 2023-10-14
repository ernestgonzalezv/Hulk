
sealed class ParenthesizedExpression : ExpressionSyntax {

    public ParenthesizedExpression(SyntaxToken left, ExpressionSyntax expression, SyntaxToken right){
        Left = left;
        Expression = expression;
        Right = right;
    }
    public override SyntaxKind Kind => SyntaxKind.ParenthesizedExpression;
    public SyntaxToken Left {get;}
    public SyntaxToken Right{get;}
    public ExpressionSyntax Expression{get;}


    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return Left;
        yield return Expression;
        yield return Right;
    }
}
