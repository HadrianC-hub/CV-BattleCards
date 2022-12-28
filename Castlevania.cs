using System.IO;
using System;
namespace BattleCards
{
    class Program
    {
        static void Main(string[] args)
        {
            //Extraer contenido de cartas del juego base
            StreamReader St = new StreamReader("edit\\Cards.txt"); 
            string code = St.ReadToEnd(); 
            //Analizar contenido de cartas del juego base
            LanguageAnalisis Code01 = new LanguageAnalisis(code);
            LanguageRevision Code02 = new LanguageRevision(Code01);
            ParserCheck Code03 = new ParserCheck(Code02);
            After_Parse Code04 = new After_Parse(Code03);
            //Extraer contenido de edición
            StreamReader Ed = new StreamReader("edit\\Editor.txt"); 
            string Edit = Ed.ReadToEnd();
            Card[] AllCards = new Card[]{};
            foreach(Card x in Code04.UCards)
            {
                AllCards=Methods.AddCard(AllCards, x);
            }
            List<string> EditErrors = new List<string>();
            if(Edit!="")
            {
                LanguageAnalisis Edit01 = new LanguageAnalisis(Edit);
                LanguageRevision Edit02 = new LanguageRevision(Edit01);
                ParserCheck Edit03 = new ParserCheck(Edit02);
                After_Parse Edit04 = new After_Parse(Edit03);
                foreach(Card x in Edit04.UCards)
                {
                    AllCards=Methods.AddCard(AllCards, x);
                }
                EditErrors=Edit04.ErrorList;
            }
            //Comienza el juego
            Screen.GameManager(AllCards,EditErrors);           
        }
    }
}
