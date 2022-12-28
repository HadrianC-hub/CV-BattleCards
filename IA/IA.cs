public interface VirtualPlayer
{
    public static (Card[], Card[], string) IATurn (Card[] IA, Card[] PlayerEnemy, Card[] Board, Card[] IAProp, string Winner, string IAId, string EnemyId)
    {
        bool EndTurn = false;
        while(!EndTurn)
        {
            Console.Clear();
            Screen.PrintGame(PlayerEnemy, IA, Board, EnemyId, IAId);
            Console.WriteLine("Es el turno de Piolin");
            Console.ReadLine();
            string Played = "";
            if(CanPlay(Board, IA))
            {
                Random r = new Random();
                
                if(Board[1].Name==""||Board[3].Name==""||Board[5].Name==""||Board[7].Name=="")
                {
                    int[] Pos = new int[]{1,3,5,7};
                    int n = r.Next(0,4);
                    while(Board[Pos[n]].Name!="")
                    {
                        n = r.Next(0,4);
                    }
                    Board[Pos[n]] = Hardest(IA);
                    IA=Methods.RemoveFrom(IA,Board[Pos[n]]);
                    Played=Board[Pos[n]].Name;
                }
                else if(Board[4].Name=="")
                {
                    Board[4]= Longest(IA);
                    IA=Methods.RemoveFrom(IA,Board[4]);
                    Played=Board[4].Name;
                }
                else if(Board[0].Name==""||Board[2].Name==""||Board[6].Name==""||Board[8].Name=="")
                {
                    int[] Pos = new int[]{0,2,6,8};
                    int n = r.Next(0,4);
                    while(Board[Pos[n]].Name!="")
                    {
                        n = r.Next(0,4);
                    }
                    Board[Pos[n]] = Strongest(IA);
                    IA=Methods.RemoveFrom(IA,Board[Pos[n]]);
                    Played=Board[Pos[n]].Name;
                }
            }
            Console.Clear();
            Screen.PrintGame(PlayerEnemy, IA, Board, EnemyId, IAId);
            if(Played!="")
            {
                Console.WriteLine("Piolin ha jugado la carta "+Played);
            }
            else
            {
                Console.WriteLine("Piolin no ha podido jugar ninguna carta");
            }
            Console.ReadLine();
            Console.Clear();
            Screen.PrintGame(PlayerEnemy, IA, Board, EnemyId, IAId);
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Piolin va a atacar");
            Console.ForegroundColor = ConsoleColor.White;
            Console.ReadLine();
             
            (int[] s, int[] t) = PositionCards(Board, IAProp);
            for(int i = 0; i<s.Length; i++)
            {
                s[i]=s[i]+1;
            }
            for(int i = 0; i<t.Length; i++)
            {
                t[i]=t[i]+1;
            }
            if(Methods.CanAttack(s, t))
            {
                Card TGT = new Card("",new Action[]{},int.MaxValue,int.MaxValue,int.MaxValue);
                Card CST = new Card();
                Card[] Targets = GetOrderedTarget(Board, IAProp);
                for(int i = 0; i<Targets.Length; i++)
                {
                    Card[] Casters = new Card[]{};
                    int pos=FindPos(Targets[i],Board);
                    int[] Cstpos = Screen.PossibleAttack(pos);
                    foreach(int x in Cstpos)
                    {
                        if(Board[x-1].Name!=""&&IAProp.Contains(Board[x-1]))
                        {
                            Casters = Methods.AddCard(Casters, Board[x-1]);
                        }
                    }
                    if(Casters.Length!=0)
                    {
                        for(int j = 0; j<Casters.Length; j++)
                        {
                            Action[] Acts = Methods.EvaluateExpression(Casters[j], Targets[i], Board, IA, PlayerEnemy);
                            if(Acts.Length!=0)
                            {
                                foreach(Action Act in Acts)
                                {
                                    Card TestCaster = new Card(Casters[j].Name,Casters[j].Actions,Casters[j].Health,Casters[j].Energy,Casters[j].Damage,Casters[j].BaseHealth,Casters[j].BaseEnergy,Casters[j].BaseDamage);
                                    Card TestTarget = new Card(Targets[i].Name,Targets[i].Actions,Targets[i].Health,Targets[i].Energy,Targets[i].Damage,Targets[i].BaseHealth,Targets[i].BaseEnergy,Targets[i].BaseDamage);;
                                    (TestCaster,TestTarget) = Evaluators.ExecuteEffect(TestCaster,TestTarget,Act);
                                    if(TestTarget.Health<TGT.Health)
                                    {
                                        TGT=TestTarget;
                                        CST=TestCaster;
                                    }
                                }
                            }
                        } 
                    }
                }
                int tpos = FindPos(TGT,Board);
                int cpos = FindPos(CST,Board);
                if(TGT.Health>0)
                {
                    Board[tpos-1] = TGT;
                }
                else
                {
                    Board[tpos-1] = new Card();
                }
                if(CST.Health>0)
                {
                    Board[cpos-1] = CST;
                }
                else
                {
                    Board[cpos-1] = new Card();
                }
            }
            Console.Clear();
            Screen.PrintGame(PlayerEnemy,IA,Board,EnemyId,IAId);
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Piolín ha terminado su turno de atacar");
            Console.ForegroundColor = ConsoleColor.White;
            Console.ReadLine();
            int[] AtkCards = new int[]{};
            int[] TgtCards = new int[]{};
            for(int i = 0; i<Board.Length; i++)
            {
                if(IAProp.Contains(Board[i]))
                {
                    AtkCards=Methods.AddInt(AtkCards, (i+1));
                }
                if((!IAProp.Contains(Board[i]))&&Board[i].Name!="")
                {
                    TgtCards=Methods.AddInt(TgtCards, (i+1));
                }
            }
            if(IA.Length==0&&PlayerEnemy.Length==0&&!Methods.CanAttack(AtkCards, TgtCards))
            {
                
                if(AtkCards.Length>TgtCards.Length)
                {
                    Winner=IAId;
                }
                if(AtkCards.Length<TgtCards.Length)
                {
                    Winner=EnemyId;
                }
                if(AtkCards.Length==TgtCards.Length)
                {
                    Winner="Draw";
                }
            }
            EndTurn=true;
        }
        return (IA, Board, Winner);
    }
    public static int FindPos(Card a, Card[] b)
    {
        for(int i = 0; i<b.Length; i++)
        {
            if(b[i].Name==a.Name)
            {
                return i+1;
            }
        }
        return 0;
    }
    public static Card[] GetOrderedTarget(Card[] Board, Card[] IAProp)
    {
        Card[] RetArray = new Card[]{};
        for(int i = 0; i<Board.Length; i++)
        {
            if(Board[i].Name!=""&&!IAProp.Contains(Board[i]))
            {
                RetArray=Methods.AddCard(RetArray,Board[i]);
            }
        }
        Array.Sort(RetArray, (x,y) =>x.Energy+x.Health.CompareTo(y.Health+y.Energy));
        return RetArray;
    }
    public static (int[], int[]) PositionCards(Card[] Board, Card[] IAProp)
    {
        int[] IAPos = new int[]{};
        int[] EnemyPos = new int[]{};
        for(int i = 0; i<Board.Length; i++)
        {
            if(Board[i].Name!=""&&IAProp.Contains(Board[i]))
            {
                IAPos=Methods.AddInt(IAPos,i);
            }
            if(Board[i].Name!=""&&!IAProp.Contains(Board[i]))
            {
                EnemyPos=Methods.AddInt(EnemyPos,i);
            }
        } 
        return (IAPos, EnemyPos);
    }
    public static bool CanPlay(Card[] Board, Card[] IACards)
    {
        if(IACards.Length==0)
        {
            return false;
        }
        for(int i=0; i<Board.Length; i++)
        {
            if(Board[i].Name=="")
            {
                return true;
            }
        }
        return false;
    }
    public static Card Longest(Card[] Hand)
    {
        Card h = Hand[0];
        for(int i = 1; i<Hand.Length; i++)
        {
            if(Hand[i].Health>h.Health)
            {
                h = Hand[i];
            }
        }
        return h;
    }
    public static Card Strongest(Card[] Hand)
    {
        Card h = Hand[0];
        for(int i = 1; i<Hand.Length; i++)
        {
            if(Hand[i].Damage>h.Damage)
            {
                h = Hand[i];
            }
        }
        return h;
    }
    public static Card Hardest(Card[] Hand)
    {
        Card h = Hand[0];
        for(int i = 1; i<Hand.Length; i++)
        {
            if(Hand[i].Energy>h.Energy)
            {
                h = Hand[i];
            }
        }
        return h;
    }
}                                    