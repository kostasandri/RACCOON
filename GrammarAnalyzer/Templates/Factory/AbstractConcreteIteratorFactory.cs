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

