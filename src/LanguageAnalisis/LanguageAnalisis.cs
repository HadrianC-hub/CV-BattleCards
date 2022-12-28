public class LanguageAnalisis
{
    public string code;
    public bool valid;
    public List<string> ErrorList;
    public LanguageAnalisis(string code)
    {
        this.code=code;
        this.ErrorList=Errors(code, KeyWords.Keys, KeyWords.Symbol, KeyWords.Words);
        this.valid=Correct(ErrorList);
    }
    public bool Correct(List<string> errors)    //Método que verifica si mi lenguaje está bien escrito
    {
        if(errors.Count!=0)
        {
            return false;
        }
        return true;
    }
    public List<string> Errors(string MainCode, List<string> Keys, char[] Symbol, List<string> Words)   //Método que devuelve los errores de compilación del código
    {
        string Evaluate(string word, List<string> Keys, char[] Symbol, List<string> Words)    //Este método evalua el tipo de una palabra o caracter en el lenguaje
        {
            if(word==Symbol[0].ToString())
            {
                return "open";
            }
            if(word==Symbol[1].ToString())
            {
                return "closed";
            }
            if(word==Symbol[2].ToString())
            {
                return "assign";
            }
            if(word==Symbol[3].ToString())
            {
                return "separated";
            }
            if(Keys.Contains(word))
            {
                return "key";
            }
            if(Words.Contains(word))
            {
                return "word";
            }
            if(word[word.Length-1]=='.')
            {
                return "definition";
            }
            else
            {
                return "value";
            }
        }
        string[] code = Methods.CodeTransform(MainCode);
        List<string> Errors = new List<string>(){};
        if(code.Length==0)
        {
            Errors.Add("El código está vacío");
            return Errors;
        }
        string previous = "null";
        string actual = Evaluate(code[0],Keys,Symbol,Words);
        string next = Evaluate(code[1],Keys, Symbol, Words);
        if(actual!="key")
        {
            Errors.Add("Su código debe comenzar con una llave de edición (key) = (card/action/condition/effect)");
            return Errors;
        }
        if(next!="definition")
        {
            Errors.Add("Su primera key debe estar definida, para insertar una definición introduzca el símbolo (.) al final del valor de definición (nombre de la clase)");
            return Errors;
        }
        for(int i = 2; i<code.Length-1; i++)
        {
            previous = actual;
            actual = next;
            next = Evaluate(code[i], Keys, Symbol, Words);
            if(!KeyWords.Check(previous, actual, next))
            {
                if(actual=="key")
                {
                    Errors.Add("La key "+code[i]+" debe estar definida, para ello declare el nombre con el símbolo (.) al final");
                    i++;
                    previous = actual;
                    actual = next;
                    next = Evaluate(code[i], Keys, Symbol, Words);
                }
                if(actual=="definition")
                {
                    Errors.Add("La definición "+code[i]+" debe ser continuada por una asignación (use ({}) para definir Key || Use (:) para definir words (stats))");
                    i++;
                    previous = actual;
                    actual = next;
                    next = Evaluate(code[i], Keys, Symbol, Words);
                }
                if(actual=="open")
                {
                    Errors.Add("La asignación "+code[i]+" es inválida");
                    i++;
                    previous = actual;
                    actual = next;
                    next = Evaluate(code[i], Keys, Symbol, Words);
                }
                if(actual=="word")
                {
                    Errors.Add("El parámetro "+code[i]+" debe poseer una asignación");
                    i++;
                    previous = actual;
                    actual = next;
                    next = Evaluate(code[i], Keys, Symbol, Words);
                }
                if(actual=="assign")
                {
                    Errors.Add("Debe asignar un valor al parámetro "+code[i-1]);
                    i++;
                    previous = actual;
                    actual = next;
                    next = Evaluate(code[i], Keys, Symbol, Words);
                }
                if(actual=="value")
                {
                    Errors.Add("Debe introducir el separador (,) tras el valor "+code[i+1]+" antes de asignar otro parámetro o finalizar las asignaciones");
                    i++;
                    previous = actual;
                    actual = next;
                    next = Evaluate(code[i], Keys, Symbol, Words);
                }
                if(actual=="closed")
                {
                    Errors.Add("Tras cerrar la asignación debe introducir otra key o finalizar el código si así lo desea");
                    i++;
                    previous = actual;
                    actual = next;
                    next = Evaluate(code[i], Keys, Symbol, Words);
                }
                if(actual=="separated")
                {
                    Errors.Add("Debe introducir otro parámetro o concluir la edición de la key (})");
                    i++;
                    previous = actual;
                    actual = next;
                    next = Evaluate(code[i], Keys, Symbol, Words);
                }
            }
        } 
        return Errors;
    }   
}