public class ParserCheck    //Clase que revisa el código y crea elementos a partir de él
{
    public string[] CardsCode;
    public string[] ActionsCode;
    public string[] ConditionsCode;
    public string[] EffectsCode;
    public List<string> ErrorList;
    public bool Validated;
    public ParserCheck(LanguageRevision L)
    {
        //Estableciendo y dividiendo el código en partes
        string[] Main = Methods.CodeTransform(L.code);
        CardsCode=ParseFunctions.CardExtract(Main);
        ActionsCode=ParseFunctions.ActionExtract(Main);
        ConditionsCode=ParseFunctions.ConditionExtract(Main);
        EffectsCode=ParseFunctions.EffectExtract(Main);
        int[] init;
        int[] end;
        //Analizando errores anteriores
        ErrorList=L.ErrorList;
        if(ErrorList.Count!=0)
        {
            ErrorList.Add("Imposible parsear debido a declaraciones incompletas");
        }
        //Analizando errores en la creación de efectos
        else
        {
            ErrorList=CheckEffect(EffectsCode, GameList.AllEffects, ErrorList);
            if(ErrorList.Count!=0)
            {
                ErrorList.Add("No se ha podido continuar el análisis de código");
            }
            else
            {  
                //Creando efectos y agregándolos a la biblioteca
                init = new int[]{};
                end = new int[]{};
                for(int i = 0; i<EffectsCode.Length; i++)
                {
                    if(EffectsCode[i][EffectsCode[i].Length-1]=='.')
                    {
                        init=Methods.AddInt(init, i);
                    }
                    if(EffectsCode[i]=="}")
                    {
                        end=Methods.AddInt(end, i);
                    }
                }
                for(int i = 0; i<init.Length; i++)
                {
                    string[] Code = new string[end[i]-init[i]+1];
                    int k = 0;
                    for(int j = init[i]; j<=end[i]; j++)
                    {
                        Code[k]=EffectsCode[j];
                        k++;
                    }
                    GameList.AllEffects=Methods.AddEffect(GameList.AllEffects, ParseFunctions.CreateEffect(Code));
                }
                //Analizando errores en la creación de condicionales
                ErrorList=CheckCondition(ConditionsCode, GameList.AllConditions, ErrorList);
                if(ErrorList.Count!=0)
                {
                    ErrorList.Add("No se ha podido continuar el análisis de código");
                }
                else
                {
                    //Creando condicionales y agregándolos a la biblioteca
                    init = new int[]{};
                    end = new int[]{};
                    for(int i = 0; i<ConditionsCode.Length; i++)
                    {
                        if(ConditionsCode[i][ConditionsCode[i].Length-1]=='.')
                        {
                            init=Methods.AddInt(init, i);
                        }
                        if(ConditionsCode[i]=="}")
                        {
                            end=Methods.AddInt(end, i);
                        }
                    }
                    for(int i = 0; i<init.Length; i++)
                    {
                        string[] Code = new string[end[i]-init[i]+1];
                        int k = 0;
                        for(int j = init[i]; j<=end[i]; j++)
                        {
                            Code[k]=ConditionsCode[j];
                            k++;
                        }
                        GameList.AllConditions=Methods.AddCondition(GameList.AllConditions, ParseFunctions.CreateCondition(Code));
                    }
                    //Analizando errores en la creación de acciones
                    ErrorList=CheckAction(ActionsCode, GameList.AllActions, ErrorList, GameList.AllConditions, GameList.AllEffects);
                    if(ErrorList.Count!=0)
                    {
                        ErrorList.Add("No se ha podido continuar el análisis de código");
                    }
                    else
                    {
                        //Creando acciones y agregándolas a la biblioteca
                        init = new int[]{};
                        end = new int[]{};
                        for(int i = 0; i<ActionsCode.Length; i++)
                        {
                            if(ActionsCode[i][ActionsCode[i].Length-1]=='.')
                            {
                                init=Methods.AddInt(init, i);
                            }
                            if(ActionsCode[i]=="}")
                            {
                                end=Methods.AddInt(end, i);
                            }
                        }
                        for(int i = 0; i<init.Length; i++)
                        {
                            string[] Code = new string[end[i]-init[i]+1];
                            int k = 0;
                            for(int j = init[i]; j<=end[i]; j++)
                            {
                                Code[k]=ActionsCode[j];
                                k++;
                            }
                        GameList.AllActions=Methods.AddAction(GameList.AllActions, ParseFunctions.CreateAction(Code, GameList.AllConditions, GameList.AllEffects));
                        }
                        //Analizando errores en la creación de cartas
                        ErrorList=CheckCard(CardsCode, GameList.AllCards, ErrorList, GameList.AllActions);
                        if(ErrorList.Count!=0)
                        {
                            ErrorList.Add("No se ha podido continuar el análisis de código");
                        }
                        //Creando cartas a partir de los datos obtenidos
                        else
                        {
                            init = new int[]{};
                            end = new int[]{};
                            for(int i = 0; i<CardsCode.Length; i++)
                            {
                                if(CardsCode[i][CardsCode[i].Length-1]=='.')
                                {
                                    init=Methods.AddInt(init, i);
                                }
                                if(CardsCode[i]=="}")
                                {
                                    end=Methods.AddInt(end, i);
                                }
                            }
                            for(int i = 0; i<init.Length; i++)
                            {
                                string[] Code = new string[end[i]-init[i]+1];
                                int k = 0;
                                for(int j = init[i]; j<=end[i]; j++)
                                {
                                    Code[k]=CardsCode[j];
                                    k++;
                                }
                            GameList.AllCards=Methods.AddCard(GameList.AllCards,ParseFunctions.CreateCard(Code, GameList.AllActions));
                            }
                        }
                    }
                }
            }           
        }
        Validated=ErrorList.Count==0;
    }
    //Métodos para comprobar si es posible crear elementos con el código
    List<string> CheckCard(string[] CardsCode, Card[] AllCards, List<string> Errors, Action[] AllActions)
    {
        int[] init = new int[]{};
        int[] end = new int[]{};
        Card[] Tester = AllCards;
        for(int i = 0; i<CardsCode.Length; i++)
        {
            if(CardsCode[i][CardsCode[i].Length-1]=='.')
            {
                init=Methods.AddInt(init, i);
            }
            if(CardsCode[i]=="}")
            {
                end=Methods.AddInt(end, i);
            }
        }
        if(init.Length!=end.Length)
        {
            Errors.Add("Su código contiene una carta incompleta, asegúrese de cerrar correctamente ( } ) al terminar de editar su carta");
            return Errors;
        }
        for(int i = 0; i<CardsCode.Length; i++)
        {
            if(CardsCode[i]=="actions")
            {
                int count = 0;
                string a = CardsCode[i+2];
                foreach(char b in a)
                {
                    if(b=='(')
                    {
                        count++;
                    }
                }
                if(count>3)
                {
                    Errors.Add("Una carta debe contener máximo 3 acciones");
                }
            }
        }
        for(int i = 0; i<init.Length; i++)
        {
            string[] Code = new string[end[i]-init[i]+1];
            int k = 0;
            for(int j = init[i]; j<=end[i]; j++)
            {
                Code[k]=CardsCode[j];
                k++;
            }
            for(int j=0; j<Code.Length; j++)
            {
                if(Code[j]=="health")
                {
                    int a;
                    bool K = int.TryParse(Code[j+2], out a);
                    if(!K)
                    {
                        Errors.Add("El valor asignado al parámetro (health) en la carta "+Code[0]+" no es un número entero");
                    }
                }
                if(Code[j]=="energy")
                {
                    int a;
                    bool K = int.TryParse(Code[j+2], out a);
                    if(!K)
                    {
                        Errors.Add("El valor asignado al parámetro (energy) en la carta "+Code[0]+" no es un número entero");
                    }
                }
                if(Code[j]=="damage")
                {
                    int a;
                    bool K = int.TryParse(Code[j+2], out a);
                    if(!K)
                    {
                        Errors.Add("El valor asignado al parámetro (damage) en la carta "+Code[0]+" no es un número entero");
                    }
                }
                if(Code[j]=="actions")
                {
                    string act = Code[j+2];
                    string[] act01 = act.Split('(');
                    int count = 0;
                    foreach(string x in act01)
                    {
                        string[] act02 = x.Split(')');
                        for(int m = 0; m<act02.Length; m++)
                        {
                            if(act02[m]!="")
                            {
                                count = 0;
                                foreach(Action y in AllActions)
                                {
                                    if(y.ID==act02[m])
                                    {
                                        count++;
                                    }
                                }
                                if(count==0)
                                {
                                    Errors.Add("La acción "+act02[m]+" no ha sido declarada");
                                }
                            }
                        }
                    }
                }
            }
            if(Tester.Contains(ParseFunctions.CreateCard(Code, AllActions)))
            {
                Errors.Add("El nombre de esta carta ("+Code[0]+") ya ha sido declarado, intente cambiar el nombre");
            }
            else
            {
                Tester=Methods.AddCard(Tester,ParseFunctions.CreateCard(Code, AllActions));
            }
        }
        return Errors;
    }
    List<string> CheckAction(string[] ActionsCode, Action[] AllActions, List<string> Errors, Condition[] AllConditions, Effect[] AllEfects)
    {
        int[] init = new int[]{};
        int[] end = new int[]{};
        Action[] Tester = AllActions;
        for(int i = 0; i<ActionsCode.Length; i++)
        {
            if(ActionsCode[i][ActionsCode[i].Length-1]=='.')
            {
                init=Methods.AddInt(init, i);
            }
            if(ActionsCode[i]=="}")
            {
                end=Methods.AddInt(end, i);
            }
        }
        if(init.Length!=end.Length)
        {
            Errors.Add("Su código contiene una acción incompleta, asegúrese de cerrar correctamente ( } ) al terminar de editar su acción");
            return Errors;
        }
        for(int i = 0; i<init.Length; i++)
        {
            string[] Code = new string[end[i]-init[i]+1];
            int k = 0;
            for(int j = init[i]; j<=end[i]; j++)
            {
                Code[k]=ActionsCode[j];
                k++;
                if(Code[k-1]=="conditions")
                {
                    string act = ActionsCode[j+2];
                    string[] act01 = act.Split('(');
                    int count = 0;
                    foreach(string x in act01)
                    {
                        string[] act02 = x.Split(')');
                        for(int m = 0; m<act02.Length; m++)
                        {
                            if(act02[m]!="")
                            {
                                count= 0;
                                foreach(Condition y in AllConditions)
                                {
                                    if(y.id==act02[m])
                                    {
                                        count++;
                                    }
                                }
                                if(count==0)
                                {
                                    Errors.Add("La condicional "+act02[m]+" no ha sido declarada");
                                }
                            }
                        }
                    }
                }
                if(Code[k-1]=="effects")
                {
                    string act = ActionsCode[j+2];
                    string[] act01 = act.Split('(');
                    int count = 0;
                    foreach(string x in act01)
                    {
                        string[] act02 = x.Split(')');
                        for(int m = 0; m<act02.Length; m++)
                        {
                            if(act02[m]!="")
                            {
                                count= 0;
                                foreach(Effect y in AllEfects)
                                {
                                    if(y.id==act02[m])
                                    {
                                        count++;
                                    }
                                }
                                if(count==0)
                                {
                                    Errors.Add("El efecto "+act02[m]+" no ha sido declarado");
                                }
                            }
                        }
                    }
                }
            }
            if(Tester.Contains(ParseFunctions.CreateAction(Code, AllConditions, AllEfects)))
            {
                Errors.Add("El nombre de esta acción ("+Code[0]+") ya ha sido declarado, intente cambiar el nombre");
            }
            else
            {
                Tester=Methods.AddAction(Tester,ParseFunctions.CreateAction(Code, AllConditions, AllEfects));
            }
        }
        return Errors;
    }
    List<string> CheckCondition(string[] ConditionsCode, Condition[] AllConditions, List<string> Errors)
    {
        Condition[] Tester = AllConditions;
        int[] init = new int[]{};
        int[] end = new int[]{};
        for(int i = 0; i<EffectsCode.Length; i++)
        {
            if(EffectsCode[i][EffectsCode[i].Length-1]=='.')
            {
                init=Methods.AddInt(init, i);
            }
            if(EffectsCode[i]=="}")
            {
                end=Methods.AddInt(end, i);
            }
        }
        if(init.Length!=end.Length)
        {
            Errors.Add("Su código contiene una condicional incompleta, asegúrese de cerrar correctamente ( } ) al terminar de editar su condicional");
            return Errors;
        }
        for(int i = 0; i<init.Length; i++)
        {
            string[] Code = new string[end[i]-init[i]+1];
            int k = 0;
            for(int j = init[i]; j<=end[i]; j++)
            {
                Code[k]=EffectsCode[j];
                k++;
            }
            if(Tester.Contains(ParseFunctions.CreateCondition(Code)))
            {
                Errors.Add("El nombre de esta condicional ("+Code[0]+") ya ha sido declarado, intente cambiar el nombre");
            }
            else
            {
                Tester = Methods.AddCondition(Tester, ParseFunctions.CreateCondition(Code));
            }
        }
        return Errors;
    }
    List<string> CheckEffect(string[] EffectsCode, Effect[] AllEffects, List<string> Errors)
    {
        int[] init = new int[]{};
        int[] end = new int[]{};
        Effect[] Tester = AllEffects;
        for(int i = 0; i<EffectsCode.Length; i++)
        {
            if(EffectsCode[i][EffectsCode[i].Length-1]=='.')
            {
                init=Methods.AddInt(init, i);
            }
            if(EffectsCode[i]=="}")
            {
                end=Methods.AddInt(end, i);
            }
        }
        if(init.Length!=end.Length)
        {
            Errors.Add("Su código contiene un efecto incompleto, asegúrese de cerrar correctamente ( } ) al terminar de editar su efecto");
            return Errors;
        }
        for(int i = 0; i<init.Length; i++)
        {
            string[] Code = new string[end[i]-init[i]+1];
            int k = 0;
            for(int j = init[i]; j<=end[i]; j++)
            {
                Code[k]=EffectsCode[j];
                k++;
            }
            if(Tester.Contains(ParseFunctions.CreateEffect(Code)))
            {
                Errors.Add("El nombre de este efecto ("+Code[0]+") ya ha sido declarado, intente cambiar el nombre");
            }
            else
            {
                Tester = Methods.AddEffect(Tester, ParseFunctions.CreateEffect(Code));
            }
        }
        return Errors;
    }
}