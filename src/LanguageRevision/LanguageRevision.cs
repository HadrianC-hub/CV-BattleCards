public class LanguageRevision       //En esta clase se revisará el contexto del código tras analizarlo
{                                   //para evitar repeticiones innecesarias y errores de programación
    public string code;
    public bool CorrectStruct;
    public List<string> ErrorList;
    public LanguageRevision (LanguageAnalisis encoder)
    {
        this.code = encoder.code;
        if(!encoder.valid)
        {
            this.ErrorList = encoder.ErrorList;
            ErrorList.Add("No se puede analizar el contenido debido a problemas de compilación");
        }
        else
        {
            this.ErrorList = CheckContext(code);
        }
        if(ErrorList.Count==0)
        {
            CorrectStruct = true;
        }
        else
        {
            CorrectStruct = false;
        }
    }
    public string[] EliminateNull(string[] a)
    {
        string[] b = new string[]{};
        foreach(string x in a)
        {
            if(x!="")
            {
                string[] c = new string[b.Length+1];
                for(int i = 0; i<b.Length; i++)
                {
                    c[i] = b[i];
                }
                c[c.Length-1]=x;
                b=c;
            }
        }
        return b;
    }
    public List<string> CheckContext(string code)   //Método que revisa el contexto y devuelve los errores
    {
        
        string[] Code = code.Split('}');
        Code = EliminateNull(Code);
        List<string> Errors = new List<string>();
        List<Context> ContextElements = new List<Context>();
        List<string> ScopeElements = new List<string>();
        List<string> CardElements = LanguageScope.CardElements;
        List<string> ActionElements = LanguageScope.ActionElements;
        List<string> ConditionElements = LanguageScope.ConditionElements;
        List<string> EffectElements = LanguageScope.EffectElements;
        foreach(string x in Code)
        {
            CardElements = LanguageScope.CardElements;
            ActionElements = LanguageScope.ActionElements;
            ConditionElements = LanguageScope.ConditionElements;
            EffectElements = LanguageScope.EffectElements;
            ScopeElements = new List<string>();
            string[] RCode = Methods.CodeTransform(x);
            if(RCode.Length>2)
            {
                Context Var = new Context(RCode[0], RCode[1]);
                if(ContextElements.Contains(Var))
                {
                    Errors.Add("La llave "+Var.key+" definida como "+Var.definition+" ya forma parte de este contexto");
                }
                else
                {
                    ContextElements.Add(Var);
                    string[] CCode = new string[RCode.Length-3];
                    for(int i = 0; i<CCode.Length; i++)
                    {
                        CCode[i]=RCode[i+3];
                    }
                    for(int i = 0; i<CCode.Length; i++)
                    {
                        if(CCode[i]==":")
                        {
                            if(ScopeElements.Contains(CCode[i-1]))
                            {
                                Errors.Add("El parámetro "+CCode[i-1]+" ya forma parte de esta definición");
                            }
                            else
                            {
                                ScopeElements.Add(CCode[i-1]);
                            }
                        }
                    }
                }
                if(RCode[0]=="card")
                {
                    foreach(string line in RCode)
                    {
                        if(CardElements.Contains(line))
                        {
                            CardElements.Remove(line);
                        }
                    }
                    if(CardElements.Count!=0)
                    {
                        Errors.Add("Es necesario definir todos los parámetros que debe contener una carta");
                    }
                }
                if(RCode[0]=="action")
                {
                    foreach(string line in RCode)
                    {
                        if(ActionElements.Contains(line))
                        {
                            ActionElements.Remove(line);
                        }
                    }
                    if(ActionElements.Count!=0)
                    {
                        Errors.Add("Es necesario definir todos los parámetros que debe contener una acción");
                    }
                }
                if(RCode[0]=="effect")
                {
                    foreach(string line in RCode)
                    {
                        if(EffectElements.Contains(line))
                        {
                            EffectElements.Remove(line);
                        }
                    }
                    if(EffectElements.Count!=0)
                    {
                        Errors.Add("Es necesario definir todos los parámetros que debe contener un efecto");
                    }
                }
                if(RCode[0]=="condition")
                {
                    foreach(string line in RCode)
                    {
                        if(ConditionElements.Contains(line))
                        {
                            ConditionElements.Remove(line);
                        }
                    }
                    if(ConditionElements.Count!=0)
                    {
                        Errors.Add("Es necesario definir todos los parámetros que debe contener una condicional");
                    }
                }
            } 
        }
        return Errors;
    }
}
public static class LanguageScope   //Listas de los objetos que deben contener cada carta, accion, efecto y condicion
{
    public static List<string> CardElements = new List<string>(){"health","energy","damage","actions"};
    public static List<string> ActionElements = new List<string>(){"conditions", "effects"};
    public static List<string> ConditionElements = new List<string>(){"value"};
    public static List<string> EffectElements = new List<string>(){"value"};
}
public class Context    //Esta clase define un contexto específico para evitar que sus eleemntos se repitan
{
    public string key;
    public string definition;
    public Context(string K, string D)
    {
        key = K;
        definition = D;
    }
}