public interface Game   //Clase de elementos básicos del juego
{
    public static (Card[], Card[], Card[]) RepCards (Card[] Cards, Card[] Player1Cards, Card[] Player2Cards) //metodo para repartir alatoriamente las cartas a inicio del juego
    {
        //repartirlas, eliminarlas del fajo 
        int m = Cards.Length;
        //repartir p1
        int position1;
        int position2;
        int position3;
        int position4;
        int position5;
        (position1, position2, position3, position4, position5) = Randoms(m);
        Player1Cards[0] = Cards[position1];
        Player1Cards[1] = Cards[position2];
        Player1Cards[2] = Cards[position3];
        Player1Cards[3] = Cards[position4];
        Player1Cards[4] = Cards[position5];
        Cards = DeleteCards(Cards, Player1Cards);
        m = Cards.Length;
        //repartir p2
        (position1, position2, position3, position4, position5) = Randoms(m);
        Player2Cards[0] = Cards[position1];
        Player2Cards[1] = Cards[position2];
        Player2Cards[2] = Cards[position3];
        Player2Cards[3] = Cards[position4];
        Player2Cards[4] = Cards[position5];
        Cards = DeleteCards(Cards, Player2Cards);
        return (Cards, Player1Cards, Player2Cards);
    }
    public static Card[] DeleteCards (Card[] a, Card[] b) //borrar las cartas del fajo una vez repartidas
    {
        Card[] newa = new Card[]{};
        for(int i = 0; i < a.Length; i++)
        {
            if(!b.Contains(a[i]))
            {
                newa = Methods.AddCard(newa, a[i]);
            }
        }
        return newa;
    }
    public static (int, int, int, int, int) Randoms(int m) //generar los numeros aleatorios de las cartas de cada jugador
    {
        Random r = new Random();
        int[] x = new int[]{};
        int frst = r.Next(0,m);
        x=Methods.AddInt(x,frst);
        int scnd = Compare(m,x);
        x=Methods.AddInt(x,scnd);
        int thrd = Compare(m,x);
        x=Methods.AddInt(x,thrd);
        int frth = Compare(m,x);
        x=Methods.AddInt(x,frth);
        int fvth = Compare(m,x);
        
        return (frst,scnd,thrd,frth,fvth);
    }
    public static int Compare (int m, int[] x)  //metodo para comparar si los numeros aleatorios son iguales
    {
        Random r = new Random();
        int y = r.Next(0,m);
        if(x.Contains(y))
        {
            return Compare(m,x);
        }
        return y;
    }
}
