public interface Evaluators  //Interfaz de métodos que se encargan de regular y evaluar expresiones de condiciones y efectos durante la partida
{
    public static Card Regulate(Card a)
    {
        Card b = a;
        if(b.Health<0)
        {
            b.Health=0;
        }
        if(b.Health>b.BaseHealth)
        {
            b.Health=b.BaseHealth;
        }
        if(b.Energy<0)
        {
            b.Energy=0;
        }
        if(b.Energy>b.BaseEnergy)
        {
            b.Energy=b.BaseEnergy;
        }
        if(b.Damage<0)
        {
            b.Damage=0;
        }
        if(b.Damage>b.BaseDamage)
        {
            b.Damage=b.BaseDamage;
        }
        return b;
    }
    public static (Card, Card) ExecuteEffect(Card Caster, Card Target, Action Act)  //Metodo que ejecuta el efecto entre dos cartas
    {
        Dictionary<string, int> Index = new Dictionary<string, int>(){};
        Index.Add("caster.health",Caster.Health);
        Index.Add("caster.energy",Caster.Energy);
        Index.Add("caster.damage",Caster.Damage);
        Index.Add("target.health",Target.Health);
        Index.Add("target.energy",Target.Energy);
        Index.Add("target.damage",Target.Damage);
        Effect[] Action = Act.Effects;
        Card NewCaster = Caster;
        Card NewTarget = Target;
        foreach(Effect x in Action)
        {
            string[] Splitted = x.Expression.Split('=');
            if(Splitted[0]=="caster.health")
            {
                NewCaster.Health=AnalyzeRightMember(Splitted[1],NewCaster,NewTarget,Index);
            }
            else if(Splitted[0]=="caster.energy")
            {
                NewCaster.Energy=AnalyzeRightMember(Splitted[1],NewCaster,NewTarget,Index);
            }
            else if(Splitted[0]=="caster.damage")
            {
                NewCaster.Damage=AnalyzeRightMember(Splitted[1],NewCaster,NewTarget,Index);
            }
            else if(Splitted[0]=="target.health")
            {
                NewTarget.Health=AnalyzeRightMember(Splitted[1],NewCaster,NewTarget,Index);
            }
            else if(Splitted[0]=="target.energy")
            {
                NewTarget.Energy=AnalyzeRightMember(Splitted[1],NewCaster,NewTarget,Index);
            }
            else if(Splitted[0]=="target.damage")
            {
                NewTarget.Damage=AnalyzeRightMember(Splitted[1],NewCaster,NewTarget,Index);
            }
        }
        NewCaster=Regulate(NewCaster);
        NewTarget=Regulate(NewTarget);
        return(NewCaster,NewTarget);
    } 
    public static int AnalyzeRightMember(string member, Card Caster, Card Target, Dictionary<string,int>Index)
    {
        if(member[0]=='(')
        {
            int count = 1;
            int mark = 1;
            for(int i = 1; i<member.Length; i++)
            {
                if(member[i]=='(')
                {
                    count++;
                }
                if(member[i]==')')
                {
                    count--;
                }
                if(count==0)
                {
                    mark=i;
                    break;
                }
            }
            char[] Memb = member.ToCharArray();
            string NewMember=Methods.SetAndConcat(Memb,1,mark-1);
            if(mark==member.Length-1)
            {
                return AnalyzeRightMember(NewMember, Caster, Target, Index);
            }
            else if(member[mark+1]=='*')
            {
                string RestMember = Methods.SetAndConcat(Memb,mark+1,Memb.Length-1);
                return (AnalyzeRightMember(NewMember, Caster, Target, Index))*(AnalyzeRightMember(RestMember,Caster,Target,Index));
            }
            else if(member[mark+1]=='/')
            {
                string RestMember = Methods.SetAndConcat(Memb,mark+1,Memb.Length-1);
                return (AnalyzeRightMember(NewMember, Caster, Target, Index))/(AnalyzeRightMember(RestMember,Caster,Target,Index));
            }
            else if(member[mark+1]=='+')
            {
                string RestMember = Methods.SetAndConcat(Memb,mark+1,Memb.Length-1);
                return (AnalyzeRightMember(NewMember, Caster, Target, Index))+(AnalyzeRightMember(RestMember,Caster,Target,Index));
            }
            else if(member[mark+1]=='-')
            {
                string RestMember = Methods.SetAndConcat(Memb,mark+1,Memb.Length-1);
                return (AnalyzeRightMember(NewMember, Caster, Target, Index))-(AnalyzeRightMember(RestMember,Caster,Target,Index));
            }

        }
        else if(member.Contains('+')||member.Contains('-')||member.Contains('*')||member.Contains('/'))
        {
            char mark = '*';
            int posmark = 1;
            for(int i = 0; i<member.Length; i++)
            {
                if(member[i]=='+')
                {
                    mark='+';
                    posmark=i;
                    break;
                }
                if(member[i]=='-')
                {
                    mark='-';
                    posmark=i;
                    break;
                }
                if(member[i]=='*')
                {
                    mark='*';
                    posmark=i;
                    break;
                }
                if(member[i]=='/')
                {
                    mark='/';
                    posmark=i;
                    break;
                }
            }
            string MI = Methods.SetAndConcat(member.ToCharArray(),0,posmark-1);
            string MD = Methods.SetAndConcat(member.ToCharArray(),posmark+1,member.Length-1);
            
            if(mark=='+')
            {
                return (AnalyzeRightMember(MI, Caster, Target, Index))+(AnalyzeRightMember(MD,Caster,Target,Index));
            }
            if(mark=='-')
            {
                return (AnalyzeRightMember(MI, Caster, Target, Index))-(AnalyzeRightMember(MD,Caster,Target,Index));
            }
            if(mark=='*')
            {
                return (AnalyzeRightMember(MI, Caster, Target, Index))*(AnalyzeRightMember(MD,Caster,Target,Index));
            }
            if(mark=='/')
            {
                return (AnalyzeRightMember(MI, Caster, Target, Index))/(AnalyzeRightMember(MD,Caster,Target,Index));
            }
        }
        else
        {
            int a;
            if(Index.TryGetValue(member,out a))
            {
                return a;
            }
            else if(int.TryParse(member, out a))
            {
                return a;
            }
            else
            {
                return -1;
            }
        }
        return 5;
    }
    public static bool CheckConditionBool(Condition x, Card Caster, Card Target, Card[] Board, Card[] P1Hand, Card[] P2Hand)    //Booleano que marca si una condicion se cumple bajo un contexto
    {
        bool ret = false;
        Dictionary<string, int> Index = new Dictionary<string, int>(){};
        Index.Add("caster.health",Caster.Health);
        Index.Add("caster.energy",Caster.Energy);
        Index.Add("caster.damage",Caster.Damage);
        Index.Add("target.health",Caster.Health);
        Index.Add("target.energy",Caster.Energy);
        Index.Add("target.damage",Caster.Damage);
        Index.Add("field.cards",Board.Length);
        string[] Expression = x.Expression.Split('|');
        foreach(string exp in Expression)
        {
            string[] Splitted = new string[]{};
            string comparer = "";
            if(exp.Contains('='))
            {
                comparer="=";
                Splitted=exp.Split('=');
            }
            if(exp.Contains('<'))
            {
                comparer="<";
                Splitted=exp.Split('<');
            }
            if(exp.Contains('>'))
            {
                comparer=">";
                Splitted=exp.Split('>');
            }
            if(exp.Contains("<="))
            {
                comparer="<=";
                Splitted=exp.Split("<=");
            }
            if(exp.Contains("=>"))
            {
                comparer="=>";
                Splitted=exp.Split("=>");
            }

            if(Splitted[0]=="player.anycard.health")
            {
                if(comparer=="=")
                {
                    foreach(Card y in P1Hand)
                    {
                        if(y.Health==AnalyzeRightMember(Splitted[1],Caster,Target,Index))
                        {
                            ret = true;
                        }
                    }
                }
                if(comparer=="=>")
                {   
                    foreach(Card y in P1Hand)
                    {
                        if(y.Health>=AnalyzeRightMember(Splitted[1],Caster,Target,Index))
                        {
                            ret = true;
                        }
                    }
                }
                if(comparer=="<=")
                {
                    foreach(Card y in P1Hand)
                    {
                        if(y.Health<=AnalyzeRightMember(Splitted[1],Caster,Target,Index))
                        {
                            ret = true;
                        }
                    }
                }
                if(comparer=="<")
                {
                    foreach(Card y in P1Hand)
                    {
                        if(y.Health<AnalyzeRightMember(Splitted[1],Caster,Target,Index))
                        {
                            ret = true;
                        }
                    }
                }
                if(comparer==">")
                {
                    foreach(Card y in P1Hand)
                    {
                        if(y.Health>AnalyzeRightMember(Splitted[1],Caster,Target,Index))
                        {
                            ret = true;
                        }
                    }
                }
            }
            else if(Splitted[0]=="player.anycard.energy")
            {
                if(comparer=="=")
                {
                    foreach(Card y in P1Hand)
                    {
                        if(y.Energy==AnalyzeRightMember(Splitted[1],Caster,Target,Index))
                        {
                            ret = true;
                        }
                    }
                }
                if(comparer=="=>")
                {   
                    foreach(Card y in P1Hand)
                    {
                        if(y.Energy>=AnalyzeRightMember(Splitted[1],Caster,Target,Index))
                        {
                            ret = true;
                        }
                    }
                }
                if(comparer=="<=")
                {
                    foreach(Card y in P1Hand)
                    {
                        if(y.Energy<=AnalyzeRightMember(Splitted[1],Caster,Target,Index))
                        {
                            ret = true;
                        }
                    }
                }
                if(comparer=="<")
                {
                    foreach(Card y in P1Hand)
                    {
                        if(y.Energy<AnalyzeRightMember(Splitted[1],Caster,Target,Index))
                        {
                            ret = true;
                        }
                    }
                }
                if(comparer==">")
                {
                    foreach(Card y in P1Hand)
                    {
                        if(y.Energy>AnalyzeRightMember(Splitted[1],Caster,Target,Index))
                        {
                            ret = true;
                        }
                    }
                }
            }
            else if(Splitted[0]=="player.anycard.damage")
            {
                if(comparer=="=")
                {
                    foreach(Card y in P1Hand)
                    {
                        if(y.Damage==AnalyzeRightMember(Splitted[1],Caster,Target,Index))
                        {
                            ret = true;
                        }
                    }
                }
                if(comparer=="=>")
                {   
                    foreach(Card y in P1Hand)
                    {
                        if(y.Damage>=AnalyzeRightMember(Splitted[1],Caster,Target,Index))
                        {
                            ret = true;
                        }
                    }
                }
                if(comparer=="<=")
                {
                    foreach(Card y in P1Hand)
                    {
                        if(y.Damage<=AnalyzeRightMember(Splitted[1],Caster,Target,Index))
                        {
                            ret = true;
                        }
                    }
                }
                if(comparer=="<")
                {
                    foreach(Card y in P1Hand)
                    {
                        if(y.Damage<AnalyzeRightMember(Splitted[1],Caster,Target,Index))
                        {
                            ret = true;
                        }
                    }
                }
                if(comparer==">")
                {
                    foreach(Card y in P1Hand)
                    {
                        if(y.Damage>AnalyzeRightMember(Splitted[1],Caster,Target,Index))
                        {
                            ret = true;
                        }
                    }
                }
            }

            else if(Splitted[0]=="player.allcards.health")
            {
                if(comparer=="=")
                {
                    int count = 0;
                    foreach(Card y in P1Hand)
                    {
                        if(y.Health==AnalyzeRightMember(Splitted[1],Caster,Target,Index))
                        {
                            count++;
                        }
                    }
                    if(count==P1Hand.Length)
                    {
                        ret = true;
                    }
                }
                if(comparer=="=>")
                {   
                    int count = 0;
                    foreach(Card y in P1Hand)
                    {
                        if(y.Health>=AnalyzeRightMember(Splitted[1],Caster,Target,Index))
                        {
                            count++;
                        }
                    }
                    if(count==P1Hand.Length)
                    {
                        ret = true;
                    }
                }
                if(comparer=="<=")
                {
                    int count = 0;
                    foreach(Card y in P1Hand)
                    {
                        if(y.Health<=AnalyzeRightMember(Splitted[1],Caster,Target,Index))
                        {
                            count++;
                        }
                    }
                    if(count==P1Hand.Length)
                    {
                        ret = true;
                    }
                }
                if(comparer=="<")
                {
                    int count = 0;
                    foreach(Card y in P1Hand)
                    {
                        if(y.Health<AnalyzeRightMember(Splitted[1],Caster,Target,Index))
                        {
                            count++;
                        }
                    }
                    if(count==P1Hand.Length)
                    {
                        ret = true;
                    }
                }
                if(comparer==">")
                {
                    int count = 0;
                    foreach(Card y in P1Hand)
                    {
                        if(y.Health>AnalyzeRightMember(Splitted[1],Caster,Target,Index))
                        {
                            count++;
                        }
                    }
                    if(count==P1Hand.Length)
                    {
                        ret = true;
                    }
                }
            }
            else if(Splitted[0]=="player.allcards.energy")
            {
                if(comparer=="=")
                {
                    int count = 0;
                    foreach(Card y in P1Hand)
                    {
                        if(y.Energy==AnalyzeRightMember(Splitted[1],Caster,Target,Index))
                        {
                            count++;
                        }
                    }
                    if(count==P1Hand.Length)
                    {
                        ret = true;
                    }
                }
                if(comparer=="=>")
                {   
                    int count = 0;
                    foreach(Card y in P1Hand)
                    {
                        if(y.Energy>=AnalyzeRightMember(Splitted[1],Caster,Target,Index))
                        {
                            count++;
                        }
                    }
                    if(count==P1Hand.Length)
                    {
                        ret = true;
                    }
                }
                if(comparer=="<=")
                {
                    int count = 0;
                    foreach(Card y in P1Hand)
                    {
                        if(y.Energy<=AnalyzeRightMember(Splitted[1],Caster,Target,Index))
                        {
                            count++;
                        }
                    }
                    if(count==P1Hand.Length)
                    {
                        ret = true;
                    }
                }
                if(comparer=="<")
                {
                    int count = 0;
                    foreach(Card y in P1Hand)
                    {
                        if(y.Energy<AnalyzeRightMember(Splitted[1],Caster,Target,Index))
                        {
                            count++;
                        }
                    }
                    if(count==P1Hand.Length)
                    {
                        ret = true;
                    }
                }
                if(comparer==">")
                {
                    int count = 0;
                    foreach(Card y in P1Hand)
                    {
                        if(y.Energy>AnalyzeRightMember(Splitted[1],Caster,Target,Index))
                        {
                            count++;
                        }
                    }
                    if(count==P1Hand.Length)
                    {
                        ret = true;
                    }
                }
            }
            else if(Splitted[0]=="player.allcards.damage")
            {
                if(comparer=="=")
                {
                    int count = 0;
                    foreach(Card y in P1Hand)
                    {
                        if(y.Damage==AnalyzeRightMember(Splitted[1],Caster,Target,Index))
                        {
                            count++;
                        }
                    }
                    if(count==P1Hand.Length)
                    {
                        ret = true;
                    }
                }
                if(comparer=="=>")
                {   
                    int count = 0;
                    foreach(Card y in P1Hand)
                    {
                        if(y.Damage>=AnalyzeRightMember(Splitted[1],Caster,Target,Index))
                        {
                            count++;
                        }
                    }
                    if(count==P1Hand.Length)
                    {
                        ret = true;
                    }
                }
                if(comparer=="<=")
                {
                    int count = 0;
                    foreach(Card y in P1Hand)
                    {
                        if(y.Damage<=AnalyzeRightMember(Splitted[1],Caster,Target,Index))
                        {
                            count++;
                        }
                    }
                    if(count==P1Hand.Length)
                    {
                        ret = true;
                    }
                }
                if(comparer=="<")
                {
                    int count = 0;
                    foreach(Card y in P1Hand)
                    {
                        if(y.Damage<AnalyzeRightMember(Splitted[1],Caster,Target,Index))
                        {
                            count++;
                        }
                    }
                    if(count==P1Hand.Length)
                    {
                        ret = true;
                    }
                }
                if(comparer==">")
                {
                    int count = 0;
                    foreach(Card y in P1Hand)
                    {
                        if(y.Damage>AnalyzeRightMember(Splitted[1],Caster,Target,Index))
                        {
                            count++;
                        }
                    }
                    if(count==P1Hand.Length)
                    {
                        ret = true;
                    }
                }
            }

            else if(Splitted[0]=="enemy.anycard.health")
            {
                if(comparer=="=")
                {
                    foreach(Card y in P2Hand)
                    {
                        if(y.Health==AnalyzeRightMember(Splitted[1],Caster,Target,Index))
                        {
                            ret = true;
                        }
                    }
                }
                if(comparer=="=>")
                {   
                    foreach(Card y in P2Hand)
                    {
                        if(y.Health>=AnalyzeRightMember(Splitted[1],Caster,Target,Index))
                        {
                            ret = true;
                        }
                    }
                }
                if(comparer=="<=")
                {
                    foreach(Card y in P2Hand)
                    {
                        if(y.Health<=AnalyzeRightMember(Splitted[1],Caster,Target,Index))
                        {
                            ret = true;
                        }
                    }
                }
                if(comparer=="<")
                {
                    foreach(Card y in P2Hand)
                    {
                        if(y.Health<AnalyzeRightMember(Splitted[1],Caster,Target,Index))
                        {
                            ret = true;
                        }
                    }
                }
                if(comparer==">")
                {
                    foreach(Card y in P2Hand)
                    {
                        if(y.Health>AnalyzeRightMember(Splitted[1],Caster,Target,Index))
                        {
                            ret = true;
                        }
                    }
                }
            }
            else if(Splitted[0]=="enemy.anycard.energy")
            {
                if(comparer=="=")
                {
                    foreach(Card y in P2Hand)
                    {
                        if(y.Energy==AnalyzeRightMember(Splitted[1],Caster,Target,Index))
                        {
                            ret = true;
                        }
                    }
                }
                if(comparer=="=>")
                {   
                    foreach(Card y in P2Hand)
                    {
                        if(y.Energy>=AnalyzeRightMember(Splitted[1],Caster,Target,Index))
                        {
                            ret = true;
                        }
                    }
                }
                if(comparer=="<=")
                {
                    foreach(Card y in P2Hand)
                    {
                        if(y.Energy<=AnalyzeRightMember(Splitted[1],Caster,Target,Index))
                        {
                            ret = true;
                        }
                    }
                }
                if(comparer=="<")
                {
                    foreach(Card y in P2Hand)
                    {
                        if(y.Energy<AnalyzeRightMember(Splitted[1],Caster,Target,Index))
                        {
                            ret = true;
                        }
                    }
                }
                if(comparer==">")
                {
                    foreach(Card y in P2Hand)
                    {
                        if(y.Energy>AnalyzeRightMember(Splitted[1],Caster,Target,Index))
                        {
                            ret = true;
                        }
                    }
                }
            }
            else if(Splitted[0]=="enemy.anycard.damage")
            {
                if(comparer=="=")
                {
                    foreach(Card y in P2Hand)
                    {
                        if(y.Damage==AnalyzeRightMember(Splitted[1],Caster,Target,Index))
                        {
                            ret = true;
                        }
                    }
                }
                if(comparer=="=>")
                {   
                    foreach(Card y in P2Hand)
                    {
                        if(y.Damage>=AnalyzeRightMember(Splitted[1],Caster,Target,Index))
                        {
                            ret = true;
                        }
                    }
                }
                if(comparer=="<=")
                {
                    foreach(Card y in P2Hand)
                    {
                        if(y.Damage<=AnalyzeRightMember(Splitted[1],Caster,Target,Index))
                        {
                            ret = true;
                        }
                    }
                }
                if(comparer=="<")
                {
                    foreach(Card y in P2Hand)
                    {
                        if(y.Damage<AnalyzeRightMember(Splitted[1],Caster,Target,Index))
                        {
                            ret = true;
                        }
                    }
                }
                if(comparer==">")
                {
                    foreach(Card y in P2Hand)
                    {
                        if(y.Damage>AnalyzeRightMember(Splitted[1],Caster,Target,Index))
                        {
                            ret = true;
                        }
                    }
                }
            }

            else if(Splitted[0]=="enemy.allcards.health")
            {
                if(comparer=="=")
                {
                    int count = 0;
                    foreach(Card y in P2Hand)
                    {
                        if(y.Health==AnalyzeRightMember(Splitted[1],Caster,Target,Index))
                        {
                            count++;
                        }
                    }
                    if(count==P2Hand.Length)
                    {
                        ret = true;
                    }
                }
                if(comparer=="=>")
                {   
                    int count = 0;
                    foreach(Card y in P2Hand)
                    {
                        if(y.Health>=AnalyzeRightMember(Splitted[1],Caster,Target,Index))
                        {
                            count++;
                        }
                    }
                    if(count==P2Hand.Length)
                    {
                        ret = true;
                    }
                }
                if(comparer=="<=")
                {
                    int count = 0;
                    foreach(Card y in P2Hand)
                    {
                        if(y.Health<=AnalyzeRightMember(Splitted[1],Caster,Target,Index))
                        {
                            count++;
                        }
                    }
                    if(count==P2Hand.Length)
                    {
                        ret = true;
                    }
                }
                if(comparer=="<")
                {
                    int count = 0;
                    foreach(Card y in P2Hand)
                    {
                        if(y.Health<AnalyzeRightMember(Splitted[1],Caster,Target,Index))
                        {
                            count++;
                        }
                    }
                    if(count==P2Hand.Length)
                    {
                        ret = true;
                    }
                }
                if(comparer==">")
                {
                    int count = 0;
                    foreach(Card y in P2Hand)
                    {
                        if(y.Health>AnalyzeRightMember(Splitted[1],Caster,Target,Index))
                        {
                            count++;
                        }
                    }
                    if(count==P2Hand.Length)
                    {
                        ret = true;
                    }
                }
            }
            else if(Splitted[0]=="enemy.allcards.energy")
            {
                if(comparer=="=")
                {
                    int count = 0;
                    foreach(Card y in P2Hand)
                    {
                        if(y.Energy==AnalyzeRightMember(Splitted[1],Caster,Target,Index))
                        {
                            count++;
                        }
                    }
                    if(count==P2Hand.Length)
                    {
                        ret = true;
                    }
                }
                if(comparer=="=>")
                {   
                    int count = 0;
                    foreach(Card y in P2Hand)
                    {
                        if(y.Energy>=AnalyzeRightMember(Splitted[1],Caster,Target,Index))
                        {
                            count++;
                        }
                    }
                    if(count==P2Hand.Length)
                    {
                        ret = true;
                    }
                }
                if(comparer=="<=")
                {
                    int count = 0;
                    foreach(Card y in P2Hand)
                    {
                        if(y.Energy<=AnalyzeRightMember(Splitted[1],Caster,Target,Index))
                        {
                            count++;
                        }
                    }
                    if(count==P2Hand.Length)
                    {
                        ret = true;
                    }
                }
                if(comparer=="<")
                {
                    int count = 0;
                    foreach(Card y in P2Hand)
                    {
                        if(y.Energy<AnalyzeRightMember(Splitted[1],Caster,Target,Index))
                        {
                            count++;
                        }
                    }
                    if(count==P2Hand.Length)
                    {
                        ret = true;
                    }
                }
                if(comparer==">")
                {
                    int count = 0;
                    foreach(Card y in P2Hand)
                    {
                        if(y.Energy>AnalyzeRightMember(Splitted[1],Caster,Target,Index))
                        {
                            count++;
                        }
                    }
                    if(count==P2Hand.Length)
                    {
                        ret = true;
                    }
                }
            }
            else if(Splitted[0]=="enemy.allcards.damage")
            {
                if(comparer=="=")
                {
                    int count = 0;
                    foreach(Card y in P2Hand)
                    {
                        if(y.Damage==AnalyzeRightMember(Splitted[1],Caster,Target,Index))
                        {
                            count++;
                        }
                    }
                    if(count==P2Hand.Length)
                    {
                        ret = true;
                    }
                }
                if(comparer=="=>")
                {   
                    int count = 0;
                    foreach(Card y in P2Hand)
                    {
                        if(y.Damage>=AnalyzeRightMember(Splitted[1],Caster,Target,Index))
                        {
                            count++;
                        }
                    }
                    if(count==P2Hand.Length)
                    {
                        ret = true;
                    }
                }
                if(comparer=="<=")
                {
                    int count = 0;
                    foreach(Card y in P2Hand)
                    {
                        if(y.Damage<=AnalyzeRightMember(Splitted[1],Caster,Target,Index))
                        {
                            count++;
                        }
                    }
                    if(count==P2Hand.Length)
                    {
                        ret = true;
                    }
                }
                if(comparer=="<")
                {
                    int count = 0;
                    foreach(Card y in P2Hand)
                    {
                        if(y.Damage<AnalyzeRightMember(Splitted[1],Caster,Target,Index))
                        {
                            count++;
                        }
                    }
                    if(count==P2Hand.Length)
                    {
                        ret = true;
                    }
                }
                if(comparer==">")
                {
                    int count = 0;
                    foreach(Card y in P2Hand)
                    {
                        if(y.Damage>AnalyzeRightMember(Splitted[1],Caster,Target,Index))
                        {
                            count++;
                        }
                    }
                    if(count==P2Hand.Length)
                    {
                        ret = true;
                    }
                }
            }
            else if(Splitted[0]=="field.cards")
            {
                int count = 0;
                foreach(Card y in Board)
                {
                    if(y.Name!="")
                    {
                        count ++;
                    }
                }
                if(comparer=="=")
                {
                    ret = count==int.Parse(Splitted[1]);
                }
                if(comparer=="=>")
                {   
                    ret = count>=int.Parse(Splitted[1]);
                }
                if(comparer=="<=")
                {
                    ret = count<=int.Parse(Splitted[1]);
                }
                if(comparer=="<")
                {
                    ret = count<int.Parse(Splitted[1]);
                }
                if(comparer==">")
                {
                    ret = count>int.Parse(Splitted[1]);
                }
            }
            else
            {
                if(comparer=="=")
                {
                    ret = Index.GetValueOrDefault(Splitted[0])==AnalyzeRightMember(Splitted[1],Caster,Target,Index);
                }
                if(comparer=="<")
                {
                    ret = Index.GetValueOrDefault(Splitted[0])<AnalyzeRightMember(Splitted[1],Caster,Target,Index);
                }
                if(comparer==">")
                {
                    ret = Index.GetValueOrDefault(Splitted[0])>AnalyzeRightMember(Splitted[1],Caster,Target,Index);
                }
                if(comparer=="<=")
                {
                    ret = Index.GetValueOrDefault(Splitted[0])<=AnalyzeRightMember(Splitted[1],Caster,Target,Index);
                }
                if(comparer=="=>")
                {
                    ret = Index.GetValueOrDefault(Splitted[0])>=AnalyzeRightMember(Splitted[1],Caster,Target,Index);
                }
            }
            if(ret==true)
            {
                return ret;
            }
        }
        return ret;
    }
}