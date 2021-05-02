using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;

namespace GrammarAnalyzer.Info_Collection
{
    class ASTGenerationInfoCollector : EBNFParserBaseVisitor<Int32>
    {
        private static Dictionary<string, List<string>> getOvveride = new Dictionary<string, List<string>>();                  //key:non_terminal      List: overrided
        private string m_nonTerminal;
        private static List<string> list = new List<string>();

        public override int VisitGrammar_rule(EBNFParser.Grammar_ruleContext context)
        {
            if (context.NON_TERMINAL() != null)
                list.Add("NT_" + context.NON_TERMINAL().GetText());                                                           // adds Non terminal nodes
            return base.VisitGrammar_rule(context);
        }

        public override int VisitGrammar_rule_rhs_overide(EBNFParser.Grammar_rule_rhs_overideContext context)
        {
            if (context.ID() != null)
                list.Add("#" + context.ID().GetText());                                                                        // adds rhs overide nodes
            return base.VisitGrammar_rule_rhs_overide(context);
        }

        public static void setAstGenerationInfo()
        {
            List<int> nt_position = new List<int>();
            List<int> ct_position = new List<int>();

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].StartsWith("NT_"))
                    nt_position.Add(i);

                if (list[i].StartsWith("#"))

                    ct_position.Add(i);
            }

            List<string>[] a = new List<string>[nt_position.Count];

            for (int i = nt_position.Count - 1; i >= 0; i--)
            {
                a[i] = new List<string>();
                for (int j = ct_position.Count - 1; j >= 0; j--)
                {
                    if (ct_position[j] > nt_position[i])
                    {
                        a[i].Add(list[ct_position[j]]);
                        ct_position.Remove(ct_position[j]);
                    }
                }
                getOvveride.Add(list[nt_position[i]], a[i]);
            }
        }

        public static Dictionary<string, List<string>> getOverride
        {
            get { return getOvveride; }
        }
    }

    class ASTGenerationArrayCollector : EBNFParserBaseVisitor<Int32>
    {
        private static Dictionary<string, List<string>> NT_CT_O = new Dictionary<string, List<string>>();
        static String[] N_TC_T = new String[ASTGenerationInfoCollector.getOverride.Count];
        int i = -1;
        public override int VisitGrammar_rule([NotNull] EBNFParser.Grammar_ruleContext context)
        {
            if (context.NON_TERMINAL() != null)
            {
                i++;
                N_TC_T[i] = context.NON_TERMINAL().ToString() + "%";
            }
            return base.VisitGrammar_rule(context);
        }
        public override int VisitClosureAsterisk(EBNFParser.ClosureAsteriskContext context)
        {
            if (context.ASTERISK() != null)
            {
                N_TC_T[i] += context.ASTERISK();
            }
            return base.VisitClosureAsterisk(context);
        }

        public override int VisitClosurePlus(EBNFParser.ClosurePlusContext context)
        {
            if (context.PLUS() != null)
            {
                N_TC_T[i] += context.PLUS();
            }
            return base.VisitClosurePlus(context);
        }

        public override int VisitClosureQMark(EBNFParser.ClosureQMarkContext context)
        {
            if (context.QMARK() != null)
            {
                N_TC_T[i] += context.QMARK();
            }
            return base.VisitClosureQMark(context);
        }

        public override int VisitBarefact(EBNFParser.BarefactContext context)
        {
            return base.VisitBarefact(context);
        }
        public override int VisitParenthesizedTerm(EBNFParser.ParenthesizedTermContext context)
        {
            string a = N_TC_T[i];
            if (context.GetText() != null)
            {
                if (a[a.Length - 1].ToString() == "+" || a[a.Length - 1].ToString() == "*" || a[a.Length - 1].ToString() == "?")
                    N_TC_T[i] = a.Insert(a.Length - 1, "$");
                else
                    N_TC_T[i] += "$";

                N_TC_T[i] += context.GetText() + "$";
            }
            return base.VisitParenthesizedTerm(context);
        }

        public override int VisitRhs_rule_terms(EBNFParser.Rhs_rule_termsContext context)
        {
            if (context.NON_TERMINAL() != null)
            {
                string a = N_TC_T[i];

                if (a[a.Length - 1].ToString() == "+" || a[a.Length - 1].ToString() == "*" || a[a.Length - 1].ToString() == "?")
                {

                    N_TC_T[i] = a.Insert(a.Length - 1, "^");
                    N_TC_T[i] += context.NON_TERMINAL().GetText() +
                    ":" + context.GetType().ToString().Replace("GrammarAnalyzer.EBNFParser+", "").Replace("Context", "");
                }
                else
                {
                    N_TC_T[i] += "^" + context.NON_TERMINAL().GetText() +
                     ":" + context.GetType().ToString().Replace("GrammarAnalyzer.EBNFParser+", "").Replace("Context", "");
                }
            }
            return base.VisitRhs_rule_terms(context);
        }

        public override int VisitTerminal(EBNFParser.TerminalContext context)
        {
            string a = N_TC_T[i];
            if (context.TERMINAL() != null)
            {
                if (a[a.Length - 1].ToString() == "+" || a[a.Length - 1].ToString() == "*" || a[a.Length - 1].ToString() == "?")
                {
                    N_TC_T[i] = a.Insert(a.Length - 1, "^");
                    N_TC_T[i] += context.TERMINAL().GetText() + ":" + context.GetType().ToString().Replace("GrammarAnalyzer.EBNFParser+", "").Replace("Context", "");
                }
                else
                {
                    N_TC_T[i] += "^" + context.TERMINAL().GetText() + ":" + context.GetType().ToString().Replace("GrammarAnalyzer.EBNFParser+", "").Replace("Context", "");
                }
            }
            return base.VisitTerminal(context);
        }

        public override int VisitGrammar_rule_rhs_overide(EBNFParser.Grammar_rule_rhs_overideContext context)
        {
            if (context.ID() != null)
                N_TC_T[i] += "@#" + context.ID().GetText() + "@";
            return base.VisitGrammar_rule_rhs_overide(context);
        }


        public static void DataFlow()
        {
            List<string>[] a = new List<string>[N_TC_T.Length];
            for (int i = 0; i < N_TC_T.Length; i++)
            {
                string[] key;
                int key_lenght;

                if (!N_TC_T[i].Contains("@"))
                {
                    key = N_TC_T[i].Split(new char[] { '%' }, 2, StringSplitOptions.RemoveEmptyEntries);

                    key_lenght = key.Length;
                }
                else
                {
                    key = N_TC_T[i].Split('%');
                    key = key[1].Split(new char[] { '@' }, StringSplitOptions.RemoveEmptyEntries);
                    key_lenght = key.Length;
                }

                for (int j = 0; j < key.Length; j++)
                {
                    if (key_lenght > 1 && !(key[j].StartsWith("$") || key[j].StartsWith("~") ||
                                            key[j].StartsWith("^") || key[j].StartsWith("?") ||
                                            key[j].StartsWith("*") || key[j].StartsWith("+") || key[j].StartsWith("(")))
                    {
                        a[i] = new List<string>();
                        string d1 = key[j];
                        string d2 = key[j + 1];
                        a[i].Add(d2);
                        NT_CT_O.Add(d1, a[i]);
                    }
                }
            }

            string[] data;
            string[] data_helper = null;
            List<String> _list = new List<String>();

            foreach (var var in NT_CT_O)
            {
                for (int i = 0; i < var.Value.Count; i++)
                {
                    data = var.Value[i].Split(new char[] { '$' }, StringSplitOptions.RemoveEmptyEntries);

                    for (int k = 0; k < data.Length; k++)
                    {

                        data_helper = data[k].Split(new char[] { '^' }, StringSplitOptions.RemoveEmptyEntries);
                        for (int l = 0; l < data_helper.Length; l++)
                        {
                            _list.Add(data_helper[l]);
                        }
                    }
                }

                var.Value.RemoveAt(0);
                foreach (var var1 in _list)
                {
                    if (!(var1.StartsWith("(") || var1.StartsWith("?(")))
                        var.Value.Add(var1);
                }

                _list.Clear();
            }

            foreach (var var2 in NT_CT_O) // remove all the implicit elements
            {
                int implicit1;
                int implicit2;
                string simpl;
                string _simpl;
                for (int i = 0; i < var2.Value.Count; i++)
                {
                    while (var2.Value[i].Contains("'"))
                    {
                        _simpl = var2.Value[i];
                        implicit1 = _simpl.IndexOf("'");
                        _simpl = _simpl.Remove(implicit1, 1);
                        implicit2 = _simpl.IndexOf("'") + 1;
                        simpl = var2.Value[i].Remove(implicit1, 1 + (implicit2 - implicit1));
                        var2.Value[i] = simpl;
                    }
                }
            }



            foreach (var var in NT_CT_O) // remove nested parenthesized rules
            {
                for (int i = 0; i < var.Value.Count; i++)
                {
                    if (var.Value[i].StartsWith("+(") || var.Value[i].StartsWith("*("))
                    {
                        string value = var.Value[i];
                        string symbol = value[0].ToString();
                        value = value.Remove(0, 1);
                        value = value + symbol;

                        for (int k = 0; k < var.Value.Count; k++)
                        {
                            if (k != i && var.Value[k].Contains(value) && (var.Value[k].StartsWith("+(") || var.Value[k].StartsWith("*(")))
                            {
                                var.Value[k] = var.Value[k].Replace(value, "");
                            }
                        }
                    }
                }
            }

            foreach (var var in NT_CT_O)
            {
                for (int i = 0; i < var.Value.Count; i++)
                {
                    if (var.Value[i].Contains("(|") || var.Value[i].Contains("|)")) // replace (| to ( and  |) to )
                    {
                        var.Value[i] = var.Value[i].Replace("(|", "(");
                        var.Value[i] = var.Value[i].Replace("|)", ")");
                    }
                }
            }

            /*foreach (var var in NT_CT_O)  
            {
                for (int i = 0; i < var.Value.Count; i++)
                {
                    if (var.Value[i].Contains(":"))
                    {
                        string[] x = var.Value[i].Split(':');
                        for (int k = 0; k < var.Value.Count; k++)
                        {
                            if (var.Value[k].Contains(x[0]) && i != k && (var.Value[k].StartsWith("+") || var.Value[k].StartsWith("*")))
                            {
                                var.Value[i] = var.Value[k][0] + var.Value[i];
                            }
                        }
                    }
                }
            }*/

            foreach (var var2 in NT_CT_O) // apply + and *
            {
                for (int i = 0; i < var2.Value.Count; i++)
                {
                    if (var2.Value[i].StartsWith("+(") || var2.Value[i].StartsWith("*("))
                    {
                        var2.Value.RemoveAt(i);
                    }
                }

                for (int i = 0; i < var2.Value.Count; i++)
                {
                    if (var2.Value[i].Equals("*") || var2.Value[i].Equals("+"))
                    {
                        var2.Value.RemoveAt(i);
                    }
                }
            }

            /*foreach (var var1 in NT_CT_O)
            {
                Console.WriteLine("After " + var1.Key + " key");
                for (int i = 0; i < var1.Value.Count; i++)
                {
                    Console.WriteLine(var1.Value[i] + " value " + i);
                }
            }*/


            List<string> removed = new List<string>();
            foreach (var var in NT_CT_O)                                    //find duplicate contexts
            {
                for (int i = 1; i < var.Value.Count; i++)
                {
                    for (int k = 0; k < var.Value.Count; k++)
                    {
                        if (var.Value[k].Contains(":") && i != k)
                        {
                            string[] x = var.Value[k].Split(':');

                            if (var.Value[k].StartsWith("*") || var.Value[k].StartsWith("+") || var.Value[k].StartsWith("?"))
                            {
                                x[0] = x[0].Substring(1);
                            }
                            if (var.Value[i].Contains(x[0]) && i != k)
                            {
                                if (var.Value[i].StartsWith("*"))
                                {
                                        var.Value[k] = var.Value[i];
                                }
                                else if (var.Value[i].StartsWith("?"))
                                {
                                    var.Value[k] = "*" + var.Value[i].Substring(1);
                                }
                                else if ((var.Value[i].StartsWith("+")))
                                {
                                    var.Value[k] = "*" + var.Value[i].Substring(1);
                                }
                                else
                                {
                                    var.Value[k] = "*" + var.Value[i];
                                }
                                removed.Add(var.Value[i]);
                            }
                        }
                    }
                }
                foreach (var el in removed)
                {
                    var.Value.Remove(el);
                }
                removed.Clear();
            }



            /*foreach (var var1 in NT_CT_O)
            {
                Console.WriteLine(var1.Key + " key");
                for (int i = 0; i < var1.Value.Count; i++)
                {
                    Console.WriteLine(var1.Value[i] + " value " + i);
                }
            }*/

        }

        public static Dictionary<string, List<string>> getN_TC_T
        {
            get { return NT_CT_O; }
        }
    }
}
