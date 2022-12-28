public static class ParseFunctions
{
    //Métodos para crear cartas, acciones, efectos y condiciones con el código
    public static Card CreateCard(string[] code, Action[] AllActions)
    {   
        char[] c = code[0].ToCharArray();
        char[] d = new char[c.Length-1];
        for(int i = 0; i<d.Length; i++)
        {
            d[i]=c[i];
        }
        string Name = String.Concat(d);
        int H = 0;
        int E = 0;
        int D = 0;
        Action[] Act = new Action[]{};
        for(int i = 0; i<code.Length; i++)
        {
            if(code[i]=="health")
            {
                H=int.Parse(code[i+2]);
            }
            if(code[i]=="energy")
            {
                E=int.Parse(code[i+2]);
            }
            if(code[i]=="damage")
            {
                D=int.Parse(code[i+2]);
            }
            if(code[i]=="actions")
            {
                string[] actions = code[i+2].Split('(');
                string[] analyze = new string []{};
                foreach (string x in actions)
                {
                    if(x!="")
                    {
                        char[] x1 = x.ToCharArray();
                        char[] y1 = new char[x1.Length-1];
                        for(int j = 0; j<y1.Length; j++)
                        {
                            y1[j]=x1[j];
                        }
                        string y = String.Concat(y1);
                        analyze = Methods.Add(analyze, y);
                    }
                }
                foreach(Action g in AllActions)
                {
                    for(int j=0; j<analyze.Length; j++)
                    {
                        if(g.ID==analyze[j])
                        {
                            Act = Methods.AddAction(Act, g);
                        }
                    }
                }
            }
        }
        Card z = new Card(Name, Act, H, E, D);
        return z;
    }
    public static Action CreateAction(string[] code, Condition[] AllConditions, Effect[] AllEffects)
    {
        char[] c = code[0].ToCharArray();
        char[] d = new char[c.Length-1];
        for(int i = 0; i<d.Length; i++)
        {
            d[i]=c[i];
        }
        string Name = String.Concat(d);
        Condition[] Cond = new Condition[]{};
        Effect[] Eff = new Effect[]{};
        for(int i = 0; i<code.Length; i++)
        {
            if(code[i]=="effects")
            {
                string[] effects = code[i+2].Split('(');
                string[] analyze = new string []{};
                foreach (string x in effects)
                {
                    if(x!="")
                    {
                        char[] x1 = x.ToCharArray();
                        char[] y1 = new char[x1.Length-1];
                        for(int j = 0; j<y1.Length; j++)
                        {
                            y1[j]=x1[j];
                        }
                        string y = String.Concat(y1);
                        analyze = Methods.Add(analyze, y);
                    }
                }
                foreach(Effect g in AllEffects)
                {
                    for(int j=0; j<analyze.Length; j++)
                    {
                        if(g.id==analyze[j])
                        {
                            Eff = Methods.AddEffect(Eff, g);
                        }
                    }
                }
            }
            if(code[i]=="conditions")
            {
                string[] conditions = code[i+2].Split('(');
                string[] analyze = new string []{};
                foreach (string x in conditions)
                {
                    if(x!="")
                    {
                        char[] x1 = x.ToCharArray();
                        char[] y1 = new char[x1.Length-1];
                        for(int j = 0; j<y1.Length; j++)
                        {
                            y1[j]=x1[j];
                        }
                        string y = String.Concat(y1);
                        analyze = Methods.Add(analyze, y);
                    }
                }
                foreach(Condition g in AllConditions)
                {
                    for(int j=0; j<analyze.Length; j++)
                    {
                        if(g.id==analyze[j])
                        {
                            Cond = Methods.AddCondition(Cond, g);
                        }
                    }
                }
            }
        }
        Action z = new Action(Name, Cond, Eff);
        return z;
    }
    public static Condition CreateCondition(string[] code)
    {
        char[] c = code[0].ToCharArray();
        char[] d = new char[c.Length-1];
        for(int i = 0; i<d.Length; i++)
        {
            d[i]=c[i];
        }
        string Name = String.Concat(d);
        Condition X = new Condition (Name, code[4]);
        return X;
    }
    public static Effect CreateEffect(string[] code)
    {
        char[] c = code[0].ToCharArray();
        char[] d = new char[c.Length-1];
        for(int i = 0; i<d.Length; i++)
        {
            d[i]=c[i];
        }
        string Name = String.Concat(d);
        Effect X = new Effect (Name, code[4]);
        return X;
    }
    //Métodos para extraer del código la base de cada objeto
    public static string[] CardExtract(string[] code)
    {
        string[] Return = new string[]{};
        for(int i = 0; i<code.Length; i++)
        {
            if(code[i]=="card")
            {
                for(int j = i+1; code[j]!="}"; j++)
                {
                    Return=Methods.Add(Return, code[j]);
                }
                Return=Methods.Add(Return, "}");
            }
        }
        return Return;
    }
    public static string[] ActionExtract(string[] code)
    {
        string[] Return = new string[]{};
        for(int i = 0; i<code.Length; i++)
        {
            if(code[i]=="action")
            {
                for(int j = i+1; code[j]!="}"; j++)
                {
                    Return=Methods.Add(Return, code[j]);
                }
                Return=Methods.Add(Return, "}");
            }
        }
        return Return;
    }
    public static string[] ConditionExtract(string[] code)
    {
        string[] Return = new string[]{};
        for(int i = 0; i<code.Length; i++)
        {
            if(code[i]=="condition")
            {
                for(int j = i+1; code[j]!="}"; j++)
                {
                    Return=Methods.Add(Return, code[j]);
                }
                Return=Methods.Add(Return, "}");
            }
        }
        return Return;
    }   
    public static string[] EffectExtract(string[] code)
    {
        string[] Return = new string[]{};
        for(int i = 0; i<code.Length; i++)
        {
            if(code[i]=="effect")
            {
                for(int j = i+1; code[j]!="}"; j++)
                {
                    Return=Methods.Add(Return, code[j]);
                }
                Return=Methods.Add(Return, "}");
            }
        }
        return Return;
    }
}