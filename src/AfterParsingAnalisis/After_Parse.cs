public class After_Parse    //Clase que realiza la revisión de expresiones
{
    public Card[] UCards;
    public Action[] UAction;
    public Effect[] UEffect;
    public Condition[] UCondition;
    public List<string> ErrorList;
    public bool Valid;
    public bool AnalyzeMath(string Math)    //Este método revisa las expresiones matemáticas y las valida para su posterior cálculo
    {
        char[] Array = Math.ToCharArray();
        int StartPar = 0;
        int EndPar = 0;
        foreach(char x in Array)
        {
            if(x=='(')
            {
                StartPar++;
            }
            if(x==')')
            {
                EndPar++;
            }
        }
        if(StartPar!=EndPar)
        {
            ErrorList.Add("Asegúrese de cerrar todos los paréntesis dentro de sus expresiones matemáticas");
            return false;
        }
        char last = '0';
        for(int i = 0; i<Array.Length; i++)
        {
            if(Array[i]!='+'&&Array[i]!='-'&&Array[i]!='*'&&Array[i]!='/')
            {
                if(Array[i]!='('&&Array[i]!=')')
                {
                    int a;
                    string y = Array[i].ToString();
                    if(!int.TryParse(y, out a))
                    {
                        if(Methods.SetAndConcat(Array, i, i+5)=="caster"||Methods.SetAndConcat(Array, i, i+5)=="target")
                        {
                            i=i+6;
                            if(i==Array.Length)
                            {
                                ErrorList.Add("Expresión referencial incompleta: "+Methods.SetAndConcat(Array, i-5, i));
                            }
                            else if(Methods.SetAndConcat(Array, i, i+6)==".health"||Methods.SetAndConcat(Array, i, i+6)==".energy"||Methods.SetAndConcat(Array, i, i+6)==".damage")
                            {
                                i=i+6;
                            }
                            else
                            {
                                ErrorList.Add("Referencia inválida a "+Methods.SetAndConcat(Array, i-5, i)+", debe referenciar solamente a propiedades parametrizables");
                            }
                        }
                        else
                        {
                            ErrorList.Add("El caracter "+Array[i]+" es inválido (debe referenciar a alguna propiedad o parámetro)");
                        }
                    }
                }
            }
            else
            {
                if(last=='+'||last=='-'||last=='*'||last=='/')
                {
                    ErrorList.Add("No debe introducir dos operadores seguidos, si quiere hacer un cálculo con números negativos intente introducirlo entre paréntesis");
                }
            }
            last = Array[i];
        }
        if(ErrorList.Count!=0)
        {
            return false;
        }
        return true;
    }
    public Card[] QualifyCards()    //Este método devuelve todas las cartas que presenten expresiones correctas
    {
        Card[] Qualified = new Card[]{};
        foreach(Card x in GameList.AllCards)
        {
            foreach(Action a in x.Actions)
            {
                if(!UAction.Contains(a))
                {
                    ErrorList.Add("La carta "+x.Name+" contiene a la acción "+a.ID+", que tiene una expresión inválida");
                }
            }
            if(ErrorList.Count==0)
            {
                Qualified=Methods.AddCard(Qualified, x);
            }
        }
        return Qualified;
    }
    public Action[] QualifyAction()     //Este método devuelve todas las acciones que presenten expresiones correctas
    {
        Action[] Qualified = new Action[]{};
        foreach(Action x in GameList.AllActions)
        {
            foreach(Condition c in x.Conditions)
            {
                if(!UCondition.Contains(c))
                {
                    ErrorList.Add("La acción "+x.ID+" contiene a la condición "+c.id+", que tiene una expresión inválida");
                }
            }
            foreach(Effect e in x.Effects)
            {
                if(!UEffect.Contains(e))
                {
                    ErrorList.Add("La acción "+x.ID+" contiene al efecto "+e.id+", que tiene una expresión inválida");
                }
            }
            if(ErrorList.Count==0)
            {
                Qualified = Methods.AddAction(Qualified, x);
            }
        }
        
        return Qualified;
    }   
    public Effect[] QualifyEffect()     //Este método devuelve todos los efectos que presenten expresiones correctas
    {
        Effect[] Qualified = new Effect[]{};
        foreach(Effect x in GameList.AllEffects)
        {
            if(x.Expression.Contains('|'))
            {
                ErrorList.Add("El efecto "+x.id+" no puede contener una condicional or (|)");
            }
            string expression = x.Expression;
            if(expression!="")
            {
                string[] Exp = expression.Split('=');
                if(Exp.Length!=2)
                {
                    ErrorList.Add("La expresión de su efecto "+x.id+" debe contener solo una asignación de tipo (=)");
                }
                if(Exp[0].StartsWith("caster.")||Exp[0].StartsWith("target."))
                {
                    string[] Analyze = Exp[0].Split('.');
                    if(Analyze[1].StartsWith("health")||Analyze[1].StartsWith("energy")||Analyze[1].StartsWith("damage"))
                    {
                        int a;
                        if(!int.TryParse(Exp[1], out a))
                        {
                            if(Exp[1].StartsWith("("))
                            {
                                char[] Exp02 = Exp[1].ToCharArray();
                                char[] Exp00 = new char[Exp02.Length-2];
                                for(int i = 0; i<Exp00.Length; i++)
                                {
                                    Exp00[i] = Exp02[i+1];
                                }
                                string Exp01 = String.Concat(Exp00);
                                if(!AnalyzeMath(Exp01))
                                {
                                    ErrorList.Add("La expresion matemática del miembro derecho de su efecto "+x.id+" es inválida");
                                }
                            }
                            else
                            {
                                ErrorList.Add("El miembro derecho de la expresión de su efecto "+x.id+" debe ser in tipo (int) o una expresión matemática contenida en ()");
                            }
                        }
                    }
                    else
                    {
                        ErrorList.Add("El miembro izquiedo de la expresión de su efecto "+x.id+" debe contener una asignación válida");
                    }
                }
                else
                {
                    ErrorList.Add("La expresión "+x.id+" debe contener un miembro izquierdo válido");
                }
            }
            if(ErrorList.Count==0)
            {
                Qualified = Methods.AddEffect(Qualified, x);
            }
        }
        return Qualified;
    }
    public Condition[] QualifyCondition()   //Este método devuelve todas las condicionales que presenten expresiones correctas
    {
        Condition[] Qualified = new Condition[]{};
        foreach(Condition x in GameList.AllConditions)
        {
            string[] OrExpression = x.Expression.Split('|');
            foreach(string expression in OrExpression)
            {
                if(expression!="")
                {
                    string[] Exp = new string[]{};
                    if(!expression.Contains('=')&&!expression.Contains('<')&&!expression.Contains('>')&&!expression.Contains("=>")&&!expression.Contains("<="))
                    {
                        ErrorList.Add("La expresión de su condicional "+x.id+" debe contener un booleano (<,>,=,<=,=>");
                    }
                    else
                    {
                        if(expression.Contains('='))
                        {
                            Exp = expression.Split('=');
                        }
                        if(expression.Contains('<'))
                        {
                            Exp = expression.Split('<');
                        }
                        if(expression.Contains('>'))
                        {
                            Exp = expression.Split('>');
                        }
                        if(expression.Contains("<="))
                        {
                            Exp = expression.Split(new char[]{'<','='});
                        }
                        if(expression.Contains("=>"))
                        {
                            Exp = expression.Split(new char[]{'>','='});
                        }
                        if(Exp.Length!=2)
                        {
                            ErrorList.Add("Su condicional "+x.id+" debe contener solo un comparador (<,>,=,<=,=>) por cada expresión");
                        }
                        if(Exp[0].StartsWith("target.")||Exp[0].StartsWith("caster."))
                        {
                            string[] Analyze = Exp[0].Split('.');
                            if(Analyze[1].StartsWith("health")||Analyze[1].StartsWith("energy")||Analyze[1].StartsWith("damage"))
                            {
                                int a;
                                if(!int.TryParse(Exp[1], out a))
                                {
                                    if(Exp[1].StartsWith("("))
                                    {
                                        char[] Exp02 = Exp[1].ToCharArray();
                                        char[] Exp00 = new char[Exp02.Length-2];
                                        for(int i = 0; i<Exp00.Length; i++)
                                        {
                                            Exp00[i] = Exp02[i+1];
                                        }
                                        string Exp01 = String.Concat(Exp00);
                                        if(!AnalyzeMath(Exp01))
                                        {
                                            ErrorList.Add("La expresion matemática del miembro derecho de su condicional "+x.id+" es inválida");
                                        }
                                    }
                                    else
                                    {
                                        ErrorList.Add("El miembro derecho de la expresión de su condicional "+x.id+" debe ser in tipo (int) o una expresión matemática contenida en []");
                                    }
                                }
                            }
                            else
                            {
                                ErrorList.Add("El miembro izquiedo de la expresión de su condicional "+x.id+" debe contener una asignación válida");
                            }
                        }
                        if(Exp[0].StartsWith("player.")||Exp[0].StartsWith("enemy."))
                        {
                            string[] Analyze = Exp[0].Split('.');
                            if(Analyze[1].StartsWith("anycard")||Analyze[1].StartsWith("allcards"))
                            {
                                if(Analyze.Length<3)
                                {
                                    ErrorList.Add("El miembro izquiedo de la expresión "+x.id+" debe contener una asignación válida");
                                }
                                if(Analyze[2].StartsWith("health")||Analyze[2].StartsWith("energy")||Analyze[2].StartsWith("damage"))
                                {
                                    int a;
                                    if(!int.TryParse(Exp[1], out a))
                                    {
                                        if(Exp[1].StartsWith("("))
                                        {
                                            char[] Exp02 = Exp[1].ToCharArray();
                                            char[] Exp00 = new char[Exp02.Length-2];
                                            for(int i = 0; i<Exp00.Length; i++)
                                            {
                                                Exp00[i] = Exp02[i+1];
                                            }
                                            string Exp01 = String.Concat(Exp00);
                                            if(!AnalyzeMath(Exp01))
                                            {
                                                ErrorList.Add("La expresion matemática del miembro derecho de su condicional "+x.id+" es inválida");
                                            }
                                        }
                                        else
                                        {
                                            ErrorList.Add("El miembro derecho de la expresión de su condicional "+x.id+" debe ser in tipo (int) o una expresión matemática contenida en []");
                                        }
                                    }
                                }
                            }
                            else
                            {
                                ErrorList.Add("El miembro izquiedo de la expresión de su condicional "+x.id+" debe contener una asignación válida");
                            }
                        }
                        if(Exp[0].StartsWith("field.cards"))
                        {
                            int a;
                            if(!int.TryParse(Exp[1],out a))
                            {
                                ErrorList.Add("El valor asignado a su condicional "+x.id+" debe ser de tipo (int)");
                            }
                            else
                            {
                                if(a<1||a>9)
                                {
                                    ErrorList.Add("El valor asignado a su condicional "+x.id+" debe estar entre 1 y 9");
                                }
                            }
                        }
                    }    
                }
            }
            if(ErrorList.Count==0)
            {
                Qualified = Methods.AddCondition(Qualified, x);
            }
        }
        return Qualified;
    }
    public After_Parse(ParserCheck x)
    {
        ErrorList=x.ErrorList;
        if(ErrorList.Count!=0)
        {
            ErrorList.Add("No se puede hacer el análisis de expresiones");
            UEffect=new Effect[]{};
            UCondition=new Condition[]{};
            UAction=new Action[]{};
            UCards=new Card[]{};
        }
        else
        {
            UEffect=QualifyEffect();
            UCondition=QualifyCondition();
            UAction=QualifyAction();
            UCards=QualifyCards();
        }
        Valid = ErrorList.Count==0;
    }
}
