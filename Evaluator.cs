public class Evaluator {
    
    private readonly ExpressionSyntax _root;

    public Evaluator(ExpressionSyntax root ){
        this._root = root;
    }

    public double Evaluate()
    {
        return EvaluateExpression(_root);
    }

    public double EvaluateExpression(ExpressionSyntax root)
    {

        
        if(root is LiteralExpressionSyntax n){
            return (double)((n.LiteralToken.Value));
        }

        if (root is UnaryExpressionSyntax u)
        {
            var operand = EvaluateExpression(u.Operand);
            switch (u.Operator.Kind){
                case SyntaxKind.PlusToken:
                    return (double)operand;
                case SyntaxKind.MinusToken:
                    return -(double)operand;
                throw new Exception("bro");
            }

        }
        if(root is BinaryExpressionSyntax b)
        {
            
            
            var right = EvaluateExpression(b.Right);
            var left = EvaluateExpression(b.Left);
            switch(b.Operator.Kind)
            {
                case SyntaxKind.PlusToken:
                    return  (double)( left) + (double)( right) ;
                case SyntaxKind.MinusToken:
                    return (double)( left) - (double)( right) ;
                case SyntaxKind.StarToken:
                    return (double)( left) * (double)( right) ;
                case SyntaxKind.SlashToken:
                    return (double)( left) / (double)( right) ;
                case SyntaxKind.ExponentialToken:
                    return  Math.Pow((double)( left) , (double)( right));
                default:
                    return -1;
                throw new Exception("bro");
            }
        }
        if(root is ParenthesizedExpression p)
        {
            return EvaluateExpression(p.Expression);
        }
        if(root is FunctionExpressionCalling f) {
            var e = 1.2;
            if(f.Parameters!="")
                e = EvaluateExpression(f.Expression);
            switch(f.Text){
                case "print":
                    //var e = EvaluateExpression(f.Expression); 
                    Console.WriteLine((e));
                    return 0;
                case "sin":
            
                    return Math.Sin((e));
                case "cos": 
                    
                    return Math.Cos((e));
                case "sqrt":
                    
                    return Math.Sqrt((e));
                case "exp":
                    
                    return Math.Exp((e));
                default :
                    //check name 
                    //separate parameters  and replace em 
                    //evaluate
                    
                    if(Program.functions.ContainsKey(f.Text)){
                        string line1 =Program.functions[f.Text].Item2;
                        if(f.Parameters!=""){ // if u actually get parameters
                            var parameters = Program.functions[f.Text].Item1.Split(",");
                            
                            var parameters1 = f.Parameters.Split(",");
                            //for(int i =0 ; i<parameters1.Length;i++)Console.WriteLine(parameters1[i]);
                            for(int i=0;i<parameters1.Length;i++){
                                line1 = line1.Replace(parameters[i], parameters1[i]);
                            }
                        }
                        var parser= new Parser(line1);
                        parser.Parse();
                        var expression = parser.ans; 
                        return EvaluateExpression(expression);
                    }else{
                        Console.WriteLine(f.Text);
                        throw new Exception("NO key for this function, not defined");
                    }
                    return -1;

            }
        }
        if(root is AssignmentExpressionSyntax a)
        {   
            Console.WriteLine(a.Left +" = " + EvaluateExpression(a.Right));

            Program.variables.Add(a.Left, a.Right.ToString());
            //Console.WriteLine(EvaluateExpression(a.Right));
            return 0 ;
        } 
        throw new Exception("bro");
    }

}