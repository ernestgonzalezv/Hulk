
public class Lexer{ // break it into tokens


    private string _text;
    private int _position;
    private List<string> _diagnostics = new List<string>(); //for errors
    public Lexer(string text){
        _text=text;
    }

    public IEnumerable<string> Diagnostics => _diagnostics;
    private char Current => Peek(0);
    private char Lookahead => Peek(1);
    private char Lookback => Peek(-1);

    private char Peek(int offset)
    {
        var index = _position+offset;
        if(index >= _text.Length || index < 0 )
            return '\0';
        return _text[index];
    }

    private void Next(){
        _position++;
    }

    public SyntaxToken NextToken()
    {
        // numbers
        // +-/*() 
        //whitespace
        if(_text[_text.Length-1] != ';' && _text[_text.Length-1]!='{') throw new Exception("EveryLine must end with a semicolon");
        if(_position >= _text.Length){
            return new SyntaxToken(SyntaxKind.EndOfFileToken, _position, "\0", null);
        }
        if(char.IsDigit(Current) || Current=='.')
        {
            var start = _position;
            while (char.IsDigit(Current) || Current == '.')
                Next();
            var length = _position - start;
            var text = _text.Substring(start, length);
            if(!double.TryParse(text, out var value)){
                throw new Exception($"The number {_text} cannot be represented by an double");
            }
            
            return new SyntaxToken(SyntaxKind.IntegerToken, start, text,value);
        }

        if(char.IsWhiteSpace(Current)){
            var start = _position;
            while (char.IsWhiteSpace(Current))
                Next();
            var length = _position - start;
            var text = _text.Substring(start, length);
            return new SyntaxToken(SyntaxKind.WhiteSpaceToken, start, text,null);
        }

        if(char.IsLetter(Current)){
            var start = _position;
            while(char.IsLetter(Current)){
            
                Next();
            }
            var length = _position- start;
            var text = _text.Substring(start, length);
            
            //turn constants into numbers
            if(text == "PI"){
                if(!double.TryParse(Math.PI.ToString(), out var value)){
                throw new Exception($"The number {_text} cannot be represented by an double");
            }
            
            return new SyntaxToken(SyntaxKind.IntegerToken, start, text,value);

            } 
            if(text == "E"){
                if(!double.TryParse(Math.E.ToString(), out var value)){
                throw new Exception($"The number {_text} cannot be represented by an double");
            }
                return new SyntaxToken(SyntaxKind.IntegerToken, start, text,value);

            }

            if(text == "function")
            {
                var functionStatement = _text.Substring(_position+1, _text.Length-_position-1);
                var name = "";
                var parameters = "";
                var body = "";
                int i =0; 
                for(;functionStatement[i] != '(' ; i++){name+=functionStatement[i];}
                i++;
                for(;functionStatement[i] != ')' ; i++){parameters+=functionStatement[i];}
                for(;functionStatement[i]!='>';i++);
                i++;
                for(;i<functionStatement.Length;i++){body+=functionStatement[i];}
                Program.functions.Add(name,(parameters,body));
                _position = text.Length;
               if(!double.TryParse("1", out var value)){
                  throw new Exception($"The number {_text} cannot be represented by an double");
                }
            
                 return new SyntaxToken(SyntaxKind.IntegerToken, start, "1",value); 
                 //fix this
            }
            
            return new SyntaxToken(SyntaxKind.FunctionExpressionCallingToken, start , text, null);
            
        }

        if(Current == '+'){ return new SyntaxToken(SyntaxKind.PlusToken, _position++, "+",null);}
        else if(Current == '='){
            if(Lookahead == '>') {
                return new SyntaxToken(SyntaxKind.ArrowToken, _position+=2, "=>", null);
            } 
            return new SyntaxToken(SyntaxKind.EqualsToken, _position++, "=",null);}
        else if(Current == ','){ return new SyntaxToken(SyntaxKind.CommaToken, _position++, ",",null);}
        //else if(Current == '>'){ return new SyntaxToken(SyntaxKind.HigherToken, _position++, "=",null);}
        else if(Current == '-'){ return new SyntaxToken(SyntaxKind.MinusToken, _position++, "-",null);}
        else if(Current == '{'){ return new SyntaxToken(SyntaxKind.OpenBodyToken, _position++, "{",null);}
        else if(Current == '}'){ return new SyntaxToken(SyntaxKind.CloseBodyToken, _position++, "}",null);}
        else if(Current == '*'){ return new SyntaxToken(SyntaxKind.StarToken, _position++, "*",null);}
        else if(Current == '/'){ return new SyntaxToken(SyntaxKind.SlashToken, _position++, "/",null);}
        else if(Current == '^'){ return new SyntaxToken(SyntaxKind.ExponentialToken, _position++, "^",null);}
        else if(Current == '('){ return new SyntaxToken(SyntaxKind.OpenParenthesisToken, _position++, "(",null);}
        else if(Current == ')'){ return new SyntaxToken(SyntaxKind.CloseParenthesisToken, _position++, ")",null);}
        else if(Current == ';'){ return new SyntaxToken(SyntaxKind.EndOfLineToken, _position++, ";",null);}
        throw new Exception($"ERROR: bad token in input: '{Current}'");
        return new SyntaxToken(SyntaxKind.BadToken, _position++, _text.Substring(_position-1,1), null);
    }
}
