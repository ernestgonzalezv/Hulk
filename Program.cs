public class Hulk
{
  private static  Diagnostics Diagnostics = new Diagnostics();
  private static Evaluator evaluator = new Evaluator(Diagnostics);
  public static void Main()
  {
    while (true)
    {
      Console.Write("> ");
      string line = Console.ReadLine();
      if (line == null)break;
      Eval(line);
      //resetting errrors
      Diagnostics.RuntimeErrors = false;
      Diagnostics.Errors = false;
    }
    return ;
  }
  
  
  private static void Eval(string line)
  {
    var lexer = new Lexer(line, Diagnostics);
    var tokens = lexer.GetTokens();
    if (Diagnostics.RuntimeErrors || Diagnostics.Errors) 
      return ;
    var parser = new Parser(tokens,Diagnostics);
    var parseResult = parser.Parse();
    if (Diagnostics.Errors || Diagnostics.RuntimeErrors) 
      return ;
    var interpretResult = evaluator.Evaluate(parseResult);
    if (Diagnostics.RuntimeErrors || Diagnostics.Errors) 
      return ;
    return ;
  }
}

