using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;

namespace GrammarAnalyzer
{
    class ASTCompoisiteConcreteTerminalCollector : EBNFParserBaseVisitor<Int32>
    {

        private static List<string> contexts = new List<string>();
        private static Dictionary<string, List<string>> context = new Dictionary<string, List<string>>();


        public override int VisitGrammar_rule(EBNFParser.Grammar_ruleContext context)
        {
            if (context.NON_TERMINAL() != null)
            {
                contexts.Add("NT_" + context.NON_TERMINAL().GetText());
            }

            return base.VisitGrammar_rule(context);
        }

        public override int VisitRhs_rule_terms(EBNFParser.Rhs_rule_termsContext context)
        {
            if (context.NON_TERMINAL() != null)
            {
                contexts.Add("CT_" + context.NON_TERMINAL().GetText());
            }

            return base.VisitRhs_rule_terms(context);
        }

        public override int VisitTerminal(EBNFParser.TerminalContext context)
        {
            if (context.TERMINAL() != null)
            {
                contexts.Add("CT_" + context.TERMINAL().GetText());

            }
            return base.VisitTerminal(context);
        }

        public override int VisitGrammar_rule_rhs_overide([NotNull] EBNFParser.Grammar_rule_rhs_overideContext context)
        {
            if (context.ID() != null)
            {
                contexts.Add("NT_" + context.ID().GetText());
            }
            return base.VisitGrammar_rule_rhs_overide(context);
        }

        public static void setContexts()
        {
            List<int> nt_position = new List<int>();
            List<int> ct_position = new List<int>();

            for (int i = 0; i < contexts.Count; i++)
            {
                if (contexts[i].StartsWith("NT_"))
                    nt_position.Add(i);

                if (contexts[i].StartsWith("CT_"))
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
                        a[i].Add(contexts[ct_position[j]]);
                        ct_position.Remove(ct_position[j]);
                    }
                }
                context.Add(contexts[nt_position[i]], a[i]);
            }

            List<string> pos = new List<string>();
            foreach (var key in context)                                                                      //remove NT if there are no contexts
            {
                if (key.Value.Count == 0)
                {
                    pos.Add(key.Key);
                }
            }
            foreach (string st in pos)
            {
                context.Remove(st);
            }

            /*foreach (var key in context)                                                                      //prints dictionary key, values
            {
                Console.WriteLine(key.Key);
                for (int i = 0; i < key.Value.Count; i++)
                {
                    Console.WriteLine("Key: {0}  Value: {1}", key.Key, key.Value[i]);
                }
            }*/
        }


        public Dictionary<string, List<string>> M_Terminals
        {
            get { return context; }
        }
    }
}
