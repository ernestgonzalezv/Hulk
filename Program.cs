class Program{
    public static Dictionary <string,(string,string)> functions = new Dictionary <string, (string,string)>();
    public static Dictionary<string,string> variables = new Dictionary<string,string>();
    //
    
    static void Main()
    {
        //functions.Add("sina", ("x,y", "(sin(x+1));"));
        functions.Add("op", ("s1,s2", "(s1+s2-s1-s2+s1+s2);"));
        Console.WriteLine(functions.ContainsKey("op"));
        while(true){
        string _text = Console.ReadLine();
        {EvaluateLine(_text);}
        }
    }

    public static void EvaluateLine(string text)
    {
    Parser parser = new Parser(text);
    parser.Parse();
    var expression = parser.ans;
    //Print(expression);
    var evaluator = new Evaluator(expression);
    evaluator.Evaluate();
    
    }

    static void Print( SyntaxNode node, string indent = "" )
{
    Console.Write(indent);
    Console.Write(node.Kind);
    if( node is SyntaxToken t && t.Value!=null)
    {
        Console.Write(" ");
        Console.Write(t.Value);
    }
    Console.WriteLine();
    indent += "    ";
    foreach(var child in node.GetChildren())
    {
        Print(child,indent);
    }

}
}

