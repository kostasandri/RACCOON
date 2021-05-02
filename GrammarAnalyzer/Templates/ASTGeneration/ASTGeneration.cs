using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using System.Collections.Generic;

namespace GrammarAnalyzer
{
    class ASTGeneration : EBNFParserBaseVisitor<int>
    {
        private CASTElement m_root;
        private Stack<CASTComposite> m_parentsStack = new Stack<CASTComposite>();
        private Stack<ContextType> m_currentContext = new Stack<ContextType>();

        public CASTElement MRoot
        {
            get { return m_root; }
            set { m_root = value; }
        }

        public ASTGeneration()
        {
            m_currentContext.Push(ContextType.CT_NA);
        }

        private ITerminalNode GetTerminalNode(ParserRuleContext node, IToken terminal)
        {

            for (int i = 0; i < node.ChildCount; i++)
            {
                ITerminalNode child = node.GetChild(i) as ITerminalNode;
                if (child != null)
                {
                    if (child.Symbol == terminal)
                    {
                        return child;
                    }
                }
            }
            return null;
        }

        private int VisitElementInContext(IParseTree element, ContextType contextType)
        {
            int res;
            m_currentContext.Push(contextType);
            res = Visit(element);
            m_currentContext.Pop();
            return res;
        }

        private int VisitElementsInContext(IEnumerable<IParseTree> context, ContextType contextType)
        {
            int res = 0;
            m_currentContext.Push(contextType);
            foreach (ParserRuleContext elem in context)
            {
                res = Visit(elem);
            }
            m_currentContext.Pop();
            return res;
        }

        public override int VisitCompileUnit(EBNFParser.CompileUnitContext context)
        {
            //1 create compile unit ast node and update root node
            CCompileUnit newNode = new CCompileUnit();
            m_root = newNode;
            //2 add compile unit to the parent children

            //3 make current node as parent for the descendants
            m_parentsStack.Push(newNode);
            //4 visit descendants
            VisitElementsInContext(context.prologue(), ContextType.CT_COMPILEUNIT_PROLOGUE);
            VisitElementsInContext(context.grammar_rule(), ContextType.CT_COMPILEUNIT_GRAMMAR_RULE);

            //5 reverse step 3
            m_parentsStack.Pop();

            return 0;
        }

        public override int VisitGram_spec(EBNFParser.Gram_specContext context)
        {
            //1 create grammar spec ast node and update root node
            CGrammarSpec newNode = new CGrammarSpec(m_parentsStack.Peek());

            if (context.tp != null)
            {
                if (context.tp.Type == EBNFParser.PARSER)
                {
                    newNode.M_GrammarType = CGrammarSpec.GrammarType.GT_PARSER;
                }
                else if (context.tp.Type == EBNFParser.LEXER)
                {
                    newNode.M_GrammarType = CGrammarSpec.GrammarType.GT_LEXER;
                }
            }
            else
            {
                newNode.M_GrammarType = CGrammarSpec.GrammarType.GT_COMBINED;
            }
            //2 add Cgrammar spec to the parent children
            m_parentsStack.Peek().AddChild(newNode, m_currentContext.Peek());
            //3 make current node as parent for the descendants
            m_parentsStack.Push(newNode);
            //4 visit descendants
            VisitElementInContext(context.ID(), ContextType.CT_GRAMMARSPEC_TYPE);

            //5 reverse step 3
            m_parentsStack.Pop();

            return 0;
        }

        public override int VisitGrammar_rule(EBNFParser.Grammar_ruleContext context)
        {
            //1 create compile unit ast node and update root node
            CGrammarRule newNode = new CGrammarRule(m_parentsStack.Peek());
            //2 add Cgrammar spec to the parent children
            m_parentsStack.Peek().AddChild(newNode, m_currentContext.Peek());
            //3 make current node as parent for the descendants
            m_parentsStack.Push(newNode);
            //4 visit descendants
            VisitElementInContext(context.NON_TERMINAL(), ContextType.CT_GRAMMARRULE_NON_TERMINAL);
            VisitElementsInContext(context.grammar_rule_rhs_overide(), ContextType.CT_GRAMMARRULE_GRAMMAR_RULE_ALTERNATIVES);

            //5 reverse step 3
            m_parentsStack.Pop();

            return 0;
        }

        public override int VisitParenthesizedTerm(EBNFParser.ParenthesizedTermContext context)
        {
            //1 create grammar spec ast node and update root node
            CRHSRuleParenthesizedTerm newNode = new CRHSRuleParenthesizedTerm(m_parentsStack.Peek());
            //2 add Cgrammar spec to the parent children
            m_parentsStack.Peek().AddChild(newNode, m_currentContext.Peek());
            //3 make current node as parent for the descendants
            m_parentsStack.Push(newNode);
            //4 visit descendants
            VisitElementsInContext(context.term(), ContextType.CT_RHSPARENTHESIZEDTERM_OR_TERMS);
            //5 reverse step 3
            m_parentsStack.Pop();

            return 0;
        }

        public override int VisitClosurePlus(EBNFParser.ClosurePlusContext context)
        {

            //1 create grammar spec ast node and update root node
            CClosure newNode = new CClosure(CClosure.ClosureType.CT_PLUS, m_parentsStack.Peek());
            //2 add Cgrammar spec to the parent children
            m_parentsStack.Peek().AddChild(newNode, m_currentContext.Peek());
            //3 make current node as parent for the descendants
            m_parentsStack.Push(newNode);
            //4 visit descendants
            VisitElementInContext(context.factor(), ContextType.CT_CLOSURE_TERM);
            //5 reverse step 3
            m_parentsStack.Pop();

            return 0;
        }

        public override int VisitClosureAsterisk(EBNFParser.ClosureAsteriskContext context)
        {
            //1 create grammar spec ast node and update root node
            CClosure newNode = new CClosure(CClosure.ClosureType.CT_ASTERISK, m_parentsStack.Peek());
            //2 add Cgrammar spec to the parent children
            m_parentsStack.Peek().AddChild(newNode, m_currentContext.Peek());
            //3 make current node as parent for the descendants
            m_parentsStack.Push(newNode);
            //4 visit descendants
            VisitElementInContext(context.factor(), ContextType.CT_CLOSURE_TERM);
            //5 reverse step 3
            m_parentsStack.Pop();

            return 0;
        }

        public override int VisitClosureQMark(EBNFParser.ClosureQMarkContext context)
        {
            //1 create grammar spec ast node and update root node
            CClosure newNode = new CClosure(CClosure.ClosureType.CT_QMARK, m_parentsStack.Peek());
            //2 add Cgrammar spec to the parent children
            m_parentsStack.Peek().AddChild(newNode, m_currentContext.Peek());
            //3 make current node as parent for the descendants
            m_parentsStack.Push(newNode);
            //4 visit descendants
            VisitElementInContext(context.factor(), ContextType.CT_CLOSURE_TERM);
            //5 reverse step 3
            m_parentsStack.Pop();

            return 0;
        }

        public override int VisitBarefact(EBNFParser.BarefactContext context)
        {
            //1 create grammar spec ast node and update root node
            CClosure newNode = new CClosure(CClosure.ClosureType.CT_BARE, m_parentsStack.Peek());
            //2 add Cgrammar spec to the parent children
            m_parentsStack.Peek().AddChild(newNode, m_currentContext.Peek());
            //3 make current node as parent for the descendants
            m_parentsStack.Push(newNode);
            //4 visit descendants
            VisitElementInContext(context.factor(), ContextType.CT_CLOSURE_TERM);
            //5 reverse step 3
            m_parentsStack.Pop();

            return 0;
        }

        public override int VisitTerminal(ITerminalNode node)
        {
            switch (node.Symbol.Type)
            {
                case EBNFParser.TERMINAL:
                    CTERMINAL newnode = new CTERMINAL(node.Symbol.Text, NodeType.NT_TERMINAL, m_parentsStack.Peek());
                    m_parentsStack.Peek().AddChild(newnode, m_currentContext.Peek());
                    break;
                case EBNFParser.IMPLICIT_TERMINAL:
                    break;
                case EBNFParser.NON_TERMINAL:
                    CNONTERMINAL newnode1 = new CNONTERMINAL(node.Symbol.Text, NodeType.NT_NONTERMINAL, m_parentsStack.Peek());
                    m_parentsStack.Peek().AddChild(newnode1, m_currentContext.Peek());
                    break;
            }
            return base.VisitTerminal(node);
        }
    }
}
