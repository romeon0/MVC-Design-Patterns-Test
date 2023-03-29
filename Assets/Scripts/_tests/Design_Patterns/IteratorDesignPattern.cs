using System.IO;
using UnityEngine;

namespace UnityGame.Tests.DesignPatterns
{
    class IteratorDesignPattern
    {
        private interface IElement
        {
          
        }

        private interface IIterator<T>
        {
            public T GetNext();
            public bool HasNext();
        }

        private interface ICollection<T>
        {
            IIterator<T> GetInterator();
        }

        private class Element : IElement
        {
            public string name;
        }

        private class Collection : ICollection<Element>
        {
            private Element[] _elements;

            public Collection(Element[] elements)
            {
                _elements = elements;
            }

            public IIterator<Element> GetInterator()
            {
                Iterator iterator = new Iterator(_elements);
                return iterator;
            }
        }

        private class Iterator : IIterator<Element>
        {
            private Element[] _elements;
            private int _currentIndex;

            public Iterator(Element[] elements)
            {
                _elements = elements;
                _currentIndex = 0;
            }

            public Element GetNext()
            {
                if (!HasNext())
                {
                    return null;
                }
                Element element = _elements[_currentIndex];
                ++_currentIndex;
                return element;
            }

            public bool HasNext()
            {
                if (_elements == null) return false;
                if (_currentIndex >= _elements.Length) return false;
                return true;
            }
        }

        public class Tester
        {
            public void Test()
            {
                Element[] elements = new Element[]
                {
                    new Element(){name = "elem_1"},
                    new Element(){name = "elem_2"},
                    new Element(){name = "elem_3"},
                    new Element(){name = "elem_4"},
                    new Element(){name = "elem_5"},
                };

                Collection collection = new Collection(elements);

                IIterator<Element> iterator = collection.GetInterator();
                while (iterator.HasNext())
                {
                    Element elem = iterator.GetNext();
                    Debug.Log($"[Iterator] Element: " + elem.name);
                }
            }
        }
    }
}
