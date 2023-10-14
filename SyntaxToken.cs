



public class SyntaxToken : SyntaxNode{
    
    public override SyntaxKind Kind{get;}
    public object  Value {get;}
        public SyntaxToken(string text, int position) 
        {
            this.Text = text;
                this.Position = position;
               
        }
            public string Text {get;}
    public int Position{get;}
    public SyntaxToken(SyntaxKind kind, int position, string text, object value)
    {
        Kind=kind;
        Text=text;
        Value=value;
        Position = position;
        Value = value;
    }
    public override IEnumerable<SyntaxNode> GetChildren()
    {
        return Enumerable.Empty<SyntaxNode>();
    }
}


