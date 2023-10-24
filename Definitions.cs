public class Definitions
{
  public Dictionary<string, List<object>> varGlobal;
  public Dictionary<string, Dictionary<int, ExpressionSyntax.FunctionExpressionSyntax>> funGlobal;
  private Diagnostics Diagnostics;
  
  public Definitions(Diagnostics diagnostics)
  {
    varGlobal = new Dictionary<string, List<object>>();
    funGlobal = new Dictionary<string, Dictionary<int, ExpressionSyntax.FunctionExpressionSyntax>>();
    Diagnostics = diagnostics;
  }

  //auxiliar methods
  public bool ExistentExactFunction(int numberOfParameters, SyntaxToken name)
  {
    if (funGlobal.ContainsKey(name.Text)) return funGlobal[name.Text].ContainsKey(numberOfParameters); 
    return false;
  }
  public ExpressionSyntax GetBody(string name, int numberOfParameters){return funGlobal[name][numberOfParameters].FunctionBody;}
  public bool BuiltInFunction(int numberOfParameters,string name)
  {
    return (name == "log" && numberOfParameters  == 2) ||
    (name == "cos" && numberOfParameters == 1) || 
    (name == "sqrt" && numberOfParameters == 1) || 
    (name == "rand" && numberOfParameters == 0 ) ||
    (name == "print" && numberOfParameters == 1) ||
    (name == "exp" && numberOfParameters ==1) ||
    (name == "sin" && numberOfParameters == 1);
  }
  public List<SyntaxToken> GetParameters(string name, int numberOfParameters){return funGlobal[name][numberOfParameters].FunctionParameters;}
  public object GetVariable(SyntaxToken name)
  {
    try
    {
      return varGlobal[name.Text].Last();
    }catch (KeyNotFoundException)
    {
      Diagnostics.RuntimeError(new RuntimeError(name.Text, $"Missing `{name}` not be declared."));
      return null;
    }
  }

  // methods

  public void FunctionDeclaration(ExpressionSyntax.FunctionExpressionSyntax function)
  {
    var numberOfParameters = function.NumberOfParameters;
    var functionName = function.FunctionName.Text;
    if (BuiltInFunction(numberOfParameters,functionName))
    {
      Diagnostics.RuntimeError(new RuntimeError(functionName, "Built-In."));
      return;
    }

    else if (funGlobal.ContainsKey(functionName))
    {
      if (funGlobal[functionName].ContainsKey(numberOfParameters))
        Diagnostics.RuntimeError(new RuntimeError(functionName, "Already declared."));
      else 
        funGlobal[functionName].Add(numberOfParameters, function);
      return ;
    }
    else
    {
      var new_function = new Dictionary<int, ExpressionSyntax.FunctionExpressionSyntax>{{numberOfParameters, function}};
      funGlobal.Add(functionName, new_function);
      return ; 
    }
  }

  public void VariableDeclaration(SyntaxToken name, object value)
  {
    if (!varGlobal.ContainsKey(name.Text))
      varGlobal.Add(name.Text, new List<object>());
    varGlobal[name.Text].Add(value);
  }

  public void Remove(SyntaxToken name)
  {
    varGlobal[name.Text].RemoveAt(varGlobal[name.Text].Count - 1);
    if (varGlobal[name.Text].Count == 0) varGlobal.Remove(name.Text);
  }

}
