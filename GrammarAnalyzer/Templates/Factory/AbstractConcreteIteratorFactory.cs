using GrammarAnalyzer.ASTEvents;
using GrammarAnalyzer.ASTIterator;

namespace GrammarAnalyzer.ASTFactories
{

    public interface IASTAbstractConcreteIteratorFactory
    {
        //non-terminals start here

        #region CompilationUnit iterator

        CAbstractIterator<CASTElement> CreateCompilationUnitIterator(CASTElement element);

        CAbstractIterator<CASTElement> CreateCompilationUnitIteratorEvents(CASTElement element, CASTGenericIteratorEvents events, object info = null);

        #endregion

        #region GrammarSpec iterator

        CAbstractIterator<CASTElement> CreateGrammarSpecIterator(CASTElement element);

        CAbstractIterator<CASTElement> CreateGrammarSpecIteratorEvents(CASTElement element, CASTGenericIteratorEvents events, object info = null);

        #endregion

        #region GrammarRule iterator

        CAbstractIterator<CASTElement> CreateGrammarRuleIterator(CASTElement element);

        CAbstractIterator<CASTElement> CreateGrammarRuleIteratorEvents(CASTElement element, CASTGenericIteratorEvents events, object info = null);

        #endregion

        #region GrammarRuleRhs iterator

        CAbstractIterator<CASTElement> CreateGrammarRuleRhsIterator(CASTElement element);

        CAbstractIterator<CASTElement> CreateGrammarRuleRhsIteratorEvents(CASTElement element, CASTGenericIteratorEvents events, object info = null);

        #endregion

        #region Closure iterator

        CAbstractIterator<CASTElement> CreateClosureIterator(CASTElement element);

        CAbstractIterator<CASTElement> CreateClosureIteratorEvents(CASTElement element, CASTGenericIteratorEvents events, object info = null);

        #endregion

        #region Parenthesized Term iterator

        CAbstractIterator<CASTElement> CreateParenthesizedTermIterator(CASTElement element);

        CAbstractIterator<CASTElement> CreateParenthesizedTermIteratorEvents(CASTElement element, CASTGenericIteratorEvents events, object info = null);

        #endregion

        //non-terminals end here
    }


    public class CASTAbstractConcreteIteratorFactory : CASTAbstractGenericIteratorFactory, IASTAbstractConcreteIteratorFactory
    {
        #region CompileUnit iterator

        public virtual CAbstractIterator<CASTElement> CreateCompilationUnitIterator(CASTElement element)
        {
            return CreateIteratorASTElementDescentantsFlatten(element);
        }

        public virtual CAbstractIterator<CASTElement> CreateCompilationUnitIteratorEvents(CASTElement element, CASTGenericIteratorEvents events, object info = null)
        {
            return CreateIteratorASTElementDescentantsFlattenEvents(element, events, info);
        }

        #endregion

        #region GrammarSpec iterator

        public virtual CAbstractIterator<CASTElement> CreateGrammarSpecIterator(CASTElement element)
        {
            return CreateIteratorASTElementDescentantsFlatten(element);
        }

        public virtual CAbstractIterator<CASTElement> CreateGrammarSpecIteratorEvents(CASTElement element,
            CASTGenericIteratorEvents events, object info = null)
        {
            return CreateIteratorASTElementDescentantsFlattenEvents(element, events, info);
        }

        #endregion

        #region GrammarRule iterator

        public virtual CAbstractIterator<CASTElement> CreateGrammarRuleIterator(CASTElement element)
        {
            return CreateIteratorASTElementDescentantsFlatten(element);
        }

        public virtual CAbstractIterator<CASTElement> CreateGrammarRuleIteratorEvents(CASTElement element,
            CASTGenericIteratorEvents events, object info = null)
        {
            return CreateIteratorASTElementDescentantsFlattenEvents(element, events, info);
        }

        #endregion

        #region GrammarRuleRhs iterator

        public virtual CAbstractIterator<CASTElement> CreateGrammarRuleRhsIterator(CASTElement element)
        {
            return CreateIteratorASTElementDescentantsFlatten(element);
        }

        public virtual CAbstractIterator<CASTElement> CreateGrammarRuleRhsIteratorEvents(CASTElement element,
            CASTGenericIteratorEvents events, object info = null)
        {
            return CreateIteratorASTElementDescentantsFlattenEvents(element, events, info);
        }

        #endregion

        #region Closure iterator

        public virtual CAbstractIterator<CASTElement> CreateClosureIterator(CASTElement element)
        {
            return CreateIteratorASTElementDescentantsFlatten(element);
        }

        public virtual CAbstractIterator<CASTElement> CreateClosureIteratorEvents(CASTElement element,
            CASTGenericIteratorEvents events, object info = null)
        {
            return CreateIteratorASTElementDescentantsFlattenEvents(element, events, info);
        }

        #endregion

        #region Parenthesized Term iterator

        public virtual CAbstractIterator<CASTElement> CreateParenthesizedTermIterator(CASTElement element)
        {
            return CreateIteratorASTElementDescentantsFlatten(element);
        }

        public virtual CAbstractIterator<CASTElement> CreateParenthesizedTermIteratorEvents(CASTElement element,
            CASTGenericIteratorEvents events, object info = null)
        {
            return CreateIteratorASTElementDescentantsFlattenEvents(element, events, info);
        }

        #endregion
    }
}

