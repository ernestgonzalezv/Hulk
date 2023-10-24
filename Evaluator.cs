using static SyntaxKind;
public class Evaluator : ExpressionSyntax.Children<object>
{
  private Definitions DefinedStuff;
  private Diagnostics Diagnostics;

  public Evaluator(Diagnostics diagnostics)
  {
    DefinedStuff = new Definitions(diagnostics);
    Diagnostics = diagnostics;
  }

  public string Evaluate(ExpressionSyntax expression)
  {
    try
    {
      var a = EvaluateExpression(expression);
      if (a == null) return null;
      return a.ToString();
    }
    catch
    {
      if (!Diagnostics.RuntimeErrors)
        Diagnostics.RuntimeError(new RuntimeError("", "Can't Evaluate line."));
      return null;
    }
  }


  //auxiliar methods
  
  private bool IFTrue(object a)
  {
    if (a == null) return false;
    if (a is bool) return (bool)a;
    return true;
  }

  //evaluate
  private object EvaluateExpression(ExpressionSyntax expression)
  {
    if (!Diagnostics.Errors) return expression.GetChildren(this);
    return null;
  }

  //Evaluate Every Single Expression
  
  public object GetBinaryExpressionSyntax(ExpressionSyntax.BinaryExpressionSyntax expression)
  {
    var left = EvaluateExpression(expression.Left);
    var right = EvaluateExpression(expression.Right);
    switch (expression.OperatorToken.Kind)
    {
      case PlusToken:
        if (!(left is double)){Diagnostics.RuntimeError(new RuntimeError(left.ToString(), "Gotta be a NumberToken ."));}
        if (!(right is double)){Diagnostics.RuntimeError(new RuntimeError(right.ToString(), "Gotta be a NumberToken ."));}
        return (double)left + (double)right;
      case MinusToken:
        if (!(left is double)){Diagnostics.RuntimeError(new RuntimeError(left.ToString(), "Gotta be a NumberToken ."));}
        if (!(right is double)){Diagnostics.RuntimeError(new RuntimeError(right.ToString(), "Gotta be a NumberToken ."));}
        return (double)left - (double)right;
      case StarToken:
        if (!(left is double)){Diagnostics.RuntimeError(new RuntimeError(left.ToString(), "Gotta be a NumberToken ."));}
        if (!(right is double)){Diagnostics.RuntimeError(new RuntimeError(right.ToString(), "Gotta be a NumberToken ."));}
        return (double)left * (double)right;
      case ModToken:
        if (!(left is double)){Diagnostics.RuntimeError(new RuntimeError(left.ToString(), "Gotta be a NumberToken ."));}
        if (!(right is double)){Diagnostics.RuntimeError(new RuntimeError(right.ToString(), "Gotta be a NumberToken ."));}
        return (double)left % (double)right;
      case SlashToken:
        if (!(left is double)){Diagnostics.RuntimeError(new RuntimeError(left.ToString(), "Gotta be a NumberToken ."));}
        if (!(right is double)){Diagnostics.RuntimeError(new RuntimeError(right.ToString(), "Gotta be a NumberToken ."));}
        return (double)left / (double)right;
      case ConcatenationToken:
        if (left is double) 
          left = left.ToString();
        if (right is double) 
          right = right.ToString();
        if (!(left is string)){Diagnostics.RuntimeError(new RuntimeError(left.ToString(), "Gotta be a string."));}
        if (!(right is string)){Diagnostics.RuntimeError(new RuntimeError(right.ToString(), "Gotta be a string."));}
        return String.Concat((string)left, (string)right);
      case PowerToken:
        if (!(left is double)){Diagnostics.RuntimeError(new RuntimeError(left.ToString(), "Gotta be a NumberToken ."));}
        if (!(right is double)){Diagnostics.RuntimeError(new RuntimeError(right.ToString(), "Gotta be a NumberToken ."));}
        return Math.Pow((double)left, (double)right);
      case LessToken:
        if (!(left is double)){Diagnostics.RuntimeError(new RuntimeError(left.ToString(), "Gotta be a NumberToken ."));}
        if (!(right is double)){Diagnostics.RuntimeError(new RuntimeError(right.ToString(), "Gotta be a NumberToken ."));}
        return (double)left < (double)right;
      case LessEqualToken:
        if (!(left is double)){Diagnostics.RuntimeError(new RuntimeError(left.ToString(), "Gotta be a NumberToken ."));}
        if (!(right is double)){Diagnostics.RuntimeError(new RuntimeError(right.ToString(), "Gotta be a NumberToken ."));}
        return (double)left <= (double)right;
      case EqualEqualToken:
        return Equals(left, right);
      case GreaterToken:
        if (!(left is double)){Diagnostics.RuntimeError(new RuntimeError(left.ToString(), "Gotta be a NumberToken ."));}
        if (!(right is double)){Diagnostics.RuntimeError(new RuntimeError(right.ToString(), "Gotta be a NumberToken ."));}
        return ((double)left > (double)right);
      case AndToken:
        if (!(left is bool)){Diagnostics.RuntimeError(new RuntimeError(left.ToString(), "Gotta be a boolean."));}
        if (!(right is bool)){Diagnostics.RuntimeError(new RuntimeError(right.ToString(), "Gotta be a boolean."));}
        return ((bool)left & (bool)right);
      case OrToken:
        if (!(left is bool)){Diagnostics.RuntimeError(new RuntimeError(left.ToString(), "Gotta be a boolean."));}
        if (!(right is bool)){Diagnostics.RuntimeError(new RuntimeError(right.ToString(), "Gotta be a boolean."));}
        return ((bool)left | (bool)right);
      case GreaterEqualToken:
        if (!(left is double)){Diagnostics.RuntimeError(new RuntimeError(left.ToString(), "Gotta be a NumberToken ."));}
        if (!(right is double)){Diagnostics.RuntimeError(new RuntimeError(right.ToString(), "Gotta be a NumberToken ."));}
        return ((double)left >= (double)right);
      case NotEqualToken:
        return !Equals(left, right);
    }
    return null;
  }

  public object GetUnaryExpressionSyntax(ExpressionSyntax.UnaryExpressionSyntax unaryExpression)
  {
    var expressionValue = EvaluateExpression(unaryExpression.Expression);
    if(unaryExpression.OperatorToken.Kind == NotToken){
      if (!(expressionValue is bool)){Diagnostics.RuntimeError(new RuntimeError(expressionValue.ToString(), "Gotta be a boolean."));}
      return !IFTrue(expressionValue);
    }else if(unaryExpression.OperatorToken.Kind == MinusToken)
    {
      if (!(expressionValue is double)){Diagnostics.RuntimeError(new RuntimeError(expressionValue.ToString(), "Gotta be a NumberToken ."));}
      return -(double) expressionValue;
    }
    return null;
  }

  public object GetLiteralExpressionSyntax(ExpressionSyntax.LiteralExpressionSyntax literal)
  {
    return literal.LiteralToken;
  }

  public object GetFunctionExpressionSyntax(ExpressionSyntax.FunctionExpressionSyntax functionExpression)
  {
    DefinedStuff.FunctionDeclaration(functionExpression);
    return null;
  }

  public object GetConditionalExpressionSyntax(ExpressionSyntax.ConditionalExpressionSyntax conditionalExpression)
  {
    var condition = EvaluateExpression(conditionalExpression.Condition);
    if (condition is bool)
    {
      if(!IFTrue(condition))return EvaluateExpression(conditionalExpression.ElseExpression);
      return EvaluateExpression(conditionalExpression.Response);
    }
    Diagnostics.RuntimeError(new RuntimeError(condition.ToString(), "Condition must be a boolean expression."));
    return null;
  }

  public object GetLetInExpressionSyntax(ExpressionSyntax.LetInExpressionSyntax LetInExpression)
  {
    var variableNamesToRemove = new List<SyntaxToken>();
    try
    {
      //declare variables
      foreach (var assignment in LetInExpression.VariableAssignments){
        DefinedStuff.VariableDeclaration(assignment.VariableName, EvaluateExpression(assignment.VariableValue));
        variableNamesToRemove.Add(assignment.VariableName);
      }

      //evaluate after everything is declared
      var ExpressionEvaluated = EvaluateExpression(LetInExpression.AfterIn);

      //delete variables 
      foreach (var assigment in LetInExpression.VariableAssignments)
      {
        DefinedStuff.Remove(assigment.VariableName);
      }

      return ExpressionEvaluated;

    }
    catch
    {
      foreach (var variableName in variableNamesToRemove){DefinedStuff.Remove(variableName);} //still we gotta get this outta the way 
      Diagnostics.RuntimeError(new RuntimeError(null, "Error in Let-In ExpressionSyntax"));
      return null;
    }
  }

  public object GetCallingExpressionSyntax(ExpressionSyntax.CallingExpressionSyntax CallingExpression)
  {
    //builtin functions
    if (DefinedStuff.BuiltInFunction(CallingExpression.NumberOfParameters,CallingExpression.FunctionName.Text))
    {
      var FunctionParameters = new List<object>();
      foreach (var Parameters in CallingExpression.FunctionParameters) 
        FunctionParameters.Add(EvaluateExpression(Parameters));
      var fp = FunctionParameters[0];
      switch (CallingExpression.FunctionName.Text)
      {
        case "print":
          Console.WriteLine(fp);
          return null;
        case "log":
          var sp = FunctionParameters[1];
          return (double)Math.Log((double)sp, (double)fp);
        case "sin":
          return (double)Math.Sin((double)fp);
        case "cos":
          return (double)Math.Cos((double)fp);
        case "rand":
          Random result = new Random();
          return result.NextDouble();  
        case "sqrt":
          return (double)Math.Sqrt((double)fp);
        case "exp":
          return (double)Math.Exp((double)fp);
        default:
          throw new NotImplementedException();
      }
    }
    // declarable functions

    //potential errors
    if (!DefinedStuff.ExistentExactFunction(CallingExpression.NumberOfParameters ,CallingExpression.FunctionName))
    {
      Diagnostics.RuntimeError(new RuntimeError(CallingExpression.FunctionName.Text, "Mistaken number of parameters for this function."));
      return null;
    }

    if (!DefinedStuff.funGlobal.ContainsKey(CallingExpression.FunctionName.Text))
    {
      Diagnostics.RuntimeError(new RuntimeError(CallingExpression.FunctionName.Text, "Not defined Function."));
      return null;
    }

    //evaluation
    var functionParameters = DefinedStuff.GetParameters(CallingExpression.FunctionName.Text, CallingExpression.NumberOfParameters);
    var parameters = CallingExpression.FunctionParameters;
    var letInAssignments = new List<ExpressionSyntax.AssignmentExpressionSyntax>();
    for (int i=0; i<CallingExpression.NumberOfParameters;i++) letInAssignments.Add(new ExpressionSyntax.AssignmentExpressionSyntax(functionParameters[i], parameters[i]));
    //evaluate as letin
    return EvaluateExpression(new ExpressionSyntax.LetInExpressionSyntax(letInAssignments,
    DefinedStuff.GetBody(CallingExpression.FunctionName.Text, CallingExpression.NumberOfParameters)));
  }

  public object GetVariableExpressionSyntax(ExpressionSyntax.VariableExpressionSyntax variableExpression){return DefinedStuff.GetVariable(variableExpression.VariableName);}
  
  public object GetAssignmentExpressionSyntax(ExpressionSyntax.AssignmentExpressionSyntax AssignmentExpression){throw new NotImplementedException();}
}

