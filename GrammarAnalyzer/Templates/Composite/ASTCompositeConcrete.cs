/*
MIT License

Copyright(c) [2016] [Grigoris Dimitroulakos]

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using GrammarAnalyzer.ASTEvents;
using GrammarAnalyzer.ASTFactories;
using GrammarAnalyzer.ASTIterator;
using GrammarAnalyzer.ASTVisitor;
using System;

namespace GrammarAnalyzer
{
    /// <summary>
    /// This file contains automatically generated code
    /// </summary>
    ///
    public enum NodeType
    {
        /// <summary>
        /// This enumeration carries the different types of AST Nodes. The numbers indicate the index in the
        /// m_contextMappings table of tuples where each specific context starts
        /// </summary>
        // NON-TERMINAL NODES
        NT_COMPILEUNIT = 0,
        NT_GRAMMAR_SPEC = 2,
        NT_GRAMMAR_RULE = 4,
        NT_GRAMMAR_RULE_RHS = 6,
        NT_CLOSURE = NT_GRAMMAR_RULE_RHS + 1,
        NT_RHS_PARENTHESIZED_TERM = NT_CLOSURE + 1, //ΟΚ

        // LEAF NODES
        /* 5 */
        NT_NONTERMINAL = NT_RHS_PARENTHESIZED_TERM + 1,
        NT_TERMINAL,
        NT_IMPLICIT_TERMINAL,


        // NODE CATEGORIES
        NT_LEAF,
        NT_NA
    }

    public enum ContextType
    {
        /* 0 */
        CT_COMPILEUNIT_PROLOGUE,
        CT_COMPILEUNIT_GRAMMAR_RULE,

        /* +2 */
        CT_GRAMMARSPEC_TYPE,
        CT_GRAMMARSPEC_ID,

        /* +4 */
        CT_GRAMMARRULE_NON_TERMINAL,
        CT_GRAMMARRULE_GRAMMAR_RULE_ALTERNATIVES,

        /* +6 */
        CT_GRAMMARRULERHS_ALTERNATIVETERMS,

        /* +7 */
        CT_CLOSURE_TERM,

        /* +8 */
        CT_RHSPARENTHESIZEDTERM_OR_TERMS,

        CT_NA
    }

    /*public enum TypeSpecifier
    {
        TP_VOID,
        TP_FLOAT
    }*/


    public class CCompileUnit : CASTComposite
    {
        public CCompileUnit() : base(NodeType.NT_COMPILEUNIT, null, NodeType.NT_NA)
        {
        }

        public override Return AcceptVisitor<Return>(CASTAbstractVisitor<Return> visitor)
        {
            IASTAbstractConcreteVisitor<Return> typedVisitor = visitor as IASTAbstractConcreteVisitor<Return>;
            if (typedVisitor != null) return typedVisitor.VisitCompileUnit(this);
            else return visitor.VisitChildren(this);
        }

        public override CAbstractIterator<CASTElement> AcceptIterator(
            CASTAbstractConcreteIteratorFactory iteratorFactory)
        {
            IASTAbstractConcreteIteratorFactory typedFactory = iteratorFactory as IASTAbstractConcreteIteratorFactory;
            if (typedFactory != null)
            {
                return iteratorFactory.CreateCompilationUnitIterator(this);
            }

            return iteratorFactory.CreateIteratorASTElementDescentantsFlatten(this);
        }

        public override CAbstractIterator<CASTElement> AcceptEventIterator(
            CASTAbstractConcreteIteratorFactory iteratorFactory,
            CASTGenericIteratorEvents events, object info = null)
        {
            IASTAbstractConcreteIteratorFactory typedFactory = iteratorFactory as IASTAbstractConcreteIteratorFactory;
            if (typedFactory != null)
            {
                return iteratorFactory.CreateCompilationUnitIteratorEvents(this, events, info);
            }

            return iteratorFactory.CreateIteratorASTElementDescentantsFlattenEvents(this, events, info);
        }
    }

    public class CGrammarSpec : CASTComposite
    {
        private int m_identifiers = 0;
        private GrammarType m_grammarType = GrammarType.GT_NA;

        public enum GrammarType
        {
            GT_NA,
            GT_PARSER,
            GT_LEXER,
            GT_COMBINED
        };

        public GrammarType M_GrammarType
        {
            get { return m_grammarType; }
            set { m_grammarType = value; }
        }


        public CGrammarSpec(CASTComposite parent) : base(NodeType.NT_GRAMMAR_SPEC, parent, NodeType.NT_NA)
        {
        }

        public override Return AcceptVisitor<Return>(CASTAbstractVisitor<Return> visitor)
        {
            IASTAbstractConcreteVisitor<Return> typedVisitor = visitor as IASTAbstractConcreteVisitor<Return>;
            if (typedVisitor != null) return typedVisitor.VisitGram_Spec(this);
            else return visitor.VisitChildren(this);
        }

        public override CAbstractIterator<CASTElement> AcceptIterator(
            CASTAbstractConcreteIteratorFactory iteratorFactory)
        {
            IASTAbstractConcreteIteratorFactory typedFactory = iteratorFactory as IASTAbstractConcreteIteratorFactory;
            if (typedFactory != null)
            {
                return iteratorFactory.CreateGrammarSpecIterator(this);
            }

            return iteratorFactory.CreateIteratorASTElementDescentantsFlatten(this);
        }

        public override CAbstractIterator<CASTElement> AcceptEventIterator(
            CASTAbstractConcreteIteratorFactory iteratorFactory,
            CASTGenericIteratorEvents events, object info = null)
        {
            IASTAbstractConcreteIteratorFactory typedFactory = iteratorFactory as IASTAbstractConcreteIteratorFactory;
            if (typedFactory != null)
            {
                return iteratorFactory.CreateGrammarSpecIteratorEvents(this, events, info);
            }

            return iteratorFactory.CreateIteratorASTElementDescentantsFlattenEvents(this, events, info);
        }
    }

    public class CGrammarRule : CASTComposite
    {
        public CGrammarRule(CASTComposite parent) : base(NodeType.NT_GRAMMAR_RULE, parent, NodeType.NT_NA)
        {
        }

        public override Return AcceptVisitor<Return>(CASTAbstractVisitor<Return> visitor)
        {
            IASTAbstractConcreteVisitor<Return> typedVisitor = visitor as IASTAbstractConcreteVisitor<Return>;
            if (typedVisitor != null) return typedVisitor.VisitGrammar_Rule(this);
            else return visitor.VisitChildren(this);
        }

        public override CAbstractIterator<CASTElement> AcceptIterator(
            CASTAbstractConcreteIteratorFactory iteratorFactory)
        {
            IASTAbstractConcreteIteratorFactory typedFactory = iteratorFactory as IASTAbstractConcreteIteratorFactory;
            if (typedFactory != null)
            {
                return iteratorFactory.CreateGrammarRuleIterator(this);
            }

            return iteratorFactory.CreateIteratorASTElementDescentantsFlatten(this);
        }

        public override CAbstractIterator<CASTElement> AcceptEventIterator(
            CASTAbstractConcreteIteratorFactory iteratorFactory,
            CASTGenericIteratorEvents events, object info = null)
        {
            IASTAbstractConcreteIteratorFactory typedFactory = iteratorFactory as IASTAbstractConcreteIteratorFactory;
            if (typedFactory != null)
            {
                return iteratorFactory.CreateGrammarRuleIteratorEvents(this, events, info);
            }

            return iteratorFactory.CreateIteratorASTElementDescentantsFlattenEvents(this, events, info);
        }
    }

    public class CClosure : CASTComposite
    {
        public enum ClosureType
        {
            CT_BARE,
            CT_PLUS,
            CT_ASTERISK,
            CT_QMARK
        }

        private ClosureType m_ClosureType;

        public ClosureType MClosureType => m_ClosureType;

        public CClosure(ClosureType type, CASTComposite parent) : base(NodeType.NT_CLOSURE, parent, NodeType.NT_NA)
        {
            m_ClosureType = type;
        }

        public override Return AcceptVisitor<Return>(CASTAbstractVisitor<Return> visitor)
        {
            IASTAbstractConcreteVisitor<Return> typedVisitor = visitor as IASTAbstractConcreteVisitor<Return>;
            if (typedVisitor != null) return typedVisitor.VisitClosure(this);
            else return visitor.VisitChildren(this);
        }

        public override CAbstractIterator<CASTElement> AcceptIterator(
            CASTAbstractConcreteIteratorFactory iteratorFactory)
        {
            IASTAbstractConcreteIteratorFactory typedFactory = iteratorFactory as IASTAbstractConcreteIteratorFactory;
            if (typedFactory != null)
            {
                return iteratorFactory.CreateClosureIterator(this);
            }

            return iteratorFactory.CreateIteratorASTElementDescentantsFlatten(this);
        }

        public override CAbstractIterator<CASTElement> AcceptEventIterator(
            CASTAbstractConcreteIteratorFactory iteratorFactory,
            CASTGenericIteratorEvents events, object info = null)
        {
            IASTAbstractConcreteIteratorFactory typedFactory = iteratorFactory as IASTAbstractConcreteIteratorFactory;
            if (typedFactory != null)
            {
                return iteratorFactory.CreateClosureIteratorEvents(this, events, info);
            }

            return iteratorFactory.CreateIteratorASTElementDescentantsFlattenEvents(this, events, info);
        }
    }

    public class CGrammarRuleRHS : CASTComposite
    {
        private string m_visitorMethodName = null;

        public CGrammarRuleRHS(CASTComposite parent, string visitorName = null) : base(NodeType.NT_GRAMMAR_RULE_RHS, parent, NodeType.NT_NA)
        {
            m_visitorMethodName = visitorName;
        }

        public override Return AcceptVisitor<Return>(CASTAbstractVisitor<Return> visitor)
        {
            IASTAbstractConcreteVisitor<Return> typedVisitor = visitor as IASTAbstractConcreteVisitor<Return>;
            if (typedVisitor != null) return typedVisitor.VisitGrammar_Rule_Rhs(this);
            else return visitor.VisitChildren(this);
        }

        public override CAbstractIterator<CASTElement> AcceptIterator(
            CASTAbstractConcreteIteratorFactory iteratorFactory)
        {
            IASTAbstractConcreteIteratorFactory typedFactory = iteratorFactory as IASTAbstractConcreteIteratorFactory;
            if (typedFactory != null)
            {
                return iteratorFactory.CreateGrammarRuleRhsIterator(this);
            }

            return iteratorFactory.CreateIteratorASTElementDescentantsFlatten(this);
        }

        public override CAbstractIterator<CASTElement> AcceptEventIterator(
            CASTAbstractConcreteIteratorFactory iteratorFactory,
            CASTGenericIteratorEvents events, object info = null)
        {
            IASTAbstractConcreteIteratorFactory typedFactory = iteratorFactory as IASTAbstractConcreteIteratorFactory;
            if (typedFactory != null)
            {
                return iteratorFactory.CreateGrammarRuleRhsIteratorEvents(this, events, info);
            }

            return iteratorFactory.CreateIteratorASTElementDescentantsFlattenEvents(this, events, info);
        }
    }

    public class CRHSRuleParenthesizedTerm : CASTComposite
    {
        private bool m_negation = true;

        public CRHSRuleParenthesizedTerm(CASTComposite parent, bool negation = true) : base(NodeType.NT_RHS_PARENTHESIZED_TERM, parent, NodeType.NT_NA)
        {
            m_negation = negation;
        }

        public override Return AcceptVisitor<Return>(CASTAbstractVisitor<Return> visitor)
        {
            IASTAbstractConcreteVisitor<Return> typedVisitor = visitor as IASTAbstractConcreteVisitor<Return>;
            if (typedVisitor != null) return typedVisitor.VisitParenthesizedTerm(this);
            else return visitor.VisitChildren(this);
        }

        public override CAbstractIterator<CASTElement> AcceptIterator(CASTAbstractConcreteIteratorFactory iteratorFactory)
        {
            IASTAbstractConcreteIteratorFactory typedFactory = iteratorFactory as IASTAbstractConcreteIteratorFactory;
            if (typedFactory != null)
            {
                return iteratorFactory.CreateParenthesizedTermIterator(this);
            }

            return iteratorFactory.CreateIteratorASTElementDescentantsFlatten(this);
        }

        public override CAbstractIterator<CASTElement> AcceptEventIterator(CASTAbstractConcreteIteratorFactory iteratorFactory, CASTGenericIteratorEvents events, object info = null)
        {
            IASTAbstractConcreteIteratorFactory typedFactory = iteratorFactory as IASTAbstractConcreteIteratorFactory;
            if (typedFactory != null)
            {
                return iteratorFactory.CreateParenthesizedTermIteratorEvents(this, events, info);
            }

            return iteratorFactory.CreateIteratorASTElementDescentantsFlattenEvents(this, events, info);
        }
    }

    public class CTERMINAL : CASTLeaf<string>
    {
        public CTERMINAL(string literal, NodeType nodetype, CASTElement parent, NodeType nodeCategory = NodeType.NT_LEAF) : base(literal, nodetype, parent, nodeCategory)
        {
        }

        protected override void AddChild(CASTElement child, int context, int pos = -1)
        {
            throw new NotImplementedException();
        }

        public override Return AcceptVisitor<Return>(CASTAbstractVisitor<Return> visitor)
        {
            IASTAbstractConcreteVisitor<Return> typedVisitor = visitor as IASTAbstractConcreteVisitor<Return>;
            if (typedVisitor != null)
                return typedVisitor.VisitTERMINAL(this);
            else
                return visitor.VisitTerminal(this);
        }

        public override CAbstractIterator<CASTElement> AcceptIterator( CASTAbstractConcreteIteratorFactory iteratorFactory)
        {
            throw new NotImplementedException();
        }

        public override CAbstractIterator<CASTElement> AcceptEventIterator(CASTAbstractConcreteIteratorFactory iteratorFactory, CASTGenericIteratorEvents events, object info = null)
        {
            throw new NotImplementedException();
        }
    }

    public class CNONTERMINAL : CASTLeaf<string>
    {
        public CNONTERMINAL(string literal, NodeType nodetype, CASTElement parent, NodeType nodeCategory = NodeType.NT_LEAF) : base(literal, nodetype, parent, nodeCategory)
        {
        }

        protected override void AddChild(CASTElement child, int context, int pos = -1)
        {
            throw new NotImplementedException();
        }

        public override Return AcceptVisitor<Return>(CASTAbstractVisitor<Return> visitor)
        {
            IASTAbstractConcreteVisitor<Return> typedVisitor = visitor as IASTAbstractConcreteVisitor<Return>;
            if (typedVisitor != null)
                return typedVisitor.VisitNONTERMINAL(this);
            else
                return visitor.VisitTerminal(this);
        }

        public override CAbstractIterator<CASTElement> AcceptIterator(
            CASTAbstractConcreteIteratorFactory iteratorFactory)
        {
            throw new NotImplementedException();
        }

        public override CAbstractIterator<CASTElement> AcceptEventIterator(
            CASTAbstractConcreteIteratorFactory iteratorFactory,
            CASTGenericIteratorEvents events, object info = null)
        {
            throw new NotImplementedException();
        }
    }
}