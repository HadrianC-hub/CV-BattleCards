public static class Methods //Métodos variados útiles en el procesamiento del texto
{
    public static Action[] EvaluateExpression(Card Caster, Card Target, Card[] Board, Card[] P1Hand, Card[] P2Hand) //Evaluador de expresiones condicionales
    {
        Action[] newa = new Action[]{};
        foreach(Action old in Caster.Actions)
        {
            bool Possible = true;
            foreach(Condition oldC in old.Conditions)
            {
                oldC.Value=Evaluators.CheckConditionBool(oldC,Caster,Target,Board,P1Hand,P2Hand);
                if(!oldC.Value)
                {
                    Possible=false;
                }
            }
            if(Possible)
            {
                newa=Methods.AddAction(newa,old);
            }
        }
        return newa;
    }
    public static Card[] TargetCards(Card[] Board, Card Card)
    {
        Card[] GetCards = new Card[]{};
        for(int i = 0; i < Board.Length; i++)
        {
            while(Board[i]==Card)
            {
                if((i-4)>=0&&!(Board[i-4].Name==""))
                {
                    GetCards=Methods.AddCard(GetCards,Board[i-4]);
                }
                if((i-3)>=0&&!(Board[i-3].Name==""))
                {
                    GetCards=Methods.AddCard(GetCards,Board[i-3]);
                }
                if((i-2)>=0&&!(Board[i-2].Name==""))
                {
                    GetCards=Methods.AddCard(GetCards,Board[i-2]);
                }
                if((i-1)>=0&&!(Board[i-1].Name==""))
                {
                   GetCards=Methods.AddCard(GetCards,Board[i-1]);
                }
                if((i+4)<Board.Length&&!(Board[i+1].Name==""))
                {
                    GetCards=Methods.AddCard(GetCards,Board[i+4]);
                }
                if((i+3)<Board.Length&&!(Board[i+2].Name==""))
                {
                    GetCards=Methods.AddCard(GetCards,Board[i+3]);
                }
                if((i+2)<Board.Length&&!(Board[i+3].Name==""))
                {
                   GetCards=Methods.AddCard(GetCards,Board[i+2]);
                }
                if((i+1)<Board.Length&&!(Board[i+4].Name==""))
                {
                    GetCards=Methods.AddCard(GetCards,Board[i+1]);
                }
            }
        }
        return GetCards;
    }
    public static bool CanAttack(int[] Atk, int[] Tgt)
    {
        if(Atk.Length==0||Tgt.Length==0)
        {
            return false;
        }
        foreach(int atk in Atk)
        {
            foreach(int tgt in Tgt)
            {
                if(atk==1)
                {
                    if(tgt==2||tgt==4||tgt==5)
                    {
                        return true;
                    }
                }
                if(atk==2)
                {
                    if(tgt==1||tgt==4||tgt==5||tgt==6||tgt==3)
                    {
                        return true;
                    }
                }
                if(atk==3)
                {
                    if(tgt==2||tgt==6||tgt==5)
                    {
                        return true;
                    }
                }
                if(atk==4)
                {
                    if(tgt==1||tgt==2||tgt==5||tgt==8||tgt==7)
                    {
                        return true;
                    }
                }
                if(atk==5)
                {
                    if(tgt==1||tgt==2||tgt==3||tgt==4||tgt==6||tgt==7||tgt==8||tgt==9)
                    {
                        return true;
                    }
                }
                if(atk==6)
                {
                    if(tgt==3||tgt==2||tgt==5||tgt==8||tgt==9)
                    {
                        return true;
                    }
                }
                if(atk==7)
                {
                    if(tgt==8||tgt==4||tgt==5)
                    {
                        return true;
                    }
                }
                if(atk==8)
                {
                    if(tgt==7||tgt==4||tgt==5||tgt==6||tgt==9)
                    {
                        return true;
                    }
                }
                if(atk==9)
                {
                    if(tgt==8||tgt==6||tgt==5)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
    public static string[] Add(string[] main, string element)
    {
        if(element=="")
        {
            return main;
        }
        string[] second = new string[main.Length+1];
        for(int i = 0; i<main.Length; i++)
        {
            second[i] = main[i];
        }
        second[second.Length-1] = element;
        return second;
    }
    public static char[] AddChar(char[] main, char element)
    {
        char[] second = new char[main.Length+1];
        for(int i = 0; i<main.Length; i++)
        {
            second[i] = main[i];
        }
        second[second.Length-1] = element;
        return second;
    }
    public static int[] AddInt(int[] main, int element)
    {
        int[] second = new int[main.Length+1];
        for(int i = 0; i<main.Length; i++)
        {
            second[i] = main[i];
        }
        second[second.Length-1] = element;
        return second;
    }
    public static Card[] AddCard(Card[] main, Card element)
    {
        Card[] second = new Card[main.Length+1];
        for(int i = 0; i<main.Length; i++)
        {
            second[i] = main[i];
        }
        second[second.Length-1] = element;
        return second;
    }
    public static Action[] AddAction(Action[] main, Action element)
    {
        Action[] second = new Action[main.Length+1];
        for(int i = 0; i<main.Length; i++)
        {
            second[i] = main[i];
        }
        second[second.Length-1] = element;
        return second;
    }
    public static Condition[] AddCondition(Condition[] main, Condition element)
    {
        Condition[] second = new Condition[main.Length+1];
        for(int i = 0; i<main.Length; i++)
        {
            second[i] = main[i];
        }
        second[second.Length-1] = element;
        return second;
    }
    public static Effect[] AddEffect(Effect[] main, Effect element)
    {
        Effect[] second = new Effect[main.Length+1];
        for(int i = 0; i<main.Length; i++)
        {
            second[i] = main[i];
        }
        second[second.Length-1] = element;
        return second;
    }
    public static string[] StringSeparator(string x)
    {
        string[] Return = new string[]{};
        char[] word = x.ToCharArray();
        char[] count = new char[]{};
        for(int i = 0; i<word.Length; i++)
        {
            if(word[i]=='{')
            {
                Return = Add(Return,String.Concat(count));
                count = new char []{};
                Return = Add(Return,"{");
            }
            if(word[i]=='}')
            {
                Return = Add(Return,String.Concat(count));
                count = new char []{};
                Return = Add(Return,"}");
            }
            if(word[i]==':')
            {
                Return = Add(Return,String.Concat(count));
                count = new char []{};
                Return = Add(Return,":");
            }
            if(word[i]==',')
            {
                Return = Add(Return,String.Concat(count));
                count = new char []{};
                Return = Add(Return,",");
            }
            if(word[i]=='[')
            {
                Return = Add(Return,String.Concat(count));
                count = new char []{};
                Return = Add(Return,"[");
            }
            if(word[i]==']')
            {
                Return = Add(Return,String.Concat(count));
                count = new char []{};
                Return = Add(Return,"]");
            }
            if(word[i]!=':'&&word[i]!=','&&word[i]!='{'&&word[i]!='}')
            {
                count = AddChar(count, word[i]);
            }
            if(i+1==word.Length)
            {
                Return = Add(Return,String.Concat(count));
            }
        }
        return Return;
    }
    public static string[] CodeTransform(string code)
    {
        string[] Return = new string[]{};
        string[] Code = code.Split(new char[]{' ','\n','\t','\r'});
        foreach(string x in Code)
        {
            string[] y = Methods.StringSeparator(x);
            foreach(string z in y)
            {
                Return = Methods.Add(Return, z);
            }
        }
        return Return;
    }
    public static string SetAndConcat(char[] array, int init, int end)
    {
        char[] ret = new char[end-init+1];
        int a = 0;
        for(int i = init; i<=end; i++)
        {
            ret[a]=array[i];
            a++;
        }
        return String.Concat(ret);
    }
    public static Card[] RemoveFrom(Card[] a, Card b)
    {
        if(a.Contains(b))
        {
            Card[] c = new Card[]{};
            foreach(Card x in a)
            {
                if(x.Name!=b.Name)
                {
                    c=AddCard(c,x);
                }
            }
            return c;
        }
        else
        {
            return a;
        }
    }
}
