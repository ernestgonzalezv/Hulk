using static SyntaxKind;
public class Lexer
{
  
  private int line = 1;
  public string _text { get; }
  private int start = 0;
  private int current = 0;
  private List<SyntaxToken> Tokens = new List<SyntaxToken>();
  private Diagnostics Diagnostics;

  public Lexer(string _text, Diagnostics diagnostics)
  {
    this._text = _text;
    Diagnostics= diagnostics;
  }


  //auxiliar methods
  private void AddToken(SyntaxKind type)
  {
    AddToken(type, null);
  }

  private void AddToken(SyntaxKind type, object literal)
  {
    var text_value = _text.Substring(start, current - start);
    Tokens.Add(new SyntaxToken(type, current, literal, text_value));
  }
  

  private void GetIdentifier()
  {
    while (IsAlphaNum(Peek())) Next();

    var text = _text.Substring(start, current - start);
    SyntaxKind kind;
    switch(text)
    {
      case "E":
        kind = EulerToken;
        break;
      case "else":
        kind = ElseToken;
        break;
      case "false":
        kind = FalseToken;
        break;
      case "function":
        kind = FunctionToken;
        break;
      case "if":
        kind = IfToken;
        break;
      case "in":
        kind = InToken;
        break;
      case "let" :
        kind = LetToken;
        break;
      case "PI":
        kind = PIToken;
        break;
      case "true":
        kind = TrueToken;
        break;
      default:
        kind = IDENTIFIER;
        break;
    }

    AddToken(kind);
  }
  

  private void GetNumber()
  {
    while (IsDigit(Peek())) Next();

    if (Peek() == '.' && IsDigit(NextToken()))
    {
      Next();
      while (IsDigit(Peek())) Next();
    }

    if (IsAlpha(Peek()))
    {
      Next();
      while (IsAlphaNum(Peek())) Next();
      string text = _text.Substring(start, current - start);
      SyntaxToken InvalidToken = new SyntaxToken(IDENTIFIER, current, null, text );
      Diagnostics.ErrorToken("!LEXICAL ERROR: ", InvalidToken, "Invalid token");
      return;
    }

    AddToken(NumberToken, double.Parse(_text.Substring(start, current - start)));
  }

  private void GetString()
  {
    while ( !AtTheEnd() && Peek() != '"')
    {
      if (NextToken() == '"' && Peek() == '\\') Next();
      if (Peek() == '\n') line++;
      Next();
    }
    if (AtTheEnd())
    {
      var text = _text.Substring(start, current);
      SyntaxToken InvalidToken = new SyntaxToken(StringToken,current, null, text);
      Diagnostics.ErrorToken("!LEXICAL ERROR: ", InvalidToken, "Invalid string.");
      return;
    }
    Next();
    string value = _text.Substring(start + 1, current - start - 2);
    value = value.Replace("\\n", "\n").Replace("\\t", "\t").Replace("\\\"", "\"");
    AddToken(StringToken, value);
  }

  private bool EatToken(char character)
  {
    if (AtTheEnd()) return false;
    if (_text[current] == character){
      Next();
      return true;
    }
    return false;
  }

  private char Peek()
  {
    if (AtTheEnd()) return '\0';
    return _text[current];
  }

  private char NextToken() 
  {
    if (current + 1 >= _text.Count()) return '\0';
    return _text[current + 1];
  }

  private bool IsAlpha(char character){return (character == '_') || ('A' <= character && character <= 'Z') || ('a' <= character && character <= 'z') ;}

  private bool IsDigit(char character){return ('0' <= character && character <= '9');}

  private bool IsAlphaNum(char character){return IsDigit(character) || IsAlpha(character) ;}

  private bool AtTheEnd()
  {
    return current >= _text.Count();
  }

  private char Next() 
  {
    return _text[current++];
  }


  //methods
  public List<SyntaxToken> GetTokens()
  {
    while (!AtTheEnd()){start = current;GetNextToken();}
    var EOFToken = new SyntaxToken(EOF, current, null, "" );
    Tokens.Add(EOFToken);
    return Tokens;
  }

  private void GetNextToken()
  {
    char character = Next();
    switch (character)
    {
      case '(':
        AddToken(OpenParenthesisToken);
        break;
      case ')':
        AddToken(CloseParenthesisToken);
        break;
      case '@':
        AddToken(ConcatenationToken);
        break;
      case '/':
        AddToken(SlashToken);
        break;
      case ',':
        AddToken(CommaToken);
        break;
      case '-':
        AddToken(MinusToken);
        break;
      case '+':
        AddToken(PlusToken);
        break;
      case '*':
        AddToken(StarToken);
        break;
      case '^':
        AddToken(PowerToken);
        break;
      case '|':
        AddToken(OrToken);
        break;
      case '!':
        AddToken(EatToken('=') ? NotEqualToken : NotToken);
        break;
      case ';':
        AddToken(SemicolonToken);
        break;
      case '%':
        AddToken(ModToken);
        break;
      case '&':
        AddToken(AndToken);
        break;
      case '=':
        AddToken(EatToken('=') ? EqualEqualToken : EatToken('>') ? ImpliesToken : EqualToken);
        break;
      case '<':
        AddToken(EatToken('=') ? LessEqualToken : LessToken);
        break;
      case '>':
        AddToken(EatToken('=') ? GreaterEqualToken : GreaterToken);
        break;
      case ' ':
      case '\r':
      case '\t':
        break;
      case '\n':
        line++;
        break;
      case '"':
        GetString();
        break;
      default:
        if (IsDigit(character))
        {
          GetNumber();
        }
        else if (IsAlpha(character))
        {
          GetIdentifier();
        }
        else
        {
          // unexpected character
          SyntaxToken invalid = new SyntaxToken(StringToken , current , null,character.ToString());
          Diagnostics.ErrorToken("!LEXICAL ERROR: ", invalid, "Unexpected character.");
        }
        break;
    }
  }
}

