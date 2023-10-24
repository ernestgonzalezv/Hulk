using static SyntaxKind;

public class Parser
{
  private class ParserError : Exception { }
  public List<SyntaxToken> Tokens;
  private readonly Diagnostics Diagnostics;
  private int Current = 0;
  

  public Parser(List<SyntaxToken> tokens, Diagnostics diagnostics)
  {
    Tokens = tokens;
    Diagnostics = diagnostics;
  }

  //auxiliar methods
  private SyntaxToken PreviousToken(){return Tokens[Current-1];}
  private SyntaxToken CurrentToken(){return Tokens[Current];}
  private SyntaxToken NextToken(){return Tokens[Current+1];}
  private bool CheckCurrent(SyntaxKind kind){if (AtTheEnd()) return false;return CurrentToken().Kind == kind;}
  private bool CheckNext(SyntaxKind kind){if (AtTheEnd()) return false;return NextToken().Kind == kind;}
  private SyntaxToken Advance(){if (!AtTheEnd()) Current++;return PreviousToken();}
  private bool AtTheEnd(){return CurrentToken().Kind == SyntaxKind.EOF;}
  private SyntaxToken EatToken(SyntaxKind type, string ErrorMessage)
  {
    if (CheckCurrent(type)) return Advance();
    Diagnostics.ErrorToken("!SYNTAX ERROR: ", CurrentToken(), ErrorMessage);
    return null;
  }

  private bool MatchToken(SyntaxKind kind)
  {
    if(CheckCurrent(kind))
    {
      Advance();
      return true;

    }
    return false;
  }
  private bool MatchTokens(SyntaxKind [] kinds)
  {
    foreach(SyntaxKind kind in kinds)
    {
      if(CheckCurrent(kind)){
        Advance();
        return true;
      }
    }
    return false;
  }


  //Parsing Methods


  //Expression Blocks
  public ExpressionSyntax Parse()
  {
    try
    {
      ExpressionSyntax result = GetExpression();
      if (MatchToken(SemicolonToken)){
        if (!AtTheEnd()){
          Diagnostics.ErrorToken("!SYNTAX ERROR: ", CurrentToken(), "EOF Token not found after `;`.");
          return null;
        }
        return result;
      }

      Diagnostics.ErrorToken("!SYNTAX ERROR: ", CurrentToken(), "`;` Token not found.");
      return null;
    }
    catch
    {
      Diagnostics.ErrorToken("!SYNTAX ERROR: ", CurrentToken(), "Unreachable code.");
      return null;
    }
  }

  private ExpressionSyntax GetExpression()
  {
    if (MatchToken(FunctionToken))
    {
      var name = EatToken(IDENTIFIER, "Function name not found.");
      EatToken(OpenParenthesisToken, $"Open parenthesis Token not found after `{name}`.");
      var parameters = new List<SyntaxToken>();
      if (!CheckCurrent(CloseParenthesisToken) && !CheckCurrent(ImpliesToken))
      {
        do
        {
          parameters.Add(EatToken(IDENTIFIER, "Parameter's name not found after `,`."));
          if (parameters.Count > 11)
          {
            Diagnostics.ErrorToken("!SYNTAX ERROR: ", CurrentToken(), "Functions cannot support more than 10 parameters.");
            return null;
          } 
        }while (MatchToken(CommaToken));    
      }
      
      if (parameters.Count == 0) EatToken(CloseParenthesisToken, "Closing parenthesis Token not found after `(` .");
      else EatToken(CloseParenthesisToken, "Closing parenthesis Token not found after " + parameters.Last().Text +" .");
      EatToken(ImpliesToken, "`=>` Token not found before body.");
      return new ExpressionSyntax.FunctionExpressionSyntax(name, parameters, ParseExpressionSyntax());
    }
    //if not then 
    return ParseExpressionSyntax();
  }

  private ExpressionSyntax GetVariables()
  {
    if (MatchToken(LetToken))
    {
      var variables = Variables();
      EatToken(InToken, "`in` Token not found at end of `let-in` expression.");
      return new ExpressionSyntax.LetInExpressionSyntax(variables, ParseExpressionSyntax());
    }
    return ParseIfStatementExpressionSyntax();
  }

  private List<ExpressionSyntax.AssignmentExpressionSyntax> Variables()
  {
    List<ExpressionSyntax.AssignmentExpressionSyntax> variables = new List<ExpressionSyntax.AssignmentExpressionSyntax>();
    do
    {
      if (!MatchToken(IDENTIFIER)) // didnt find a variable name 
      {
        Diagnostics.ErrorToken("!SYNTAX ERROR: ", CurrentToken(), "Invalid token in `let-in` expression.");
        return null;
      }

      SyntaxToken Variable = PreviousToken();
      EatToken(EqualToken, $" `=` not found after variable `{Variable.Text}`.");
      variables.Add(new ExpressionSyntax.AssignmentExpressionSyntax(Variable, ParseExpressionSyntax()));
    } while (MatchToken(CommaToken));

    return variables;
  }

  private ExpressionSyntax ParseIfStatementExpressionSyntax()
  {
    if (MatchToken(IfToken))
    {
      EatToken(OpenParenthesisToken, "Open parenthesis Token not found after `if` expression.");
      var IfCondition = ParseExpressionSyntax();
      EatToken(CloseParenthesisToken, "Closing parenthesis Token not found after condition.");
      var Response = ParseExpressionSyntax();
      EatToken(ElseToken, "Else expression expected after expression."); 
      return new ExpressionSyntax.ConditionalExpressionSyntax(IfCondition, Response, ParseExpressionSyntax());
    }
    return ParseLogicalOr();
  }

  private ExpressionSyntax ParseExpressionSyntax()
  {
    return GetVariables();
  }


  //Single Expressions
  private ExpressionSyntax ParseLogicalOr()
  {
    var Left = ParseLogicalAnd();
    while (MatchToken(OrToken))
    {
      var OperatorToken = PreviousToken();
      var Right = ParseLogicalAnd();
      Left = new ExpressionSyntax.BinaryExpressionSyntax(Left, OperatorToken, Right);
    }

    return Left;
  }

  private ExpressionSyntax ParseLogicalAnd()
  {
    var Left = Operations4();
    while (MatchToken(AndToken))
    {
      var OperatorToken = PreviousToken();
      var Right = Operations4();
      Left = new ExpressionSyntax.BinaryExpressionSyntax(Left, OperatorToken, Right);
    }
    return Left;
  }

  private ExpressionSyntax ParseLiteralExpressionSyntax()
  {
    switch (CurrentToken().Kind)
    {
      
      case TrueToken:
        Advance();
        return new ExpressionSyntax.LiteralExpressionSyntax(true);
      case FalseToken:
        Advance();
        return new ExpressionSyntax.LiteralExpressionSyntax(false);
      case EulerToken:
        Advance();
        return new ExpressionSyntax.LiteralExpressionSyntax(Math.E);
      case StringToken:
      case NumberToken:
        return new ExpressionSyntax.LiteralExpressionSyntax(Advance().Literal);
      case IDENTIFIER:
        return new ExpressionSyntax.VariableExpressionSyntax(Advance());
      case PIToken:
        Advance();
        return new ExpressionSyntax.LiteralExpressionSyntax(Math.PI);

      
      default:
        Diagnostics.ErrorToken("!SYNTAX ERROR: ", CurrentToken(), "Expression Expected and not found.");
        return null;
    }
  }

  private List<ExpressionSyntax> GetParameters()
  {
    var parameters = new List<ExpressionSyntax>();
    if (!CheckCurrent(CloseParenthesisToken))
    {
      do{parameters.Add(ParseExpressionSyntax());} while (MatchToken(CommaToken));
    }
    return parameters;
  }
  private ExpressionSyntax ParseFunctionCallingExpressionSyntax()
  {
    if (CheckCurrent(IDENTIFIER) && CheckNext(OpenParenthesisToken))
    {
      SyntaxToken name = Advance();
      EatToken(OpenParenthesisToken, $"Open parenthesis Token not found after {name.Text}.");
      List<ExpressionSyntax> parameters = GetParameters();
      EatToken(CloseParenthesisToken, $"Closing parenthesis Token not found after parameters.");
      return new ExpressionSyntax.CallingExpressionSyntax(name, parameters);
    }
    return ParseLiteralExpressionSyntax();
  }
  private ExpressionSyntax ParseParenthesizedExpressionSyntax()
  {
    if (MatchToken(OpenParenthesisToken))
    {
      ExpressionSyntax expression = ParseExpressionSyntax();
      EatToken(CloseParenthesisToken, "Closing parenthesis Token not found after expression.");
      return expression;
    }
    return ParseFunctionCallingExpressionSyntax();
  }
  private ExpressionSyntax ParseUnaryExpressionSyntax()
  {
    if (MatchToken(NotToken) || MatchToken(MinusToken))
    {
      var OperatorToken = PreviousToken();
      var Expression = ParseUnaryExpressionSyntax();
      return new ExpressionSyntax.UnaryExpressionSyntax(OperatorToken, Expression);
    }

    return ParseParenthesizedExpressionSyntax();
  }

  //SyntaxFacts Precedence
  private ExpressionSyntax ParseBinaryExpressionSyntax(Func<ExpressionSyntax> HigherBinaryOperatorPrecedence, params SyntaxKind[] Kinds)
  {
    var Left = HigherBinaryOperatorPrecedence();
    while (MatchTokens(Kinds))
    {
      var OperatorToken = PreviousToken();
      var Right = HigherBinaryOperatorPrecedence();
      Left = new ExpressionSyntax.BinaryExpressionSyntax(Left, OperatorToken, Right);
    }
    return Left;
  }

  private ExpressionSyntax Operations()
  {
    return ParseBinaryExpressionSyntax(ParseUnaryExpressionSyntax, PowerToken);
  }
  private ExpressionSyntax Operations1()
  {
    return ParseBinaryExpressionSyntax(Operations , StarToken , SlashToken , ModToken);
  }
  private ExpressionSyntax Operations2()
  {
    return ParseBinaryExpressionSyntax(Operations1, PlusToken,  MinusToken,  ConcatenationToken);
  }
  private ExpressionSyntax Operations3()
  {
    return ParseBinaryExpressionSyntax(Operations2 , GreaterEqualToken , GreaterToken, LessToken, LessEqualToken);
  }
  private ExpressionSyntax Operations4()
  {
    return ParseBinaryExpressionSyntax(Operations3, EqualEqualToken , NotEqualToken);
  }

  

  
}
