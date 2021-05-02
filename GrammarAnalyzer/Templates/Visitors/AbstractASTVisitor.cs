using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrammarAnalyzer.ASTEvents;
using GrammarAnalyzer.ASTFactories;
using GrammarAnalyzer.ASTIterator;

namespace GrammarAnalyzer.ASTVisitor {
    /// <summary>
    /// A visitor class referring to an AST composite structure with a top
    /// level hierarchy class of type T and Visit methods returning type
    /// "Return"
    /// </summary>
    /// <typeparam name="Return">The return type of the Visit methods.</typeparam>
    public class CASTAbstractVisitor<Return> {

        private CASTGenericIteratorEvents m_events;

        private IAbstractASTIteratorsFactory m_iteratorFactory;

        public CASTAbstractVisitor(IAbstractASTIteratorsFactory iteratorFactory = null, CASTGenericIteratorEvents events=null) {
            if (iteratorFactory == null) {
                m_iteratorFactory = new CASTAbstractConcreteIteratorFactory();
            }
            else {
                m_iteratorFactory = iteratorFactory;
            }
            if (events == null) {
                m_events = new CASTGenericIteratorEvents();
            }
            else {
                m_events = events;
            }

        }

        /// <summary>
        /// <c>VisitChildren</c> method is used to visit child nodes of the node given
        /// as a parameter. This method provides default functionality for
        /// </summary>
        /// <param name="currentNode">The current node.</param>
        /// <returns></returns>
        public virtual Return VisitChildren(CASTElement currentNode) {

            CAbstractIterator<CASTElement> it = m_iteratorFactory.CreateIteratorASTElementDescentantsFlatten(currentNode);

            // Call Accept to all children of the current node
            for (it.ItBegin(); !it.ItEnd(); it.ITNext()) {
                it.M_item.AcceptVisitor(this);
            }

            return default(Return);
        }

        /// <summary>
        /// Visits the children of the specified context in the currentNode
        /// </summary>
        /// <param name="currentNode">The current node.</param>
        /// <returns></returns>
        public virtual Return VisitContext(CASTElement currentNode, ContextType context) {

            CAbstractIterator<CASTElement> it = m_iteratorFactory.CreateIteratorASTElementDescentantsContext(currentNode);

            // Call Accept to all children in the specified context of the current node
            for (it.ItBegin(context); !it.ItEnd(); it.ITNext()) {
                it.M_item.AcceptVisitor(this);
            }

            return default(Return);
        }

        public virtual Return VisitContextEvents(CASTElement currentNode, ContextType context) {
            CAbstractIterator<CASTElement> it = m_iteratorFactory.CreateIteratorASTElementDescentantsContextEvents(currentNode, m_events, this);

            // Call Accept to all children in the specified context of the current node
            for (it.ItBegin(context); !it.ItEnd(); it.ITNext()) {
                it.M_item.AcceptVisitor(this);
            }

            return default(Return);
        }

        /// <summary>
        /// Visits the children of the specified node <c>current</c> raising events
        /// at specific sequence points determined by the iterator
        /// </summary>
        /// <param name="currentNode">The current node.</param>
        /// <returns></returns>
        public virtual Return VisitChildrenEvents(CASTElement currentNode) {
            CAbstractIterator<CASTElement> it =
                m_iteratorFactory.CreateIteratorASTElementDescentantsFlattenEvents(currentNode,m_events,this);

            // Call Accept to all children of the current node
            for (it.ItBegin(); !it.ItEnd(); it.ITNext()) {
                it.M_item.AcceptVisitor(this);
            }

            return default(Return);
        }

        /// <summary>
        /// Visits the specified node by calling its Accept function. The method
        /// can be used to start a new traversal from the specified node
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns></returns>
        public virtual Return Visit(CASTElement node) {
            // Call Accept of the specific node
            return node.AcceptVisitor<Return>(this);
        }


        /// <summary>
        /// Visits the terminal. It doen't go any further since this is a leaf node
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns></returns>
        public virtual Return VisitTerminal(CASTElement node) {
            return default(Return);
        }

    }
}
