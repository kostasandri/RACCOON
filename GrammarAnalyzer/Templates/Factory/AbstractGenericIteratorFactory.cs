using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrammarAnalyzer.ASTEvents;
using GrammarAnalyzer.ASTIterator;

namespace GrammarAnalyzer.ASTFactories {

    public interface IAbstractASTIteratorsFactory {

        CAbstractIterator<CASTElement> CreateIteratorASTElementDescentantsFlatten(CASTElement element);

        CAbstractIterator<CASTElement> CreateIteratorASTElementDescentantsFlattenEvents(CASTElement element, CASTGenericIteratorEvents events, object info = null);

        CAbstractIterator<CASTElement> CreateIteratorASTElementDescentantsContext(CASTElement element);

        CAbstractIterator<CASTElement> CreateIteratorASTElementDescentantsContextEvents(CASTElement element, CASTGenericIteratorEvents events, object info = null);

    }

    public abstract class CASTAbstractGenericIteratorFactory : IAbstractASTIteratorsFactory {

        public virtual CAbstractIterator<CASTElement> CreateIteratorASTElementDescentantsFlatten(CASTElement element) {
            return new CASTElementDescentantsFlattenIterator(element);
        }

        public virtual CAbstractIterator<CASTElement> CreateIteratorASTElementDescentantsFlattenEvents(CASTElement element, CASTGenericIteratorEvents events, object info=null) {
            return new CASTElementDescentantsFlattenEventIterator(element,events,info);
        }

        public virtual CAbstractIterator<CASTElement> CreateIteratorASTElementDescentantsContext(CASTElement element) {
            return new CASTElementDescentantsContextIterator(element);
        }

        public virtual CAbstractIterator<CASTElement> CreateIteratorASTElementDescentantsContextEvents(CASTElement element, CASTGenericIteratorEvents events, object info = null) {
            return new CASTElementDescentantsContextEventIterator(element, events, info);
        }

    }
}
