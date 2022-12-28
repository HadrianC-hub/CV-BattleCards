public static class KeyWords  //Clase que contiene palabras clave del lenguaje y un método de corrección gramatical
{
    public static List<string> Keys = new List<string>(){"card","action","condition","effect"};

    public static char[] Symbol = new char[]{'{','}',':',','}; //0 y 1 para editar parametros Words, 2 para asignar valor,
                                                               //3 para separar valores
    public static List<string> Words = new List<string>(){"actions","health","energy","damage","id","conditions","effects","expression","value","target"};
    //Método para corregir la gramática
    public static bool Check (string previous, string actual, string next)
    {       
        if(actual=="key")
        {
            if(next=="definition"&&(previous=="null"||previous=="closed"))
            {
                return true;
            }
        }
        if(actual=="definition")
        {
            if((next=="open"&&previous=="key")||(next=="assign"&&previous=="word"))
            {
                return true;
            }
        }
        if(actual=="open")
        {
            if(next=="word"||next=="closed")
            {
                return true;
            }
        }
        if(actual=="word")
        {
            if(next=="assign")
            {
                return true;
            }
        }
        if(actual=="assign")
        {
            if(next=="value")
            {
                return true;
            }
        }
        if(actual=="value")
        {
            if(next=="separated")
            {
                return true;
            }
        }
        if(actual=="closed")
        {
            if(next=="key")
            {
                return true;
            }
        }
        if(actual=="value")
        {
            if(next=="operator"||next=="value")
            {
                return true;
            }
        }
        if(actual=="separated")
        {
            if(next=="closed"||next=="word")
            {
                return true;
            }
        }
        return false;
    }
}
