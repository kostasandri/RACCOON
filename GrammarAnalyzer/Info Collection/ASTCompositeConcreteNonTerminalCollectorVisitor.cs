using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;

namespace GrammarAnalyzer
{   

    /// <summary>
    /// get non terminals and store it in list
    /// </summary>
    class ASTCompositeConcreteNonTerminalCollector : EBNFParserBaseVisitor<Int32>
    {
        private List<string> m_nonTerminals = new List<string>();


        public override int VisitGrammar_rule(EBNFParser.Grammar_ruleContext context)
        {
            m_nonTerminals.Add(context.NON_TERMINAL().GetText());
            return 0;
        }

        public List<string> M_NonTerminals
        {
            get { return m_nonTerminals; }
        }
    }
}
