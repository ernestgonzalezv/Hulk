public abstract class ExpressionSyntax
{
  
  public class UnaryExpressionSyntax : ExpressionSyntax
  {
    public SyntaxToken OperatorToken;
    public ExpressionSyntax Expression;

    public UnaryExpressionSyntax(SyntaxToken operatorToken, ExpressionSyntax expression)
    {
      OperatorToken = operatorToken;
      Expression = expression;
    }

    public override R GetChildren<R>(Children<R> U)
    {
      return U.GetUnaryExpressionSyntax(this);
    }
  }
  public class CallingExpressionSyntax : ExpressionSyntax
  {
    public SyntaxToken FunctionName;
    public List<ExpressionSyntax> FunctionParameters;
    public int NumberOfParameters => FunctionParameters.Count;


    public CallingExpressionSyntax(SyntaxToken functionName, List<ExpressionSyntax> functionParameters)
    {
      FunctionName = functionName;
      FunctionParameters = functionParameters;
    }
    public override R GetChildren<R>(Children<R> C)
    {
      return C.GetCallingExpressionSyntax(this);
    }
  }
  public class AssignmentExpressionSyntax : ExpressionSyntax
  {
    public ExpressionSyntax VariableValue;
    public SyntaxToken VariableName;
    public AssignmentExpressionSyntax(SyntaxToken variableName, ExpressionSyntax variableValue)
    {
      VariableValue = variableValue;
      VariableName = variableName;
    }

    public override R GetChildren<R>(Children<R> A)
    {
      return A.GetAssignmentExpressionSyntax(this);
    }
  }
  public class LiteralExpressionSyntax : ExpressionSyntax
  {
    public object LiteralToken;
    public LiteralExpressionSyntax(object literalToken)
    {
      LiteralToken = literalToken;
    }

    public override R GetChildren<R>(Children<R> L)
    {
      return L.GetLiteralExpressionSyntax(this);
    }
  }

  public class FunctionExpressionSyntax : ExpressionSyntax
  {
    public SyntaxToken FunctionName;
    public List<SyntaxToken> FunctionParameters;
    public ExpressionSyntax FunctionBody;
    public int NumberOfParameters => FunctionParameters.Count;
    public bool CallAgain;

    public FunctionExpressionSyntax(SyntaxToken functionName, List<SyntaxToken> functionParameters, 
    ExpressionSyntax functionBody, bool callAgain = false)
    {
      FunctionName = functionName;
      FunctionParameters = functionParameters;
      FunctionBody = functionBody;
      CallAgain = callAgain;
    }
    public override R GetChildren<R>(Children<R> F)
    {
      return F.GetFunctionExpressionSyntax(this);
    }
  }
  public class VariableExpressionSyntax : ExpressionSyntax
  {
    public SyntaxToken VariableName;

    public VariableExpressionSyntax(SyntaxToken variableName)
    {
      VariableName = variableName;
    }

    public override R GetChildren<R>(Children<R> V)
    {
      return V.GetVariableExpressionSyntax(this);
    }
  }

  public class BinaryExpressionSyntax : ExpressionSyntax
  {
    public ExpressionSyntax Left;
    public SyntaxToken OperatorToken;
    public ExpressionSyntax Right;

    public BinaryExpressionSyntax(ExpressionSyntax left, SyntaxToken operatorToken, ExpressionSyntax right)
    {
      Left = left;
      OperatorToken = operatorToken;
      Right = right;
    }

    public override R GetChildren<R>(Children<R> B)
    {
      return B.GetBinaryExpressionSyntax(this);
    }
  }
  public class LetInExpressionSyntax : ExpressionSyntax
  {
    public List<AssignmentExpressionSyntax> VariableAssignments;
    public ExpressionSyntax AfterIn;

    public LetInExpressionSyntax(List<AssignmentExpressionSyntax> variableAssignments, ExpressionSyntax afterIn)
    {
      VariableAssignments = variableAssignments;
      AfterIn = afterIn;
    }

    public override R GetChildren<R>(Children<R> L)
    {
      return L.GetLetInExpressionSyntax(this);
    }
  }
  public class ConditionalExpressionSyntax : ExpressionSyntax
  {
    public ExpressionSyntax Condition;
    public ExpressionSyntax Response;
    public ExpressionSyntax ElseExpression;

    public ConditionalExpressionSyntax(ExpressionSyntax condition, ExpressionSyntax response, ExpressionSyntax elseExpression)
    {
      Condition = condition;
      Response = response;
      ElseExpression = elseExpression;
    }

    public override R GetChildren<R>(Children<R> C)
    {
      return C.GetConditionalExpressionSyntax(this);
    }
  }
  public abstract R GetChildren<R>(Children<R> visitor);
  public interface Children<R>
  {
    R GetUnaryExpressionSyntax(UnaryExpressionSyntax UnaryExpression);
    R GetFunctionExpressionSyntax(FunctionExpressionSyntax FunctionExpression);
    R GetLiteralExpressionSyntax(LiteralExpressionSyntax LiteralExpression);
    R GetCallingExpressionSyntax(CallingExpressionSyntax CallingExpression);
    R GetAssignmentExpressionSyntax(AssignmentExpressionSyntax AssignmentExpression);
    R GetVariableExpressionSyntax(VariableExpressionSyntax VariableExpression);
    R GetLetInExpressionSyntax(LetInExpressionSyntax LetInExpression);
    R GetBinaryExpressionSyntax(BinaryExpressionSyntax BinaryExpression);
    R GetConditionalExpressionSyntax(ConditionalExpressionSyntax ConditionalExpression);
  }

  
}
