sealed class BinaryExpressionSyntax: ExpressionSyntax
{
    public override SyntaxKind Kind => SyntaxKind.BinaryExpression;
    public ExpressionSyntax Left{get;}
    public SyntaxToken Operator{get;}
    public ExpressionSyntax Right {get;}

    

    public BinaryExpressionSyntax (ExpressionSyntax left, SyntaxToken op, ExpressionSyntax right)
    {
        Left = left;
        Operator = op;
        Right=right;

    }

    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return Left;
        yield return Operator;
        yield return Right;
    }
    
}





sealed class AssignmentExpressionSyntax: ExpressionSyntax
{
    public override SyntaxKind Kind => SyntaxKind.BinaryExpression;
    public string Left{get;}
    public SyntaxToken Operator{get;}
    public ExpressionSyntax Right {get;}

    

    public AssignmentExpressionSyntax (string left, SyntaxToken op, ExpressionSyntax right)
    {
        Left = left;
        Operator = op;
        Right=right;

    }

    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return Operator;
        yield return Right;
    }
    
}