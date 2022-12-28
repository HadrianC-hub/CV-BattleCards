public class Card       //Definicion de carta
{
    public string Name;
    public Action[] Actions;
    public int Health;
    public int Energy;
    public int Damage;
    public int BaseHealth;
    public int BaseEnergy;
    public int BaseDamage;
    public Card(string N, Action[] Act, int H, int E, int D)
    {
        Name=N;
        Actions=Act;
        Health=H;
        BaseHealth=H;
        Energy=E;
        BaseEnergy=E;
        Damage=D;
        BaseDamage=D;
    }
    public Card(string N, Action[] Act, int H, int E, int D, int bh, int be, int bd)
    {
        Name=N;
        Actions=Act;
        Health=H;
        BaseHealth=bh;
        Energy=E;
        BaseEnergy=be;
        Damage=D;
        BaseDamage=bd;
    }
    public Card()
    {
        Name="";
        Actions=new Action[]{new Action("",new Condition[]{new Condition("","")},new Effect[]{new Effect("","")})};
        Health=0;
        BaseHealth=0;
        Energy=0;
        BaseEnergy=0;
        Damage=0;
        BaseDamage=0;
    }
}
public class Action //Esta clase representa todos los posibles ataques o acciones de una carta
{
    public string ID;
    public Condition[] Conditions;
    public Effect[] Effects;
    public Action(string id, Condition[] cond, Effect[] ef)
    {
        ID=id;
        Conditions = cond;
        Effects = ef;
    }
}
public class Condition  //Esta clase representa una condicion que se debe o no cumplir
{
    public string id;
    public bool Value;      
    public string Expression;   //Este valor debe ser analizado durante el juego para definir el valor del value
    public Condition(string ID, string Expression)
    {
        id=ID;
        Value=false;
        this.Expression=Expression;
    }
}
public class Effect      //Esta clase representa el efecto que tendrá una carta
{
    public string id;
    public string Expression;
    public Effect(string ID, string exp)
    {
        id=ID;
        Expression=exp;
    }
}