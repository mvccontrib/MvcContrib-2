using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using MvcContrib.UI.InputBuilder.Helpers;

namespace MvcContrib.TestHelper.Ui
{
    public class InputForm<TFormType>
    {
        private readonly IBrowserDriver _browserDriver;
        protected readonly LinkedList<IInputTester> _inputTesters = new LinkedList<IInputTester>();

        public InputForm(IBrowserDriver browserDriver)
        {
            _browserDriver = browserDriver;
            SubmitName = "Submit";
        }

        public string SubmitName { get; set; }

        private LinkedList<IInputTester> InputTesters
        {
            get { return _inputTesters; }
        }

        public InputForm<TFormType> Input(Expression<Func<TFormType, object>> expression, string text)
        {
            PropertyInfo propertyInfo = ReflectionHelper.FindPropertyFromExpression(expression);

            IInputTesterFactory factory = GetInputForProperty(propertyInfo);

            return Input(factory.Create(expression, text));
        }

        private IInputTesterFactory GetInputForProperty(PropertyInfo propertyInfo)
        {            
            return InputTesterFactory.Default().Where(factory => factory.CanHandle(propertyInfo)).First();
        }

        public InputForm<TFormType> Input(IInputTester tester)
        {
            _inputTesters.AddLast(tester);
            return this;
        }

        public IBrowserDriver Submit()
        {
            SetInputs();

            _browserDriver.ClickButton(SubmitName);
            return _browserDriver;
        }

        private void SetInputs()
        {
            EnumerableExtensions.ForEach(_inputTesters, x => x.SetInput(_browserDriver));
        }

        public IBrowserDriver BrowserDriver
        {
            get
            {
                return _browserDriver;   
            }
        }

    }
}