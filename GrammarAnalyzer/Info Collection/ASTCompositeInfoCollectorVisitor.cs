using System;

namespace GrammarAnalyzer
{
    class ASTCompositeInfoCollectorVisitor : EBNFParserBaseVisitor<Int32>
    {
        // grammar name from g4 file
        private string m_grammarID;

        /// <summary>
        /// acquires grammar name from parse tree 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override int VisitGram_spec(EBNFParser.Gram_specContext context)
        {
            m_grammarID = context.ID().GetText();
            Console.WriteLine(m_grammarID);
            return 0;
        }

        public string M_GrammarID
        {
            get { return m_grammarID; }
        }

    }
}
