public interface Screen //Interfaz que contiene todos los métodos de implementación visual
{
    public static void GameManager(Card[] code, List<string> Errorlist) //Método principal del juego
    {
        while(true)
        {
            Card[] AllCards = code;
            Card[] Player1 = new Card[5];
            Card[] Player2 = new Card[5];
            (AllCards, Player1, Player2) = Game.RepCards(AllCards, Player1, Player2);
            Console.Clear();
            if(Errorlist.Count!=0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Su codigo no ha podido ser importado por los siguientes errores:");
                Console.ForegroundColor = ConsoleColor.White;
                foreach(string Error in Errorlist)
                {
                    Console.WriteLine(Error);
                }
            }
            PrintMainMenu();
            Console.ReadLine();
            PrintGameMode();
            string Selection = Console.ReadLine();
            if(Selection=="3")
            {
                PrintTutorial();
            }
            else if(Selection=="1")
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Introduzca su nombre...");
                Console.ForegroundColor = ConsoleColor.White;
                string PID = Console.ReadLine();
                Console.WriteLine("Primer turno de:  1-) "+PID+"      2-) Jugador Virtual");
                string Selector = Console.ReadLine();
                while(Selector!="1"&&Selector!="2")
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Selección Inválida");
                    Console.ForegroundColor = ConsoleColor.White;
                    Selector = Console.ReadLine();
                }
                if(Selector=="1")
                {
                    PlayerVSIA(Player1,Player2, PID, true);
                }
                else
                {
                    PlayerVSIA(Player1,Player2, PID, false);
                }
                
                Console.ReadLine();
            }
            else if(Selection=="2")
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Jugador 1 introduzca su nombre...");
                string P1Id = Console.ReadLine();
                Console.WriteLine("Jugador 2 introduzca su nombre...");
                string P2Id = Console.ReadLine();
                Console.ForegroundColor = ConsoleColor.White;
                PlayerVSPlayer(Player1,Player2, P1Id, P2Id);
                Console.ReadLine();
            }
            else if(Selection=="4")
            {
                PrintAllCards(code);
            }
            else
            {
                Console.WriteLine("Su opción no es válida, por favor introduzca un número [1]|[2]|[3]|[4]");
                Console.ReadLine();
            }
        }
    }
    static void PlayerVSIA(Card[] Player1, Card[] IA, string PID, bool Playerstart) //Modo Un Jugador
    {
        Console.Clear();
        Card[] Board = new Card[9]{new Card(),new Card(),new Card(),new Card(),new Card(),new Card(),new Card(),new Card(),new Card()};
        Card[] Player1Prop = Player1;
        Card[] IAProp = IA;
        string Winner = "";
        while(Winner=="")
        {
            if(Playerstart)
            {
                (Player1, Board, Winner) = Turn(Player1,IA,Board,Player1Prop,Winner,PID,"Piolín");
                (IA, Board, Winner) = VirtualPlayer.IATurn(IA,Player1,Board,IAProp,Winner,"Piolín",PID);
            }
            else
            {
                (IA, Board, Winner) = VirtualPlayer.IATurn(IA,Player1,Board,IAProp,Winner,"Piolín",PID);
                (Player1, Board, Winner) = Turn(Player1,IA,Board,Player1Prop,Winner,PID,"Piolín");
            }
            
        }
        if(Winner=="Draw")
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("La partida ha quedado en empate");
            Console.ForegroundColor = ConsoleColor.White;
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(Winner+" ha ganado la partida");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
    static void PlayerVSPlayer(Card[] Player1, Card[] Player2, string P1Id, string P2Id)    //Modo Dos Jugadores
    {
        Console.Clear();
        Card[] Board = new Card[9]{new Card(),new Card(),new Card(),new Card(),new Card(),new Card(),new Card(),new Card(),new Card()};
        Card[] Player1Prop = Player1;
        Card[] Player2Prop = Player2;
        string Winner = "";
        while(Winner=="")
        {
            (Player1, Board, Winner) = Turn(Player1,Player2,Board,Player1Prop,Winner,P1Id,P2Id);
            (Player2, Board, Winner) = Turn(Player2,Player1,Board,Player2Prop,Winner,P2Id,P1Id);
        }
        if(Winner=="Draw")
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("La partida ha quedado en empate");
            Console.ForegroundColor = ConsoleColor.White;
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(Winner+" ha ganado la partida");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
    static (Card[], Card[], string) Turn(Card[] Player, Card[] Enemy, Card[] Board, Card[] Prop, string Winner, string PlayerId, string EnemyId) //Implementación de turno del jugador
    {  
        bool EndTurn = false;
        while(!EndTurn)
        {
            Console.Clear();
            PrintGame(Enemy,Player,Board,EnemyId,PlayerId);
            Console.WriteLine("Es el turno de "+PlayerId+"...");
            if(Player.Count()>0&&Playable(Board))
            {
                int[] count = new int[]{};
                for(int i = 0; i<Player.Length; i++)
                {
                    Console.WriteLine("Para jugar la carta "+Player[i].Name+" presione la tecla "+(i+1));
                    count=Methods.AddInt(count,i+1);
                }
                string Mark = Console.ReadLine();
                int selector;
                while(!(int.TryParse(Mark,out selector)&&count.Contains(selector)))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Accion Invalida");
                    Console.ForegroundColor = ConsoleColor.White;
                    Mark = Console.ReadLine();
                }
                PrintCard(Player[selector-1]);
                Console.WriteLine("");
                Console.WriteLine("                                                                [1 2 3]");
                Console.WriteLine("Elija la posicion del campo en la que quiere colocar esta carta [4 5 6], para regresar presione 0");
                Console.WriteLine("                                                                [7 8 9]");
                int[] playable = new int[]{};
                for(int i = 0; i<Board.Length; i++)
                {
                    if(Board[i].Name=="")
                    {
                        playable=Methods.AddInt(playable,(i+1));
                    }
                }
                string SecondMark = Console.ReadLine();
                if(SecondMark!="0")
                {
                    while(!int.TryParse(SecondMark,out int b)||(!playable.Contains(int.Parse(SecondMark))))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Accion Invalida");
                        Console.ForegroundColor = ConsoleColor.White;
                        SecondMark = Console.ReadLine();
                    }
                    int a = int.Parse(SecondMark);
                    Board[a-1]=Player[selector-1];
                    Player=Methods.RemoveFrom(Player, Player[selector-1]);
                    Console.Clear();
                    PrintGame(Enemy,Player,Board,EnemyId,PlayerId);
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine(PlayerId+" va a atacar");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.ReadLine();
                    int[] AtkCards = new int[]{};
                    int[] TgtCards = new int[]{};
                    for(int i = 0; i<Board.Length; i++)
                    {
                        if(Prop.Contains(Board[i]))
                        {
                            AtkCards=Methods.AddInt(AtkCards, (i+1));
                        }
                        if((!Prop.Contains(Board[i]))&&Board[i].Name!="")
                        {
                            TgtCards=Methods.AddInt(TgtCards, (i+1));
                        }
                    }
                    if(Methods.CanAttack(AtkCards, TgtCards))
                    {
                        bool AttackIn=true;
                        while(AttackIn)
                        {
                            Console.WriteLine("Elige la carta Caster");          
                            int[] PossibleCastersPosition = new int[]{};
                            for(int i = 0; i<Board.Length; i++)
                            {
                                if(Prop.Contains(Board[i]))
                                {
                                    Console.WriteLine("Presione "+(i+1)+" para elegir a "+Board[i].Name);
                                    PossibleCastersPosition=Methods.AddInt(PossibleCastersPosition,i+1);
                                }
                            }   
                            string CP = Console.ReadLine();
                            while(CP==""||!int.TryParse(CP,out int b)||!PossibleCastersPosition.Contains(int.Parse(CP)))
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Seleccion Invalida");
                                Console.ForegroundColor = ConsoleColor.White;
                                CP=Console.ReadLine();
                            }
                            Console.Clear();
                            PrintGame(Player,Enemy,Board,PlayerId,EnemyId);
                            PrintCard(Board[int.Parse(CP)-1]);
                            Console.WriteLine("");
                            Console.WriteLine("Seleccione a su carta Target");
                            int[] PossibleTargetPosition = PossibleAttack(int.Parse(CP));
                            if(PossibleTargetPosition.Length==0)
                            {
                                Console.ForegroundColor = ConsoleColor.Gray;
                                Console.WriteLine("No hay objetivos cerca, el turno de ataque ha terminado...");
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.ReadLine();
                                AttackIn=false;
                            }
                            foreach(int x in PossibleTargetPosition)
                            {
                                if(Board[x-1].Name!="")
                                {
                                    Console.WriteLine("Presione "+(x)+" para elegir a "+Board[x-1].Name);
                                }
                            }
                            string TP = Console.ReadLine();
                            while(TP==""||!int.TryParse(TP,out int b)||!PossibleTargetPosition.Contains(int.Parse(TP))||Board[int.Parse(TP)-1].Name=="")
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Seleccion Invalida");
                                Console.ForegroundColor = ConsoleColor.White;
                                TP=Console.ReadLine();
                            }
                            Card Caster = Board[int.Parse(CP)-1];
                            Card Target = Board[int.Parse(TP)-1];
                            Action[] PossibleAct = Methods.EvaluateExpression(Caster,Target,Board,Player,Enemy);
                            int[] Actions = new int[]{};
                            if(PossibleAct.Length!=0)
                            {
                                for(int i = 0; i<PossibleAct.Length; i++)
                                {
                                    Actions = Methods.AddInt(Actions, i+1);
                                    Console.WriteLine("Para la accion "+PossibleAct[i].ID+" presione "+(i+1));
                                }
                                string ActMark = Console.ReadLine();
                                while(ActMark==""||!int.TryParse(ActMark,out int b)||!Actions.Contains(int.Parse(ActMark)))
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("Seleccion Invalida");
                                    Console.ForegroundColor = ConsoleColor.White;
                                    ActMark = Console.ReadLine();
                                }
                                (Caster,Target) = Evaluators.ExecuteEffect(Caster, Target, PossibleAct[int.Parse(ActMark)-1]);
                                if(Caster.Health==0)
                                {
                                  Board[int.Parse(CP)-1]=new Card();
                                }
                                else
                                {
                                    Board[int.Parse(CP)-1]=Caster;
                                }
                                if(Target.Health==0)
                                {
                                    Board[int.Parse(TP)-1]=new Card();
                                }
                                else
                                {
                                    Board[int.Parse(TP)-1]=Target;
                                }
                                PrintGame(Player,Enemy,Board,PlayerId,EnemyId);
                                Console.ForegroundColor = ConsoleColor.Gray;
                                Console.WriteLine("El ataque ha concluido");
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.ReadLine();
                                foreach(Card x in Board)
                                {
                                    if(x.Name!="")
                                    {
                                        if(x.Health<x.BaseHealth)
                                        {
                                            x.Health=x.Health+1;
                                        }
                                        x.Energy=x.Energy+5;
                                    }
                                }
                                AttackIn=false;
                            }
                            else
                            {
                                Console.WriteLine("No hay acciones disponibles para esta Target");
                                Console.ReadLine();
                            }
                        } 
                    }
                    AtkCards = new int[]{};
                    TgtCards = new int[]{};
                    for(int i = 0; i<Board.Length; i++)
                    {
                        if(Prop.Contains(Board[i]))
                        {
                            AtkCards=Methods.AddInt(AtkCards, (i+1));
                        }
                        if((!Prop.Contains(Board[i]))&&Board[i].Name!="")
                        {
                            TgtCards=Methods.AddInt(TgtCards, (i+1));
                        }
                    }
                    if(Player.Length==0&&Enemy.Length==0&&!Methods.CanAttack(AtkCards, TgtCards))
                    {
                         if(AtkCards.Length>TgtCards.Length)
                        {
                            Winner=PlayerId;
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
            }
            else
            {
                Console.Clear();
                PrintGame(Enemy,Player,Board,EnemyId,PlayerId);
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(PlayerId+" va a atacar");
                Console.ForegroundColor = ConsoleColor.White;
                Console.ReadLine();
                int[] AtkCards = new int[]{};
                int[] TgtCards = new int[]{};
                for(int i = 0; i<Board.Length; i++)
                {
                    if(Prop.Contains(Board[i]))
                    {
                        AtkCards=Methods.AddInt(AtkCards, (i+1));
                    }
                    if((!Prop.Contains(Board[i]))&&Board[i].Name!="")
                    {
                        TgtCards=Methods.AddInt(TgtCards, (i+1));
                    }
                }
                if(Methods.CanAttack(AtkCards, TgtCards))
                {
                    bool AttackIn=true;
                    while(AttackIn)
                    {
                        Console.WriteLine("Elige la carta Caster");          
                        int[] PossibleCastersPosition = new int[]{};
                        for(int i = 0; i<Board.Length; i++)
                        {
                            if(Prop.Contains(Board[i]))
                            {
                                Console.WriteLine("Presione "+(i+1)+" para elegir a "+Board[i].Name);
                                PossibleCastersPosition=Methods.AddInt(PossibleCastersPosition,i+1);
                            }
                        }   
                        string CP = Console.ReadLine();
                        while(CP==""||!int.TryParse(CP,out int b)||!PossibleCastersPosition.Contains(int.Parse(CP)))
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Seleccion Invalida");
                            Console.ForegroundColor = ConsoleColor.White;
                            CP=Console.ReadLine();
                        }
                        Console.Clear();
                        PrintGame(Player,Enemy,Board,PlayerId,EnemyId);
                        PrintCard(Board[int.Parse(CP)-1]);
                        Console.WriteLine("");
                        Console.WriteLine("Seleccione a su carta Target");
                        int[] PossibleTargetPosition = PossibleAttack(int.Parse(CP));
                        foreach(int x in PossibleTargetPosition)
                        {
                            if(Board[x-1].Name!="")
                            {
                                Console.WriteLine("Presione "+(x)+" para elegir a "+Board[x-1].Name);
                            }
                        }
                        string TP = Console.ReadLine();
                        while(TP==""||!int.TryParse(TP,out int b)||!PossibleTargetPosition.Contains(int.Parse(TP))||Board[int.Parse(TP)-1].Name=="")
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Seleccion Invalida");
                            Console.ForegroundColor = ConsoleColor.White;
                            TP=Console.ReadLine();
                        }
                        Card Caster = Board[int.Parse(CP)-1];
                        Card Target = Board[int.Parse(TP)-1];
                        Action[] PossibleAct = Methods.EvaluateExpression(Caster,Target,Board,Player,Enemy);
                        int[] Actions = new int[]{};
                        if(PossibleAct.Length!=0)
                        {
                            for(int i = 0; i<PossibleAct.Length; i++)
                            {
                                Actions = Methods.AddInt(Actions, i+1);
                                Console.WriteLine("Para la accion "+PossibleAct[i].ID+" presione "+(i+1));
                            }
                            string ActMark = Console.ReadLine();
                            while(ActMark==""||!int.TryParse(ActMark,out int b)||!Actions.Contains(int.Parse(ActMark)))
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Seleccion Invalida");
                                Console.ForegroundColor = ConsoleColor.White;
                                ActMark = Console.ReadLine();
                            }
                            (Caster,Target) = Evaluators.ExecuteEffect(Caster, Target, PossibleAct[int.Parse(ActMark)-1]);
                            if(Caster.Health==0)
                            {
                                Board[int.Parse(CP)-1]=new Card();
                            }
                            else
                            {
                                Board[int.Parse(CP)-1]=Caster;
                            }
                            if(Target.Health==0)
                            {
                                Board[int.Parse(TP)-1]=new Card();
                            }
                            else
                            {
                                Board[int.Parse(TP)-1]=Target;
                            }
                            PrintGame(Player,Enemy,Board,PlayerId,EnemyId);
                            Console.ForegroundColor = ConsoleColor.Gray;
                            Console.WriteLine("El ataque ha concluido");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.ReadLine();
                            foreach(Card x in Board)
                            {
                                if(x.Name!="")
                                {
                                    if(x.Health<x.BaseHealth)
                                    {
                                        x.Health=x.Health+1;
                                    }
                                    x.Energy=x.Energy+5;
                                }
                            }
                            AttackIn=false;
                        }
                        else
                        {
                            Console.WriteLine("No hay acciones disponibles para esta Target");
                            Console.ReadLine();
                        }
                    }
                }
                AtkCards = new int[]{};
                TgtCards = new int[]{};
                for(int i = 0; i<Board.Length; i++)
                {
                    if(Prop.Contains(Board[i]))
                    {
                        AtkCards=Methods.AddInt(AtkCards, (i+1));
                    }
                    if((!Prop.Contains(Board[i]))&&Board[i].Name!="")
                    {
                        TgtCards=Methods.AddInt(TgtCards, (i+1));
                    }
                }
                if(Player.Length==0&&Enemy.Length==0&&!Methods.CanAttack(AtkCards, TgtCards))
                {
                        if(AtkCards.Length>TgtCards.Length)
                    {
                        Winner=PlayerId;
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
        }
        return (Player, Board, Winner);
    }
    static void PrintGame(Card[] Player1, Card[] Player2, Card[] Board, string P1Id, string P2Id)   //Imprime todos los elementos de la partida
    {
        PrintPlayer(Player1, P1Id);
        PrintBoard(Board);
        PrintPlayer(Player2, P2Id);
    }
    static void PrintPlayer(Card[] Player, string PlayerId) //Imprime la lista de cartas de un jugador
    {
        if(Player.Length==5)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(PlayerId);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("╔══════════╦══════════╦══════════╦══════════╦══════════╗");
            Console.WriteLine("║"+Validate(Player[0].Name)+"║"+Validate(Player[1].Name)+"║"+Validate(Player[2].Name)+"║"+Validate(Player[3].Name)+"║"+Validate(Player[4].Name)+"║");
            Console.WriteLine("║"+Validate("HP"+ConcatInStr(Player[0].Health,Player[0].BaseHealth))+"║"+Validate("HP"+ConcatInStr(Player[1].Health,Player[1].BaseHealth))+"║"+Validate("HP"+ConcatInStr(Player[2].Health,Player[2].BaseHealth))+"║"+Validate("HP"+ConcatInStr(Player[3].Health,Player[3].BaseHealth))+"║"+Validate("HP"+ConcatInStr(Player[4].Health,Player[4].BaseHealth))+"║");
            Console.WriteLine("║"+Validate("EP"+ConcatInStr(Player[0].Energy,Player[0].BaseEnergy))+"║"+Validate("EP"+ConcatInStr(Player[1].Energy,Player[1].BaseEnergy))+"║"+Validate("EP"+ConcatInStr(Player[2].Energy,Player[2].BaseEnergy))+"║"+Validate("EP"+ConcatInStr(Player[3].Energy,Player[3].BaseEnergy))+"║"+Validate("EP"+ConcatInStr(Player[4].Energy,Player[4].BaseEnergy))+"║");
            Console.WriteLine("║"+Validate("ATK"+ConcatInStr(Player[0].Damage,Player[0].BaseDamage))+"║"+Validate("ATK"+ConcatInStr(Player[1].Damage,Player[1].BaseDamage))+"║"+Validate("ATK"+ConcatInStr(Player[2].Damage,Player[2].BaseDamage))+"║"+Validate("ATK"+ConcatInStr(Player[3].Damage,Player[3].BaseDamage))+"║"+Validate("ATK"+ConcatInStr(Player[4].Damage,Player[4].BaseDamage))+"║");
            Console.WriteLine("╚══════════╩══════════╩══════════╩══════════╩══════════╝");
        }
        if(Player.Length==4)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(PlayerId);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("╔══════════╦══════════╦══════════╦══════════╦══════════╗");
            Console.WriteLine("║"+Validate(Player[0].Name)+"║"+Validate(Player[1].Name)+"║"+Validate(Player[2].Name)+"║"+Validate(Player[3].Name)+"║          ║");
            Console.WriteLine("║"+Validate("HP"+ConcatInStr(Player[0].Health,Player[0].BaseHealth))+"║"+Validate("HP"+ConcatInStr(Player[1].Health,Player[1].BaseHealth))+"║"+Validate("HP"+ConcatInStr(Player[2].Health,Player[2].BaseHealth))+"║"+Validate("HP"+ConcatInStr(Player[3].Health,Player[3].BaseHealth))+"║          ║");
            Console.WriteLine("║"+Validate("EP"+ConcatInStr(Player[0].Energy,Player[0].BaseEnergy))+"║"+Validate("EP"+ConcatInStr(Player[1].Energy,Player[1].BaseEnergy))+"║"+Validate("EP"+ConcatInStr(Player[2].Energy,Player[2].BaseEnergy))+"║"+Validate("EP"+ConcatInStr(Player[3].Energy,Player[3].BaseEnergy))+"║          ║");
            Console.WriteLine("║"+Validate("ATK"+ConcatInStr(Player[0].Damage,Player[0].BaseDamage))+"║"+Validate("ATK"+ConcatInStr(Player[1].Damage,Player[1].BaseDamage))+"║"+Validate("ATK"+ConcatInStr(Player[2].Damage,Player[2].BaseDamage))+"║"+Validate("ATK"+ConcatInStr(Player[3].Damage,Player[3].BaseDamage))+"║          ║");
            Console.WriteLine("╚══════════╩══════════╩══════════╩══════════╩══════════╝");
        }
        if(Player.Length==3)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(PlayerId);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("╔══════════╦══════════╦══════════╦══════════╦══════════╗");
            Console.WriteLine("║"+Validate(Player[0].Name)+"║"+Validate(Player[1].Name)+"║"+Validate(Player[2].Name)+"║          ║          ║");
            Console.WriteLine("║"+Validate("HP"+ConcatInStr(Player[0].Health,Player[0].BaseHealth))+"║"+Validate("HP"+ConcatInStr(Player[1].Health,Player[1].BaseHealth))+"║"+Validate("HP"+ConcatInStr(Player[2].Health,Player[2].BaseHealth))+"║          ║          ║");
            Console.WriteLine("║"+Validate("EP"+ConcatInStr(Player[0].Energy,Player[0].BaseEnergy))+"║"+Validate("EP"+ConcatInStr(Player[1].Energy,Player[1].BaseEnergy))+"║"+Validate("EP"+ConcatInStr(Player[2].Energy,Player[2].BaseEnergy))+"║          ║          ║");
            Console.WriteLine("║"+Validate("ATK"+ConcatInStr(Player[0].Damage,Player[0].BaseDamage))+"║"+Validate("ATK"+ConcatInStr(Player[1].Damage,Player[1].BaseDamage))+"║"+Validate("ATK"+ConcatInStr(Player[2].Damage,Player[2].BaseDamage))+"║          ║          ║");
            Console.WriteLine("╚══════════╩══════════╩══════════╩══════════╩══════════╝");
        }
        if(Player.Length==2)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(PlayerId);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("╔══════════╦══════════╦══════════╦══════════╦══════════╗");
            Console.WriteLine("║"+Validate(Player[0].Name)+"║"+Validate(Player[1].Name)+"║          ║          ║          ║");
            Console.WriteLine("║"+Validate("HP"+ConcatInStr(Player[0].Health,Player[0].BaseHealth))+"║"+Validate("HP"+ConcatInStr(Player[1].Health,Player[1].BaseHealth))+"║          ║          ║          ║");
            Console.WriteLine("║"+Validate("EP"+ConcatInStr(Player[0].Energy,Player[0].BaseEnergy))+"║"+Validate("EP"+ConcatInStr(Player[1].Energy,Player[1].BaseEnergy))+"║          ║          ║          ║");
            Console.WriteLine("║"+Validate("ATK"+ConcatInStr(Player[0].Damage,Player[0].BaseDamage))+"║"+Validate("ATK"+ConcatInStr(Player[1].Damage,Player[1].BaseDamage))+"║          ║          ║          ║");
            Console.WriteLine("╚══════════╩══════════╩══════════╩══════════╩══════════╝");
        }
        if(Player.Length==1)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(PlayerId);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("╔══════════╦══════════╦══════════╦══════════╦══════════╗");
            Console.WriteLine("║"+Validate(Player[0].Name)+"║          ║          ║          ║          ║");
            Console.WriteLine("║"+Validate("HP"+ConcatInStr(Player[0].Health,Player[0].BaseHealth))+"║          ║          ║          ║          ║");
            Console.WriteLine("║"+Validate("EP"+ConcatInStr(Player[0].Energy,Player[0].BaseEnergy))+"║          ║          ║          ║          ║");
            Console.WriteLine("║"+Validate("ATK"+ConcatInStr(Player[0].Damage,Player[0].BaseDamage))+"║          ║          ║          ║          ║");
            Console.WriteLine("╚══════════╩══════════╩══════════╩══════════╩══════════╝");
        }   
        if(Player.Length==0)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(PlayerId);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("╔══════════╦══════════╦══════════╦══════════╦══════════╗");
            Console.WriteLine("║          ║          ║          ║          ║          ║");
            Console.WriteLine("║          ║          ║          ║          ║          ║");
            Console.WriteLine("║          ║          ║          ║          ║          ║");
            Console.WriteLine("║          ║          ║          ║          ║          ║");
            Console.WriteLine("╚══════════╩══════════╩══════════╩══════════╩══════════╝");
        }
    }
    static void PrintBoard(Card[] Board)    //Imprime el tablero del juego
    { 
        Console.WriteLine("           ╔══════════╦══════════╦══════════╗");
        Console.WriteLine("           ║"+Validate(Board[0].Name)+"║"+Validate(Board[1].Name)+"║"+Validate(Board[2].Name)+"║");
        Console.WriteLine("           ║"+Validate("HP"+ConcatInStr(Board[0].Health,Board[0].BaseHealth))+"║"+Validate("HP"+ConcatInStr(Board[1].Health,Board[1].BaseHealth))+"║"+Validate("HP"+ConcatInStr(Board[2].Health,Board[2].BaseHealth))+"║");
        Console.WriteLine("           ║"+Validate("EP"+ConcatInStr(Board[0].Energy,Board[0].BaseEnergy))+"║"+Validate("EP"+ConcatInStr(Board[1].Energy,Board[1].BaseEnergy))+"║"+Validate("EP"+ConcatInStr(Board[2].Energy,Board[2].BaseEnergy))+"║");
        Console.WriteLine("           ║"+Validate("ATK"+ConcatInStr(Board[0].Damage,Board[0].BaseDamage))+"║"+Validate("ATK"+ConcatInStr(Board[1].Damage,Board[1].BaseDamage))+"║"+Validate("ATK"+ConcatInStr(Board[2].Damage,Board[2].BaseDamage))+"║");
        Console.WriteLine("           ╠══════════╬══════════╬══════════╣");
        Console.WriteLine("           ║"+Validate(Board[3].Name)+"║"+Validate(Board[4].Name)+"║"+Validate(Board[5].Name)+"║");
        Console.WriteLine("           ║"+Validate("HP"+ConcatInStr(Board[3].Health,Board[3].BaseHealth))+"║"+Validate("HP"+ConcatInStr(Board[4].Health,Board[4].BaseHealth))+"║"+Validate("HP"+ConcatInStr(Board[5].Health,Board[5].BaseHealth))+"║");
        Console.WriteLine("           ║"+Validate("EP"+ConcatInStr(Board[3].Energy,Board[3].BaseEnergy))+"║"+Validate("EP"+ConcatInStr(Board[4].Energy,Board[4].BaseEnergy))+"║"+Validate("EP"+ConcatInStr(Board[5].Energy,Board[5].BaseEnergy))+"║");
        Console.WriteLine("           ║"+Validate("ATK"+ConcatInStr(Board[3].Damage,Board[3].BaseDamage))+"║"+Validate("ATK"+ConcatInStr(Board[4].Damage,Board[4].BaseDamage))+"║"+Validate("ATK"+ConcatInStr(Board[5].Damage,Board[5].BaseDamage))+"║");
        Console.WriteLine("           ╠══════════╬══════════╬══════════╣");
        Console.WriteLine("           ║"+Validate(Board[6].Name)+"║"+Validate(Board[7].Name)+"║"+Validate(Board[8].Name)+"║");
        Console.WriteLine("           ║"+Validate("HP"+ConcatInStr(Board[6].Health,Board[6].BaseHealth))+"║"+Validate("HP"+ConcatInStr(Board[7].Health,Board[7].BaseHealth))+"║"+Validate("HP"+ConcatInStr(Board[8].Health,Board[8].BaseHealth))+"║");
        Console.WriteLine("           ║"+Validate("EP"+ConcatInStr(Board[6].Energy,Board[6].BaseEnergy))+"║"+Validate("EP"+ConcatInStr(Board[7].Energy,Board[7].BaseEnergy))+"║"+Validate("EP"+ConcatInStr(Board[8].Energy,Board[8].BaseEnergy))+"║");
        Console.WriteLine("           ║"+Validate("ATK"+ConcatInStr(Board[6].Damage,Board[6].BaseDamage))+"║"+Validate("ATK"+ConcatInStr(Board[7].Damage,Board[7].BaseDamage))+"║"+Validate("ATK"+ConcatInStr(Board[8].Damage,Board[8].BaseDamage))+"║");
        Console.WriteLine("           ╚══════════╩══════════╩══════════╝");
    }   
    static void PrintMainMenu() //Imprime el menú principal
    {
        Console.WriteLine("A BATTLECARDS GAME INSPIRED ON CASTLEVANIA SERIES\t\t\tVer: 1.0 - 26/12/2022");
        Console.WriteLine("");
        Console.WriteLine("Desarrollo: Laura Martir Beltrán - C111");
        Console.WriteLine("            Adrián Hernández Castellanos - C112");
        Console.WriteLine("Estudiantes de la carrera de Ciencias de la Computación de la Universidad de La Habana");
        Console.WriteLine("");
        Console.WriteLine("");
        Console.WriteLine("Por favor, presione Enter para comenzar a jugar");
    }
    static void PrintGameMode() //Imprime la pantalla de selección de modo de juego
    {
        Console.WriteLine("Presione la tecla que marque la opción correspondiente a su selección");
        Console.WriteLine("");
        Console.WriteLine("1-) Jugador vs IA");
        Console.WriteLine("2-) Jugador vs Jugador");
        Console.WriteLine("3-) Tutorial");
        Console.WriteLine("4-) Ver todas las cartas disponibles");
    }
    static void PrintTutorial() //Imprime el tutorial del juego
    {
        Console.Clear();
        Console.WriteLine("Castlevania BattleCards es un juego de consola en el que usted dispone de cierta cantidad de cartas");
        Console.WriteLine("con habilidades y propiedades únicas. Usted puede contribuir a la creación de cartas a través del");
        Console.WriteLine("editor de cartas. Consulte el archivo .pdf en la carpeta del juego para más información");
        Console.WriteLine("");
        Console.WriteLine("REGLAS DEL JUEGO:");
        Console.WriteLine("De todas las cartas del juego disponibles, usted y su contrincante recibirán 5 cartas aleatorias.");
        Console.WriteLine("Basándose en las propiedades únicas y habilidades de cada carta, usted debe jugar sus cartas en una de");
        Console.WriteLine("las nueve posiciones disponibles del campo. Cada carta situada en una posición puede atacar a todas");
        Console.WriteLine("las cartas que le rodean, incluido a cartas aliadas (no recomendable), pero su rango de ataque no");
        Console.WriteLine("se extiende más que eso. Por cada turno de juego, el jugador debe colocar una carta en el campo y atacar");
        Console.WriteLine("con una carta, o realizar una acción específica de una carta particular, pero solamente una por turno.");
        Console.WriteLine("En el caso en que el campo esté lleno de cartas o no exista ninguna carta en el rango de ataque, se");
        Console.WriteLine("omitirá esta fase del turno. Al finalizar estas acciones será el turno del jugador contrario, y la condición");
        Console.WriteLine("de victoria es destruir todas las cartas del enemigo. En el caso en que todas las cartas queden posicionadas");
        Console.WriteLine("fuera del rango de ataque, la victoria sera del jugador con mas cartas en el campo, y si ambos tienen la");
        Console.WriteLine("misma cantidad, el juego quedara en empate.");
        Console.WriteLine("");
        Console.WriteLine("Esto es todo por ahora...");
        Console.ReadLine();
    }
    static void PrintAllCards(Card[] code)  //Imprime la lista completa de cartas con sus acciones
    {
        Console.Clear();
        foreach(Card x in code)
        {
            Console.WriteLine("\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\");
            PrintCard(x);
        }
        Console.ReadLine();
    }
    static void PrintCard(Card code)    //Imprime una carta específica con sus acciones
    {
        Console.WriteLine("Nombre: "+code.Name);            
        Console.WriteLine("Salud: "+code.BaseHealth);
        Console.WriteLine("Energía: "+code.BaseEnergy);
        Console.WriteLine("Daño: "+code.BaseDamage);
        foreach(Action y in code.Actions)
        {
            Console.WriteLine("-----------------------------------------------------");
            Console.WriteLine("Acción: "+y.ID);
            foreach(Effect z1 in y.Effects)
            {
                Console.WriteLine("Efecto: "+z1.id);
                Console.WriteLine("Valor: "+z1.Expression);
            }
            foreach(Condition z2 in y.Conditions)
            {
                Console.WriteLine("Condición: "+z2.id);
                Console.WriteLine("Valor: "+z2.Expression);
            }
            
        }
    }
    static string ConcatInStr(int a, int b) //Retorna el delimitador para las popiedades de las cartas
    {
        return a+"|"+b;
    }
    static string Validate(string a)    //Convierte cada string a un string de 10 caracteres para evitar errores gráficos
    {
        char[]b=a.ToCharArray();
        if(b.Length>10)
        {
            char[]c = new char[7];
            for(int i = 0; i<7; i++)
            {
                c[i]=b[i];
            }
            c=Methods.AddChar(c,'.');
            c=Methods.AddChar(c,'.');
            c=Methods.AddChar(c,'.');
            return String.Concat(c);
        }
        if(b.Length<10)
        {
            while(b.Length!=10)
            {
                b=Methods.AddChar(b,' ');
            }
            return String.Concat(b);
        }
        return a;
    }
    static bool Playable(Card[] Board)      //Determina si un espacio es jugable
    {
        foreach(Card x in Board)
        {
            if(x.Name==""&&x.Health==0)
            {
                return true;
            }
        }
        return false;
    }
    static int[] PossibleAttack(int casterpos)  //Determina si es posible realizar un ataque
    {
        int[] returner = new int[]{};
        if(casterpos==1)
        {
            returner=Methods.AddInt(returner,2);
            returner=Methods.AddInt(returner,4);
            returner=Methods.AddInt(returner,5);
            return returner;
        }
        if(casterpos==2)
        {       
            returner=Methods.AddInt(returner,1);
            returner=Methods.AddInt(returner,3);
            returner=Methods.AddInt(returner,4);
            returner=Methods.AddInt(returner,5);
            returner=Methods.AddInt(returner,6);
            return returner;
        }
        if(casterpos==3)
        {
            returner=Methods.AddInt(returner,2);
            returner=Methods.AddInt(returner,6);
            returner=Methods.AddInt(returner,5);
            return returner;
        }
        if(casterpos==4)
        {
            returner=Methods.AddInt(returner,1);
            returner=Methods.AddInt(returner,2);
            returner=Methods.AddInt(returner,8);
            returner=Methods.AddInt(returner,5);
            returner=Methods.AddInt(returner,7);
            return returner;
        }
        if(casterpos==5)
        {
            returner=Methods.AddInt(returner,1);
            returner=Methods.AddInt(returner,2);
            returner=Methods.AddInt(returner,3);
            returner=Methods.AddInt(returner,4);
            returner=Methods.AddInt(returner,9);
            returner=Methods.AddInt(returner,6);
            returner=Methods.AddInt(returner,7);
            returner=Methods.AddInt(returner,8);
            return returner;
        }
        if(casterpos==6)
        {
            returner=Methods.AddInt(returner,3);
            returner=Methods.AddInt(returner,2);
            returner=Methods.AddInt(returner,5);
            returner=Methods.AddInt(returner,8);
            returner=Methods.AddInt(returner,9);
            return returner;
        }
        if(casterpos==7)
        {
            returner=Methods.AddInt(returner,8);
            returner=Methods.AddInt(returner,4);
            returner=Methods.AddInt(returner,5);
            return returner;
        }
        if(casterpos==8)
        {
            returner=Methods.AddInt(returner,7);
            returner=Methods.AddInt(returner,9);
            returner=Methods.AddInt(returner,4);
            returner=Methods.AddInt(returner,5);
            returner=Methods.AddInt(returner,6);
            return returner;
        }
        if(casterpos==9)
        {
            returner=Methods.AddInt(returner,8);
            returner=Methods.AddInt(returner,6);
            returner=Methods.AddInt(returner,5);
            return returner;
        }
        else
        {
            return new int[]{0};
        }
    }
}