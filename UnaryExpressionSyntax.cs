sealed class UnaryExpressionSyntax: ExpressionSyntax
{
    public override SyntaxKind Kind => SyntaxKind.UnaryExpression;
    public SyntaxToken Operator{get;}
    public ExpressionSyntax Operand {get;}

    

    public UnaryExpressionSyntax (SyntaxToken op, ExpressionSyntax operand)
    {
        Operator = op;
        Operand=operand;

    }

    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return Operator;
        yield return Operand;
    }
    
}