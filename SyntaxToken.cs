
public class SyntaxToken
{
  public SyntaxKind Kind { get; private set; }
  public object Literal { get; private set; }
  public int Position { get; private set; }
  public string Text { get; private set; }
  

  public SyntaxToken(SyntaxKind kind , int position , object literal, string text)
  {
    Literal = literal;
    Position = position;
    Kind = kind;
    Text = text;
    
  }

  public override string ToString()
  {
    return $"SyntaxToken({Kind}, {Text}, {Literal})";
  }
}
