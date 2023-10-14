
class Parser
{
    public ExpressionSyntax ans;
    public SyntaxToken [] _tokens;
    int _position;
    bool x = true;
    public string _text; 
    public Parser(string text)
    {
        _text= text; 
        var tokens = new List<SyntaxToken> ();
        var lexer = new Lexer(text);
        SyntaxToken token = lexer.NextToken() ; 
        while(token.Kind != SyntaxKind.EndOfFileToken)
        {
            if(token.Kind != SyntaxKind.WhiteSpaceToken && token.Kind != SyntaxKind.BadToken)
            {
                tokens.Add(token);
            }
            token = lexer.NextToken();
        }
        tokens.Add(lexer.NextToken());
        _tokens = tokens.ToArray();

    }
    private SyntaxToken Peek(int offset){ // look ahead at the next token to decide how to parse everything
        var index = _position+ offset;
        if(index>=_tokens.Length)
            return _tokens[_tokens.Length-1];
        return _tokens[index];
    }

    private SyntaxToken Current => Peek(0);
    private SyntaxToken LookAhead => Peek(1);


    private SyntaxToken NextToken()
    {
        var current= Current;
        _position++;
        return current;
    } 
    private SyntaxToken Match(SyntaxKind kind)
    {
        if(Current.Kind == kind )
            return NextToken();
        return new SyntaxToken(kind,Current.Position,null,null);
    }


    public void Parse()
    {
        ans= ParseExpressionSyntax();
        Console.WriteLine(Current.Kind);
        while(Current.Kind != SyntaxKind.EndOfLineToken)
        {
            var commaToken = Current;
            NextToken();
            ans = new BinaryExpressionSyntax(ans, commaToken , ParseExpressionSyntax());
        }
    }

    public ExpressionSyntax ParseExpressionSyntax( int ParentPrecedence = 0)
    {
        ExpressionSyntax left ; 

        var unaryOperatorPrecedence = Current.Kind.GetUnaryOperatorPrecedence();

        if(unaryOperatorPrecedence != 0 && unaryOperatorPrecedence>=ParentPrecedence)
        {
            var operatorToken = NextToken();
            var operand = ParseExpressionSyntax(unaryOperatorPrecedence);
            left = new UnaryExpressionSyntax(operatorToken, operand);
        }
        else
        {
            left = ParsePrimaryExpression();
        }

        while(true){
            if(Current.Kind == SyntaxKind.CommaToken){
                if(x==true)break;
            }
            var precedence = Current.Kind.GetBinaryOperatorPrecedence();
            if(precedence == 0 || precedence<=ParentPrecedence) break;
            var operatorToken = NextToken();
            var right = ParseExpressionSyntax(precedence);
            left = new BinaryExpressionSyntax(left, operatorToken,right);
        }
        return left ; 

    }
    


    private ExpressionSyntax ParsePrimaryExpression()
    {

        if(Current.Kind == SyntaxKind.FunctionExpressionCallingToken)
        {
            var text = Current.Text;
            //Console.WriteLine(text);
            if(LookAhead.Kind == SyntaxKind.EqualsToken){
                //assignment
                var left1 = Current.Text;
                var op = NextToken();
                NextToken();
                x=true;
                var right1 = ParseExpressionSyntax();
                return new AssignmentExpressionSyntax(left1, op , right1);
            }
            NextToken();

            //Console.WriteLine(Current.Text + " "+ text);;
            var left = NextToken();
            x=false;
            var expression = ParseExpressionSyntax();
            var right = Match(SyntaxKind.CloseParenthesisToken);
            Console.WriteLine(text+" "+left.Position + " " + right.Position);
            var parameters= _text.Substring(left.Position+1, right.Position-left.Position-1);
            //Console.WriteLine(parameters);
            return new FunctionExpressionCalling(text,left,expression,right, parameters);
            
        }

        if(Current.Kind == SyntaxKind.OpenParenthesisToken)
        {
            var left = NextToken();
            var expression = ParseExpressionSyntax();
            var right = Match(SyntaxKind.CloseParenthesisToken);

           
            return new ParenthesizedExpression(left, expression,right);
        }       

        var literalToken = Match(SyntaxKind.IntegerToken);
        return new LiteralExpressionSyntax(literalToken);
    }

    
    
    
}
