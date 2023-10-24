public class Diagnostics
{ 
  public bool Errors;
  public bool RuntimeErrors;
  public void ErrorToken(string kind , SyntaxToken token , string message)
  {
    if (token.Kind != SyntaxKind.EOF)
      Error(kind, token.Text, message);
    else
      Error(kind, "the end of the file", message);
    Errors = true;
  }
  
  
  public void RuntimeError(RuntimeError error)
  {
    Console.WriteLine($"! SEMANTIC ERROR: `{error.Token}` {error.Message}");
    RuntimeErrors = true;
  }

  public void Error(string errorKind, string errorPosition, string message)
  {
    Console.WriteLine($"! {errorKind} ERROR at '{errorPosition}' : {message}");
    Errors = true;
  }

}

public class RuntimeError : Exception
{
  public string Token{get;set;}

  public RuntimeError(string token, string message) : base(message)
  {
    Token = token;
  }
}
