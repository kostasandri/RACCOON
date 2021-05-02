$NameSpaces$

namespace $GrammarName$ {
    /// <summary>
    /// <c>AbstractIterator</c> provides the general interface of the iterator pattern
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class CAbstractIterator<T> {

        protected T m_item;

        /// <summary>
        /// Get the current item pointed by the iterator
        /// </summary>
        /// <value>
        /// The item
        /// </value>
        public virtual T M_item {
            get { return m_item; }
        }

        /// <summary>
        /// This method provides configurable initialization of the iterator.
        /// The default version works exactly as Begin(). It can be overriden
        /// by a subclass if a configurable initialization is necessary
        /// </summary>
        /// <param name="param">The parameter.</param>
        /// <returns></returns>
        public virtual T Begin(object param=null) {
            return Begin();
        }

        /// <summary>
        /// This method provides configurable initialization of the iterator.
        /// The default version works exactly as ItBegin(). It can be overriden
        /// by a subclass if a configurable initialization is necessary
        /// </summary>
        /// <param name="param">The parameter.</param>
        public virtual void ItBegin(object param = null) {
            ItBegin();
        }

        /// <summary>
        /// Initializes the iterator an returns the first element.
        /// </summary>
        /// <returns></returns>
        public abstract T Begin();

        /// <summary>
        /// Assures iteration inside the loop bounds
        /// </summary>
        /// <returns></returns>
        public abstract bool End();

        /// <summary>
        /// Increases the iterator and get the element it points (after increament).
        /// </summary>
        /// <returns></returns>
        public abstract T Next();

        /// <summary>
        /// Initializes the iterator
        /// </summary>
        public abstract void ItBegin();

        /// <summary>
        /// Assures iteration inside the loop bounds
        /// </summary>
        /// <returns></returns>
        public abstract bool ItEnd();

        /// <summary>
        /// Increases the iterator to the next item
        /// </summary>
        public abstract void ITNext();
    }
}
